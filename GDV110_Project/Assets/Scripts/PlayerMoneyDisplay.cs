using UnityEngine;
using UnityEngine.UI;

public class PlayerMoneyDisplay : MonoBehaviour
{
    public Text text;

    private void Update()
    {
        text.text = $"${RunManager.Instance.money}";
    }
}
