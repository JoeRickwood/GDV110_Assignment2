using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPlayManager : MonoBehaviour
{
    public int cardsDrawnPerTurn;

    public List<Card> cards;

    public GameObject physicalCardPrefab;

    public GameObject currentHeld;
    public Transform heldTransform;
    public LineRenderer dropLine;

    public WaveLayoutGroup group;

    //At The Start Of The Round, Reset The Played Deck
    private void Start()
    {
        RunManager.Instance.deck.ResetDeck();
    }

    private void Update()
    {
        group.spacing = 2000f / group.transform.childCount;

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
            currentHeld.transform.parent = heldTransform;
            Debug.Log(currentHeld);
        }

        //When A Player Released The Left Click Button
        if(Input.GetMouseButtonUp(0) && currentHeld != null)
        {
            List<RaycastResult> raycastResults = GetEventSystemRaycastResults();

            if (raycastResults.Count < 1)
            {
                currentHeld.transform.parent = group.transform;
                currentHeld = null;
                return;
            }

            if (!currentHeld.GetComponent<PlayableCard>().card.OnDrop())
            {
                currentHeld.transform.parent = group.transform;
            }else
            {
                Destroy(currentHeld);
            }

            
            currentHeld = null;
        }

        if (currentHeld != null)
        {
            currentHeld.GetComponent<PlayableCard>().card.OnHover(currentHeld);


            Debug.Log(currentHeld.GetComponent<PlayableCard>().card.GetType());

            float distanceX = (currentHeld.GetComponent<RectTransform>().position.x - Input.mousePosition.x) / 75f;
            float distanceY = (currentHeld.GetComponent<RectTransform>().position.y - Input.mousePosition.y) / 25f;
            currentHeld.GetComponent<RectTransform>().position = Vector3.Lerp(currentHeld.GetComponent<RectTransform>().position, Input.mousePosition, Time.deltaTime * 10f);
            currentHeld.GetComponent<RectTransform>().rotation = Quaternion.Euler(distanceY * 2f, 0f, distanceX);

        }else
        {
            dropLine.enabled = false;
        }
    }

    public void DrawCards(int count)
    {
        for (int i = 0; i < count; i++) 
        {
            Card card = RunManager.Instance.deck.DrawCard(0);
            GameObject cur = Instantiate(physicalCardPrefab, group.transform);
            cur.GetComponent<PlayableCard>().UpdateCardData(card);
        }
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
