using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("General")]
    public GameObject mainElements;

    //Template Prefab To Instantiating Inside Of The Resolutions Screen
    public GameObject textButtonPrefab;

    [Header("Menu")]
    public Button applyButton;
    public Button settingsBackButton;

    GameObject resolutionButton;
    GameObject fullscreenButton;
    GameObject vsyncButton;

    [Header("Screen")]
    public GameObject resolutionsParent;
    public GameObject fullscreenParent;
    public GameObject vsyncParent;

    [Header("Volume")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;


    public void Start()
    {
        mainElements.SetActive(true);

        resolutionsParent.SetActive(false);
        fullscreenParent.SetActive(false);  
        vsyncParent.SetActive(false);

        applyButton.onClick.AddListener(() =>
        {
            GameManager.settingsManager.ApplySettings();
        });

        settingsBackButton.onClick.AddListener(() => 
        { 
            resolutionsParent.SetActive(false);
            fullscreenParent.SetActive(false);
            vsyncParent.SetActive(false);
            mainElements.SetActive(true);
        });

        //RESOLUTION
        resolutionButton = Instantiate(textButtonPrefab, mainElements.transform);
        resolutionButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            resolutionsParent.SetActive(true);
            mainElements.SetActive(false);
        });
        resolutionButton.GetComponent<Text>().text = $"Resolution {GameManager.settingsManager.screenResolution.width}x {GameManager.settingsManager.screenResolution.height}";

        //FULLSCREEN
        fullscreenButton = Instantiate(textButtonPrefab, mainElements.transform);
        fullscreenButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            fullscreenParent.SetActive(true);
            mainElements.SetActive(false);
        });
        fullscreenButton.GetComponent<Text>().text = $"Fullscreen {(GameManager.settingsManager.fullscreen ? "TRUE" : "FALSE")}";

        //VSYNC
        vsyncButton = Instantiate(textButtonPrefab, mainElements.transform);
        vsyncButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            vsyncParent.SetActive(true);
            mainElements.SetActive(false);
        });
        vsyncButton.GetComponent<Text>().text = $"VSYNC {(GameManager.settingsManager.vsync ? "TRUE" : "FALSE")}";

        vsyncButton.transform.SetSiblingIndex(1);
        fullscreenButton.transform.SetSiblingIndex(1);
        resolutionButton.transform.SetSiblingIndex(1);


        //Assign Slider Function
        masterSlider.onValueChanged.AddListener((float t) =>
        {
            GameManager.settingsManager.masterVolume = t;
        });

        sfxSlider.onValueChanged.AddListener((float t) =>
        {
            GameManager.settingsManager.sfxVolume = t;
        });

        musicSlider.onValueChanged.AddListener((float t) =>
        {
            GameManager.settingsManager.musicVolume = t;
        });


        //ASSIGN RESOLUTION BUTTONS
        for (int i = 0; i < GameManager.settingsManager.resolutions.Length; i++)
        {

            int x = i;

            GameObject cur = Instantiate(textButtonPrefab, resolutionsParent.transform);
            cur.GetComponent<Text>().text = $"{GameManager.settingsManager.resolutions[i].width}x {GameManager.settingsManager.resolutions[i].height}";

            cur.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log(x);
                GameManager.settingsManager.screenResolution = GameManager.settingsManager.resolutions[x];
                mainElements.SetActive(true);
                resolutionsParent.SetActive(false);
                resolutionButton.GetComponent<Text>().text = $"Resolution : {GameManager.settingsManager.resolutions[x].width}x {GameManager.settingsManager.resolutions[x].height}";
            });
        }

        //ASSIGN FULLSCREEN BUTTONS
        GameObject curfs = Instantiate(textButtonPrefab, fullscreenParent.transform);
        curfs.GetComponent<Text>().text = $"TRUE";
        curfs.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameManager.settingsManager.fullscreen = true;
            mainElements.SetActive(true);
            fullscreenParent.SetActive(false);
            fullscreenButton.GetComponent<Text>().text = $"Fullscreen {(GameManager.settingsManager.fullscreen ? "TRUE" : "FALSE")}";
        });

        curfs = Instantiate(textButtonPrefab, fullscreenParent.transform);
        curfs.GetComponent<Text>().text = $"FALSE";
        curfs.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameManager.settingsManager.fullscreen = false;
            mainElements.SetActive(true);
            fullscreenParent.SetActive(false);
            fullscreenButton.GetComponent<Text>().text = $"Fullscreen {(GameManager.settingsManager.fullscreen ? "TRUE" : "FALSE")}";
        });


        //ASSIGN VSYNC BUTTONS
        GameObject curvs = Instantiate(textButtonPrefab, vsyncParent.transform);
        curvs.GetComponent<Text>().text = $"TRUE";
        curvs.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameManager.settingsManager.vsync = true;
            mainElements.SetActive(true);
            vsyncParent.SetActive(false);
            vsyncButton.GetComponent<Text>().text = $"VSYNC {(GameManager.settingsManager.vsync ? "TRUE" : "FALSE")}";
        });

        curvs = Instantiate(textButtonPrefab, vsyncParent.transform);
        curvs.GetComponent<Text>().text = $"FALSE";
        curvs.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameManager.settingsManager.vsync = false;
            mainElements.SetActive(true);
            vsyncParent.SetActive(false);
            vsyncButton.GetComponent<Text>().text = $"VSYNC {(GameManager.settingsManager.vsync ? "TRUE" : "FALSE")}";
        });
    }
}
