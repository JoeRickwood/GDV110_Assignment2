using System;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance;

    [Header("Run Data")]
    public GameState currentState = GameState.Fight;

    public int seed;
    public int money;

    public int randIteration;

    public Deck deck;
    private Upgrade[] upgrades;

    private System.Random random;

    //Converts A Integer To A Hexadecimal Value
    public string ToHex(int _Value)
    {
        return String.Format("0x{0:X}", _Value);
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

        //Initialize Upgrade Database
        InitializeUpgrades();
    }


    //Adds All Upgrades To The Array
    public void InitializeUpgrades()
    {
        //IMPLEMENT 
    }


    //Loads Saved Run From Continue File
    public void LoadRun()
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
    }


    //The Order When Saving Goes Seed -> Money -> RandIteration -> Game State
    public void SaveRun()
    {
        //Saves Current Run To Continue File

        string path = Application.persistentDataPath + "/" + GameManager.Instance.continueRunPath;

        string runString = $"{ToHex((int)seed)}-{ToHex((int)money)}-{ToHex((int)randIteration)}-{ToHex((int)currentState)}";

        //Maybe Encrypt??????

        Debug.Log(runString);

        File.WriteAllText(path, runString);
    }

    //Creates New Run And Initializes The Random
    public void NewRun(int _Seed)
    {
        randIteration = 0;
        seed = _Seed;
        random = new System.Random(seed);

        currentState = GameState.Fight;
    }

    //----------------------------- RANDOM NUMBER GENERATING ----------------------------
    //Only Use These Methods When Generating Run-Changing Mechanics,
    //For Example Items In Shops, Upgrades, Enemies To Spawn Ect

    public float GetRandomFloat(float min, float max)
    {
        randIteration++;
        return min + ((max - min) * (float)random.NextDouble());
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
