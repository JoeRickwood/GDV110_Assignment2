using System.Collections;
using UnityEngine;

public class ObjectShake : MonoBehaviour
{
    public bool isShaking;
    public float shakeIntensity;
    public float shakeSpeed;

    float t;

    public void Update()
    {
        if(isShaking)
        {
            t += Time.deltaTime * shakeSpeed;

            transform.localRotation = Quaternion.Euler(0f, 0f, (Mathf.PerlinNoise(t, t) - 0.5f) * 2f);
            transform.localPosition = new Vector3((Mathf.PerlinNoise(t + 2321f, t - 37291f) - 0.5f) * shakeIntensity, (Mathf.PerlinNoise(t + 21f, t - 371f) - 0.5f) * shakeIntensity, 0f);
        }
        else
        {
            t = 0;
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 2f);
        }
    }

    public void StartShake(float time)
    {
        StartCoroutine(ShakeOverTime(time));
    }

    public IEnumerator ShakeOverTime(float t)
    {
        StopCoroutine("ShakeOverTime");
        isShaking = true;

        yield return new WaitForSeconds(t);

        isShaking = false;
    }
}
