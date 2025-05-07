using Controllers;
using UnityEngine;

namespace Managers
{
    public class BlockSpawnManager : MonoSingleton<BlockSpawnManager>
    {
         [Header("References")]
         [SerializeField] private JellyBlock blockPrefab;
         [SerializeField] private Transform spawnTransform;

         public void SpawnJellyBlock()
         {
             JellyBlock jellyBlock = Instantiate(blockPrefab, spawnTransform);
         }
         
    }
}
