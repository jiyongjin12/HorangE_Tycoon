using UnityEngine;

public class InteractionPoint : MonoBehaviour
{
    public float requiredTime = 2f;

    public bool pubool = false;

    private bool isInside = false;
    private float ETime;

    private void OnTriggerEnter(Collider other)
    {
        isInside = true;
        ETime = Time.time;
    }

    private void OnTriggerExit(Collider other)
    {
        isInside = false;
    }

}
