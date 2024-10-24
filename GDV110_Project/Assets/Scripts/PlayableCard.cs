using UnityEngine;

[RequireComponent(typeof(CardRenderer))]
public class PlayableCard : MonoBehaviour
{
    public Card card;

    public void UpdateCardData(Card _Card)
    {
        card = _Card;
        GetComponent<CardRenderer>().UpdateCardData(card);
    }
}
