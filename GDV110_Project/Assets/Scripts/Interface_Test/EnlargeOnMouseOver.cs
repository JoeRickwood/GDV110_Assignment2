using UnityEngine;
using UnityEngine.EventSystems;

public class EnlargeOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool mouseOver;
    float timer;

    [SerializeField] private AnimationCurve startCurve;
    [SerializeField] private AnimationCurve endCurve;
    [SerializeField] private float speed;

    public float scalar;

    public AudioSource clickSource;

    private void Update()
    {
        timer += Time.unscaledDeltaTime * speed;
        timer = Mathf.Clamp01(timer);
        if(mouseOver)
        {
            float scale = (startCurve.Evaluate(timer) * scalar) + 1f;
            transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            float scale = (endCurve.Evaluate(timer) * scalar) + 1f;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    public void SetState(bool stateChange)
    {
        mouseOver = stateChange;
        timer = 0f;

        if(stateChange == true && clickSource != null)
        {
            clickSource.Play();
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
