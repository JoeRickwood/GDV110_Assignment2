using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameManager : MonoBehaviour
{
    public bool autoStart = false;
    public int sceneIndex = 3;

    public ScreenTransition transition;

    int currentDifficultyValue;
    public Text difficultyText;

    Difficulty[] difficultyArray;

    [Header("User Interface")]
    public Button increaseDifficulty;
    public Button decreaseDifficulty;

    private void Start()
    {
        difficultyArray = new Difficulty[] {
            Difficulty.Easy,
            Difficulty.Normal,
            Difficulty.Hard
        };

        if (autoStart)
        {
            StartNewGame();
        }
        else
        {
            increaseDifficulty.onClick.AddListener(() =>
            {
                currentDifficultyValue++;
                if (currentDifficultyValue > difficultyArray.Length - 1)
                {
                    currentDifficultyValue = 0;
                }

                difficultyText.text = $"Difficulty : {Enum.GetName(typeof(Difficulty), difficultyArray[currentDifficultyValue])}";
            });

            decreaseDifficulty.onClick.AddListener(() =>
            {
                currentDifficultyValue--;
                if (currentDifficultyValue < 0)
                {
                    currentDifficultyValue = difficultyArray.Length - 1;
                }

                difficultyText.text = $"Difficulty : {Enum.GetName(typeof(Difficulty), difficultyArray[currentDifficultyValue])}";
            });
        }
        
    }

    public void StartNewGame()
    {
        StartCoroutine(StartNewGameCoroutine());
    }

    public IEnumerator StartNewGameCoroutine()
    {
        StartCoroutine(transition.StartScreenTransition(false));

        RunManager.Instance.NewRun(UnityEngine.Random.Range(0, 10000), difficultyArray[currentDifficultyValue]);

        float t = 1f / transition.speed;

        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        SceneManager.LoadScene(sceneIndex);
    }
}
