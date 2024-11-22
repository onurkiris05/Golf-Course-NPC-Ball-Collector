using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public abstract class Collectable_Base : MonoBehaviour
{
    public int RewardPoint => _rewardPoint;
    [ShowNonSerializedField]
    private int _rewardPoint;

    protected virtual void Start()
    {
    }

    public virtual void Setup(int rewardPoint)
    {
        _rewardPoint = rewardPoint;
    }

    public abstract void Collect();
}
