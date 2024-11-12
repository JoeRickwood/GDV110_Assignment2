using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGamePause : MonoBehaviour
{
    public OpenAnimation openAnim;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            openAnim.Open(!openAnim.dir);
        }
    }
}
