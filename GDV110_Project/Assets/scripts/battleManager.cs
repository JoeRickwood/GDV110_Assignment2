using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class battleManager : MonoBehaviour
{
    public static battleManager Instance;

    public Vector2 mousePos;
    public LayerMask entityLayer;

    public LineRenderer atkLine;
    public float atkLineLength;

    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> waffles = new List<GameObject>();

    public GameObject waffleSelected;
    public GameObject enemySelected;
    public GameObject entitySelected;

    public LayerMask Entitylayer;

    public int turnInt;

    bool inTurn;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //enemies = new List<GameObject>();
        waffles = new List<GameObject>();

        Cursor.lockState = CursorLockMode.None;

        turnInt = 0;
    }
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        for (int i = 0; i < waffles.Count; i++)
        {
            if (waffles[i] == null)
            {
                waffles.RemoveAt(i);
            }
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
            }
        }

        if (waffles.Count <= 0 || enemies.Count <= 0)
        {
            atkLine.enabled = false;
            return;
        }

        if(waffleSelected != null && enemySelected != null)
        {
            atkLine.enabled = true;
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
        Collider2D hit = Physics2D.OverlapPoint(mousePos, Entitylayer);

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
        inTurn = true;
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

        if (waffleSelected != null && enemySelected != null)
        {
            atkLineLength = Vector2.Distance(waffleSelected.transform.position, enemySelected.transform.position);
            atkLine.SetPosition(0, waffleSelected.transform.position);
            atkLine.SetPosition(1, enemySelected.transform.position);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerDamageEnemy();
                Debug.Log("Player Attacked");
                turnInt = 1;
                inTurn = false;
            }
        }
    }

    void playerDamageEnemy()
    {
        waffleSelected.GetComponent<entity>().dealDamage(waffleSelected.GetComponent<Character>().damage, enemySelected);
    }

    async void enemyTurn()
    {
        if (inTurn == true)
        {
            return;
        }

        inTurn = true;
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
            await Task.Delay(500);

            enemyDamagePlayer();
            Debug.Log("Enemy Attacked");
            turnInt = 0;
            inTurn = false;
        }
    }

    void enemyDamagePlayer()
    {
        enemySelected.GetComponent<entity>().dealDamage(enemySelected.GetComponent<enemy>().damage, waffleSelected);
    }
}

