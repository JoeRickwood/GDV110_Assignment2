using UnityEngine;

[ExecuteAlways]
public class WaveLayoutGroup : MonoBehaviour
{
    RectTransform rect;
    public float curveScale;
    public float rotScale;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

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

            Vector3 newPos = new Vector3(Mathf.Lerp(0f, rect.rect.width, (float)i / (float)transform.childCount), y, 0f);

            transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition, newPos, Time.deltaTime * 20f);
            transform.GetChild(i).GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
    }
}
