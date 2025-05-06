using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EssentialManagers.Packages.GridManager.Scripts
{
    public class GridManager : MonoSingleton<GridManager>
    {
        [Header("Config")] [SerializeField] private bool autoGenerate = true;
        public int gridWidth = 6; // Width of the grid
        public int gridHeight = 6; // Height of the grid
        public float cellSpacing = 1f; // Spacing between cells
        [Header("References")] public GameObject cellPrefab; // Prefab for the cell
        [Header("Debug")] public List<CellController> gridPlan;

        #region Initialize

        protected override void Awake()
        {
            base.Awake();
            if (autoGenerate)
                CreateGrid();
            else
            {
                gridPlan = new List<CellController>();
                List<CellController> tempList = transform.GetComponentsInChildren<CellController>().ToList();

                // X-Z düzlemine göre sırala: önce Z'ye göre azalan (en alt satır en önce), sonra X'e göre artan (soldan sağa)
                var sortedCells = tempList.OrderBy(cell => cell.transform.position.z)
                    .ThenBy(cell => cell.transform.position.x)
                    .ToArray();

                int index = 0;
                for (int row = 0; row < gridHeight; row++)
                {
                    for (int col = 0; col < gridWidth; col++)
                    {
                        var cell = sortedCells[index];
                        if (cell == null) continue;
                        if (!cell.gameObject.activeInHierarchy) continue;

                        cell.Initialize(new Vector2Int(row, col)); // row = Z yönü, col = X yönü
                        gridPlan.Add(cell);
                        index++;
                    }
                }
            }
        }

        private void CreateGrid()
        {
            for (var x = 0; x < gridWidth; x++)
            {
                for (var y = 0; y < gridHeight; y++)
                {
                    Vector2Int coordinates = new Vector2Int(x, y);
                    GameObject cell = Instantiate(cellPrefab, new Vector3(x * cellSpacing, 0, y * cellSpacing),
                        cellPrefab.transform.rotation);
                    CellController cellController = cell.GetComponent<CellController>();
                    cellController.Initialize(coordinates);
                    cell.transform.parent = transform;
                    gridPlan.Add(cellController);
                }
            }
        }

        #endregion

        #region Helper Methods

        public List<CellController> GetCellsInSameColumn(CellController referenceCell)
        {
            if (referenceCell == null || gridPlan == null || gridPlan.Count == 0)
            {
                Debug.LogWarning("Reference cell or grid plan is invalid.");
                return new List<CellController>();
            }

            Vector2Int refCoords = referenceCell.GetCoordinates();
            int targetColumn = refCoords.y; // y = X axis (column)

            return gridPlan
                .Where(cell => cell.GetCoordinates().y == targetColumn)
                .OrderBy(cell => cell.GetCoordinates().x) // Optional: order from bottom to top
                .ToList();
        }

        public CellController GetClosestGridCell(Vector3 from)
        {
            if (gridPlan == null || gridPlan.Count == 0)
            {
                Debug.LogWarning("GridPlan list is empty or null!");
                return null;
            }

            CellController closestCellController = null;
            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < gridPlan.Count; i++)
            {
                CellController cellController = gridPlan[i];
                float distance = Vector3.Distance(cellController.transform.position, from);

                if (distance < closestDistance)
                {
                    closestCellController = cellController;
                    closestDistance = distance;
                }
            }

            return closestCellController;
        }

        public CellController GetGridCellByCoordinates(Vector2Int coordinates)
        {
            if (gridPlan == null || gridPlan.Count == 0)
            {
                return null;
            }

            for (int i = 0; i < gridPlan.Count; i++)
            {
                CellController cellController = gridPlan[i];
                if (cellController.GetCoordinates() == coordinates)
                {
                    return cellController;
                }
            }


            return null;
        }

        #endregion

        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                foreach (var c in gridPlan)
                {
                    c.RemoveHighlight();
                }
            }
        }


        public void HighlightColumn(List<CellController> newColumn, List<CellController> oldColumn)
        {
            if (oldColumn.Count != 0)
            {
                foreach (var c in oldColumn)
                {
                    c.RemoveHighlight();
                }
            }

            foreach (var c in newColumn)
            {
                c.SetHighlight();
            }
        }
    }
}