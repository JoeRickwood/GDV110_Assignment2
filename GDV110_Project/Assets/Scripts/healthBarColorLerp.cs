using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBarColorLerp : MonoBehaviour
{
    public Image rndr;
    public Transform parent;
    public float currentHealth;
    public float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        maxHealth = parent.GetComponent<EntityClass>().stats[(int)StatType.Health].baseValue;
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = parent.GetComponent<EntityClass>().stats[(int)StatType.Health].currentValue;
        rndr.color = Color.Lerp(Color.red, Color.green, currentHealth / maxHealth);
        rndr.fillAmount = Mathf.Lerp(rndr.fillAmount, currentHealth / maxHealth, Time.deltaTime * 5f);
    }
}
