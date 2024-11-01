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
        /*if (bell == null)
        {
            //bell = GameObject.Find("Bell");
        }*/

        //Draw Cards
        RunManager.Instance.deck.Shuffle();
        cardPlayManager.DrawCards(7);

        //find all waffles and enemies in the scene and adds them to their respective list (just needed for making the system, probably can be taken out when properly implemented because waffles and enemies won't exist before scene start)
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
        /*if (selectedEntity == bell && battleStartable == true)
        {
            //bell.GetComponent<Bell>().Ding();
        }*/

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

    //updates the lists for each entity by making a new list and ordering them based on the speed stat of the entities in the list
    void updateEntityLists()
    {
        waffleList = new List<GameObject>(GameObject.FindGameObjectsWithTag("waffle").ToList());
        waffleList = new List<GameObject>(waffleList.OrderByDescending(x => x.GetComponent<EntityClass>().stats[(int)StatType.Speed].currentValue).ToList());
        for (int i = 0; i < waffleList.Count; i++)
        {
            waffleList[i].transform.position = new Vector2(-2 - (i * 2), waffleList[i].transform.position.y);
        }

        enemyList = new List<GameObject>(GameObject.FindGameObjectsWithTag("enemy").ToList());
        enemyList = new List<GameObject>(enemyList.OrderByDescending(x => x.GetComponent<EntityClass>().stats[(int)StatType.Speed].currentValue).ToList());
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].transform.position = new Vector2(2 + (i * 2), enemyList[i].transform.position.y);
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
        for (int i = 0; i < waffleList.Count; i++)
        {
            activationIndicator.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(waffleList[i].transform.position + new Vector3(0f, 1.2f, 0f));
            activationIndicator.Activate("$1");
            RunManager.Instance.money++;
            RunManager.Instance.level++;
            yield return new WaitForSeconds(1f);
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
        cardPlayManager.DrawCards(1);
    }
}