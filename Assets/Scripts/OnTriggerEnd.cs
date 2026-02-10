using System;
using UnityEngine;

public class OnTriggerEnd : MonoBehaviour
{
    public static event Action OnPlayerCollision;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerCollision?.Invoke();
        }
    }
}
