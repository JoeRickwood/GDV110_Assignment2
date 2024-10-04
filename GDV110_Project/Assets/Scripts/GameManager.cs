using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public string continueRunPath = "Continue.RUN";

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }
}
