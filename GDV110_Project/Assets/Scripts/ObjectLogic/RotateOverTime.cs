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

        transform.rotation = Quaternion.Euler(axis.x > 0 ? Mathf.Sin(t) * axis.x : transform.eulerAngles.x, 
                        axis.y > 0 ? Mathf.Sin(t) * axis.y : transform.eulerAngles.y,
                        axis.z > 0 ? Mathf.Sin(t) * axis.z : transform.eulerAngles.z);
    }
}
