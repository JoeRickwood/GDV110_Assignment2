using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class EnlargeOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool mouseOver;
    float timer;

    [SerializeField] private AnimationCurve startCurve;
    [SerializeField] private AnimationCurve endCurve;
    [SerializeField] private float speed;
    public float maxScale;
    public float minScale;

    public bool isActive = true;

    public SoundEffect soundType;

    private void Update()
    {
        if(isActive == false)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * minScale, Time.deltaTime * 10f);
            return;
        }

        timer += Time.unscaledDeltaTime * speed;
        timer = Mathf.Clamp01(timer);
        if(mouseOver)
        {
            float scale = Mathf.Lerp(minScale, maxScale, startCurve.Evaluate(timer)); 
            transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            float scale = Mathf.Lerp(minScale, maxScale, endCurve.Evaluate(timer));
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    public void SetState(bool stateChange)
    {
        mouseOver = stateChange;
        timer = 0f;

        if(stateChange == true && isActive == true)
        {
            if(GameManager.Instance == null)
            {
                return;
            }

            GameManager.Instance.PlaySFX(soundType);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetState(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetState(false);
    }
}
