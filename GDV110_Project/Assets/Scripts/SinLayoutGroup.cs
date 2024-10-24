using UnityEngine;

[ExecuteAlways]
public class SinLayoutGroup : MonoBehaviour
{
    public float curveScale;
    public float frequency;
    public float spacing;
    public float offset;

    private void Update()
    {
        PositionObjects();
    }

    public void PositionObjects()
    {
        Vector3 pos = transform.GetComponent<RectTransform>().position;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<RectTransform>().position = pos + new Vector3((i - ((float)transform.childCount / 2f)) * spacing, Mathf.Cos(((float)i / frequency) + offset) * curveScale, 0f);
        }
    }
}
