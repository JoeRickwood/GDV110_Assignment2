using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityClass))]
public class ScaleEntityStats : MonoBehaviour
{
    private void Start()
    {
        EntityClass entity = GetComponent<EntityClass>();

        for (int i = 0; i < entity.stats.Count; i++)
        {
            float newVal = entity.stats[i].baseValue * RunManager.Instance.DifficultyValue();

            entity.stats[i].baseValue = newVal;
            entity.stats[i].currentValue = newVal;
        }       
    }
}
