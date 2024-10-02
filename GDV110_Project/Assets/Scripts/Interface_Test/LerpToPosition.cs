using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToPosition : MonoBehaviour
{
    public Vector3 target;
    public float speed = 5f;
    void Update()
    {
        GetComponent<RectTransform>().position = Vector3.Lerp(GetComponent<RectTransform>().position, target, Time.deltaTime * speed);
    }
}
