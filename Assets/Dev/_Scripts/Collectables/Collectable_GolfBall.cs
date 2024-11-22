using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collectable_GolfBall : Collectable_Base
{
    [Header("Animation Settings")]
    [SerializeField] float offsetYValue = 0.5f;
    [SerializeField] float speed = 1f;
    private Coroutine _animationCoroutine;

    protected override void Start()
    {
        base.Start();
        _animationCoroutine = StartCoroutine(AnimateCollectable());
    }

    IEnumerator AnimateCollectable()
    {
        var startPos = transform.position;
        while (true)
        {
            var sinValue = (Mathf.Sin(Time.time * speed) + 1) / 2;
            var newY = startPos.y + sinValue * offsetYValue;

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            yield return null;
        }
    }

    public override void Collect()
    {
        if (_animationCoroutine != null)
            StopCoroutine(_animationCoroutine);

        Debug.Log("It worked!");
    }
}
