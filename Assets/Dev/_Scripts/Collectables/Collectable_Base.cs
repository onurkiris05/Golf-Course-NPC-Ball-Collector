using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Collectables
{
    public abstract class Collectable_Base : MonoBehaviour
    {
        public int RewardPoint => _rewardPoint;
        [ShowNonSerializedField]
        private int _rewardPoint;

        public virtual void Setup(int rewardPoint)
        {
            _rewardPoint = rewardPoint;
        }

        public abstract int Collect();
    }
}
