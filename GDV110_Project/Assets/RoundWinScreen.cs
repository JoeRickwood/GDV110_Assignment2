using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundWinScreen : MonoBehaviour
{
    public GameObject[] components;

    public List<Sprite> wafflesPlayedSprites;
    public List<Sprite> enemiesKilledSprites;

    public Transform wafflesPlayedTransform;
    public Transform enemiesKilledTransform;

    public Text waffleMoneyText;
    public Text totalText;

    private void Start()
    {
        wafflesPlayedSprites = new List<Sprite>();
        enemiesKilledSprites = new List<Sprite>();
    }

    public void RenderOutMenu()
    {
        StartCoroutine(RenderOutMenuCoroutine());
    }

    public IEnumerator RenderOutMenuCoroutine()
    {
        waffleMoneyText.text = $"Remaining Waffles : ${FindObjectOfType<BattleManager>().waffleList.Count}";
        totalText.text = $"Total : ${FindObjectOfType<BattleManager>().waffleList.Count + 5}";

        for (int i = 0; i < wafflesPlayedSprites.Count; i++)
        {
            GameObject go = new GameObject($"Waffles Played {i}", typeof(RectTransform), typeof(Image), typeof(CanvasRenderer));
            go.GetComponent<Image>().sprite = wafflesPlayedSprites[i];
            go.transform.parent = wafflesPlayedTransform;
        }

        for (int i = 0; i < enemiesKilledSprites.Count; i++)
        {
            GameObject go = new GameObject($"Enemies Killed {i}", typeof(RectTransform), typeof(Image), typeof(CanvasRenderer));
            go.GetComponent<Image>().sprite = enemiesKilledSprites[i];
            go.transform.parent = enemiesKilledTransform;
        }


        for (int i = 0; i < components.Length; i++)
        {
            components[i].gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < components.Length; i++)
        {
            components[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
