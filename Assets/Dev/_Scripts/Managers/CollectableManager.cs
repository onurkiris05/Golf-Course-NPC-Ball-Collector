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
        [Header("Components")]
        [SerializeField] CollectableData[] collectables;

        public List<Collectable_Base> SpawnedCollectables => _spawnedCollectables;
        private List<Collectable_Base> _spawnedCollectables = new();

        #region UNITY EVENTS
        protected override void Awake()
        {
            base.Awake();
            AdjustSpawnPoints();
            SpawnCollectables();
        }
        #endregion

        #region PRIVATE METHODS
        private void AdjustSpawnPoints()
        {
            foreach (var collectable in collectables)
            {
                foreach (var spawnPoint in collectable.PossibleSpawnPoints)
                {
                    if (Terrain.activeTerrain != null)
                    {
                        var position = spawnPoint.position;
                        var terrainHeight = Terrain.activeTerrain.SampleHeight(position);
                        position.y = terrainHeight + 0.5f;
                        spawnPoint.position = position;
                    }
                }
            }
        }

        private void SpawnCollectables()
        {
            foreach (var collectable in collectables)
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
                    instance.Setup(collectable.RewardPoint);
                    _spawnedCollectables.Add(instance);
                    availableSpawnPoints.RemoveAt(randomIndex);
                }
            }
        }
        #endregion
    }
}