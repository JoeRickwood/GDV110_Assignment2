using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveToShopBtn : MonoBehaviour
{
    public Button btn;
    public ScreenTransition screenTransition;

    private void Start()
    {
        btn.onClick.AddListener(() => { StartCoroutine(MoveToShop()); });
    }

    public IEnumerator MoveToShop()
    {
        StartCoroutine(screenTransition.StartScreenTransition(false));
        float timer = 1f / screenTransition.speed;
        while (timer > 0f) 
        {
            timer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        SceneManager.LoadScene(2);
    }
}
