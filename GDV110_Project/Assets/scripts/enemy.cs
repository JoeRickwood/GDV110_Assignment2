using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : entity
{
    // Start is called before the first frame update
    void Start()
    {
        damage = 5;
        health = 50;
    }

    bool enemyTurn(GameObject[] waffleList)
    {

        return true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            dealDamage(damage, target); 
        }

        if (health <= 0)
        {
            Debug.Log(transform.name + " " + "Dead");
            gameObject.SetActive(false);
        }
    }
}
