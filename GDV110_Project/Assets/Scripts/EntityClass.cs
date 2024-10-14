using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityClass : MonoBehaviour
{
    public string entityName;
    public float entityCurrentHealth;
    public float entityMaxHealth;
    public float entityBaseDamage;
    public float entityDamage;
    public float entitySpeed;

    public List<Upgrade> entityUpgrades = new List<Upgrade>();
    public GameObject battleManager;

    //Damages entity
    public void TakeDamage(float _Damage)
    {
        entityCurrentHealth -= _Damage;

        if(entityCurrentHealth <= 0 ) 
        {
            Die();
        }
    }

    //Heals entity and clamps the health value so the entity does not heal over max health
    public void Heal(float _HealthGained)
    {
        entityCurrentHealth += _HealthGained;
        entityCurrentHealth = Mathf.Clamp(entityCurrentHealth, entityCurrentHealth, entityMaxHealth);
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
}
