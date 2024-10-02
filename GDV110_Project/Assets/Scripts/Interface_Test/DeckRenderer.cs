using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DeckRenderer : MonoBehaviour
{
    public GameObject Deck;
    public Text DeckCountText;
    public GameObject deckTopPrefab;
    public Transform deckTransform;

    public async void UpdateVisuals(InterfaceTestManager manager, bool anim = true)
    {
        for (int i = 0; i < deckTransform.childCount; i++)
        {
            Destroy(deckTransform.GetChild(i).gameObject);
        }

        for (int i = 0; i < manager.deck.cards.Count; i++) 
        {
            GameObject cur = Instantiate(deckTopPrefab, deckTransform);
            if (anim)
            {
                cur.GetComponent<LerpToPosition>().target = cur.GetComponent<RectTransform>().position + new Vector3(0, i * 4, 0f);
                cur.GetComponent<RectTransform>().position = cur.GetComponent<RectTransform>().position + new Vector3(-200f, i * 4, 0f);
            }
            else
            {
                cur.GetComponent<LerpToPosition>().target = cur.GetComponent<RectTransform>().position + new Vector3(0f, i * 4, 0f);
                cur.GetComponent<RectTransform>().position = cur.GetComponent<RectTransform>().position + new Vector3(0, i * 4, 0f);
            }
            
            cur.transform.rotation = Quaternion.Euler(0f, 0f, (Mathf.Sin((float)i / 3f) + Mathf.Sin((float)i / 10f)) * 2f);

            if(anim)
            {
                await Task.Delay(20);
            }
            
        }

        if(manager.deck.cards.Count <= 0)
        {
            Deck.SetActive(false);
        }

        DeckCountText.text = $"Card Remaining : {manager.deck.cards.Count}";
    }
}
