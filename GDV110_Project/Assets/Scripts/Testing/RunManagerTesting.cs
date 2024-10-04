using UnityEngine;

public class RunManagerTesting : MonoBehaviour
{
    private void Start()
    {
        RunManager.Instance.NewRun(100);

        RunManager.Instance.SaveRun();

        RunManager.Instance.LoadRun();
    }

    private void Update()
    {
        //Debug.Log(RunManager.Instance.GetRandomInt(0, 100));
    }
}
