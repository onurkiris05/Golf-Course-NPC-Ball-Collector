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
        protected int _rewardPoint;

        public abstract void Setup(int rewardPoint);
        public abstract int Collect();
    }
}
