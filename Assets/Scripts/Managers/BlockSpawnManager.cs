using System.Collections.Generic;
using Controllers;
using Data;
using Data.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class BlockSpawnManager : MonoSingleton<BlockSpawnManager>
    {
        [Header("References")] [SerializeField]
        private List<JellyBlock> blockPrefabs;


        [SerializeField] private Transform spawnTransform;


        private void Start()
        {
            // MatchCheckerManager.instance.AddAllMatchesCompletedListener(SpawnJellyBlock);
        }

        protected override void Awake()
        {
            base.Awake();
            
            SpawnJellyBlock();
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