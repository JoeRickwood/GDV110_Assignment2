using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Card data;

    public GameObject cardVisual;
    public GameObject buyVisual;
    public GameObject boughtVisual;
    public Text priceText;

    public ShopManager shop;
    public EnlargeOnMouseOver mouseOverEnlarge;

    public Tooltip tooltip;

    public bool mouseOver;
    public bool selected;
    public bool bought;

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        selected = false;
    }

    private void Start()
    {
        bought = false;
        mouseOverEnlarge.isActive = true;
    }

    private void Update()
    {
        priceText.text = $"${data.price}";

        if(bought)
        {
            buyVisual.SetActive(false);
            cardVisual.SetActive(false);
            boughtVisual.SetActive(true);
            return;
        }


        if (Input.GetMouseButtonDown(0) && mouseOver)
        {
            Select();
        }

        buyVisual.SetActive(selected);
        cardVisual.SetActive(!selected);
    }

    public void Select()
    {
        if(selected == true) 
        {
            if(RunManager.Instance.money < data.price)
            {
                shop.activationIndicator.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
                shop.activationIndicator.Activate("Not Enough Money");
                return;
            }


            //Buy Card
            bought = true;
            tooltip.hover = false;
            mouseOverEnlarge.isActive = false;
            RunManager.Instance.deck.AddCardStatic(data);
            RunManager.Instance.money -= data.price;
            shop.activationIndicator.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
            shop.activationIndicator.Activate("Added");
        }else
        {
            selected = true;
        }
    }

    public void UpdateCardData(Card _Data)
    {
        data = _Data;
        GetComponent<CardRenderer>().UpdateCardData(data);
    }
}
