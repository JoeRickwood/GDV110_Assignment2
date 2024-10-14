using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject selectedEntity;
    public List<GameObject> waffleList = new List<GameObject>();
    public List<GameObject> enemyList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            MouseClick();
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            Instantiate(selectedEntity, new Vector3(selectedEntity.transform.position.x, selectedEntity.transform.position.y + 1f, selectedEntity.transform.position.z), Quaternion.identity);
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            selectedEntity.SendMessage("Die");
        }
    }

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

    void playerTurn()
    {

    }

    void enemyTurn()
    {

    }
}
