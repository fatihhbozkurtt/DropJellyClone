using System;
using System.Collections.Generic;
using Data;
using Data.Enums;
using DG.Tweening;
using EssentialManagers.Packages.GridManager.Scripts;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class InnerPiece : MonoBehaviour
    {
        [Header("Config")] [SerializeField] InnerPieceData innerPieceData;

        [Header("References")] [SerializeField]
        MeshRenderer meshRenderer;

        [Header("Debug")] [SerializeField] JellyBlock parentJellyBlock;
        public bool IsMatched { get; private set; } 

        private void Awake()
        {
            parentJellyBlock = transform.parent.GetComponent<JellyBlock>();
            parentJellyBlock.PieceRemovedEvent += OnPieceRemoved;

            #region Namification

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

            #endregion
        }

        public void InitializeRuntime(ColorEnum colorEnum)
        {
            innerPieceData.ColorEnum = colorEnum;
            SetMaterialColor();
        }

        public void InitializeEditor(InnerPieceData data)
        {
            innerPieceData = data;
            Vector3 scale = meshRenderer.transform.localScale;
            Vector3 posOffset = Vector3.zero;
            float generalOffset = ScaleModifier.GeneralPosOffset();
            
            // ASSIGN SCALE AND POSITION    
            if (innerPieceData.ScaleType == ScaleType.OneByOne)
            {
                switch (data.PiecePositionEnum)
                {
                    case PiecePositionEnum.None:
                        break;
                    case PiecePositionEnum.First:
                        posOffset = new Vector3(-generalOffset, 0, generalOffset);
                        break;
                    case PiecePositionEnum.Second:
                        posOffset = new Vector3(generalOffset, 0, generalOffset);
                        break;
                    case PiecePositionEnum.Third:
                        posOffset = new Vector3(-generalOffset, 0, -generalOffset);
                        break;
                    case PiecePositionEnum.Fourth:
                        posOffset = new Vector3(generalOffset, 0, -generalOffset);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (innerPieceData.ScaleType == ScaleType.TwoByOne)
            {
                switch (data.EdgeEnum)
                {
                    case EdgeEnum.None:
                        break;
                    case EdgeEnum.Left:
                        scale = ScaleModifier.GetVerticalScale();
                        posOffset = new Vector3(-generalOffset, 0, 0);
                        break;
                    case EdgeEnum.Right:
                        scale = ScaleModifier.GetVerticalScale();
                        posOffset = new Vector3(generalOffset, 0, 0);
                        break;
                    case EdgeEnum.Top:
                        scale = ScaleModifier.GetHorizontalScale();
                        posOffset = new Vector3(0, 0, generalOffset);
                        break;
                    case EdgeEnum.Bottom:
                        scale = ScaleModifier.GetHorizontalScale();
                        posOffset = new Vector3(0, 0, -generalOffset);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (innerPieceData.ScaleType == ScaleType.TwoByTwo)
            {
                scale = ScaleModifier.GetFullScale();
            }
 
            meshRenderer.transform.localScale = scale;
            meshRenderer.transform.localPosition += posOffset;
            SetMaterialColor();
        }

        private void OnPieceRemoved(InnerPieceData removedPieceData)
        {
            // re-modify your scale, position, and rotation
            ScaleData scaleData = ScaleModifier.GetModifiedScale(removedPieceData, this, parentJellyBlock);

            if (scaleData.ModifiedPieceData == null) return;

            // assign piece position if the scaleType is 2x2
            Vector3 newPos = scaleData.ModifiedPieceData.ScaleType == ScaleType.TwoByTwo
                ? Vector3.zero
                : transform.localPosition;
            if (newPos != transform.localPosition) transform.DOLocalMove(newPos, 0.2f);

            innerPieceData = scaleData.ModifiedPieceData;

            Sequence sequence = DOTween.Sequence();
            sequence.Join(meshRenderer.transform.DOScale(scaleData.Scale, 0.25f));
            sequence.Join(meshRenderer.transform.DOLocalMove(scaleData.Pos, 0.25f));
            sequence.OnComplete(() => parentJellyBlock.TriggerMatchChecking());
        }

        public void CheckMatches()
        {
            List<JellyBlock> neighborJellyBlocks = new List<JellyBlock>();
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

            List<InnerPiece> facedPieces = FacedPieceChecker.GetFacedPieces(
                innerPieceData,
                neighborJellyBlocks,
                myCoord);

            #endregion

            #region Check For Color Matches

            bool isMatched = false;
            foreach (var piece in facedPieces)
            {
                if (piece.GetInnerPieceData().ColorEnum != innerPieceData.ColorEnum) continue;

                piece.OnMatchOccuredDestroySelf();
                isMatched = true;
            }

            if (isMatched)
            {
                MatchCheckerManager.instance.RegisterMatchCheck();
                OnMatchOccuredDestroySelf(true);
            }

            #endregion
        }

        private void OnMatchOccuredDestroySelf(bool unregister = false)
        {
            IsMatched = true;
            transform.DOScale(Vector3.zero, .5f).OnComplete(() =>
            {
                parentJellyBlock.PieceRemovedEvent -= OnPieceRemoved;
                parentJellyBlock.RemoveInnerPiece(this);
                if (unregister) MatchCheckerManager.instance.UnregisterMatchCheck();
                Destroy(gameObject);
            });
        }

        #region Getters/Setter

        public InnerPieceData GetInnerPieceData()
        {
            return innerPieceData;
        }

        public Vector3 GetMeshPos()
        {
            return meshRenderer.transform.localPosition;
        }
        private void SetMaterialColor()
        {
            meshRenderer.material = DataExtensions.GetMaterialByColorEnum(innerPieceData.ColorEnum).Material;
        }
        #endregion
        
    }
}