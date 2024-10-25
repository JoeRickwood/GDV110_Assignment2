using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnToMenuButton : MonoBehaviour
{
    public Button target;

    private void Start()
    {
        target.onClick.AddListener(() =>
        {
            RunManager.Instance.EndRun();
            SceneManager.LoadScene(0);
        });
    }
}
