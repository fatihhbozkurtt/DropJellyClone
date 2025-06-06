using System.Collections.Generic;
using Controllers;
using Data;
using Data.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class JellySpawnManager : MonoSingleton<JellySpawnManager>
    {
        [Header("References")] [SerializeField]
        private List<JellyBlock> blockPrefabs;

        [SerializeField] private Transform spawnTransform;

        protected override void Awake()
        {
            base.Awake();

            SpawnJellyBlock();
        }

        private void Start()
        {
            MatchCheckerManager.instance.AddAllMatchesCompletedListener(SpawnJellyBlock);
            LevelLoadManager.instance.NewLevelLoadedEvent += SpawnJellyBlock;
        }


        public void SpawnJellyBlock()
        {
            JellyBlock randomJellyBlock = blockPrefabs[Random.Range(0, blockPrefabs.Count)];
            JellyBlock clone = Instantiate(randomJellyBlock, spawnTransform.position, Quaternion.identity);

            int pieceCount = clone.GetInnerPieces(true).Count;
            List<ColorEnum> colorPool = DataExtensions.GetUniqueRandomColors(pieceCount);

            clone.InitializeRuntime(colorPool);
        }
    }
}