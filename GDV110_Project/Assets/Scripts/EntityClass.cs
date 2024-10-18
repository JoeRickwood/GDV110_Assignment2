using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityClass : MonoBehaviour
{
    public string entityName;
    public float entityAttackDelay;

    public List<Upgrade> entityUpgrades = new List<Upgrade>();
    public GameObject battleManager;

    public List<Stat> stats;

    void Start()
    {
        if(battleManager == null)
        {
            battleManager = GameObject.Find("BattleManager");
        }

        entityAttackDelay = 1f;

        for (int i = 0; i < stats.Count; i++)
        {
            stats[i].Reset();
        }
    }

    //Damages entity
    public void TakeDamage(float _Damage)
    {
        stats[(int)StatType.Health].currentValue -= _Damage;

        if(stats[(int)StatType.Health].currentValue <= 0 ) 
        {
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
