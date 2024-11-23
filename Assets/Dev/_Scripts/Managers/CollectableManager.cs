using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Game.Collectables;
using Game.Utilities;

namespace Game.Managers
{
    [System.Serializable]
    public class CollectableData
    {
        public string Name;
        [ShowAssetPreview]
        public Collectable_Base Prefab;
        public int SpawnCount;
        public int RewardPoint;
        public Transform[] PossibleSpawnPoints;
    }

    public class CollectableManager : StaticInstance<CollectableManager>
    {
        [SerializeField] CollectableData[] collectables;

        public List<Collectable_Base> SpawnedCollectables => _spawnedCollectables;
        private List<Collectable_Base> _spawnedCollectables = new();

        protected override void Awake()
        {
            base.Awake();
            SpawnCollectables();
        }

        private void SpawnCollectables()
        {
            foreach (CollectableData collectable in collectables)
            {
                var availableSpawnPoints = new List<Transform>(collectable.PossibleSpawnPoints);

                for (var i = 0; i < collectable.SpawnCount; i++)
                {
                    if (availableSpawnPoints.Count == 0)
                    {
                        Debug.Log($"Max spawn point count reached! {i}/{collectable.SpawnCount} created.");
                        break;
                    }

                    var randomIndex = Random.Range(0, availableSpawnPoints.Count);
                    var instance = Instantiate(collectable.Prefab, availableSpawnPoints[randomIndex].position, Quaternion.identity);
                    _spawnedCollectables.Add(instance);
                    collectable.Prefab.Setup(collectable.RewardPoint);
                    availableSpawnPoints.RemoveAt(randomIndex);
                }
            }
        }
    }
}