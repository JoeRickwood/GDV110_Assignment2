/***
Bachelor of Software Engineering
Media Design School
Auckland
New Zealand
(c) [2024] Media Design School
File Name : Deck.cs
Description : Contains All The Implementation Code For The Deck And Cards Stored Within, Mostly Used As Shell Code
Author : Joe Rickwood
Mail : Joe.Rickwood@mds.ac.nz
**/

//Deck Manager Class
//Has All Methods To Add AND Remove Cards From Deck At Will

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#region Deck

//Deck Contains A List Of Cards 
[System.Serializable]
public class Deck
{
    System.Random random;

    public List<Card> cards;


    //Initializing The Randomness Of The Deck
    public void Initalize()
    {
        random = new System.Random(Guid.NewGuid().GetHashCode());
    }

    //Add Card To Inventory, Then Has Option To Shuffle The Deck Afterwards
    public void AddCard(Card card, bool shuffle = false)
    {
        cards.Add(card);

        if(shuffle == true)
        {
            Shuffle();
        }
    }

    //Removes Card From The Deck, Has Option To Shuffle The Deck Afterwards
    public void RemoveCard(int index, bool shuffle) 
    {
        cards.RemoveAt(index);

        if (shuffle == true)
        {
            Shuffle();
        }
    }

    //Shuffle Deck By Doing A Data-Swap
    public void Shuffle() 
    {
        int swapIndex = 0;
        Card tmp = null;

        //Shuffles All Cards
        for (int i = 0; i < cards.Count; i++)
        {
            swapIndex = random.Next(0, cards.Count - 1);
            tmp = cards[i];

            cards[i] = cards[swapIndex];
            cards[swapIndex] = tmp;
        }
    }

    public Card Draw()
    {
        if(cards.Count <= 0)
        {
            return null; 
        }
        Card c = cards[0];

        cards.RemoveAt(0);

        return c;
    }
}

#endregion

#region CARDS
#region Base

//Card Base Implementation
[System.Serializable]
public class Card
{
    public string name;

    //Default Card Constructor
    public Card(string name)
    {
        this.name = name;   
    }

    //Convert OnHeldHover To Be A Virtual With basic Drag And Drop Functionality
    public virtual void OnHeldHover() { }

    //On Drop Is Called When The Mouse Is Let Go From A Card
    public virtual void OnDrop(GameObject obj) 
    {
        UnityEngine.Debug.Log("Help Me");
    }
}

#endregion

//Chracter Card Implementation
[System.Serializable]
public class CharacterCard : Card
{
    //private Character data

    public CharacterCard(string name/* Add Character Reference */) : base(name) 
    { 
        //Assign Character Card Character Here
    }

    //Implement Hover Function
    public override void OnHeldHover()
    {
        
    }

    //Implement Drop Function
    public override void OnDrop(GameObject obj)
    {
        GameObject prefab = Resources.Load<GameObject>("Character");
        GameObject cur = GameObject.Instantiate(prefab);
        cur.transform.position = new Vector3(UnityEngine.Random.Range(-8f, 0f), UnityEngine.Random.Range(0f, -1f));
    }
}


//Topping Card Implementation
[System.Serializable]
public class ToppingCard : Card
{
    public Upgrade data;

    public ToppingCard(string name, Upgrade data) : base(name)
    {
        this.data = data;
    }

    //Implement Hover Function
    public override void OnHeldHover()
    {
    }

    //Implement Drop Function
    public override void OnDrop(GameObject obj)
    {
        if(obj == null)
        {
            return;
        }

        obj.GetComponent<Character>().AddUpgrade(data);
        obj.GetComponent<Character>().damage += 15;
    }
}

#endregion