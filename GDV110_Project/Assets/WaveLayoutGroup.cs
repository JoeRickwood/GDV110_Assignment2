using UnityEngine;

[ExecuteAlways]
public class WaveLayoutGroup : MonoBehaviour
{
    public float curveScale;
    public float spacing;
    public float rotScale;

    private void Update()
    {
        PositionObjects();
    }

    public void PositionObjects()
    {
        Vector3 pos = transform.GetComponent<RectTransform>().position;

        for (int i = 0; i < transform.childCount; i++)
        {
            float y = (transform.childCount - 1) - Mathf.Abs(i - ((transform.childCount - 1) / 2f)) * curveScale;

            float rotZ = (i - ((transform.childCount - 1) / 2f)) * -rotScale;

            transform.GetChild(i).GetComponent<RectTransform>().position = pos + new Vector3((i - ((float)(transform.childCount - 1) / 2f)) * spacing, y, 0f);
            transform.GetChild(i).GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
    }
}
