using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeckViewer : MonoBehaviour
{
    public Transform cardTransform;
    public GameObject cardRendererPrefab;
    public GridLayoutGroup layoutGroup;

    public void UpdateVisuals()
    {
        for (int i = 0; i < cardTransform.childCount; i++)
        {
            Destroy(cardTransform.GetChild(i).gameObject);
        }

        StartCoroutine(DrawCards());
    }


    public IEnumerator DrawCards() 
    {
        int count = RunManager.Instance.deck.staticDeck.Count;

        layoutGroup.spacing = new Vector2(Mathf.Max(400 / count, 50), 75f);

        for (int i = 0; i < count; i++)
        {
            GameObject cur = Instantiate(cardRendererPrefab, cardTransform);
            yield return new WaitForSeconds(0.02f);
        }
    }
}
