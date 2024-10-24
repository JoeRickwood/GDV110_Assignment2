using UnityEngine;
using UnityEngine.UI;

public class DeckRenderer : MonoBehaviour
{
    public Text countText;
    public Image visual;

    private void Update()
    {
        if(RunManager.Instance == null)
        {
            return;
        }

        int currentCount = RunManager.Instance.deck.currentDeck.Count;
        int baseCount = RunManager.Instance.deck.staticDeck.Count;

        countText.text = $"{currentCount}/{baseCount}";
        if(currentCount <= 0)
        {
            visual.gameObject.SetActive(false);
        }else
        {
            visual.gameObject.SetActive(true);
        }
    }
}
