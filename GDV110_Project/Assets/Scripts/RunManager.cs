using System;
using System.Globalization;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance;

    [Header("Run Data")]
    public GameState currentState = GameState.Fight;

    public Difficulty difficulty;

    public int seed;

    public void EndRun()
    {
        Destroy(gameObject);
        transform.parent = null;
        Instance = null;
    }

    public int money;

    public int randIteration;

    public int level;
    public int health;

    public Deck deck;

    //[CardType][Cards]
    private Card[][] allCards;

    private System.Random random;

    //Converts A Integer To A Hexadecimal Value
    public string ToHex(int _Value)
    {
        return String.Format("0x{0:X}", _Value);
    }


    public float DifficultyValue()
    {
        return Mathf.Pow(level, 0.9f + ((int)(difficulty) / 13f));
    }

    //Converts From A Hexadecimal Value Back To A Integer
    public int FromHexToInt(string _Value)
    {
        if (_Value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            _Value = _Value.Substring(2);
        }
        return Int32.Parse(_Value, NumberStyles.HexNumber);
    }

    //Converts From A Integer To A GameState
    public GameState FromIntToGameState(int _Value)
    {
        switch (_Value) 
        {
            case 0:
                return GameState.Fight;
            case 1:
                return GameState.PostFight;
            case 2:
                return GameState.Shop;
            default:
                return GameState.Fight;
        }
    }

    //Initializes The Static Instance Of The RunManager, 
    //If A RunManager Already Exists, Destroy This Object
    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        //Initialize Upgrade Database
        InitializeDeck();
        InitializeCards();
    }

    public void InitializeDeck()
    {
        deck = new Deck();
    }


    //Adds All Upgrades To The Array
    public void InitializeCards()
    {
        //Initialize All Cards
        allCards = new Card[][]
        {
            new Card[] //Waffles
            {
                new CharacterCard("Classic Waffle", 0, WaffleType.Classic),
                new CharacterCard("Square Waffle", 2, WaffleType.Square),
            },

            new Card[] //Toppings
            {
                new ToppingCard("Maple Syrup", 1, new StrengthUpgrade(1, 2, Operation.Add)),
                new ToppingCard("Butter", 3, new HealthUpgrade(3, 5)),
                new ToppingCard("Orange Juice", 4, new HealUpgrade(2, 20)),
                new ToppingCard("Jam", 6, new StrengthUpgrade(3, 2, Operation.Add), new HealthUpgrade(4, 10)),
                new ToppingCard("Lemon And Sugar", 8, new StrengthUpgrade(3, 2, Operation.Multiply), new DecayUpgrade(3, 25)),
                new ToppingCard("Chocolate Coating", 9, new TemporaryInvincibilityUpgrade(5)),
                new ToppingCard("Chopped Nuts", 10, new DamageMultipleEnemiesUpgrade(7, 50)),
                new ToppingCard("Peanut Butter", 11, new ChunkyDamageChanceUpgrade(5, 2, Operation.Multiply, 50)),
                new ToppingCard("Golden Syrup", 12, new FadingStrengthUpgrade(5, 12, Operation.Add, 4)),
            },

            new Card[] //Passive Cards
            {
                new CantripCard("Service Bell", 5, new ReverseTurnOrderUpgrade(4)),
                new CantripCard("Cinnamon Shaker", 7, new DrawCardsUpgrade(4, 2)),
            },

            new Card[] //Developer Cards
            {
                new CharacterCardIsobel("Iz Yippee Waffle", 13, WaffleType.Isobel_Rainbow),
                new CantripCard("Lara's Waffle Army", 14, new FillBoardUpgrade(20)),
                new CantripCard("Sofia's Fork", 15, new SofiasForkUpgrade(20)),
                new CharacterCard("Lil Guy", 16, WaffleType.Lil_Guy),
                new CantripCard("Joe's Hypnotic Coffee", 17, new EnemyWaffleSwapUpgrade(20)),
            }               
        };

        for (int i = 0; i < allCards.Length; i++) 
        { 
            for (int j = 0; j < allCards[i].Length; j++) 
            {
                allCards[i][j].CalculatePrice();
            }
        }
    }

    //Loads Saved Run From Continue File
    /* public void LoadRun()
    {
        //If No File Exists Creates A New Run Instead
        //Also Creates New Run If Theres A Problem With The Save File

        //Load Memory From Previous Run
        //
        //-----------------------------

        string path = Application.persistentDataPath + "/" + GameManager.Instance.continueRunPath;

        string[] runString = File.ReadLines(path).ToArray();

        runString = runString[0].Split("-");

        try
        {
            seed = FromHexToInt(runString[0]);
            money = FromHexToInt(runString[1]);
            randIteration = FromHexToInt(runString[2]);
            currentState = FromIntToGameState(FromHexToInt(runString[2]));
        }
        catch (System.Exception)
        {
            NewRun(UnityEngine.Random.Range(0, 10000));
        }

        random = new System.Random(seed);

        //Looping Through The Rand Iteration Count From Previous Run In Order To Start Receiving The Correct Results
        for(int i = 0; i < randIteration; i++)
        {
            random.Next();
        }

        Debug.Log($"{seed} > {money} > {randIteration} > {nameof(currentState)}");
    } */


    //The Order When Saving Goes Seed -> Money -> RandIteration -> Game State
    /*public void SaveRun()
    {
        //Saves Current Run To Continue File

        string path = Application.persistentDataPath + "/" + GameManager.Instance.continueRunPath;

        string currentCards = "";
        string staticCards = "";

        for (int i = 0; i < deck.currentDeck.Count; i++)
        {
            currentCards = $"{deck.currentDeck[i].ID}-";
        }

        for (int i = 0; i < deck.staticDeck.Count; i++)
        {
            staticCards = $"{deck.staticDeck[i].ID}-";
        }

        string[] runString =
        {
            $"{ToHex((int)seed)}-{ToHex((int)money)}-{ToHex((int)randIteration)}-{ToHex((int)currentState)}", //RunManagerData
            staticCards, //Static Cards
            currentCards, //Current Cards
        };

        //Maybe Encrypt??????

        Debug.Log(runString[0] + "\n" + runString[1] + "\n" + runString[2]);

        File.WriteAllText(path, runString[0] + "\n" + runString[1] + "\n" + runString[2]);
    } */

    //Creates New Run And Initializes The Random
    public void NewRun(int _Seed, Difficulty _Difficulty)
    {
        health = 3;
        level = 1;
        randIteration = 0;
        seed = _Seed;
        random = new System.Random(seed);
        difficulty = _Difficulty;
        money = 5;

        currentState = GameState.Fight;


        //Add Base Cards To Deck
        for (int i = 0; i < 2; i++)
        {
            deck.AddCardStatic(GetCardWithID(0));
            deck.AddCardStatic(GetCardWithID(2));
        }

        for (int i = 0; i < 9; i++)
        {
            deck.AddCardStatic(GetCardWithID(1));
        }

        deck.AddCardStatic(GetCardWithID(3));
        deck.AddCardStatic(GetCardWithID(3));

        deck.Shuffle();
    }


    //----------------------------- CARD GETTING ----------------------------------------
    //Will Be Weighted On Rarity Unless Got With A Specific ID Of The Card

    public Card GetCardWithID(int _ID)
    {
        for (int i = 0; i < allCards.Length; i++)
        {
            for(int j = 0; j < allCards[i].Length; j++)
            {
                if (allCards[i][j].ID == _ID)
                {
                    return allCards[i][j].Clone(); //Return Card If IDs Match, Clone It
                }
            }
        }

        return null;
    }

    public Card GetRandomCard(params CardTypeReturn[] cardTypeReturn)
    {
        //Use Random Generation From The Game To Give Card

        int first = (int)cardTypeReturn[GetRandomInt(0, cardTypeReturn.Length)];
        int randomCard = GetRandomInt(0, first == 0 ? 2 : allCards[first].Length);

        return allCards[first][randomCard].Clone(); //Clone Card At Index
    }

    //----------------------------- RANDOM NUMBER GENERATING ----------------------------
    //Only Use These Methods When Generating Run-Changing Mechanics,
    //For Example Items In Shops, Upgrades, Enemies To Spawn Ect

    public float GetRandomFloat(float _Min, float _Max)
    {
        randIteration++;
        return _Min + ((_Max - _Min) * (float)random.NextDouble());
    }

    public int GetRandomInt(int min, int max)
    {
        randIteration++;

        float n = GetRandomFloat(min, max);

        return (int)(n - (n % 1));
    }

}

public enum GameState
{
    Fight,
    PostFight,
    Shop
}

public enum CardTypeReturn : int
{
    Waffle = 0,
    Topping = 1,
    Cantrip = 2,
    Developer = 3
}

public enum Difficulty
{
    Easy,
    Normal,
    Hard
}
