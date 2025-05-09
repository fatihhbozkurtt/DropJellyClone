using System.Collections.Generic;
using Controllers;
using Data.Enums;
using UnityEngine;

namespace Data
{
    public static class FacedPieceChecker
    {
        public static List<InnerPiece> GetFacedPieces(InnerPieceData myData,
            List<JellyBlock> neighborJellyBlocks,
            Vector2Int myCoord)
        {
            List<InnerPiece> facedPieces = new();

            foreach (var jellyBlock in neighborJellyBlocks)
            {
                Vector2Int neighborCoord = jellyBlock.GetCell().GetCoordinates();
                Vector2Int interval = neighborCoord - myCoord;

                foreach (var neighborPiece in jellyBlock.GetInnerPieces())
                {
                    var neighborData = neighborPiece.GetInnerPieceData();

                    if (neighborData.ScaleType == ScaleType.OneByOne) // check if neighbor 1x1
                    {
                        switch (myData.ScaleType)
                        {
                            case ScaleType.OneByOne:
                                facedPieces.AddRange(GetFacedFor1X1(interval, myData.PiecePositionEnum, neighborPiece));
                                break;

                            case ScaleType.TwoByOne:
                                facedPieces.AddRange(GetFacedFor2X1(interval, myData.EdgeEnum, neighborPiece));
                                break;

                            case ScaleType.TwoByTwo:
                                facedPieces.AddRange(GetFacedFor2X2(interval, neighborPiece));
                                break;
                        }
                    }

                    if (neighborData.ScaleType == ScaleType.TwoByOne) // check if neighbor 2x1
                    {
                        switch (myData.ScaleType)
                        {
                            case ScaleType.OneByOne:
                                facedPieces.AddRange(GetFacedFor1X1VersionTwo(interval, myData.PiecePositionEnum,
                                    neighborPiece));
                                break;

                            case ScaleType.TwoByOne:
                                facedPieces.AddRange(GetFacedFor2X1VersionTwo(interval, myData.EdgeEnum,
                                    neighborPiece));
                                break;

                            case ScaleType.TwoByTwo:
                                facedPieces.AddRange(GetFacedFor2X2VersionTwo(interval, neighborPiece));
                                break;
                        }
                    }

                    if (neighborData.ScaleType == ScaleType.TwoByTwo) // check if neighbor 2x2
                    {
                        switch (myData.ScaleType)
                        {
                            case ScaleType.OneByOne:
                                facedPieces.AddRange(GetFacedFor1X1VersionThree(interval, myData.PiecePositionEnum,
                                    neighborPiece));
                                break;

                            case ScaleType.TwoByOne:
                                facedPieces.AddRange(GetFacedFor2X1VersionThree(interval, myData.EdgeEnum,
                                    neighborPiece));
                                break;

                            case ScaleType.TwoByTwo:
                                facedPieces.Add(neighborPiece);
                                break;
                        }
                    }
                }
            }

            return facedPieces;
        }

        #region Faced Neighbor 1x1 Solutions

        private static List<InnerPiece> GetFacedFor1X1(Vector2Int interval, PiecePositionEnum myPos,
            InnerPiece neighbor)
        {
            List<InnerPiece> list = new();
            var pos = neighbor.GetInnerPieceData().PiecePositionEnum;

            if (myPos == PiecePositionEnum.First)
            {
                if (interval == Vector2Int.left && pos == PiecePositionEnum.Second) list.Add(neighbor);
                if (interval == Vector2Int.up && pos == PiecePositionEnum.Third) list.Add(neighbor);
            }
            else if (myPos == PiecePositionEnum.Second)
            {
                if (interval == Vector2Int.right && pos == PiecePositionEnum.First) list.Add(neighbor);
                if (interval == Vector2Int.up && pos == PiecePositionEnum.Fourth) list.Add(neighbor);
            }
            else if (myPos == PiecePositionEnum.Third)
            {
                if (interval == Vector2Int.left && pos == PiecePositionEnum.Fourth) list.Add(neighbor);
                if (interval == Vector2Int.down && pos == PiecePositionEnum.First) list.Add(neighbor);
            }
            else if (myPos == PiecePositionEnum.Fourth)
            {
                if (interval == Vector2Int.right && pos == PiecePositionEnum.Third) list.Add(neighbor);
                if (interval == Vector2Int.down && pos == PiecePositionEnum.Second) list.Add(neighbor);
            }

            return list;
        }

