using UnityEngine;

public class OutlineOnMouseOver : MonoBehaviour
{
    public SpriteOutline outline;

    public bool mouseOver;
    public bool isActive;

    private void Update()
    {
        if(!isActive)
        {
            outline.outlineSize = 0f;
            return;
        }

        if(mouseOver)
        {
            outline.outlineSize = 5f;
        }else
        {
            outline.outlineSize = 0f;
        }
    }

    private void OnMouseEnter()
    {
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;    
    }
}
