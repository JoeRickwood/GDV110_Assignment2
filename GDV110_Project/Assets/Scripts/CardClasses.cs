using System.Collections.Generic;
using UnityEngine;

public enum WaffleType
{
    Classic,
    Square,
    Isobel_Rainbow,
    Lil_Guy
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

        Card c = currentDeck[index].Clone();
        currentDeck.RemoveAt(index);

        if(currentDeck.Count <= 0)
        {
            ResetDeck();
            Shuffle();
        }

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

    public Card(string _Name, int _ID)
    {
        name = _Name;
        ID = _ID;
    }

    public Card Clone()
    {
        return (Card)this.MemberwiseClone();
    }

    public virtual void Initialize() { }

    public virtual void OnHover(GameObject cardObj) { }

    public virtual bool OnDrop() { return false; }

    public virtual string GetCardDescription()
    {
        return "Card Description";
    }

    public virtual void CalculatePrice()
    {

    }
}

//Upgrades
[System.Serializable]
public class Upgrade
{
    public int price;
    public int ID;
    public EntityClass connectedEntity;


    public Sprite entityApplySprite;

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

    public virtual void OnPlayStatic()
    {
        Debug.Log($"Played Static Card");
    }

    public virtual void OnAttack(EntityClass attacked, ref float damage)
    {
        Debug.Log($"Played Static Card");
    }

    public virtual void OnCombatEnd()
    {
        Debug.Log($"On Combat End");
    }

    public virtual void OnTakeDamage(ref float damage)
    {
        Debug.Log($"On Take Damage {damage}");
    }

    public virtual string GetUpgradeString()
    {
        return "\n";
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

    public CharacterCard(string _Name, int _ID, WaffleType _Type) : base(_Name, _ID)
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

        if(Camera.main.ScreenToWorldPoint(Input.mousePosition).y < -2.5f)
        {
            return false;
        }

        float speedValue = 0;
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        for (int i = 0; i < GameObject.FindObjectOfType<BattleManager>().waffleList.Count; i++)
        {
            if(pos.x > GameObject.FindObjectOfType<BattleManager>().waffleList[i].transform.position.x)
            {
                if(GameObject.FindObjectOfType<BattleManager>().waffleList[i].GetComponent<EntityClass>().stats[(int)StatType.Speed].baseValue + 1 > speedValue)
                {
                    speedValue = GameObject.FindObjectOfType<BattleManager>().waffleList[i].GetComponent<EntityClass>().stats[(int)StatType.Speed].baseValue + 1;
                }
            }
        }

        GameObject cur = GameObject.Instantiate(characterPrefab);
        cur.GetComponent<EntityClass>().stats[(int)StatType.Speed].baseValue = speedValue;
        cur.GetComponent<EntityClass>().stats[(int)StatType.Speed].currentValue = speedValue;
        pos.z = 0f;
        cur.transform.position = pos;

        GameObject.FindObjectOfType<RoundWinScreen>().wafflesPlayedSprites.Add(cur.GetComponent<EntityClass>().entityIcon);

        return true;
    }

    public override string GetCardDescription()
    {
        return $"Creates A {name}";
    }

    public override void CalculatePrice()
    {
        switch (character) 
        {
            case WaffleType.Classic:
                price = 5;
                break;
            case WaffleType.Square:
                price = 4;
                break;
            case WaffleType.Isobel_Rainbow:
                price = 20;
                break;
            case WaffleType.Lil_Guy:
                price = 20;
                break;
            default:
                price = 4;
                break;
        }
    }
}

public class CharacterCardIsobel : CharacterCard
{
    Gradient grad;
    float t;

    public CharacterCardIsobel(string _Name, int _ID, WaffleType _Type) : base(_Name, _ID, _Type)
    {
        grad = new Gradient();
        grad.SetKeys(new GradientColorKey[]
        {
            new GradientColorKey(Color.red, 0f),
            new GradientColorKey(Color.green, 0.25f),
            new GradientColorKey(Color.magenta, 75f),
            new GradientColorKey(Color.red, 1f),
        },
        
        new GradientAlphaKey[]
        {
            new GradientAlphaKey(1f, 0f),
        });
    }

    public override void OnHover(GameObject cardObj)
    {
        t += Time.deltaTime * 0.3f;
        if(t >= 1f)
        {
            t = 0f;
        }

        cardObj.GetComponent<CardRenderer>().border.color = grad.Evaluate(t);
    }
}

