using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using Data;
using Data.Enums;
using UnityEngine;

namespace Data
{
    public static class ScaleModifier
    {
        public static ScaleData GetModifiedScale(InnerPieceData removedData,
            InnerPiece myPiece, JellyBlock parentJelly)
        {
            ScaleData scaleData = new ScaleData();

            List<InnerPiece> pieceExceptMeList =
                parentJelly.GetInnerPieces().Where(p => p != myPiece && !p.IsMatched).ToList();
            List<PiecePositionEnum> positionEnums =
                pieceExceptMeList.Select(t => t.GetInnerPieceData().PiecePositionEnum).ToList();
            List<EdgeEnum> edgeEnums = pieceExceptMeList.Select(t => t.GetInnerPieceData().EdgeEnum).ToList();

            var myData = myPiece.GetInnerPieceData();

            switch (myData.ScaleType)
            {
                case ScaleType.OneByOne:
                    if (myData.PiecePositionEnum == PiecePositionEnum.First)
                    {
                        if (!positionEnums.Contains(PiecePositionEnum.Second) && !edgeEnums.Contains(EdgeEnum.Right))
                        {
                            scaleData = new ScaleData
                            {
                                Scale = GetHorizontalScale(),
                                Pos = GetHorizontalPosition(myPiece.GetMeshPos(), myData.PiecePositionEnum),
                                ModifiedPieceData = new InnerPieceData
                                {
                                    ColorEnum = myData.ColorEnum,
                                    ScaleType = ScaleType.TwoByOne,
                                    PiecePositionEnum = PiecePositionEnum.None,
                                    EdgeEnum = EdgeEnum.Top
                                }
                            };
                        }

                        if (!positionEnums.Contains(PiecePositionEnum.Third) && !edgeEnums.Contains(EdgeEnum.Bottom))
                        {
                            scaleData = new ScaleData
                            {
                                Scale = GetVerticalScale(),
                                Pos = GetVerticalPosition(myPiece.GetMeshPos(), myData.PiecePositionEnum),
                                ModifiedPieceData = new InnerPieceData
                                {
                                    ColorEnum = myData.ColorEnum,
                                    ScaleType = ScaleType.TwoByOne,
                                    PiecePositionEnum = PiecePositionEnum.None,
                                    EdgeEnum = EdgeEnum.Left
                                }
                            };
                        }
                    }
                    else if (myData.PiecePositionEnum == PiecePositionEnum.Second)
                    {
                        if (!positionEnums.Contains(PiecePositionEnum.First) && !edgeEnums.Contains(EdgeEnum.Left))
                        {
                            scaleData = new ScaleData
                            {
                                Scale = GetHorizontalScale(),
                                Pos = GetHorizontalPosition(myPiece.GetMeshPos(), myData.PiecePositionEnum),
                                ModifiedPieceData = new InnerPieceData
                                {
                                    ColorEnum = myData.ColorEnum,
                                    ScaleType = ScaleType.TwoByOne,
                                    PiecePositionEnum = PiecePositionEnum.None,
                                    EdgeEnum = EdgeEnum.Top
                                }
                            };
                        }

                        if (!positionEnums.Contains(PiecePositionEnum.Fourth) && !edgeEnums.Contains(EdgeEnum.Bottom))
                        {
                            scaleData = new ScaleData
                            {
                                Scale = GetVerticalScale(),
                                Pos = GetVerticalPosition(myPiece.GetMeshPos(), myData.PiecePositionEnum),
                                ModifiedPieceData = new InnerPieceData
                                {
                                    ColorEnum = myData.ColorEnum,
                                    ScaleType = ScaleType.TwoByOne,
                                    PiecePositionEnum = PiecePositionEnum.None,
                                    EdgeEnum = EdgeEnum.Right
                                }
                            };
                        }
                    }
                    else if (myData.PiecePositionEnum == PiecePositionEnum.Third)
                    {
                        if (!positionEnums.Contains(PiecePositionEnum.Fourth) && !edgeEnums.Contains(EdgeEnum.Right))
                        {
                            scaleData = new ScaleData
                            {
                                Scale = GetHorizontalScale(),
                                Pos = GetHorizontalPosition(myPiece.GetMeshPos(), myData.PiecePositionEnum),
                                ModifiedPieceData = new InnerPieceData
                                {
                                    ColorEnum = myData.ColorEnum,
                                    ScaleType = ScaleType.TwoByOne,
                                    PiecePositionEnum = PiecePositionEnum.None,
                                    EdgeEnum = EdgeEnum.Bottom
                                }
                            };
                        }

                        if (!positionEnums.Contains(PiecePositionEnum.First) && !edgeEnums.Contains(EdgeEnum.Top))
                        {
                            scaleData = new ScaleData
                            {
                                Scale = GetVerticalScale(),
                                Pos = GetVerticalPosition(myPiece.GetMeshPos(), myData.PiecePositionEnum),
                                ModifiedPieceData = new InnerPieceData
                                {
                                    ColorEnum = myData.ColorEnum,
                                    ScaleType = ScaleType.TwoByOne,
                                    PiecePositionEnum = PiecePositionEnum.None,
                                    EdgeEnum = EdgeEnum.Left
                                }
                            };
                        }
                    }
                    else if (myData.PiecePositionEnum == PiecePositionEnum.Fourth)
                    {
                        if (!positionEnums.Contains(PiecePositionEnum.Third) && !edgeEnums.Contains(EdgeEnum.Left))
                        {
                            scaleData = new ScaleData
                            {
                                Scale = GetHorizontalScale(),
                                Pos = GetHorizontalPosition(myPiece.GetMeshPos(), myData.PiecePositionEnum),
                                ModifiedPieceData = new InnerPieceData
                                {
                                    ColorEnum = myData.ColorEnum,
                                    ScaleType = ScaleType.TwoByOne,
                                    PiecePositionEnum = PiecePositionEnum.None,
                                    EdgeEnum = EdgeEnum.Bottom
                                }
                            };
                        }

                        if (!positionEnums.Contains(PiecePositionEnum.Second) && !edgeEnums.Contains(EdgeEnum.Top))
                        {
                            scaleData = new ScaleData
                            {
                                Scale = GetVerticalScale(),
                                Pos = GetVerticalPosition(myPiece.GetMeshPos(), myData.PiecePositionEnum),
                                ModifiedPieceData = new InnerPieceData
                                {
                                    ColorEnum = myData.ColorEnum,
                                    ScaleType = ScaleType.TwoByOne,
                                    PiecePositionEnum = PiecePositionEnum.None,
                                    EdgeEnum = EdgeEnum.Right
                                }
                            };
                        }
                    }

                    break;
                case ScaleType.TwoByOne:
                    if (pieceExceptMeList.Count == 0)
                        scaleData = new ScaleData
                        {
                            Scale = GetFullScale(),
                            Pos = GetFullPos(),
                            ModifiedPieceData = new InnerPieceData
                            {
                                ColorEnum = myData.ColorEnum,
                                ScaleType = ScaleType.TwoByTwo,
                                PiecePositionEnum = PiecePositionEnum.None,
                                EdgeEnum = EdgeEnum.None
                            }
                        };
                    break;
                case ScaleType.TwoByTwo:
                    ;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return scaleData;
        }

        #region Horizontal Calculations

        public static Vector3 GetHorizontalScale()
        {
            return new Vector3(0.9f, .1f, 0.4f);
        }

        public static Vector3 GetHorizontalPosition(Vector3 originPos, PiecePositionEnum piecePositionEnum)
        {
            float sign = -1;
            if (piecePositionEnum is PiecePositionEnum.First or PiecePositionEnum.Third)
                sign = 1;

            return originPos + (sign * new Vector3(.25f, 0, 0));
        }

        #endregion

        #region Vertical Calculations

        public static Vector3 GetVerticalScale()
        {
            return new Vector3(0.4f, .1f, 0.9f);
        }

        private static Vector3 GetVerticalPosition(Vector3 originPos, PiecePositionEnum piecePositionEnum)
        {
            float sign = 1;
            if (piecePositionEnum is PiecePositionEnum.First or PiecePositionEnum.Second)
                sign = -1;

            return originPos + (sign * new Vector3(0, 0, 0.25f));
        }

        #endregion

        #region Full shape calculations

        public static Vector3 GetFullScale()
        {
            return new Vector3(0.9f, .1f, 0.9f);
        }

        private static Vector3 GetFullPos()
        {
            return new Vector3(0, 0.2f, 0);
        }

        #endregion
        
        public static float GeneralPosOffset()
        {
            return 0.25f;
        }
    }
}

[Serializable]
public class ScaleData
{
    public Vector3 Scale;
    public Vector3 Pos;
    public InnerPieceData ModifiedPieceData;
}