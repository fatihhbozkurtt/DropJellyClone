using System;
using System.Collections.Generic;
using System.Linq;
using Data.Enums;
using EssentialManagers.Packages.GridManager.Scripts;
using So;
using UnityEngine;

namespace Data
{
    public static class DataExtensions
    {
        public static CellController GetEmptyCell(List<CellController> referenceList)
        {
            foreach (var cell in referenceList)
            {
                if (cell == null) continue;
                if (cell.isOccupied) continue;
                return cell;
            }
            
            return null;
        }
        
        public static ColorMaterialData GetMaterialByColorEnum(ColorEnum colorEnum)
        {
            ColorEnumSo generalList = Resources.Load<ColorEnumSo>($"ColorMaterialDataList");

            var data = generalList?.ColorMaterialDataList.FirstOrDefault(x => x.ColorEnum == colorEnum);
            return data;
        }
        
        public static ColorEnum GetRandomColorEnum()
        {
            Array values = Enum.GetValues(typeof(ColorEnum));
            int randomIndex = UnityEngine.Random.Range(0, values.Length);
            return (ColorEnum)values.GetValue(randomIndex);
        }
    }
}