//Topping Card To Be Played On The Player's Waffles
[System.Serializable]
public class ToppingCard : Card
{
    public Upgrade[] upgrade;

    public EntityClass currentTarget;

    public ToppingCard(string _Name, int _ID, params Upgrade[] _Upgrade) : base(_Name, _ID)
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

        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y < -2.5f || currentTarget == null)
        {
            currentTarget.GetComponent<SpriteOutline>().outlineSize = 0f;
            return false;
        }


        //Add Upgrade To Entity
        for (int i = 0; i < upgrade.Length; i++)
        {
            upgrade[i].entityApplySprite = GameManager.Instance.GetSprite(ID);
            currentTarget.AddUpgrade(upgrade[i]);
            upgrade[i].OnApplyToEntity(currentTarget);
        }

        currentTarget.GetComponent<SpriteOutline>().outlineSize = 0f;

        return true;
    }

    public override string GetCardDescription()
    {
        string str = "";

        for (int i = 0; i < upgrade.Length; i++)
        {
            str += $"{upgrade[i].GetUpgradeString()}\n";
        }

        return str;
    }

    public override void CalculatePrice()
    {
        for (int i = 0;i < upgrade.Length;i++) 
        {
            price += upgrade[i].price;
        }
    }
}

[System.Serializable]
public class CantripCard : Card
{
    public Upgrade[] upgrade;

    public CantripCard(string _Name, int _ID, params Upgrade[] _Upgrade) : base(_Name, _ID)
    {
        level = 1;
        upgrade = _Upgrade;
    }

    public override bool OnDrop()
    {
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y < -2.5f)
        {
            return false;
        }

        for (int i = 0;i < upgrade.Length;i++) 
        {
            upgrade[i].OnPlayStatic();
        }

        return true;
    }

    public override string GetCardDescription()
    {
        string str = "";

        for (int i = 0; i < upgrade.Length; i++)
        {
            str += $"{upgrade[i].GetUpgradeString()}\n";
        }

        return str;
    }

    public override void CalculatePrice()
    {
        for (int i = 0; i < upgrade.Length; i++)
        {
            price += upgrade[i].price;
        }
    }
}



public enum Operation
{
    Add,
    Multiply
}

public class StrengthUpgrade : Upgrade
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

    public override string GetUpgradeString()
    {
        return $"{(operation == Operation.Add ? "+" : "x")}{increase} Damage";
    }
}

public class HealUpgrade : Upgrade
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

    public override string GetUpgradeString() 
    {
        return $"Heal Waffle For {healAmount} Health";
    }
}

public class ReverseTurnOrderUpgrade : Upgrade
{
    public ReverseTurnOrderUpgrade(int _Price) : base(_Price, 2)
    {

    }

    public override void OnPlayStatic()
    {
        List<GameObject> enemyList = GameObject.FindObjectOfType<BattleManager>().enemyList;

        for (int i = enemyList.Count - 1; i >= 0; i--)
        {
            enemyList[i].GetComponent<EntityClass>().stats[(int)StatType.Speed].currentValue = i - enemyList.Count;
            enemyList[i].GetComponent<EntityClass>().stats[(int)StatType.Speed].baseValue = i - enemyList.Count;
        }
    }

    public override string GetUpgradeString()
    {
        return $"Reverses Enemy Order";
    }
}

public class ApplyDamagePreventionToEnemyUpgrade : Upgrade
{
    public ApplyDamagePreventionToEnemyUpgrade(int _Price) : base(_Price, 3)
    {

    }

    public override void OnAttack(EntityClass attacked, ref float damage)
    {
        attacked.AddUpgrade(new PreventDamageOnAttack(0, attacked));
    }

    public override string GetUpgradeString()
    {
        return $"Prevents Attacked Enemies From Dealing Damage";
    }
}

public class PreventDamageOnAttack : Upgrade
{
    EntityClass AppliedEntity;

    public PreventDamageOnAttack(int _Price, EntityClass entClass) : base(_Price, 4)
    {
        AppliedEntity = entClass;
    }

    public override void OnAttack(EntityClass attacked, ref float damage)
    {
        damage = 0;
    }

    public override void OnCombatEnd()
    {
        AppliedEntity.RemoveUpgrade(this);
    }

    public override string GetUpgradeString()
    {
        return $"Prevents This Entity From Dealing Damage";
    }
}

public class HealthUpgrade : Upgrade
{
    int increase;

