using System.Collections.Generic;
using UnityEngine;

namespace So
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
    public class LevelDataSo : ScriptableObject
    {
        public List<GameObject> LevelGridPrefabs;
    }
}