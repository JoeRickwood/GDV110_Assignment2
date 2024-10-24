using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    public float t;
    public float speed = 2f;
    public Vector3 axis;

    private void Start()
    {
        t = Random.Range(0f, 100f);  
    }


    private void Update()
    {
        t += Time.deltaTime * speed;

        transform.rotation = Quaternion.Euler(axis * t);
    }
}
