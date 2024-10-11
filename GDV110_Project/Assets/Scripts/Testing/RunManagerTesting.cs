using UnityEngine;

public class RunManagerTesting : MonoBehaviour
{
    private void Start()
    {
        RunManager.Instance.NewRun(100);

        for (int i = 0; i < 52; i++)
        {
            RunManager.Instance.deck.AddCardStatic(RunManager.Instance.GetRandomCard());
        }
        


        RunManager.Instance.deck.ResetDeck();

        Card c = RunManager.Instance.deck.DrawCard(0);

        Debug.Log(c.ID);

        //RunManager.Instance.SaveRun();

        //RunManager.Instance.LoadRun();
    }

    private void Update()
    {
        //Debug.Log(RunManager.Instance.GetRandomInt(0, 100));
    }
}
