using System.Collections.Generic;
using EssentialManagers.Packages.GridManager.Scripts;

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
    }
}