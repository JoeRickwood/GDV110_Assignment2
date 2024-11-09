using UnityEngine;

[RequireComponent(typeof(CardRenderer))]
public class PlayableCard : MonoBehaviour
{
    public Card card;

    public bool isDropping;
    float t;

    Vector3 startPos;
    Vector3 startScale;
    Vector3 endPos;
    public float dropSpeed;

    public AnimationCurve dropCurve;

    private void Start()
    {
        isDropping = false;
    }

    public void UpdateCardData(Card _Card)
    {
        card = _Card;
        GetComponent<CardRenderer>().UpdateCardData(card);
    }

    public void Drop(Vector3 targetPos)
    {
        t = 1f;
        isDropping = true;
        startPos = GetComponent<RectTransform>().position;
        endPos = Camera.main.WorldToScreenPoint(targetPos);
        startScale = GetComponent<RectTransform>().localScale;
        GetComponent<EnlargeOnMouseOver>().enabled = false;
    }

    private void Update()
    {
        if (!isDropping) 
        {
            return;
        }

        if(t <= 0)
        {
            Destroy(gameObject);
        }

        t -= Time.deltaTime * dropSpeed;

        GetComponent<RectTransform>().position = Vector3.Lerp(endPos, startPos, dropCurve.Evaluate(t));
        GetComponent<RectTransform>().localScale = Vector3.Lerp(Vector3.zero, startScale, dropCurve.Evaluate(t));
    }
}
