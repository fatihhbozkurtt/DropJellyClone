using System.Collections.Generic;
using Data;
using DG.Tweening;
using EssentialManagers.Packages.GridManager.Scripts;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class MovePerformer : MonoBehaviour
    {
        [Header("Config")] [SerializeField] private float moveLimit = 3f;

        [Header("Debug")] private JellyBlock _jellyBlock;
        private Camera _mainCamera;
        private float _fixedY;
        private float _fixedZ;
        private float _startX;
        private int _lastColumnIndex = -1;
        private GridManager _gridManager;
        private List<CellController> _prevColumn = new();
        [SerializeField] private List<CellController> closestColumn = new();

        void Start()
        {
            _jellyBlock = GetComponent<JellyBlock>();
            _gridManager = GridManager.instance;
            _mainCamera = Camera.main;
            _fixedY = transform.position.y;
            _fixedZ = transform.position.z;
            _startX = transform.position.x;
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                HandleMouseDrag();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                HandleMouseUp();
            }
        }

        #region Mouse Functions

        // ReSharper disable Unity.PerformanceAnalysis
        private void HandleMouseDrag()
        {
            // Track finger position in world space
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = Mathf.Abs(_mainCamera.transform.position.z - transform.position.z);
            Vector3 worldPos = _mainCamera.ScreenToWorldPoint(mouseScreenPos);

            // Clamp X movement
            float targetX = Mathf.Clamp(worldPos.x, _startX - moveLimit, _startX + moveLimit);
            transform.position = new Vector3(targetX, _fixedY, _fixedZ);

            // Get the closest cell to current position
            CellController closestCell = _gridManager.GetClosestGridCell(transform.position);
            if (closestCell == null) return;

            int currentColumnIndex = closestCell.GetCoordinates().x;

            // Only update if column has changed
            if (currentColumnIndex != _lastColumnIndex)
            {
                _lastColumnIndex = currentColumnIndex;
                _prevColumn = closestColumn;
                closestColumn = _gridManager.GetCellsInSameColumn(closestCell);

                _gridManager.HighlightColumn(closestColumn, _prevColumn);
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void HandleMouseUp()
        {
            CellController emptyCell = DataExtensions.GetEmptyCell(closestColumn);
            closestColumn.Clear();

            if (emptyCell == null) return;

            emptyCell.SetOccupied(_jellyBlock);
            _jellyBlock.SetCell(emptyCell);

            transform.position = new Vector3(emptyCell.transform.position.x, _fixedY, _fixedZ);
            transform.DOMoveZ(emptyCell.transform.position.z, 1f)
                .SetEase(Ease.OutBounce)
                .OnComplete(() =>
                {
                    _jellyBlock.TriggerMatchChecking();
                    BlockSpawnManager.instance.SpawnJellyBlock();
                    GetComponent<MovePerformer>().enabled = false;
                });
        }

        #endregion
    }
}