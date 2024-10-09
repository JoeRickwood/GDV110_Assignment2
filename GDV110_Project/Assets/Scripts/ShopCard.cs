using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Card data;

    public GameObject cardVisual;
    public GameObject buyVisual;
    public GameObject boughtVisual;

    public ShopManager shop;
    public EnlargeOnMouseOver mouseOverEnlarge;

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
            //Buy Card
            bought = true;
            mouseOverEnlarge.isActive = false;
            shop.activationIndicator.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
            shop.activationIndicator.Activate();
        }else
        {
            selected = true;
        }
    }
}
