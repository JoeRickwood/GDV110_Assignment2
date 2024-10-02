using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleManager : MonoBehaviour
{
    public Vector2 mousePos;
    public LayerMask entityLayer;

    public LineRenderer atkLine;
    public float atkLineLength;

    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> waffles = new List<GameObject>();

    public GameObject waffleSelected;
    public GameObject enemySelected;
    public GameObject entitySelected;

    public int turnInt;

    void Start()
    {
        waffleSelected = GameObject.FindGameObjectsWithTag("waffle")[0];
        enemySelected = GameObject.FindGameObjectsWithTag("enemy")[0];
        Cursor.lockState = CursorLockMode.None;

        for(int i = 0; i < 3; i++)
        {
            waffles.Add(GameObject.FindGameObjectsWithTag("waffle")[i]);
            enemies.Add(GameObject.FindGameObjectsWithTag("enemy")[i]);
        }

        turnInt = 0;
    }
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        for (int i = 0; i < waffles.Count; i++)
        {
            if (waffles[i].activeInHierarchy == false)
            {
                waffles.RemoveAt(i);
            }
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].activeInHierarchy == false)
            {
                enemies.RemoveAt(i);
            }
        }

        if (turnInt == 0)
        {
            playerTurn();
        } else if(turnInt == 1)
        {
            enemyTurn();
        }
    }


    GameObject mouseClick()
    {
        Collider2D hit = Physics2D.OverlapPoint(mousePos);
        Debug.Log(hit.transform.name);

        if(hit != null)
        {
            entitySelected = hit.transform.gameObject;
            return entitySelected;
        }
        else
        {
            return null;
        }
        
    }

    void playerTurn()
    {
        if (Input.GetMouseButtonDown(0) && mouseClick() != null)
        {
            if (entitySelected.tag == "waffle")
            {
                waffleSelected = entitySelected;
            }

            if (entitySelected.tag == "enemy")
            {
                enemySelected = entitySelected;
            }
        }

        if(waffleSelected != null && enemySelected != null)
        {
            atkLineLength = Vector2.Distance(waffleSelected.transform.position, enemySelected.transform.position);
            atkLine.SetPosition(0, waffleSelected.transform.position);
            atkLine.SetPosition(1, enemySelected.transform.position);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerDamageEnemy();
                Debug.Log("Player Attacked");
                turnInt = 1;
            }
        }
    }

    void playerDamageEnemy()
    {
        waffleSelected.GetComponent<entity>().dealDamage(waffleSelected.GetComponent<waffle>().damage, enemySelected);
    }

    void enemyTurn()
    {
        System.Random rand = new System.Random();

        int randomWaffle = rand.Next(0, waffles.Count);
        int randomEnemy = rand.Next(0, enemies.Count);

        waffleSelected = waffles[randomWaffle];
        enemySelected = enemies[randomEnemy];

        if(waffleSelected != null && enemySelected != null)
        {
            atkLineLength = Vector2.Distance(enemySelected.transform.position, waffleSelected.transform.position);
            atkLine.SetPosition(0, enemySelected.transform.position);
            atkLine.SetPosition(1, waffleSelected.transform.position);
         
            enemyDamagePlayer();
            Debug.Log("Enemy Attacked");
            turnInt = 0;
        }
    }

    void enemyDamagePlayer()
    {
        enemySelected.GetComponent<entity>().dealDamage(enemySelected.GetComponent<enemy>().damage, waffleSelected);
    }
}

