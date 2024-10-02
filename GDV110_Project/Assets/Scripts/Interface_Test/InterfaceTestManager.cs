using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InterfaceTestManager : MonoBehaviour
{
    public const int handSize = 5;

    public Deck deck;
    public List<Card> hand;

    public DeckRenderer deckRenderer;
    public HandRenderer handRenderer;

    private async void Start()
    {
        deck.Initalize();

        for (int i = 0; i < 52; i++) 
        {
            int rand = Random.Range(0, 2);
            if(rand == 0)
            {
                deck.AddCard(new ToppingCard($"Topping {i}", new Upgrade("Chocolate", 0, "Its Yummy", 1)));
            }
            else if(rand == 1)
            {
                deck.AddCard(new CharacterCard($"Waffle {i}"));
            }
            //await Task.Delay(50);
        }

        deckRenderer.UpdateVisuals(this, true);

        deck.Shuffle();

        for (int i = 0; i < handSize; i++) 
        {
            Card c = deck.Draw();
            if (c != null)
            {
                hand.Add(c);
                UpdateUI();
                await Task.Delay(100);
            }
        }
    }

    public void UpdateUI()
    {
        deckRenderer.UpdateVisuals(this, false);
        handRenderer.UpdateVisuals(this);
    }
}
