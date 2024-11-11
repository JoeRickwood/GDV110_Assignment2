using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class BattleManager : MonoBehaviour
{
    public Bell bell;

    public ActivationIndicator activationIndicator;

    public CardPlayManager cardPlayManager;

    public RoundWinScreen roundWinScreen;

    public ObjectShake cameraShake;

    public List<GameObject> waffleList = new List<GameObject>();
    public List<GameObject> enemyList = new List<GameObject>();

    public GameObject gameLossGraphic;
    public GameObject gameWinGraphic;

    public bool notAttacking;
    public bool battleStartable;
    public bool canPlayCards;
    public bool gameStarted;
    public bool gameEnded;

    public bool isActive;

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
        isActive = true;

        gameLossGraphic.SetActive(false);
        gameWinGraphic.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive)
        {
            return;
        }

        FindObjectOfType<CardPlayManager>().handActive = canPlayCards;

        //updates lists for waffles and enemies every frame
        updateEntityLists();

        if (RunManager.Instance.health <= 0)
        {
            bell.isActive = false;
            canPlayCards = false;
            isActive = false;
            gameEnded = true;
            FindObjectOfType<CardPlayManager>().handActive = false;
            gameLossGraphic.SetActive(true);
            gameLossGraphic.GetComponent<MoveToPosition>().isActive = true;
            //Battle Over Player Lost
        }
        else if (enemyList.Count == 0 && waffleList.Count > 0)
        {
            bell.isActive = false;
            canPlayCards = false;
            isActive = false;
            EndBattle();
            gameEnded = true;
            //Battle Over
        }
    }

    public void StartBattlePhase()
    {
        StartCoroutine(battlePhase());
        for (int i = 0; i < waffleList.Count; i++)
        {
            for (int j = 0; j < waffleList[i].GetComponent<EntityClass>().entityUpgrades.Count; j++)
            {
                waffleList[i].GetComponent<EntityClass>().entityUpgrades[j].OnCombatEnd();
            }
            
        }
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < Random.Range(1 + (RunManager.Instance.level / 5), 3 + (RunManager.Instance.level / 5) * 1.2f); i++)
        {
            GameObject toSpawn = GameManager.Instance.GetEnemyWithID(Random.Range(0, 3));

            if(toSpawn != null)
            {
                GameObject obj = Instantiate(toSpawn, null);
                GameObject.FindObjectOfType<RoundWinScreen>().enemiesKilledSprites.Add(obj.GetComponent<EntityClass>().entityIcon);
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

        float damage = dealer.GetComponent<EntityClass>().stats[(int)StatType.Damage].currentValue;

        for (int i = 0; i < dealer.GetComponent<EntityClass>().entityUpgrades.Count; i++)
        {
            dealer.GetComponent<EntityClass>().entityUpgrades[i].OnAttack(receiver.GetComponent<EntityClass>(), ref damage);
        }

        if (damage > 0)
        {
            receiver.GetComponent<EntityClass>().TakeDamage(damage);

            if (receiver.GetComponent<EntityClass>().isDead == true)
            {
                activationIndicator.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(receiver.transform.position + new Vector3(0f, 1.2f, 0f));
                activationIndicator.Activate("Dead");
            }
        } else
        {
            activationIndicator.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(dealer.transform.position + new Vector3(0f, 1.2f, 0f));
            activationIndicator.Activate("Skip");
        }

        notAttacking = true;
    }

    void EndBattle()
    {
        RunManager.Instance.level++;
        FindObjectOfType<CardPlayManager>().handActive = false;

        if(RunManager.Instance.level == 11)
        {
            gameWinGraphic.SetActive(true);
            gameWinGraphic.GetComponent<MoveToPosition>().isActive = true;
            return;
        }

        StartCoroutine(EndBattleSecondPhase());
    }

    public void StartEndBattleSecondPhase(GameObject objToDisable)
    {
        objToDisable.SetActive(false);
        StartCoroutine(EndBattleSecondPhase());
    }

    IEnumerator EndBattleSecondPhase()
    {
        roundWinScreen.RenderOutMenu();
        RunManager.Instance.health = 3;

        RunManager.Instance.money += 5;

        for (int i = 0; i < waffleList.Count; i++)
        {
            RunManager.Instance.money++;
        }

        for (int i = 0; i < waffleList.Count; i++)
        {
            activationIndicator.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(waffleList[i].transform.position + new Vector3(0f, 1.2f, 0f));
            activationIndicator.Activate("$1");
            yield return new WaitForSeconds(1.5f);
        }
    }

    IEnumerator battlePhase()
    {
        if (waffleList.Count <= 0 || enemyList.Count <= 0)
        {
            yield return false;
        }

        for (int i = 0; i < waffleList.Count; i++)
        {
            waffleList[i].GetComponent<EntityClass>().CalculateStats();
        }

        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].GetComponent<EntityClass>().CalculateStats();
        }

        canPlayCards = false;
        bell.isActive = false;
        battleStartable = false;

        for (int i = 0; i < waffleList.Count; i++)
        {
            if (waffleList.Count <= 0 || enemyList.Count <= 0)
            {
                break;
            }
            StartCoroutine(attack(enemyList[0], waffleList[i]));
            yield return new WaitUntil(() => notAttacking);
        }

        for (int i = 0; i < enemyList.Count; i++)
        {
            if(waffleList.Count <= 0)
            {
                yield return new WaitForSeconds(1f);
                RunManager.Instance.health--;
                StartCoroutine(cameraShake.ShakeOverTime(0.2f));
                continue;
            }else if(enemyList.Count <= 0)
            {
                break;
            }

            StartCoroutine(attack(waffleList[0], enemyList[i]));
            yield return new WaitUntil(() => notAttacking);
        }

        battleStartable = true;
        bell.isActive = true;
        canPlayCards = true;

        int count = Mathf.Clamp(5 - cardPlayManager.cardsInHand, 0, 5);
        for (int i = 0; i < count; i++)
        {
            cardPlayManager.DrawCards(1);
            yield return new WaitForSeconds(0.1f);
        }   
    }
}