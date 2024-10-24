using UnityEngine;
using UnityEngine.UI;

public class ActivationIndicator : MonoBehaviour
{
    public float t;

    public float speed;

    public float maxScale;

    public GameObject allVisuals;

    public Transform textTransform;
    public AnimationCurve textTransformScaleCurve;
    public float textMaxScale;

    public Image backgroundRenderer;
    public AnimationCurve backgroundAlphaCurve;
    public AnimationCurve textRotationCurve;
    public float textMaxRotation;

    public Color startColor;
    public Color endColor;

    public Color startTextColor;
    public Color endTextColor;

    public AudioSource source;


    public void Activate(string textSet)
    {
        textTransform.GetComponent<Text>().text = textSet;
        t = 0f;
        allVisuals.SetActive(true);
        source.Play();
    }

    private void Start()
    {
        allVisuals.SetActive(false);
    }

    private void Update()
    {
        if (t >= 1)
        {
            allVisuals.SetActive(false);
            return;
        }

        t += Time.deltaTime * speed;

        backgroundRenderer.transform.localScale = Vector3.one * (maxScale * t);
        backgroundRenderer.color = Color.Lerp(startColor, endColor, backgroundAlphaCurve.Evaluate(t));

        textTransform.localScale = Vector3.one * textTransformScaleCurve.Evaluate(t) * textMaxScale;
        textTransform.localRotation = Quaternion.Euler(0f, 0f, textRotationCurve.Evaluate(t) * textMaxRotation);
        textTransform.GetComponent<Text>().color = Color.Lerp(startTextColor, endTextColor, backgroundAlphaCurve.Evaluate(t));
    }
}
