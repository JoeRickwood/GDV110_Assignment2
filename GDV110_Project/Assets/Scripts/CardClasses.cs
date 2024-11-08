using System;
using System.Collections.Generic;
using UnityEngine;

public enum WaffleType
{
    Classic,
    Square
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

        //We Check If A Hand Has A Waffle In It, If It Dosent Force The First Waffle Starting from the bottom into the top position of the deck
        bool ret = false;
        for (int i = 0; i < Mathf.Min(5, currentDeck.Count); i++)
        {
            if (currentDeck[i].GetType() == typeof(CharacterCard))
            {
                ret = true;
            }
        }

        if(ret == false)
        {
            for (int i = currentDeck.Count - 1; i > 0; i--)
            {
                if (currentDeck[i].GetType() == typeof(CharacterCard))
                {
                    tmp = currentDeck[i];

                    currentDeck[i] = currentDeck[0];
                    currentDeck[0] = tmp;
                }
            }
        }
    }


}

//Card Types
[System.Serializable]
public class Card
{
    public string name;
    public int level;
    public int ID;
    public int price;

    public Card(string _Name, int _ID, int price)
    {
        name = _Name;
        ID = _ID;
        this.price = price;
    }

    public Card Clone()
    {
        return (Card)this.MemberwiseClone();
    }

    public virtual void Initialize() { }

    public virtual void OnHover(GameObject cardObj) { }

    public virtual bool OnDrop() { return false; }
}

//Upgrades
[System.Serializable]
public class Upgrade
{
    public int price;
    public int ID;
    public EntityClass connectedEntity;

    public Upgrade Clone()
    {
        return (Upgrade)this.MemberwiseClone();
    }

    public Upgrade(int _Price, int _ID)
    {
        price = _Price;
        ID = _ID;
    }

    //Activate Item
    public virtual void OnCalculateStats(EntityClass entity)
    {
        Debug.Log("Item Activated");
    }

    public virtual void OnApplyToEntity(EntityClass entity)
    {
        Debug.Log($"Applied To Entity {entity.name}");
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

    public CharacterCard(string _Name, int _ID, int price, WaffleType _Type) : base(_Name, _ID, price)
    {
        character = _Type;
        level = 1;

        Initialize();
    }

    public override void Initialize()
    {
        characterPrefab = Resources.Load<GameObject>("Waffle_" + name);
    }

    public override bool OnDrop()
    {
        GameObject.FindObjectOfType<BattleManager>().gameStarted = true;

        if (GameObject.FindObjectOfType<BattleManager>().waffleList.Count >= 4)
        {
            return false;
        }

        if(Camera.main.ScreenToWorldPoint(Input.mousePosition).y < -2f)
        {
            return false;
        }

        GameObject cur = GameObject.Instantiate(characterPrefab);
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        cur.transform.position = pos;

        return true;
    }
}

//Topping Card To Be Played On The Player's Waffles
[System.Serializable]
public class ToppingCard : Card
{
    public Upgrade[] upgrade;

    public EntityClass currentTarget;

    public ToppingCard(string _Name, int _ID, int price, params Upgrade[] _Upgrade) : base(_Name, _ID, price)
    {
        level = 1;
        upgrade = _Upgrade;
    }

    public override void OnHover(GameObject cardObj)
    {
        if(GameObject.FindObjectOfType<BattleManager>().waffleList.Count <= 0)
        {
            return;
        }

        GameObject.FindObjectOfType<CardPlayManager>().dropLine.enabled = true;

        if (currentTarget != null)
        {
            if(currentTarget.transform.position.x > cardObj.transform.position.x)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(cardObj.transform.position);
                pos.z = 0f;

                GameObject.FindObjectOfType<CardPlayManager>().dropLine.SetPositions(new Vector3[] 
                { 
                    currentTarget.transform.position,
                    pos
                });
            }
            else
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(cardObj.transform.position);
                pos.z = 0f;

                GameObject.FindObjectOfType<CardPlayManager>().dropLine.SetPositions(new Vector3[] 
                {
                    currentTarget.transform.position,
                    pos
                });
            }
        }


        GameObject[] entities = GameObject.FindGameObjectsWithTag("waffle");

        if(entities.Length < 1)
        {
            return;
        }

        int currentIndex = 0;
        float distance = Vector2.Distance(entities[0].transform.position, Camera.main.ScreenToWorldPoint(cardObj.transform.position));
        for (int i = 1; i < entities.Length; i++) 
        { 
            float tmpDist = Vector2.Distance(entities[i].transform.position, Camera.main.ScreenToWorldPoint(cardObj.transform.position));
            if (tmpDist < distance)
            {
                currentIndex = i;
                distance = tmpDist;
            }
        }

        currentTarget = entities[currentIndex].GetComponent<EntityClass>();

        for (int i = 0; i < entities.Length; i++) 
        {
            if (entities[i] == currentTarget.gameObject)
            {
                entities[i].GetComponent<SpriteOutline>().outlineSize = 5f;
            }else
            {
                entities[i].GetComponent<SpriteOutline>().outlineSize = 0f;
            }
        }

        base.OnHover(cardObj);
    }

    public override bool OnDrop()
    {
        if(currentTarget == null)
        {
            return false;
        }

        GameObject.FindObjectOfType<BattleManager>().gameStarted = true;

        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y < -2f || currentTarget == null)
        {
            currentTarget.GetComponent<SpriteOutline>().outlineSize = 0f;
            return false;
        }


        //Add Upgrade To Entity
        for (int i = 0; i < upgrade.Length; i++)
        {
            currentTarget.AddUpgrade(upgrade[i]);
            upgrade[i].OnApplyToEntity(currentTarget);
        }

        currentTarget.GetComponent<SpriteOutline>().outlineSize = 0f;

        return true;
    }
}


enum Operation
{
    Add,
    Multiply
}

class StrengthUpgrade : Upgrade
{
    int increase;
    Operation operation;

    public StrengthUpgrade(int _Price, int _Increase, Operation _Operation) : base(_Price, 0) 
    { 
        increase = _Increase;
        operation = _Operation;
    }

    public override void OnCalculateStats(EntityClass entity)
    {
        switch (operation) 
        { 
            case Operation.Add:
                entity.stats[(int)StatType.Damage].currentValue += increase;
                break;
            case Operation.Multiply:
                entity.stats[(int)StatType.Damage].currentValue *= increase;
                break;
        }
    }
}

class HealUpgrade : Upgrade
{
    int healAmount;
  
    public HealUpgrade(int _Price, int _HealAmount) : base(_Price, 1)
    {
        healAmount = _HealAmount;
    }

    public override void OnApplyToEntity(EntityClass entity)
    {
        entity.Heal(healAmount);
    }

}