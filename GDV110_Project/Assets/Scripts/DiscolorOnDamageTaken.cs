using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(EntityClass))]
public class DiscolorOnDamageTaken : MonoBehaviour
{
    public Color basecolor = Color.white;
    public Color damagedColor = Color.red;

    private void Start()
    {
        GetComponent<EntityClass>().onTakeDamage += DiscolorObject;
    }

    public void DiscolorObject(float amount)
    {
        GetComponent<SpriteRenderer>().color = damagedColor;
    }

    private void Update()
    {
        GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, basecolor, Time.deltaTime * 15f);
    }

}
