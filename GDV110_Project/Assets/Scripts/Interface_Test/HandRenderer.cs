using UnityEngine;

public class HandRenderer : MonoBehaviour
{
    public Transform HandTransform;
    public GameObject CardPrefab;

    public void UpdateVisuals(InterfaceTestManager manager)
    {
        for (int i = 0; i < HandTransform.childCount; i++) 
        {
            Destroy(HandTransform.GetChild(i).gameObject);
        }

        for (int i = 0; i < manager.hand.Count; i++)
        {
            GameObject cur = Instantiate(CardPrefab, HandTransform);
            cur.GetComponent<CardRenderer>().SetData(manager.hand[i]);
        }
    }
}
