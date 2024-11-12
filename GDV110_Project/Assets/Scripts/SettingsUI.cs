using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("General")]
    public GameObject textButton;
    public OpenAnimation anim;

    [Header("Fullscreen Menu")]
    public Text fullscreenButton;

    [Header("Vsync Menu")]
    public Text vsyncButton;

    [Header("Volume Menu")]
    public Text musicVolume;
    public Text sfxVolume;

    public bool open;

    public void OpenSettings(bool behaviorOpen = true)
    {
        open = behaviorOpen;
        anim.Open(behaviorOpen);

        musicVolume.text = $"{GameManager.settingsManager.musicVolume + 80}";
        sfxVolume.text = $"{GameManager.settingsManager.sfxVolume + 80}";
        //Create 
    }

    public void SetFullscreenMode(bool mode)
    {
        GameManager.settingsManager.fullscreen = mode;
    }

    public void changeSFXVolume(float change)
    {
        GameManager.settingsManager.sfxVolume += change;

        GameManager.settingsManager.sfxVolume = Mathf.Clamp(GameManager.settingsManager.sfxVolume, -80f, 20f);

        sfxVolume.text = $"{GameManager.settingsManager.sfxVolume + 80}";
    }

    public void changeMusicVolume(float change)
    {
        GameManager.settingsManager.musicVolume += change;

        GameManager.settingsManager.musicVolume = Mathf.Clamp(GameManager.settingsManager.musicVolume, -80f, 20f);

        musicVolume.text = $"{GameManager.settingsManager.musicVolume + 80}";
    }

    public void SetVsyncMode(bool mode)
    {
        GameManager.settingsManager.vsync = mode;
    }

    public void ApplySettings()
    {
        GameManager.settingsManager.ApplySettings();
    }

    private void Update()
    {
        fullscreenButton.GetComponent<Text>().text = $"FULLSCREEN {(GameManager.settingsManager.fullscreen ? "True" : "False")}";
        vsyncButton.GetComponent<Text>().text = $"VSYNC {(GameManager.settingsManager.vsync ? "True" : "False")}";
    }
}
