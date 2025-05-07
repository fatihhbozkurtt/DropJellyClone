using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace Managers
{
    public class BlockSpawnManager : MonoSingleton<BlockSpawnManager>
    {
        [Header("References")] [SerializeField]
        private List<JellyBlock> blockPrefabs;

        [SerializeField] private Transform spawnTransform;

        public void SpawnJellyBlock()
        {
            GameObject randomJellyBlock = blockPrefabs[Random.Range(0, blockPrefabs.Count)].gameObject;
            Instantiate(randomJellyBlock, spawnTransform.position, Quaternion.identity);
        }
    }
}