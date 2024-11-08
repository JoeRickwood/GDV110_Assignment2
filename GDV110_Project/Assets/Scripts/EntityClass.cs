using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityClass : MonoBehaviour
{
    public string entityName;
    public float entityAttackDelay;

    public List<Upgrade> entityUpgrades = new List<Upgrade>();
    public GameObject battleManager;
    public GameObject entityHealthBar;

    public delegate void FloatDelegate(float damageCount);
    public delegate void UpgradeDelegate(Upgrade upgrade);
    public event FloatDelegate onTakeDamage;
    public event UpgradeDelegate onItemAdded;

    public List<Stat> stats;

    public float healthbarHeight;
    public bool isDead;

    void Start()
    {
        if(battleManager == null)
        {
            battleManager = GameObject.Find("BattleManager");
        }

        entityAttackDelay = 1f;

        ResetStats();

        entityHealthBar = Instantiate(entityHealthBar, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.65f), Quaternion.identity);
        entityHealthBar.transform.parent = this.gameObject.transform;
    }

    void Update()
    {
        entityHealthBar.transform.localScale = new Vector2((stats[(int)StatType.Health].currentValue / stats[(int)StatType.Health].baseValue) / 2f, 0.05f);
        entityHealthBar.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + healthbarHeight);
    }

    //Damages Entity
    public void TakeDamage(float _Damage)
    {
        GameObject cur = Instantiate(Resources.Load<GameObject>("Star_Particle"), transform.position + new Vector3(0f, 0.3f), Quaternion.identity);
        Destroy(cur, 2f);

        onTakeDamage?.Invoke(_Damage);
        stats[(int)StatType.Health].currentValue -= _Damage;

        if(stats[(int)StatType.Health].currentValue <= 0) 
        {
            isDead = true;
            Die();
        }
    }

    //Heals entity and clamps the health value so the entity does not heal over max health
    public void Heal(float _HealthGained)
    {
        stats[(int)StatType.Health].currentValue += _HealthGained;
        stats[(int)StatType.Health].currentValue = Mathf.Clamp(stats[(int)StatType.Health].currentValue, stats[(int)StatType.Health].currentValue, stats[(int)StatType.Health].baseValue);
    }

    //Destroys the entity
    public void Die()
    {
        Instantiate(Resources.Load<GameObject>("Death Cloud"), transform.position, Quaternion.identity);

        Destroy(this.gameObject);
    }

    //Prepares entity for destruction
    public virtual void OnDeath()
    {
        //Just in case there is any preparation to be done before entity destruction
    }

    //Adds upgrade to the list of upgrades on the entity
    public void AddUpgrade(Upgrade _UpgradeToAdd)
    {
        entityUpgrades.Add(_UpgradeToAdd);

        CalculateStats();
    }

    public void CalculateStats()
    {
        for (int i = 0; i < stats.Count; i++)
        {
            if (i != (int)StatType.Health)
            {
                stats[i].currentValue = stats[i].baseValue;
            }
        }

        for (int i = 0; i < entityUpgrades.Count; i++)
        {
            entityUpgrades[i].OnCalculateStats(this);
        }
    }

    public void ResetStats()
    {
        for (int i = 0; i < stats.Count; i++)
        {
            stats[i].currentValue = stats[i].baseValue;
        }
    }

    //Removes upgrade from the list of upgrades on the entity
    public void RemoveUpgrade(Upgrade _UpgradeToRemove)
    {
        entityUpgrades.Remove(_UpgradeToRemove);
    }

    private void OnValidate()
    {
        if(stats.Count < (int)StatType.MaxStatType)
        {
            for (int i = 0; i < ((int)StatType.MaxStatType - stats.Count); i++)
            {
                stats.Add(new Stat(0));
            }
        }
    }
}
public enum StatType : int
{
    Health = 0,
    Damage = 1,
    Speed = 2, 
    MaxStatType = 3
}

[System.Serializable]
public class Stat
{
    public float baseValue;
    public float currentValue;

    public Stat(float _BaseValue)
    {
        baseValue = _BaseValue;
    }

    public void Reset()
    {
        currentValue = baseValue;
    }
}
