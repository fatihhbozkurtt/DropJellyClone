using So;
using UnityEngine;

namespace Managers
{
    public class LevelLoadManager : MonoSingleton<LevelLoadManager>
    {
        public event System.Action NewLevelLoadedEvent; 
        
        private const string ResourcePath = "LevelData";
        private LevelDataSo _levelData;
        private int _currentLevelIndex;
        private GameObject _currentLevelInstance;

        protected override void Awake()
        {
            base.Awake();

            // Load level data from Resources
            _levelData = Resources.Load<LevelDataSo>(ResourcePath);

            if (_levelData == null)
            {
                Debug.LogError("LevelDataSo not found in Resources/" + ResourcePath);
                return;
            }

            LoadCurrentLevel();
        }

        private void LoadCurrentLevel()
        {
            if (_currentLevelInstance != null)
                DestroyImmediate(_currentLevelInstance);

            if (_currentLevelIndex >= _levelData.LevelGridPrefabs.Count)
            {
                Debug.Log("All levels completed.");
                _currentLevelIndex = 0;
            }

            if (_currentLevelIndex < 0) _currentLevelIndex = _levelData.LevelGridPrefabs.Count - 1;
            
            NewLevelLoadedEvent?.Invoke();

            GameObject prefab = _levelData.LevelGridPrefabs[_currentLevelIndex];
            _currentLevelInstance = Instantiate(prefab);
        }

        public void LoadNextLevel()
        {
            _currentLevelIndex++;
            LoadCurrentLevel();
        }

        public void LoadPreviousLevel()
        {
            _currentLevelIndex--;
            LoadCurrentLevel();
        }

        public void ResetLevels()
        {
            _currentLevelIndex = 0;
            LoadCurrentLevel();
        }
    }
}