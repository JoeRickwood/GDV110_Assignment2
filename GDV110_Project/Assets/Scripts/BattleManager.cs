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

    public bool isPlayerTurn;
    public bool attacking;
    public bool battleOngoing;
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
        if (waffleList.Count == 0 || enemyList.Count == 0)
        {
            battleOngoing = false;
        }

        //updates lists for waffles and enemies every frame
        updateEntityLists();

        //alternates turns
        if (battleOngoing == true)
        {
            if (isPlayerTurn == true)
            {
                playerTurn();
                turnText.text = "Player is attacking";
            }
            else
            {
                enemyTurn();
                turnText.text = "Enemy is attacking";
            }
        }
        else
        {
            turnText.text = "Battle Over";
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
    void playerTurn()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(attack());
        }
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
