using UnityEngine;
using UnityEngine.UI;

public class CardRenderer : MonoBehaviour
{
    public Card card;

    public Text descriptionText;
    public Text titleText;
    public Image image;

    public void UpdateCardData(Card _Card)
    {
        card = _Card;
        titleText.text = card.name;
    }
}
