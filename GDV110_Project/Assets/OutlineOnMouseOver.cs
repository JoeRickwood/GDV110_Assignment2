using UnityEngine;

public class OutlineOnMouseOver : MonoBehaviour
{
    public SpriteOutline outline;

    public bool mouseOver;
    public bool isActive;

    private void Update()
    {
        if (!isActive)
        {
            outline.outlineSize = 0f;
            return;
        }

        Collider2D col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.2f);
        if(col == GetComponent<Collider2D>())
        {
            mouseOver = true;
        } else
        {
            mouseOver= false;
        }


        if(mouseOver)
        {
            outline.outlineSize = 5f;
        }else
        {
            outline.outlineSize = 0f;
        }
    }
}
