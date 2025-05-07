using System;
using System.Collections.Generic;
using System.Linq;
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

            gameObject.name = "Piece_" + innerPieceData.ColorEnum + "_" + innerPieceData.piecePositionEnum;
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

            foreach (var jellyBlock in neighborJellyBlocks)
            {
                List<InnerPiece> pieces = jellyBlock.GetInnerPieces();
                List<InnerPiece> facedInnerPieces = new List<InnerPiece>();

                Vector2Int myCellCoor = parentCell.GetCoordinates();
                Vector2Int neighborCellCoor = jellyBlock.GetCell().GetCoordinates();
                Vector2Int interval = neighborCellCoor - myCellCoor;

                switch (innerPieceData.piecePositionEnum)
                {
                    case PiecePositionEnum.First:
                        if (interval == Vector2Int.left) // neighbor jelly is on my left
                        {
                            facedInnerPieces.AddRange(pieces.Where(t =>
                                t.GetInnerPieceData().piecePositionEnum == PiecePositionEnum.Second));
                        }

                        if (interval == Vector2Int.up)
                        {
                            facedInnerPieces.AddRange(pieces.Where(t =>
                                t.GetInnerPieceData().piecePositionEnum == PiecePositionEnum.Third));
                        }

                        break;
                    case PiecePositionEnum.Second:

                        if (interval == Vector2Int.right) // neighbor jelly is on my left
                        {
                            facedInnerPieces.AddRange(pieces.Where(t =>
                                t.GetInnerPieceData().piecePositionEnum == PiecePositionEnum.First));
                        }

                        if (interval == Vector2Int.up)
                        {
                            facedInnerPieces.AddRange(pieces.Where(t =>
                                t.GetInnerPieceData().piecePositionEnum == PiecePositionEnum.Fourth));
                        }

                        break;
                    case PiecePositionEnum.Third:

                        if (interval == Vector2Int.left) // neighbor jelly is on my left
                        {
                            facedInnerPieces.AddRange(pieces.Where(t =>
                                t.GetInnerPieceData().piecePositionEnum == PiecePositionEnum.Fourth));
                        }

                        if (interval == Vector2Int.down)
                        {
                            facedInnerPieces.AddRange(pieces.Where(t =>
                                t.GetInnerPieceData().piecePositionEnum == PiecePositionEnum.First));
                        }

                        break;
                    case PiecePositionEnum.Fourth:

                        if (interval == Vector2Int.right) // neighbor jelly is on my left
                        {
                            facedInnerPieces.AddRange(pieces.Where(t =>
                                t.GetInnerPieceData().piecePositionEnum == PiecePositionEnum.Third));
                        }

                        if (interval == Vector2Int.down)
                        {
                            facedInnerPieces.AddRange(pieces.Where(t =>
                                t.GetInnerPieceData().piecePositionEnum == PiecePositionEnum.Second));
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                facedPieces.AddRange(facedInnerPieces);
            }

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