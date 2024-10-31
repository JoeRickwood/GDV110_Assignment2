using UnityEngine;

public class OutlineOnMouseOver : MonoBehaviour
{
    public SpriteOutline outline;

    public bool mouseOver;

    private void Update()
    {
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
