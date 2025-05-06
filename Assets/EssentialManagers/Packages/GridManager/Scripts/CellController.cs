using System.Collections.Generic;
using UnityEngine;

namespace EssentialManagers.Packages.GridManager.Scripts
{
    public class CellController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform cellGround;

        [SerializeField] private GameObject occupierObjectPrefab;
        [SerializeField] private GameObject highlightMesh;
        [SerializeField] private GameObject standartMesh;
    

        [Header("Debug")] public bool isPickable;
        public bool isOccupied;
        [SerializeField] private GameObject spawnedOccupierObj;
        [SerializeField] Vector2Int coordinates;
        public List<CellController> neighbours;

        private void Start()
        {
            name = coordinates.ToString();

            neighbours = GetNeighbors();
        }

        public void Initialize(Vector2Int initCoords)
        {
            coordinates = initCoords;
        }

        public void SetHighlight()
        {
            highlightMesh.SetActive(true);
            standartMesh.SetActive(false);
        }

        public void RemoveHighlight()
        {
            highlightMesh.SetActive(false);
            standartMesh.SetActive(true);
        }
    
        #region GETTERS & SETTERS

        public void SetOccupied(GameObject _csh)
        {
            spawnedOccupierObj = _csh;
            isOccupied = true;
        }

        public void SetFree()
        {
            spawnedOccupierObj = null;
            isOccupied = false;
        }

        public GameObject GetOccupierObject()
        {
            return spawnedOccupierObj;
        }

        public Vector2Int GetCoordinates()
        {
            return coordinates;
        }

        private List<CellController> GetNeighbors()
        {
            List<CellController> gridCells = GridManager.instance.gridPlan;
            List<CellController> neighbors = new();

            // Direction vectors for 8 directions (including diagonals)
            int[] dx = { 1, 1, 0, -1, -1, -1, 0, 1 };
            int[] dz = { 0, 1, 1, 1, 0, -1, -1, -1 };

            for (int i = 0; i < dx.Length; i++)
            {
                Vector2Int neighborCoordinates = coordinates + new Vector2Int(dx[i], dz[i]);
                CellController neighbor = gridCells.Find(cell => cell.coordinates == neighborCoordinates);

                if (neighbor != null)
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

        #endregion
    }
}