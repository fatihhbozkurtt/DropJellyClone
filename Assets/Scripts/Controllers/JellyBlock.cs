using System.Collections.Generic;
using System.Linq;
using Data;
using DG.Tweening;
using EssentialManagers.Packages.GridManager.Scripts;
using UnityEngine;

namespace Controllers
{
    public class JellyBlock : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private CellController parentCell;

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
            for (int i = 0; i < innerPieces.Count; i++)
            {
                innerPieces[i].CheckMatches();
            }
        }

        #region Getters/Setters

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

        public List<InnerPiece> GetFacedInnerPieces(Vector2Int targetCoordinates, out Vector2Int facingDirection)
        {
            Vector2Int interval = parentCell.GetCoordinates() - targetCoordinates;

            // Corrected mapping
            Dictionary<Vector2Int, PiecePositionEnum[]> directionMap = new()
            {
                { Vector2Int.left, new[] { PiecePositionEnum.Second, PiecePositionEnum.Fourth } },
                { Vector2Int.right, new[] { PiecePositionEnum.First, PiecePositionEnum.Third } },
                { Vector2Int.up, new[] { PiecePositionEnum.Third, PiecePositionEnum.Fourth } },
                { Vector2Int.down, new[] { PiecePositionEnum.First, PiecePositionEnum.Second } }
            };

            if (!directionMap.TryGetValue(interval, out var validPositions))
            {
                facingDirection = Vector2Int.zero;
                Debug.LogWarning(
                    $"Target at {targetCoordinates} is not a direct neighbor of {parentCell.GetCoordinates()}");
                return new List<InnerPiece>();
            }

            facingDirection = interval;
            Debug.Log(
                $"Facing direction from {parentCell.GetCoordinates()} to {targetCoordinates} is: {facingDirection}");

            return innerPieces
                .Where(piece => validPositions.Contains(piece.GetInnerPieceData().piecePositionEnum))
                .ToList();
        }

        #endregion

        public void RemoveInnerPiece(InnerPiece innerPiece)
        {
            if (innerPieces.Contains(innerPiece))
                innerPieces.Remove(innerPiece);

            if (innerPieces.Count == 0)
            {
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
            transform.DOMove(pos, 0.5f).OnComplete(() => parentCell.SetOccupied(this));
        }

        private void DestroySelf()
        {
            transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => Destroy(gameObject, 0.2f));
        }
    }
}