using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsRenderer : MonoBehaviour
{
    public Text levelText;
    public Text moneyText;

    private void Update()
    {
        levelText.text = $"Level {RunManager.Instance.level}";
        moneyText.text = $"${RunManager.Instance.money}";
    }
}
