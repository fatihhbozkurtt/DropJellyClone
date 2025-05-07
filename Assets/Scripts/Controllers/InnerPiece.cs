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

            foreach (var jellyBlock in neighborJellyBlocks)
            {
                List<InnerPiece> pieces = jellyBlock.GetInnerPieces();
                List<InnerPiece> facedInnerPieces = new List<InnerPiece>();

                Vector2Int myCellCoor = parentCell.GetCoordinates();
                Vector2Int neighborCellCoor = jellyBlock.GetCell().GetCoordinates();
                Vector2Int interval = neighborCellCoor - myCellCoor;

                if (innerPieceData.ScaleType == ScaleType.OneByOne)
                {
                    switch (innerPieceData.PiecePositionEnum)
                    {
                        case PiecePositionEnum.First:
                            if (interval == Vector2Int.left) // neighbor jelly is on my left
                            {
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Second));
                            }

                            if (interval == Vector2Int.up)
                            {
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Third));
                            }

                            break;
                        case PiecePositionEnum.Second:

                            if (interval == Vector2Int.right) // neighbor jelly is on my left
                            {
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.First));
                            }

                            if (interval == Vector2Int.up)
                            {
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Fourth));
                            }

                            break;
                        case PiecePositionEnum.Third:

                            if (interval == Vector2Int.left) // neighbor jelly is on my left
                            {
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Fourth));
                            }

                            if (interval == Vector2Int.down)
                            {
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.First));
                            }

                            break;
                        case PiecePositionEnum.Fourth:

                            if (interval == Vector2Int.right) // neighbor jelly is on my left
                            {
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Third));
                            }

                            if (interval == Vector2Int.down)
                            {
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Second));
                            }

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (innerPieceData.ScaleType == ScaleType.TwoByOne)
                {
                    switch (innerPieceData.EdgeEnum)
                    {
                        case EdgeEnum.None:
                            Debug.LogWarning("Something wrong with the edge enum. It cannot be none.");
                            break;
                        case EdgeEnum.Left:
                            if (interval == Vector2Int.left)
                            {
                                // we assume neighbor inner piece is OneByOne type
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Second
                                    || t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Fourth));
                            }

                            if (interval == Vector2Int.up)
                            {
                                // we assume neighbor inner piece is OneByOne type
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Third));
                            }

                            if (interval == Vector2Int.down)
                            {
                                // we assume neighbor inner piece is OneByOne type
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.First));
                            }

                            break;
                        case EdgeEnum.Right:
                            if (interval == Vector2Int.right) // neighbor jelly is on my right
                            {
                                // we assume neighbor inner piece is OneByOne type
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.First
                                    || t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Third));
                            }

                            if (interval == Vector2Int.up)
                            {
                                // we assume neighbor inner piece is OneByOne type
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Fourth));
                            }

                            if (interval == Vector2Int.down)
                            {
                                // we assume neighbor inner piece is OneByOne type
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Second));
                            }

                            break;
                        case EdgeEnum.Top:
                            if (interval == Vector2Int.left)
                            {
                                // we assume neighbor inner piece is OneByOne type
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Second));
                            }

                            if (interval == Vector2Int.right)
                            {
                                // we assume neighbor inner piece is OneByOne type
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.First));
                            }

                            if (interval == Vector2Int.up)
                            {
                                // we assume neighbor inner piece is OneByOne type
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Third
                                    || t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Fourth));
                            }

                            break;
                        case EdgeEnum.Bottom:
                            if (interval == Vector2Int.left)
                            {
                                // we assume neighbor inner piece is OneByOne type
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Fourth));
                            }

                            if (interval == Vector2Int.right)
                            {
                                // we assume neighbor inner piece is OneByOne type
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Third));
                            }

                            if (interval == Vector2Int.down)
                            {
                                // we assume neighbor inner piece is OneByOne type
                                facedInnerPieces.AddRange(pieces.Where(t =>
                                    t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.First
                                    || t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Second));
                            }

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (innerPieceData.ScaleType == ScaleType.TwoByTwo)
                {
                    #region 1x1 Solutions
                   
                    if (interval == Vector2Int.left) // neighbor jelly is on my left
                    {
                        facedInnerPieces.AddRange(pieces.Where(t =>
                            t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Second
                            || t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Fourth));
                    }
                    
                    if (interval == Vector2Int.right) // neighbor jelly is on my right
                    {
                        facedInnerPieces.AddRange(pieces.Where(t =>
                            t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.First
                            || t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Third));
                    }

                    if (interval == Vector2Int.up) // neighbor jelly is on top of me
                    {
                        facedInnerPieces.AddRange(pieces.Where(t =>
                            t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Third
                            || t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Fourth));
                    }
                    
                    if (interval == Vector2Int.down) // neighbor jelly is under me
                    {
                        facedInnerPieces.AddRange(pieces.Where(t =>
                            t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.First
                            || t.GetInnerPieceData().PiecePositionEnum == PiecePositionEnum.Second));
                    }
                    #endregion

                    // #region 2x1 Solutions
                    //
                    // #endregion
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