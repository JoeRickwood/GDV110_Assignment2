using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float Time;

    private void Start()
    {
        Destroy(gameObject, Time);
    }
}
