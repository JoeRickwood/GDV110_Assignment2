using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : entity
{
    // Start is called before the first frame update
    bool enemyTurn(GameObject[] waffleList)
    {

        return true;
    }

    void Update()
    {
        if (health <= 0)
        {
            Debug.Log(transform.name + " " + "Dead");
            Destroy(gameObject);
        }
    }
}
