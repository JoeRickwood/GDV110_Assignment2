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
        startPos = transform.localPosition;
    }

    private void Update()
    {
        t += Time.deltaTime * speed;

        transform.localPosition = startPos + (new Vector3(Mathf.Sin(t) * axis.x, Mathf.Sin(t) * axis.y, Mathf.Sin(t) * axis.z) * scale);
    }

}
