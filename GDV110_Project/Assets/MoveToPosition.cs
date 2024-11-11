using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum EasingMode
{
    Bounce,
    Smooth
}

public class MoveToPosition : MonoBehaviour
{
    Vector3 startPos;
    public Vector3 finalPosition;
    public float timer;
    public EasingMode easeMode;

    float startT;

    public bool isActive;

    private void Start()
    {
        startT = timer;
        timer = 0;
        startPos = transform.GetComponent<RectTransform>().anchoredPosition;
    }
    private void Update()
    {
        if(!isActive)
        {
            return;
        }

        timer += Time.deltaTime;

        if(easeMode == EasingMode.Bounce)
        {
            transform.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(startPos, finalPosition, easeOutBounce(timer / startT));
        }
        else
        {
            transform.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(Mathf.Lerp(startPos.x, finalPosition.x, easeOutBack(timer / startT)), Mathf.Lerp(startPos.y, finalPosition.y, easeOutBack(timer / startT)), Mathf.Lerp(startPos.z, finalPosition.z, easeOutBack(timer / startT)));
        }
        
    }

    float easeOutBounce(float x)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;

        if (x < 1 / d1)
        {
            return n1 * x * x;
        }
        else if (x < 2 / d1)
        {
            return n1 * (x -= 1.5f / d1) * x + 0.75f;
        }
        else if (x < 2.5 / d1)
        {
            return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        }
        else
        {
            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
    }

    float easeOutBack(float x) 
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;

        return 1f + c3 * Mathf.Pow(x - 1f, 3f) + c1* Mathf.Pow(x - 1f, 2f);
    }
}
