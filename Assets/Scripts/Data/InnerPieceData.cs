using UnityEngine.Serialization;

namespace Data
{
    [System.Serializable]
    public class InnerPieceData
    {
        public ColorEnum ColorEnum;
        [FormerlySerializedAs("DirectionEnum")] public PiecePositionEnum piecePositionEnum;
    }
}