        private static List<InnerPiece> GetFacedFor2X1(Vector2Int interval, EdgeEnum myEdge, InnerPiece neighbor)
        {
            List<InnerPiece> list = new();
            var pos = neighbor.GetInnerPieceData().PiecePositionEnum;

            switch (myEdge)
            {
                case EdgeEnum.Left:
                    if (interval == Vector2Int.left &&
                        (pos == PiecePositionEnum.Second || pos == PiecePositionEnum.Fourth)) list.Add(neighbor);
                    if (interval == Vector2Int.up && pos == PiecePositionEnum.Third) list.Add(neighbor);
                    if (interval == Vector2Int.down && pos == PiecePositionEnum.First) list.Add(neighbor);
                    break;

                case EdgeEnum.Right:
                    if (interval == Vector2Int.right &&
                        (pos == PiecePositionEnum.First || pos == PiecePositionEnum.Third)) list.Add(neighbor);
                    if (interval == Vector2Int.up && pos == PiecePositionEnum.Fourth) list.Add(neighbor);
                    if (interval == Vector2Int.down && pos == PiecePositionEnum.Second) list.Add(neighbor);
                    break;

                case EdgeEnum.Top:
                    if (interval == Vector2Int.left && pos == PiecePositionEnum.Second) list.Add(neighbor);
                    if (interval == Vector2Int.right && pos == PiecePositionEnum.First) list.Add(neighbor);
                    if (interval == Vector2Int.up &&
                        (pos == PiecePositionEnum.Third || pos == PiecePositionEnum.Fourth)) list.Add(neighbor);
                    break;

                case EdgeEnum.Bottom:
                    if (interval == Vector2Int.left && pos == PiecePositionEnum.Fourth) list.Add(neighbor);
                    if (interval == Vector2Int.right && pos == PiecePositionEnum.Third) list.Add(neighbor);
                    if (interval == Vector2Int.down &&
                        (pos == PiecePositionEnum.First || pos == PiecePositionEnum.Second)) list.Add(neighbor);
                    break;
            }

            return list;
        }

        private static List<InnerPiece> GetFacedFor2X2(Vector2Int interval, InnerPiece neighbor)
        {
            List<InnerPiece> list = new();
            var pos = neighbor.GetInnerPieceData().PiecePositionEnum;

            if (interval == Vector2Int.left && (pos == PiecePositionEnum.Second || pos == PiecePositionEnum.Fourth))
                list.Add(neighbor);
            if (interval == Vector2Int.right && (pos == PiecePositionEnum.First || pos == PiecePositionEnum.Third))
                list.Add(neighbor);
            if (interval == Vector2Int.up && (pos == PiecePositionEnum.Third || pos == PiecePositionEnum.Fourth))
                list.Add(neighbor);
            if (interval == Vector2Int.down && (pos == PiecePositionEnum.First || pos == PiecePositionEnum.Second))
                list.Add(neighbor);

            return list;
        }

        #endregion

        #region Faced Neighbor 2x1 Solutions

        private static List<InnerPiece> GetFacedFor1X1VersionTwo(Vector2Int interval, PiecePositionEnum myPos,
            InnerPiece neighbor)
        {
            List<InnerPiece> list = new();
            var pData = neighbor.GetInnerPieceData();

            if (myPos == PiecePositionEnum.First)
            {
                if (interval == Vector2Int.left &&
                    (pData.EdgeEnum == EdgeEnum.Right ||
                     pData.EdgeEnum == EdgeEnum.Top)) list.Add(neighbor);
                if (interval == Vector2Int.up &&
                    (pData.EdgeEnum == EdgeEnum.Bottom ||
                     pData.EdgeEnum == EdgeEnum.Left)) list.Add(neighbor);
            }
            else if (myPos == PiecePositionEnum.Second)
            {
                if (interval == Vector2Int.right &&
                    (pData.EdgeEnum == EdgeEnum.Left ||
                     pData.EdgeEnum == EdgeEnum.Top)) list.Add(neighbor);
                if (interval == Vector2Int.up &&
                    (pData.EdgeEnum == EdgeEnum.Bottom ||
                     pData.EdgeEnum == EdgeEnum.Right)) list.Add(neighbor);
            }
            else if (myPos == PiecePositionEnum.Third)
            {
                if (interval == Vector2Int.left &&
                    (pData.EdgeEnum == EdgeEnum.Bottom ||
                     pData.EdgeEnum == EdgeEnum.Right)) list.Add(neighbor);
                if (interval == Vector2Int.down &&
                    (pData.EdgeEnum == EdgeEnum.Left ||
                     pData.EdgeEnum == EdgeEnum.Top)) list.Add(neighbor);
            }
            else if (myPos == PiecePositionEnum.Fourth)
            {
                if (interval == Vector2Int.right &&
                    (pData.EdgeEnum == EdgeEnum.Bottom ||
                    pData.EdgeEnum == EdgeEnum.Left)) list.Add(neighbor);
                if (interval == Vector2Int.down &&
                    pData.EdgeEnum == EdgeEnum.Top ||
                    pData.EdgeEnum == EdgeEnum.Right) list.Add(neighbor);
            }

            return list;
        }

