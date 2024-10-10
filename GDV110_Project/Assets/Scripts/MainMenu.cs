using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public ScreenTransition transition;

    public void ChangeScene(int _SceneID)
    {
        StartCoroutine(ChangeSceneCoroutine(_SceneID));
    }

    public IEnumerator ChangeSceneCoroutine(int _SceneID)
    {
        float time = 1f;

        StartCoroutine(transition.StartScreenTransition(false));

        yield return new WaitForSeconds(time);  

        SceneManager.LoadScene(_SceneID);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
