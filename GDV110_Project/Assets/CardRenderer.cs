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
        
        Sprite spr = GameManager.Instance.GetSprite(_Card.ID);

        if(spr == null)
        {
            image.color = new Color(0f, 0f, 0f, 0f);
        }
        else
        {
            image.sprite = spr;
        }

        
    }
}
