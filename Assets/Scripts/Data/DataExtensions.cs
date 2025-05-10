using System;
using System.Collections.Generic;
using System.Linq;
using Data.Enums;
using EssentialManagers.Packages.GridManager.Scripts;
using So;
using UnityEngine;
using Random = System.Random;

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

        private static Random rng = new();

        public static List<ColorEnum> GetUniqueRandomColors(int count)
        {
            var allColors = Enum.GetValues(typeof(ColorEnum)).Cast<ColorEnum>().ToList();

            if (count > allColors.Count)
                throw new ArgumentException(
                    $"İstenen {count} adet değer, mevcut {allColors.Count} enum sayısından fazla olamaz.");

            return allColors.OrderBy(x => rng.Next()).Take(count).ToList();
        }

        public static Vector3 GetAveragePosition(List<Vector3> positions)
        {
            if (positions.Count < 1)
            {
                Debug.LogWarning("Average position cannot be calculated.");
                return Vector3.zero;
            }

            Vector3 total = Vector3.zero;
            foreach (var t in positions)
            {
                total += t;
            }

            return total / positions.Count;
        }
    }
}