    public HealthUpgrade(int _Price, int _Increase) : base(_Price, 5)
    {
        increase = _Increase;
    }

    public override void OnApplyToEntity(EntityClass entity)
    {
        entity.stats[(int)StatType.Health].baseValue += increase;
        entity.stats[(int)StatType.Health].currentValue += increase;
    }

    public override string GetUpgradeString()
    {
        return $"Increase Waffle Health By {increase}";
    }
}

public class DrawCardsUpgrade : Upgrade
{
    int drawCount = 0;

    public DrawCardsUpgrade(int _Price = 0, int _DrawCount = 0) : base(_Price, 6)
    {
        drawCount = _DrawCount;
    }

    public override void OnPlayStatic()
    {
        GameObject.FindObjectOfType<CardPlayManager>().DrawCards(drawCount);
    }

    public override string GetUpgradeString()
    {
        return $"Draw {drawCount} Cards On Play";
    }
}

public class DecayUpgrade : Upgrade
{
    int percentagePerTurn;

    public DecayUpgrade(int _Price = 0, int _PercentagePerTurn = 0) : base(_Price, 6)
    {
        percentagePerTurn = _PercentagePerTurn;
    }

    public override void OnApplyToEntity(EntityClass entity)
    {
        connectedEntity = entity;
    }

    public override void OnAttack(EntityClass attacked, ref float damage)
    {
        float amount = connectedEntity.stats[(int)StatType.Health].baseValue * (percentagePerTurn / 100f);
        connectedEntity.TakeDamage(amount);
    }

    public override string GetUpgradeString()
    {
        return $"Takes {percentagePerTurn}% Of max Health As Damage Each Turn";
    }
}
public class TemporaryInvincibilityUpgrade : Upgrade
{
    public TemporaryInvincibilityUpgrade(int _Price) : base(_Price, 7)
    {

    }

    public override void OnApplyToEntity(EntityClass entity)
    {
        connectedEntity = entity;
    }

    public override void OnTakeDamage(ref float damage)
    {
        damage = 0f;
    }

    public override void OnCombatEnd()
    {
        Debug.Log("here");
        connectedEntity.RemoveUpgrade(this);
        connectedEntity.OnItemAdded();
    }

    public override string GetUpgradeString()
    {
        return $"Makes A Waffle Temporarily Invincible";
    }
}

public class DamageMultipleEnemiesUpgrade : Upgrade
{
    int chance;

    public DamageMultipleEnemiesUpgrade(int _Price, int _Chance) : base(_Price, 8)
    {
        chance = _Chance;
    }

    public override void OnApplyToEntity(EntityClass entity)
    {
        connectedEntity = entity;
    }

    public override void OnAttack(EntityClass attacked, ref float damage)
    {
        var b = GameObject.FindObjectOfType<BattleManager>();

        bool success = UnityEngine.Random.Range(0, 100) < chance;

        if(success)
        {
            int rand = UnityEngine.Random.Range(0, b.enemyList.Count);


            if(b.enemyList[rand].GetComponent<EntityClass>() == attacked)
            {
                return;
            }

            float dmg = connectedEntity.stats[(int)StatType.Damage].currentValue;

            for(int i = 0; i < connectedEntity.entityUpgrades.Count; i++) 
            {
                connectedEntity.entityUpgrades[i].OnAttack(b.enemyList[rand].GetComponent<EntityClass>(), ref dmg);
            }

            b.enemyList[rand].GetComponent<EntityClass>().TakeDamage(damage);
        }
    }

    public override string GetUpgradeString()
    {
        return $"{chance}% To Damage A Bonus Random Enemy On Attack";
    }
}

public class ChunkyDamageChanceUpgrade : Upgrade
{
    int increase;
    Operation operation;
    int chance;

    public ChunkyDamageChanceUpgrade(int _Price, int _Increase, Operation _Operation, int _Chance) : base(_Price, 9)
    {
        increase = _Increase;
        operation = _Operation;
        chance = _Chance;
    }

    public override void OnAttack(EntityClass attacked, ref float damage)
    {
        if(UnityEngine.Random.Range(0, 100) > chance)
        {
            return;
        }

        switch (operation)
        {
            case Operation.Add:
                damage += increase;
                break;
            case Operation.Multiply:
                damage *= increase;
                break;
        }
    }

    public override string GetUpgradeString()
    {
        return $"{chance}% Chance To Deal {(operation == Operation.Add ? "+" : "x")} {increase} Damage";
    }
}

