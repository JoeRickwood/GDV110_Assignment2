using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSin : MonoBehaviour
{
    public float speed;
    public float scale;

    public Vector3 axis;

    float t;

    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        t += Time.deltaTime * speed;

        transform.position = startPos + (new Vector3(Mathf.Sin(t) * axis.x, Mathf.Sin(t) * axis.y, Mathf.Sin(t) * axis.z) * scale);
    }

}
