using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsRenderer : MonoBehaviour
{
    public Text levelText;
    public Text moneyText;

    public Transform heartTransform;

    public Sprite[] heartSprites;

    private void Update()
    {
        levelText.text = $"Level {RunManager.Instance.level}";
        moneyText.text = $"${RunManager.Instance.money}";

        for (int i = 0; i < heartTransform.childCount; i++)
        {
            if(i < RunManager.Instance.health)
            {
                heartTransform.GetChild(i).GetComponent<Image>().sprite = heartSprites[1];
            }
            else
            {
                heartTransform.GetChild(i).GetComponent<Image>().sprite = heartSprites[0];
            }
        }
    }
}
