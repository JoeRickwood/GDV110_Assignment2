using UnityEngine;

public class OpenAnimation : MonoBehaviour
{
    [Header("Open Animation")]
    public AnimationCurve openCurve;
    public RectTransform mainRect;
    float t;
    public float speed;
    [HideInInspector] public bool dir;

    private void Start()
    {
        dir = false;
        t = 1f;
    }

    public void Open(bool _Dir)
    {
        dir = _Dir;
        t = 0f;
    }

    private void Update()
    {
        t += Time.deltaTime * speed;
        t = Mathf.Clamp01(t);

        if (dir) //Settings Menu Open
        {
            mainRect.anchoredPosition = Vector3.Lerp(new Vector3(0, -mainRect.sizeDelta.y, 0), new Vector3(0, mainRect.sizeDelta.y / 2, 0), openCurve.Evaluate(t));
            mainRect.gameObject.SetActive(true);
        }
        else //Settings Menu Closed
        {
            mainRect.anchoredPosition = Vector3.Lerp(new Vector3(0, mainRect.sizeDelta.y / 2, 0), new Vector3(0, -mainRect.sizeDelta.y, 0), openCurve.Evaluate(t));
            if (t >= 1f)
            {
                mainRect.gameObject.SetActive(false);
            }
        }
    }
}
