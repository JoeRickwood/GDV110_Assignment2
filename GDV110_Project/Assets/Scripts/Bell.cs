using UnityEngine;

public class Bell : MonoBehaviour
{
    public new SpriteRenderer renderer;
    public BattleManager BattleManager;
    public OutlineOnMouseOver mouseOver;
    public Animator anim;
    public bool isActive = true;
    public GameObject dingParticlePrefab;

    public Color activeColor;
    public Color inactiveColor;

    public GameObject visualIndicator;


    public void Ding()
    {
        Destroy(Instantiate(dingParticlePrefab, transform.position, Quaternion.Euler(-90f, 0f, 0f)), 2f);
        anim.SetTrigger("Ding");
        BattleManager.StartBattlePhase();
    }

    private void Update()
    {
        if(mouseOver.mouseOver == true && isActive == true)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Ding();
            }
        }

        mouseOver.isActive = isActive;
        if (isActive == true)
        {
            visualIndicator.SetActive(true);
            renderer.color = activeColor;
        }
        else
        {
            visualIndicator.SetActive(false);
            renderer.color = inactiveColor;
        }
    }
}
