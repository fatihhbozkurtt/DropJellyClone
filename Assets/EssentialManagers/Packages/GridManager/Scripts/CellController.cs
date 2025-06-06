using System.Collections.Generic;
using Controllers;
using Data;
using DG.Tweening;
using UnityEngine;

namespace EssentialManagers.Packages.GridManager.Scripts
{
    public class CellController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private GameObject occupierObjectPrefab;

        [SerializeField] private GameObject highlightMesh;
        [SerializeField] private GameObject standartMesh;
        
        [Header("Debug")] public bool isOccupied;
        [SerializeField] private JellyBlock occupantJellyBlock;
        [SerializeField] Vector2Int coordinates;
        [SerializeField] List<CellController> neighbours;
        
        [Header("LEVEL DESIGN DATA")]
        [SerializeField] private List<InnerPieceData> spawnablePieceDataList;

        private void Start()
        {
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce).From(Vector3.zero);
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

        public void SetOccupied(JellyBlock jb)
        {
            occupantJellyBlock = jb;
            isOccupied = true;
        }

        public void SetFree()
        {
            occupantJellyBlock = null;
            isOccupied = false;
        }

        public List<InnerPieceData> GetSpawnablePieceDataList()
        {
            return spawnablePieceDataList;
        }

        public JellyBlock GetOccupantJB()
        {
            return occupantJellyBlock;
        }

        public Vector2Int GetCoordinates()
        {
            return coordinates;
        }

        public List<CellController> GetNeighbors()
        {
            List<CellController> gridCells = GridManager.instance.gridPlan;
            List<CellController> neighbors = new();

            // Direction vectors for 4 cardinal directions: Right, Up, Left, Down
            int[] dx = { 1, 0, -1, 0 };
            int[] dz = { 0, 1, 0, -1 };

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

[System.Serializable]
public class JellySpawnData
{
    
}