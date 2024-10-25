using UnityEngine;

public class RandomizeScale : MonoBehaviour
{
    public float minScale;
    public float maxScale;

    private void Start()
    {
        transform.localScale = Vector3.one * Random.Range(minScale, maxScale);
    }

}
