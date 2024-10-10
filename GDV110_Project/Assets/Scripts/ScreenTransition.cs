using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    public Material screenTransitionMat;
    public bool callOnStart;
    public bool onStartCallDirection;
    public float speed;

    private void Start()
    {
        if(!callOnStart)
        {
            return;
        }

        StartCoroutine(StartScreenTransition(onStartCallDirection));
    }

    public IEnumerator StartScreenTransition(bool direction)
    {
        float time = 1f / speed;
        float t = time;

        while (t > 0)
        {
            t -= Time.deltaTime;

            if(direction)
            {
                screenTransitionMat.SetFloat("_Distance", 1f - (t / time));
            }
            else
            {
                screenTransitionMat.SetFloat("_Distance", (t / time));
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
