using System;
using UnityEngine;

public class VictoryPoint : MonoBehaviour
{
    public static event Action OnPlayerEnter;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerEnter?.Invoke();
        }
    }
}