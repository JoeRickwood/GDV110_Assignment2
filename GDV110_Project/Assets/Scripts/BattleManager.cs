using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro; 

public class BattleManager : MonoBehaviour
{
    public GameObject selectedEntity;

    public List<GameObject> waffleList = new List<GameObject>();
    public List<GameObject> enemyList = new List<GameObject>();

    public Canvas battleUI;
    public TMP_Text turnText;


    public GameObject attackBtn;
    public GameObject nextRoundBtn;
    public GameObject returnToMenuBtn;

    public bool isPlayerTurn;
    public bool attacking;
    public bool battleOngoing;
    public bool playerWon;
    public bool anythingPlayed;

    // Start is called before the first frame update
    void Start()
    {
        //find all waffles and enemies in the scene and adds them to their respective list (just needed for making the system, probably can be taken out when properly implemented because waffles and enemies won't exist before scene start)
        waffleList = GameObject.FindGameObjectsWithTag("waffle").ToList();
        waffleList = waffleList.OrderByDescending(x => x.GetComponent<EntityClass>().stats[(int)StatType.Speed].currentValue).ToList();
        for (int i = 0; i < waffleList.Count; i++)
        {
            waffleList[i].transform.position = new Vector2(-2 - (i * 2), waffleList[i].transform.position.y);
        }

        enemyList = GameObject.FindGameObjectsWithTag("enemy").ToList();
        enemyList = enemyList.OrderByDescending(x => x.GetComponent<EntityClass>().stats[(int)StatType.Speed].currentValue).ToList();
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].transform.position = new Vector2(2 + (i * 2), enemyList[i].transform.position.y);
        }

        isPlayerTurn = true;
        battleOngoing = true;

        FindObjectOfType<CardPlayManager>().DrawCards(7);
    }

    // Update is called once per frame
    void Update()
    {
        //checks mouse input
        if (Input.GetMouseButton(0))
        {
            MouseClick();
        }

        //checks if either side has no fighters left, if not, battle ends
        if ((waffleList.Count == 0 || enemyList.Count == 0) && anythingPlayed == true)
        {
            battleOngoing = false;
            if(waffleList.Count > 0)
            {
                playerWon = true;
            }else
            {
                playerWon = false;
            }
        }else
        {
            battleOngoing = true;
        }

        //updates lists for waffles and enemies every frame
        updateEntityLists();

        //alternates turns
        if (battleOngoing == true)
        {
            if (isPlayerTurn == true)
            {
                attackBtn.SetActive(true);
                //playerTurn();
                turnText.text = "Player is attacking";
            }
            else
            {
                attackBtn.SetActive(false);
                enemyTurn();
                turnText.text = "Enemy is attacking";
            }
        }
        else
        {
            if(playerWon)
            {
                returnToMenuBtn.SetActive(false);
                nextRoundBtn.SetActive(true);       
                turnText.text = "Battle Over";
            }
            else
            {
                returnToMenuBtn.SetActive(true);
                returnToMenuBtn.SetActive(false);
                turnText.text = "You Lose";
            }

            attackBtn.SetActive(false);

            battleOver();
        }
        
    }

    //logic for clicking waffles/enemies in the scene
    void MouseClick()
    {
        if(Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) != null)
        {
            selectedEntity = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).gameObject;
        }
        else
        {
            selectedEntity = null;
        }
    }

    //player turn
    public void playerTurn()
    {
        StartCoroutine(attack());
    }

    //enemy turn
    void enemyTurn()
    {
        if(attacking == false)
        {
            StartCoroutine(attack());
            attacking = true;
        }
    }

    //battle finished, will be added to when implemented in with other scenes
    void battleOver()
    {
        Debug.Log("battle finished");
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
    IEnumerator attack()
    {
        GameObject receiver;
        GameObject dealer;
        if(isPlayerTurn == true)
        {
            receiver = enemyList[0];
            dealer = waffleList[0];
        }
        else
        {
            receiver = waffleList[0];
            dealer = enemyList[0];
        }
        
        yield return new WaitForSeconds(dealer.GetComponent<EntityClass>().entityAttackDelay);
        receiver.GetComponent<EntityClass>().TakeDamage(dealer.GetComponent<EntityClass>().stats[(int)StatType.Damage].currentValue);
        isPlayerTurn = !isPlayerTurn;
        attacking = false;
    }
}
