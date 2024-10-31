using UnityEngine;

public class Bell : MonoBehaviour
{
    public BattleManager BattleManager;
    public Animator anim;

    public void Ding()
    {
        anim.SetTrigger("Ding");
        BattleManager.StartBattlePhase();
    }
}
