using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyText : MonoBehaviour
{
    public Text text;

    private void Update()
    {
        GetComponent<Text>().text = text.text;
    }
}
