using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    public Bell bell;

    public ActivationIndicator activationIndicator;

    public GameObject returnToMenuBtn;
    public GameObject moveToShopBtn;

    public CardPlayManager cardPlayManager;

    public List<GameObject> waffleList = new List<GameObject>();
    public List<GameObject> enemyList = new List<GameObject>();

    public bool notAttacking;
    public bool battleStartable;
    public bool canPlayCards;
    public bool gameStarted;
    public bool gameEnded;

    void Start()
    {
        //Draw Cards
        RunManager.Instance.deck.Shuffle();
        SpawnEnemies();
        cardPlayManager.DrawCards(7);

        //Find all waffles and enemies in the scene and adds them to their respective list (just needed for making the system, probably can be taken out when properly implemented because waffles and enemies won't exist before scene start)
        updateEntityLists();
        notAttacking = true;
        battleStartable = true;
        gameStarted = false;
        canPlayCards = true;
        gameEnded = false;

        returnToMenuBtn.SetActive(false);
        moveToShopBtn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        FindObjectOfType<CardPlayManager>().handActive = battleStartable;

        //updates lists for waffles and enemies every frame
        updateEntityLists();

        if(waffleList.Count == 0)
        {
            bell.isActive = false;
        } else if (waffleList.Count == 0 && enemyList.Count > 0 && gameStarted == true)
        {
            bell.isActive = false;
            canPlayCards = false;
            gameEnded = true;
            returnToMenuBtn.SetActive(true);
            moveToShopBtn.SetActive(false);
            //Battle Over Player Lost
        }
        else if (enemyList.Count == 0 && waffleList.Count > 0)
        {
            bell.isActive = false;
            canPlayCards = false;
            returnToMenuBtn.SetActive(false);
            moveToShopBtn.SetActive(true);
            if (gameEnded == false)
            {
                StartCoroutine(EndBattle());
            }
            gameEnded = true;
            //Battle Over
        }else if(battleStartable)
        {
            bell.isActive = true;
        }
    }

    public void StartBattlePhase()
    {
        StartCoroutine(battlePhase());
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < Random.Range(1, 3 + (RunManager.Instance.level / 5)); i++)
        {
            GameObject toSpawn = GameManager.Instance.GetEnemyWithID(Random.Range(0, 3));

            if(toSpawn != null)
            {
                GameObject obj = Instantiate(toSpawn, null);
            }
        }

        updateEntityLists();
    }

    //updates the lists for each entity by making a new list and ordering them based on the speed stat of the entities in the list
    void updateEntityLists()
    {
        waffleList = new List<GameObject>(GameObject.FindGameObjectsWithTag("waffle").ToList());
        waffleList = new List<GameObject>(waffleList.OrderByDescending(x => x.GetComponent<EntityClass>().stats[(int)StatType.Speed].currentValue).ToList());
        for (int i = 0; i < waffleList.Count; i++)
        {
            waffleList[i].transform.position = Vector3.Lerp(waffleList[i].transform.position, new Vector2(-2 - (i * 2), Mathf.Clamp(waffleList[i].transform.position.y, -1f, 0f)), Time.deltaTime * 5f);
        }

        enemyList = new List<GameObject>(GameObject.FindGameObjectsWithTag("enemy").ToList());
        enemyList = new List<GameObject>(enemyList.OrderByDescending(x => x.GetComponent<EntityClass>().stats[(int)StatType.Speed].currentValue).ToList());
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].transform.position = Vector3.Lerp(enemyList[i].transform.position, new Vector2(2 + (i * 2), Mathf.Clamp(enemyList[i].transform.position.y, -1f, 0f)), Time.deltaTime * 5f);
        }
    }

    //attack function, has a delay for animations which can be set in each entity's personal EntityClass script (default is 1s)
    IEnumerator attack(GameObject receiver, GameObject dealer)
    {
        notAttacking = false;
        yield return new WaitForSeconds(dealer.GetComponent<EntityClass>().entityAttackDelay);
        receiver.GetComponent<EntityClass>().TakeDamage(dealer.GetComponent<EntityClass>().stats[(int)StatType.Damage].currentValue);

        if(receiver.GetComponent<EntityClass>().isDead == true)
        {
            activationIndicator.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(receiver.transform.position + new Vector3(0f, 1.2f, 0f));
            activationIndicator.Activate("Dead");
        }
        notAttacking = true;
    }

    IEnumerator EndBattle()
    {
        RunManager.Instance.level++;
        for (int i = 0; i < waffleList.Count; i++)
        {
            activationIndicator.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(waffleList[i].transform.position + new Vector3(0f, 1.2f, 0f));
            activationIndicator.Activate("$1");
            RunManager.Instance.money++;
            yield return new WaitForSeconds(1.5f);
        }
    }

    IEnumerator battlePhase()
    {
        canPlayCards = false;
        bell.isActive = false;
        battleStartable = false;
        for (int i = 0; i < waffleList.Count; i++)
        {
            StartCoroutine(attack(enemyList[0], waffleList[i]));
            yield return new WaitUntil(() => notAttacking);
        }

        for (int i = 0; i < enemyList.Count; i++)
        {
            StartCoroutine(attack(waffleList[0], enemyList[i]));
            yield return new WaitUntil(() => notAttacking);
        }

        battleStartable = true;
        bell.isActive = true;
        canPlayCards = true;

        for (int i = 0; i < Mathf.Clamp(5 - cardPlayManager.cards.Count, 0, 5); i++)
        {
            cardPlayManager.DrawCards(1);
            yield return new WaitForSeconds(0.1f);
        }   
    }
}