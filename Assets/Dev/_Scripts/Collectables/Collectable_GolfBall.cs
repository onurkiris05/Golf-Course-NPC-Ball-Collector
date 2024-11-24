using System.Collections;
using UnityEngine;

namespace Game.Collectables
{
    public class Collectable_GolfBall : Collectable_Base
    {
        [Header("Animation Settings")]
        [SerializeField] Transform model;
        [SerializeField] float offsetYValue = 0.5f;
        [SerializeField] float speed = 1f;

        private MeshRenderer _meshRenderer;
        private Collider _collider;
        private Coroutine _animationCoroutine;

        private void Awake()
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            _collider = GetComponent<Collider>();
        }

        private void Start()
        {
            _animationCoroutine = StartCoroutine(AnimateCollectable());
        }

        public override void Setup(int rewardPoint)
        {
            _rewardPoint = rewardPoint;
        }

        public override int Collect()
        {
            if (_animationCoroutine != null)
                StopCoroutine(_animationCoroutine);

            SetCollectable(false);
            Debug.Log($"{name} Triggered!");
            return RewardPoint;
        }

        private void SetCollectable(bool state)
        {
            _meshRenderer.enabled = state;
            _collider.enabled = state;
        }

        IEnumerator AnimateCollectable()
        {
            var startPos = model.position;
            while (true)
            {
                var sinValue = (Mathf.Sin(Time.time * speed) + 1) / 2;
                var newY = startPos.y + sinValue * offsetYValue;

                model.position = new Vector3(model.position.x, newY, model.position.z);
                yield return null;
            }
        }
    }
}