        private static List<InnerPiece> GetFacedFor2X1VersionTwo(Vector2Int interval, EdgeEnum myEdge,
            InnerPiece neighbor)
        {
            List<InnerPiece> list = new();
            var pData = neighbor.GetInnerPieceData();

            switch (myEdge)
            {
                case EdgeEnum.Left:
                    if (interval == Vector2Int.left && pData.EdgeEnum != EdgeEnum.Left) list.Add(neighbor);
                    if (interval == Vector2Int.up &&
                        (pData.EdgeEnum == EdgeEnum.Left || pData.EdgeEnum == EdgeEnum.Bottom)) list.Add(neighbor);
                    if (interval == Vector2Int.down &&
                        (pData.EdgeEnum == EdgeEnum.Left || pData.EdgeEnum == EdgeEnum.Top)) list.Add(neighbor);
                    break;

                case EdgeEnum.Right:
                    if (interval == Vector2Int.right && pData.EdgeEnum != EdgeEnum.Right) list.Add(neighbor);
                    if (interval == Vector2Int.up &&
                        (pData.EdgeEnum == EdgeEnum.Right || pData.EdgeEnum == EdgeEnum.Bottom)) list.Add(neighbor);
                    if (interval == Vector2Int.down &&
                        (pData.EdgeEnum == EdgeEnum.Right || pData.EdgeEnum == EdgeEnum.Top)) list.Add(neighbor);
                    break;

                case EdgeEnum.Top:
                    if (interval == Vector2Int.left &&
                        (pData.EdgeEnum == EdgeEnum.Right || pData.EdgeEnum == EdgeEnum.Top)) list.Add(neighbor);
                    if (interval == Vector2Int.right &&
                        (pData.EdgeEnum == EdgeEnum.Left || pData.EdgeEnum == EdgeEnum.Top)) list.Add(neighbor);
                    if (interval == Vector2Int.up && pData.EdgeEnum != EdgeEnum.Top) list.Add(neighbor);
                    break;

                case EdgeEnum.Bottom:
                    if (interval == Vector2Int.left &&
                        (pData.EdgeEnum == EdgeEnum.Right || pData.EdgeEnum == EdgeEnum.Bottom)) list.Add(neighbor);
                    if (interval == Vector2Int.right &&
                        (pData.EdgeEnum == EdgeEnum.Left || pData.EdgeEnum == EdgeEnum.Bottom)) list.Add(neighbor);
                    if (interval == Vector2Int.down && pData.EdgeEnum != EdgeEnum.Bottom) list.Add(neighbor);
                    break;
            }

            return list;
        }


        private static List<InnerPiece> GetFacedFor2X2VersionTwo(Vector2Int interval, InnerPiece neighbor)
        {
            List<InnerPiece> list = new();
            var pData = neighbor.GetInnerPieceData();

            if (interval == Vector2Int.left && pData.EdgeEnum != EdgeEnum.Left) list.Add(neighbor);
            if (interval == Vector2Int.right && pData.EdgeEnum != EdgeEnum.Right) list.Add(neighbor);
            if (interval == Vector2Int.up && pData.EdgeEnum != EdgeEnum.Top) list.Add(neighbor);
            if (interval == Vector2Int.down && pData.EdgeEnum != EdgeEnum.Bottom) list.Add(neighbor);

            return list;
        }

        #endregion

        #region Faced Neighbor 2x2 Solutions

        private static List<InnerPiece> GetFacedFor1X1VersionThree(Vector2Int interval, PiecePositionEnum myPos,
            InnerPiece neighbor)
        {
            List<InnerPiece> list = new();

            switch (myPos)
            {
                case PiecePositionEnum.First:
                {
                    if (interval == Vector2Int.left || interval == Vector2Int.up) list.Add(neighbor);
                    break;
                }
                case PiecePositionEnum.Second:
                {
                    if (interval == Vector2Int.right || interval == Vector2Int.up) list.Add(neighbor);
                    break;
                }
                case PiecePositionEnum.Third:
                {
                    if (interval == Vector2Int.left || interval == Vector2Int.down) list.Add(neighbor);
                    break;
                }
                case PiecePositionEnum.Fourth:
                {
                    if (interval == Vector2Int.right || interval == Vector2Int.down) list.Add(neighbor);
                    break;
                }
            }

            return list;
        }

        private static List<InnerPiece> GetFacedFor2X1VersionThree(Vector2Int interval, EdgeEnum myEdge,
            InnerPiece neighbor)
        {
            List<InnerPiece> list = new();

            switch (myEdge)
            {
                case EdgeEnum.Left:
                    if (interval != Vector2Int.right) list.Add(neighbor);
                    break;

                case EdgeEnum.Right:
                    if (interval != Vector2Int.left) list.Add(neighbor);
                    break;

                case EdgeEnum.Top:
                    if (interval != Vector2Int.down) list.Add(neighbor);
                    break;

                case EdgeEnum.Bottom:
                    if (interval != Vector2Int.up) list.Add(neighbor);
                    break;
            }

            return list;
        }

        #endregion
    }
}