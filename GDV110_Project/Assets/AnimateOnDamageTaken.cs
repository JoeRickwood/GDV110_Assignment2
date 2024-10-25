using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityClass))]
public class AnimateOnDamageTaken : MonoBehaviour
{
    public Animator anim;
    
    private void Start()
    {
        GetComponent<EntityClass>().onTakeDamage += OnDamagetaken;
    }
    public void OnDamagetaken(float damage)
    {
        anim.SetTrigger("Hurt");
    }

}
