using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBarColorLerp : MonoBehaviour
{
    public SpriteRenderer rndr;
    public Transform parent;
    public float currentHealth;
    public float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        maxHealth = parent.GetComponent<EntityClass>().stats[(int)StatType.Health].baseValue;
        rndr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = parent.GetComponent<EntityClass>().stats[(int)StatType.Health].currentValue;
        rndr.color = Color.Lerp(Color.red, Color.green, transform.localScale.x * 2);
    }
}
