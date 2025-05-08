using System.Collections.Generic;
using Data;
using DG.Tweening;
using EssentialManagers.Packages.GridManager.Scripts;
using UnityEngine;

namespace Controllers
{
    public class InnerPiece : MonoBehaviour
    {
        [Header("Config")] [SerializeField] InnerPieceData innerPieceData;

        [Header("References")] [SerializeField]
        MeshRenderer meshRenderer;

        [Header("Debug")] [SerializeField] JellyBlock parentJellyBlock;
        [SerializeField] private List<InnerPiece> tempFacedInnerPieces;

        private void Awake()
        {
            innerPieceData.ColorEnum = DataExtensions.GetRandomColorEnum();
            parentJellyBlock = transform.parent.GetComponent<JellyBlock>();
            meshRenderer.material = DataExtensions.GetMaterialByColorEnum(innerPieceData.ColorEnum).Material;
            string nameSuffix;

            if (innerPieceData.ScaleType != ScaleType.TwoByTwo)
            {
                nameSuffix = innerPieceData.PiecePositionEnum == PiecePositionEnum.None
                    ? innerPieceData.EdgeEnum.ToString()
                    : innerPieceData.PiecePositionEnum.ToString();
            }
            else
            {
                nameSuffix = innerPieceData.ScaleType.ToString();
            }

            gameObject.name = "Piece_" + innerPieceData.ColorEnum + "_" + nameSuffix;
        }

        public void CheckMatches()
        {
            List<JellyBlock> neighborJellyBlocks = new List<JellyBlock>();
            List<InnerPiece> facedPieces = new List<InnerPiece>();
            CellController parentCell = parentJellyBlock.GetCell();

            #region Get Neighbor Jelly Blocks

            for (int i = 0; i < parentCell.GetNeighbors().Count; i++)
            {
                CellController neighborCell = parentCell.GetNeighbors()[i];
                if (neighborCell == null) continue;
                if (!neighborCell.isOccupied) continue;
                if (neighborCell.GetOccupantJB() == null) continue;

                neighborJellyBlocks.Add(neighborCell.GetOccupantJB());
            }

            #endregion

            #region Get Faced Inner Pieces

            Vector2Int myCoord = parentCell.GetCoordinates();
            
            facedPieces = FacedPieceChecker.GetFacedPieces(
                innerPieceData,
                neighborJellyBlocks,
                myCoord);

            tempFacedInnerPieces = facedPieces;

            #endregion


            #region Check For Color Matches

            foreach (var piece in facedPieces)
            {
                if (piece.GetInnerPieceData().ColorEnum != innerPieceData.ColorEnum) continue;

                piece.PerformMatch();
                PerformMatch();
            }

            #endregion
        }

        private void PerformMatch()
        {
            transform.DOScale(Vector3.zero, 1f).OnComplete(() =>
            {
                parentJellyBlock.RemoveInnerPiece(this);
                Destroy(gameObject);
            });
        }

        public InnerPieceData GetInnerPieceData()
        {
            return innerPieceData;
        }
    }
}