public class FadingStrengthUpgrade : Upgrade
{
    int increase;
    Operation operation;
    int decrease;

    public FadingStrengthUpgrade(int _Price, int _Increase, Operation _Operation, int _Decrease) : base(_Price, 9)
    {
        increase = _Increase;
        operation = _Operation;
        decrease = _Decrease;
    }

    public override void OnApplyToEntity(EntityClass entity)
    {
        connectedEntity = entity;
    }

    public override void OnAttack(EntityClass attacked, ref float damage)
    {
        switch (operation)
        {
            case Operation.Add:
                damage += increase;
                break;
            case Operation.Multiply:
                damage *= increase;
                break;
        }
    }

    public override void OnCombatEnd()
    {
        increase -= decrease;

        if(increase <= 0)
        {
            connectedEntity.RemoveUpgrade(this);
        }
    }

    public override string GetUpgradeString()
    {
        return $"{(operation == Operation.Add ? "+" : "x")}{increase} Damage, Fades Over {Mathf.RoundToInt((float)increase / (float)decrease)} Turns";
    }
}

public class FillBoardUpgrade : Upgrade
{
    public FillBoardUpgrade(int _Price) : base(_Price, 10)
    {

    }

    public override void OnPlayStatic()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject.FindObjectOfType<BattleManager>().gameStarted = true;

            if (GameObject.FindObjectOfType<BattleManager>().waffleList.Count >= 4)
            {
                return;
            }

            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y < -2f)
            {
                return;
            }

            GameObject cur = GameObject.Instantiate(Resources.Load<GameObject>("Waffle_Lara_Minion"));
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0f;
            pos.y -= 0.5f;
            cur.transform.position = pos;

            GameObject.FindObjectOfType<RoundWinScreen>().wafflesPlayedSprites.Add(cur.GetComponent<EntityClass>().entityIcon);        }
    }
}

public class SofiasForkUpgrade : Upgrade
{
    public SofiasForkUpgrade(int _Price) : base(_Price, 10)
    {

    }

    public override void OnPlayStatic()
    {
        if(GameObject.FindObjectOfType<BattleManager>().enemyList.Count <= 1)
        {
            return;
        }

        //InstaKill Enemy
        int rand = UnityEngine.Random.Range(0, GameObject.FindObjectOfType<BattleManager>().enemyList.Count);

        GameObject.FindObjectOfType<BattleManager>().enemyList[rand].GetComponent<EntityClass>().TakeDamage(GameObject.FindObjectOfType<BattleManager>().enemyList[rand].GetComponent<EntityClass>().stats[(int)StatType.Health].baseValue);
    }

    public override string GetUpgradeString()
    {
        return "Instantly Kills Random Enemy\nAs Long As Theres More Than 1";
    }
}

public class EnemyWaffleSwapUpgrade : Upgrade
{
    public EnemyWaffleSwapUpgrade(int _Price) : base(_Price, 11)
    {

    }

    public override void OnPlayStatic()
    {
        if(GameObject.FindObjectOfType<BattleManager>().waffleList.Count <= 0)
        {
            return;
        }

        GameObject enemy = GameObject.FindObjectOfType<BattleManager>().enemyList[0];
        enemy.GetComponent<SpriteRenderer>().flipX = !enemy.GetComponent<SpriteRenderer>().flipX;
        float enemySpeed = enemy.GetComponent<EntityClass>().stats[(int)StatType.Speed].baseValue;
        string enemyTag = enemy.tag;

        GameObject waffle = GameObject.FindObjectOfType<BattleManager>().waffleList[0];
        waffle.GetComponent<SpriteRenderer>().flipX = !waffle.GetComponent<SpriteRenderer>().flipX;
        float waffleSpeed = waffle.GetComponent<EntityClass>().stats[(int)StatType.Speed].baseValue;
        string waffleTag = waffle.tag;

        enemy.tag = waffleTag;
        enemy.GetComponent<EntityClass>().stats[(int)StatType.Speed].baseValue = waffleSpeed;
        enemy.GetComponent<EntityClass>().stats[(int)StatType.Speed].currentValue = waffleSpeed;

        waffle.tag = enemyTag;
        waffle.GetComponent<EntityClass>().stats[(int)StatType.Speed].baseValue = enemySpeed;
        waffle.GetComponent<EntityClass>().stats[(int)StatType.Speed].currentValue = enemySpeed;
    }

    public override string GetUpgradeString()
    {
        return "Swaps The First Waffle And Enemy";
    }
}