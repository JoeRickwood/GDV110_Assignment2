using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomiseAnimationOffset : MonoBehaviour
{
    public string animHashName;

    private void Start()
    {
        GetComponent<Animator>().Play(animHashName, 0, Random.Range(0f, 1f));
    }
}
