using System;
using UnityEngine;

public class InteractionPoint : MonoBehaviour
{
    public event Action<Collider> OnEntered;
    public event Action<Collider> OnExited;

    public bool Check = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnEntered?.Invoke(other);
            Check = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnExited?.Invoke(other);
            Check = false;
        }
    }
}
