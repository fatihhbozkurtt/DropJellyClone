using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Enums;
using DG.Tweening;
using EssentialManagers.Packages.GridManager.Scripts;
using Managers;
using UnityEditor;
using UnityEngine;

namespace Controllers
{
    public class JellyBlock : MonoBehaviour
    {
        public event System.Action<InnerPieceData> PieceRemovedEvent;

        [Header("References")] [SerializeField]
        private CellController parentCell;

        [SerializeField] private InnerPiece innerPiecePrefab;

        [Header("Debug")] private GridManager _gridManager;
        [SerializeField] private List<InnerPiece> innerPieces;
        MovePerformer _movePerformer;
        bool _isDestroyed;

        public readonly Vector3
            JellySpawnPos = new(0, 0.25f, 0); // değişmeyecek değer varsaydığım için hard-coded atama yaptım


        private void Start()
        {
            _movePerformer = GetComponent<MovePerformer>();
            LevelLoadManager.instance.NewLevelLoadedEvent += DestroySelf;

            transform.parent = null;
            if (parentCell != null) // intantiated on a cell by editor
            {
                _movePerformer.enabled = false;
                parentCell.SetOccupied(this);
            }

            _gridManager = GridManager.instance;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce).From(Vector3.zero)
                .OnComplete(() => _movePerformer.enabled = !parentCell);
        }

        public void InitializeRuntime(List<ColorEnum> colorEnumPool)
        {
            innerPieces = GetComponentsInChildren<InnerPiece>().ToList();

            if (colorEnumPool.Count != innerPieces.Count)
            {
                Debug.LogWarning("Incorrect number of InnerPieces or ColorEnumPool");
                return;
            }

            for (int i = 0; i < innerPieces.Count; i++)
            {
                innerPieces[i].InitializeRuntime(colorEnumPool[i]);
            }
        }

        public void TriggerMatchChecking(out List<bool> matchOccuredList)
        {
            matchOccuredList = new List<bool>();

            foreach (var piece in innerPieces)
            {
                if (piece == null) continue;
                piece.CheckMatches(out bool matchOccured);
                matchOccuredList.Add(matchOccured);
            }
        }

        public void RemoveInnerPiece(InnerPiece innerPiece)
        {
            if (innerPieces.Contains(innerPiece))
            {
                innerPieces.Remove(innerPiece);
                PieceRemovedEvent?.Invoke(innerPiece.GetInnerPieceData()); // trigger for scale adjustment
                // of remaining pieces
            }

            if (innerPieces.Count == 0)
            {
                parentCell.SetFree();
                _gridManager.OnAJellyBlockDestroyed(parentCell.GetCoordinates(), this);
                DestroySelf();
            }
        }

        private Tween _moveColumnTween;

        public void MoveDownOnColumn()
        {
            Vector2Int targetCoordinates = parentCell.GetCoordinates() + new Vector2Int(0, -1);

            parentCell.SetFree();
            parentCell = _gridManager.GetGridCellByCoordinates(targetCoordinates);

            Vector3 pos = new Vector3(transform.position.x, transform.position.y, parentCell.transform.position.z);

            transform.DOMove(pos, 0.5f).OnComplete(() =>
            {
                parentCell.SetOccupied(this);
                TriggerMatchChecking(out _); // if move happens check again for matching
            });
        }


        private void DestroySelf()
        {
            _isDestroyed = true;
            LevelLoadManager.instance.NewLevelLoadedEvent -= DestroySelf;
            transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
            {
                transform.DOKill();
                parentCell?.SetFree();
                Destroy(gameObject);
            });
        }

        #region Getters/Setters

        public void SetInnerPieces(List<InnerPieceData> pieceDataList)
        {
#if UNITY_EDITOR
            foreach (var pieceData in pieceDataList)
            {
                // Editor'da prefab bağlantısını koruyarak instantiate
                InnerPiece clone = (InnerPiece)PrefabUtility.InstantiatePrefab(innerPiecePrefab, transform);
                clone.transform.localPosition = Vector3.zero;
                clone.InitializeEditor(pieceData);

                // Undo kaydı ekle (geri alınabilirlik için)
                Undo.RegisterCreatedObjectUndo(clone.gameObject, "Spawn InnerPiece");
            }
#else
    Debug.LogWarning("SetInnerPieces was called outside of editor, but uses editor-only methods.");
#endif

            innerPieces = GetComponentsInChildren<InnerPiece>().ToList();
        }


        public void SetCell(CellController cell)
        {
            parentCell = cell;
        }

        public CellController GetCell()
        {
            return parentCell;
        }

        public List<InnerPiece> GetInnerPieces(bool getFromHierarchy = false)
        {
            return getFromHierarchy
                ? GetComponentsInChildren<InnerPiece>().ToList()
                : innerPieces;
        }

        #endregion
    }
}