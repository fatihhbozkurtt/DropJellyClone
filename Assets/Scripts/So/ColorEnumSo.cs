using System.Collections.Generic;
using Data;
using UnityEngine;

namespace So
{
    [CreateAssetMenu(fileName = "ColorData", menuName = "ScriptableObjects/ColorData", order = 1)]
    public class ColorEnumSo: ScriptableObject
    {
        public List<ColorMaterialData> ColorMaterialDataList;
    }
    
    [System.Serializable]
    public class ColorMaterialData
    {
        public ColorEnum ColorEnum;
        public Material Material;
    }
}