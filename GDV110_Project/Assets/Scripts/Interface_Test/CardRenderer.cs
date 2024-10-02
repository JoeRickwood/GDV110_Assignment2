using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardRenderer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text text;
    public Card data;

    bool mouseOver;

    PointerEventData pointerEventData;
    GraphicRaycaster raycaster;

    public LayerMask layerMask;

    bool destroying;
    bool pickedUp;

    private void Start()
    {
        destroying = false;
        raycaster = FindObjectOfType<GraphicRaycaster>();
    }

    public void SetData(Card card)
    {
        data = card;
        text.text = card.name;
    }

    private void Update()
    {
        if(pickedUp)
        {
            GetComponent<RectTransform>().position = Vector2.Lerp(GetComponent<RectTransform>().position, Input.mousePosition, Time.deltaTime * 10f);
        }else
        {
            transform.parent = GameObject.FindGameObjectWithTag("Hand").transform;
        }

        if(Input.GetMouseButton(0)) 
        {
            if(mouseOver && pickedUp == false)
            {
                pickedUp = true;
                transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform;  
            }
        }

        if(Input.GetMouseButtonUp(0) && pickedUp == true)
        {
            pickedUp = false;

            Collider2D hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Collider2D hit2 = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), layerMask);
            Debug.Log(hit.transform);
            if(hit.gameObject == null) { return; }

            if(hit2 == null)
            {
                data.OnDrop(null);
            }
            else
            {
                data.OnDrop(hit2.gameObject);
            }

            transform.parent = GameObject.FindGameObjectWithTag("Hand").transform;
            FindObjectOfType<InterfaceTestManager>().hand.Remove(data);
            Destroy(gameObject);
            destroying = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }
}
