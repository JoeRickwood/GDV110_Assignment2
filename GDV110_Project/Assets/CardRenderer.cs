using UnityEngine;
using UnityEngine.UI;

public class CardRenderer : MonoBehaviour
{
    public Card card;

    public Text descriptionText;
    public Text titleText;
    public Image image;
    public GameObject greyedOutPanel;
    public Image border;

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

        if(_Card.GetType() == typeof(CharacterCard))
        {
            border.color = new Color(0.87f, 0.26f, 0.18f, 1f);
            titleText.color = Color.white;
        }else if(_Card.GetType() == typeof(ToppingCard))
        {
            border.color = new Color(0.4f, 0.4f, 0.4f, 1f);
            titleText.color = new Color(1f, 0.37f, 0f, 1f);
        }
        else if (_Card.GetType() == typeof(CantripCard))
        {
            border.color = new Color(0.2f, 0.57f, 0.71f, 1f);
            titleText.color = new Color(1f, 0.37f, 0f, 1f);
        }
    }
}
