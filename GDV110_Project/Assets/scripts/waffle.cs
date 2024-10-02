using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waffle : entity
{
    // Start is called before the first frame update
    void Start()
    {
        damage = 10;
        health = 50;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
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
