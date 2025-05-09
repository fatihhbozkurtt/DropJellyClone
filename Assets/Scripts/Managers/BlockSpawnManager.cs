using System;
using System.Collections.Generic;
using Controllers;
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

        public void SpawnJellyBlock()
        {
            GameObject randomJellyBlock = blockPrefabs[Random.Range(0, blockPrefabs.Count)].gameObject;
            Instantiate(randomJellyBlock, spawnTransform.position, Quaternion.identity);
        }
    }
}