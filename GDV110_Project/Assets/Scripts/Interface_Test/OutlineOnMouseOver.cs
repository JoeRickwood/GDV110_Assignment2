using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject outline;

    private void Start()
    {
        //outline.SetActive(false);
    }

    public void SetState(bool state)
    {
        outline.SetActive(state);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outline.SetActive(false);
    }
}
