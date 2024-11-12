using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ShopCard parent;

    [Header("Card Description")]
    public RectTransform cardDescriptionObject;
    public Text cardDescriptionText;

    public bool hover;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover = false;
        
    }

    private void Update()
    {
        cardDescriptionObject.gameObject.SetActive(hover);

        cardDescriptionText.text = parent.data.GetCardDescription();
    }
}
