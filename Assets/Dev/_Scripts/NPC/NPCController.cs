using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable") && other.TryGetComponent<Collectable_Base>(out var collectable))
        {
            collectable.Collect();
        }
    }

}
