using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public entity connectedEntity;
    public Transform scaler;

    private void Update()
    {
        scaler.localScale = new Vector3(connectedEntity.health / 100f, 1f, 1f);
    }
}
