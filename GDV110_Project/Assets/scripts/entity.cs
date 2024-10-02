using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entity : MonoBehaviour
{
    public float damage;
    public float health;
    public GameObject target;

    public void dealDamage(float damage, GameObject target)
    {
        target.SendMessage("takeDamage", damage); 
    }

    public void takeDamage(float damage)
    {
        health -= damage;
    }
}
