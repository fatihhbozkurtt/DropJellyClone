using System.Collections.Generic;
using System.Linq;
using Data;
using DG.Tweening;
using EssentialManagers.Packages.GridManager.Scripts;
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

        void Start()
        {
            if (parentCell != null)
            {
                GetComponent<MovePerformer>().enabled = false;
                parentCell.SetOccupied(this);
            }

            _gridManager = GridManager.instance;
            innerPieces = GetComponentsInChildren<InnerPiece>().ToList();
        }

        public void TriggerMatchChecking()
        {
            foreach (var piece in innerPieces)
            {
                piece.CheckMatches();
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

        public void MoveDownOnColumn()
        {
            Vector2Int targetCoordinates = parentCell.GetCoordinates() + new Vector2Int(0, -1);

            parentCell.SetFree();
            parentCell = _gridManager.GetGridCellByCoordinates(targetCoordinates);


            Vector3 pos = new Vector3(transform.position.x, transform.position.y, parentCell.transform.position.z);
            transform.DOMove(pos, 0.5f).OnComplete(() =>
            {
                parentCell.SetOccupied(this);
                TriggerMatchChecking(); // if move happens check again for matching
            });
        }

        private void DestroySelf()
        {
            transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => Destroy(gameObject, 0.2f));
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
                clone.Initialize(pieceData);

                // Undo kaydı ekle (geri alınabilirlik için)
                Undo.RegisterCreatedObjectUndo(clone.gameObject, "Spawn InnerPiece");
            }
#else
    Debug.LogWarning("SetInnerPieces was called outside of editor, but uses editor-only methods.");
#endif
        }


        public void SetCell(CellController cell)
        {
            parentCell = cell;
        }

        public CellController GetCell()
        {
            return parentCell;
        }

        public List<InnerPiece> GetInnerPieces()
        {
            return innerPieces;
        }

        #endregion
    }
}