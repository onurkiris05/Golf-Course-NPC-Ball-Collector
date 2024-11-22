using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

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

public class CollectableManager : MonoBehaviour
{
    [SerializeField] CollectableData[] collectables;

    private void Start() => SpawnCollectables();

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
                Instantiate(collectable.Prefab, availableSpawnPoints[randomIndex].position, Quaternion.identity);
                collectable.Prefab.Setup(collectable.RewardPoint);

                availableSpawnPoints.RemoveAt(randomIndex);
            }
        }
    }
}
