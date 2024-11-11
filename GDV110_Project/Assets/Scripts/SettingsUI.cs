using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("General")]
    public GameObject textButton;

    [Header("Open Animation")]
    public AnimationCurve openCurve;
    public RectTransform mainRect;
    float t;
    public float speed;

    [Header("Fullscreen Menu")]
    public Text fullscreenButton;


    [Header("Vsync Menu")]
    public Text vsyncButton;

    public bool open;

    public void OpenSettings(bool behaviorOpen = true)
    {
        open = behaviorOpen;
        t = 0f;
        //Create 
    }

    public void SetFullscreenMode(bool mode)
    {
        GameManager.settingsManager.fullscreen = mode;
    }

    public void ApplySettings()
    {
        GameManager.settingsManager.ApplySettings();
    }

    private void Update()
    {
        t += Time.deltaTime * speed;
        t = Mathf.Clamp01(t);

        if(open) //Settings Menu Open
        {
            mainRect.anchoredPosition = Vector3.Lerp(new Vector3(0, -mainRect.sizeDelta.y, 0), new Vector3(0, mainRect.sizeDelta.y / 2, 0), openCurve.Evaluate(t));
            mainRect.gameObject.SetActive(true);
        }else //Settings Menu Closed
        {
            mainRect.anchoredPosition = Vector3.Lerp(new Vector3(0, mainRect.sizeDelta.y / 2, 0), new Vector3(0, -mainRect.sizeDelta.y, 0), openCurve.Evaluate(t));
            if(t >= 1f)
            {
                mainRect.gameObject.SetActive(false);
            }
        }

        fullscreenButton.GetComponent<Text>().text = $"FULLSCREEN {(GameManager.settingsManager.fullscreen ? "True" : "False")}";
        vsyncButton.GetComponent<Text>().text = $"VSYNC {(GameManager.settingsManager.vsync ? "True" : "False")}";
    }
}
