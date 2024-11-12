using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CardPlayManager : MonoBehaviour
{
    public int cardsDrawnPerTurn;

    public BattleManager battleManager;

    public GameObject physicalCardPrefab;

    public GameObject currentHeld;
    public GameObject crumbParticleEffect;
    public Transform heldTransform;
    public LineRenderer dropLine;
    public RectTransform deckTransform;

    public WaveLayoutGroup group;

    [Header("Card Description")]
    public RectTransform cardDescriptionObject;
    public Text cardDescriptionText;

    public bool handActive;

    public int cardsInHand;

    float handPosActive;
    float handPosInActive;

    //At The Start Of The Round, Reset The Played Deck
    private void Start()
    {
        RunManager.Instance.deck.ResetDeck();

        handPosActive = group.GetComponent<RectTransform>().position.y;
        handPosInActive = group.GetComponent<RectTransform>().position.y + -200;
    }

    private void Update()
    {
        if(!handActive)
        {
            group.GetComponent<RectTransform>().position = Vector3.Lerp(group.GetComponent<RectTransform>().position, new Vector3(group.GetComponent<RectTransform>().position.x, handPosInActive), Time.deltaTime * 10f);
            for (int i = 0; i < group.transform.childCount; i++)
            {
                group.transform.GetChild(i).GetComponent<CardRenderer>().greyedOutPanel.SetActive(true);
            }
            return;
        }else
        {
            for (int i = 0; i < group.transform.childCount; i++)
            {
                group.transform.GetChild(i).GetComponent<CardRenderer>().greyedOutPanel.SetActive(false);
            }
            group.GetComponent<RectTransform>().position = Vector3.Lerp(group.GetComponent<RectTransform>().position, new Vector3(group.GetComponent<RectTransform>().position.x, handPosActive), Time.deltaTime * 10f);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            //DrawCards();
        }

        //When A Player Presses The Left Click Button
        if(Input.GetMouseButtonDown(0))
        {
            List<RaycastResult> raycastResults = GetEventSystemRaycastResults();

            if(raycastResults.Count < 1)
            {
                return;
            }

            if(raycastResults[0].gameObject.GetComponent<PlayableCard>() == null)
            {
                return;
            }

            currentHeld = raycastResults[0].gameObject;
            currentHeld.transform.SetParent(heldTransform);
            Debug.Log(currentHeld);
        }

        //When A Player Released The Left Click Button
        if(Input.GetMouseButtonUp(0) && currentHeld != null)
        {
            List<RaycastResult> raycastResults = GetEventSystemRaycastResults();

            if (raycastResults.Count < 1)
            {
                currentHeld.transform.SetParent(group.transform);
                currentHeld = null;
                return;
            }

            if (battleManager.canPlayCards == true)
            {
                if (!currentHeld.GetComponent<PlayableCard>().card.OnDrop())
                {
                    currentHeld.transform.parent = group.transform;
                }
                else
                {
                    cardsInHand--;

                    if(currentHeld.GetComponent<PlayableCard>().card.GetType() == typeof(ToppingCard))
                    {
                        ToppingCard c = (ToppingCard)currentHeld.GetComponent<PlayableCard>().card;

                        currentHeld.GetComponent<PlayableCard>().Drop(c.currentTarget.transform.position);
                    }
                    else
                    {
                        currentHeld.GetComponent<PlayableCard>().Drop(Camera.main.ScreenToWorldPoint(currentHeld.GetComponent<RectTransform>().position));
                    }

                    currentHeld = null;
                    //Destroy(currentHeld);
                }
            }else
            {
                currentHeld.transform.parent = group.transform;
            }

            
            currentHeld = null;
        }

        if (currentHeld != null)
        {
            currentHeld.GetComponent<PlayableCard>().card.OnHover(currentHeld);

            float distanceX = (currentHeld.GetComponent<RectTransform>().position.x - Input.mousePosition.x) / 75f;
            float distanceY = (currentHeld.GetComponent<RectTransform>().position.y - Input.mousePosition.y) / 25f;
            currentHeld.GetComponent<RectTransform>().position = Vector3.Lerp(currentHeld.GetComponent<RectTransform>().position, Input.mousePosition, Time.deltaTime * 10f);
            currentHeld.GetComponent<RectTransform>().rotation = Quaternion.Euler(distanceY * 2f, 0f, distanceX);

            cardDescriptionObject.gameObject.SetActive(true);
            cardDescriptionObject.GetComponent<RectTransform>().position = currentHeld.GetComponent<RectTransform>().position + new Vector3((currentHeld.GetComponent<RectTransform>().sizeDelta.x / 2f) * currentHeld.GetComponent<RectTransform>().lossyScale.x, (currentHeld.GetComponent<RectTransform>().sizeDelta.y / 2f) * currentHeld.GetComponent<RectTransform>().lossyScale.y);

            cardDescriptionText.text = currentHeld.GetComponent<PlayableCard>().card.GetCardDescription();
        }
        else
        {
            cardDescriptionObject.gameObject.SetActive(false);
            dropLine.enabled = false;
        }
    }

    public void DrawCards(int count)
    {
        Destroy(Instantiate(crumbParticleEffect, Camera.main.ScreenToWorldPoint(deckTransform.GetComponent<RectTransform>().position), Quaternion.identity), 3f);
        cardsInHand += count;
        for (int i = 0; i < count; i++) 
        {
            Card card = RunManager.Instance.deck.DrawCard(0);
            if(card != null)
            {
                GameObject cur = Instantiate(physicalCardPrefab, group.transform);
                cur.transform.GetComponent<RectTransform>().position = deckTransform.position;
                cur.GetComponent<PlayableCard>().UpdateCardData(card);
            }
        }
    }

    public void DiscardHand()
    {
        for (int i = 0; i < cardsInHand; i++)
        {
            Destroy(group.transform.GetChild(i).gameObject);
        }

        cardsInHand = 0;
    }

    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
