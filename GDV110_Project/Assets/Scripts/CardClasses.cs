using System;
using System.Collections.Generic;
using UnityEngine;

public enum WaffleType
{
    Classic
}

//Main Deck
[System.Serializable]
public class Deck
{
    //Hold References To The Current Deck For The Fight, This Gets Reset To The Static Deck After Each Fight Ends
    public List<Card> currentDeck;
    public List<Card> staticDeck;

    public Deck()
    {
        currentDeck = new List<Card>();
        staticDeck = new List<Card>();
    }

    //Resets The Current Deck To The Static Deck
    public void ResetDeck()
    {
        currentDeck = new List<Card>();

        for (int i = 0; i < staticDeck.Count; i++)
        {
            currentDeck.Add(staticDeck[i]);
        }
    }

    //Draws Card From Index From Deck
    public Card DrawCard(int index)
    {
        if(currentDeck.Count <= 0)
        {
            Debug.Log("Deck Is Empty");
            return null;
        }

        if(currentDeck.Count <= index)
        {
            Debug.Log("Unable To Draw From This Index");
            return null;
        }

        Card c = currentDeck[index];
        currentDeck.RemoveAt(index);
        return c;
    }

    //Adds Card To Static Deck
    public void AddCardStatic(Card card)
    {
        card.Initialize();

        staticDeck.Add(card);   
    }

    //Adds Card To The Current Deck
    public void AddCardCurrent(Card card, bool shuffle = false, int index = 0)
    {
        currentDeck.Insert(index, card);

        if(shuffle)
        {
            Shuffle();
        }   
    }

    //Shuffles The Current Deck Of Cards
    public void Shuffle()
    {
        int swapIndex = 0;
        Card tmp = null;

        //Shuffles All Cards
        for (int i = 0; i < currentDeck.Count; i++)
        {
            swapIndex = RunManager.Instance.GetRandomInt(0, currentDeck.Count);
            tmp = currentDeck[i];

            currentDeck[i] = currentDeck[swapIndex];
            currentDeck[swapIndex] = tmp;
        }
    }


}

//Card Types
[System.Serializable]
public class Card
{
    public int level;
    public int ID;
    public int price;

    public Card(int _ID, int price)
    {
        ID = _ID;
        this.price = price;
    }

    public Card Clone()
    {
        return (Card)this.MemberwiseClone();
    }

    public virtual void Initialize() { }

    public virtual void OnHover() { }

    public virtual void OnDrop(GameObject _Obj) { }
}

//Upgrades
[System.Serializable]
public class Upgrade
{
    public string name;
    public string description;
    public int price;

    public Upgrade Clone()
    {
        return (Upgrade)this.MemberwiseClone();
    }

    //Initialize Variables
    public virtual void Initialize()
    {
        name = "";
        description = "";
        price = 0;
    }

    //Activate Item
    public virtual void Activate(/* Reference To Entity Its Placed On */)
    {
        Debug.Log("Item Activated");
    }
}

//CHARACTERS are loaded from the resources folder, 
//The File Names Should Be 'Waffle_' Followed By The Waffle Type As A Prefab,
//For Example 'Waffle_Classic.prefab'


//Character Cards To Be Played On The Field To Create Waffles
[System.Serializable]
public class CharacterCard : Card
{
    //Reference To Character Prefab????
    public WaffleType character;

    GameObject characterPrefab = null;

    public CharacterCard(int _ID, int price, WaffleType _Type) : base(_ID, price)
    {
        character = _Type;
        level = 1;

        Initialize();
    }

    public override void Initialize()
    {
        characterPrefab = Resources.Load<GameObject>("Waffle_" + nameof(character));
    }
}

//Topping Card To Be Played On The Player's Waffles
[System.Serializable]
public class ToppingCard : Card
{
    public Upgrade[] upgrade;

    public ToppingCard(int _ID, int price, params Upgrade[] _Upgrade) : base(_ID, price)
    {
        level = 1;
        upgrade = _Upgrade;
    }

    public override void OnDrop(GameObject obj)
    {
        //Add Upgrade To Entity
    }
}
