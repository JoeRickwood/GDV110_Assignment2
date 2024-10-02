using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : entity
{
    public List<Upgrade> upgrades;

    public GameObject upgradeCountVisuals;
    public Text upgradeCountText;

    private void Start()
    {
        battleManager.Instance.waffles.Add(gameObject);

        upgrades = new List<Upgrade>();

        UpdateVisuals();
    }

    private void Update()
    {
        if (health <= 0)
        {
            Debug.Log(transform.name + " " + "Dead");
            gameObject.SetActive(false);
        }
    }

    public void AddUpgrade(Upgrade upgrade)
    {

        upgrades.Add(upgrade);

        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        if(upgrades.Count <= 0)
        {
            upgradeCountVisuals.SetActive(false);
        }
        else
        {
            upgradeCountVisuals.SetActive(true);
        }

        upgradeCountText.text = upgrades.Count.ToString();
    }
}
