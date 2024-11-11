using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundEffect : int
{
    CardPlace,
    CardClick,
    ActivationTrigger,
    MaxSoundEffect
}

public class GameManager : MonoBehaviour
{
    //public string continueRunPath = "Continue.RUN";

    public static GameManager Instance;
    public static SettingsManager settingsManager;

    [Header("Enemy Prefabs")]
    public GameObject[] enemyPrefabs;

    [Header("Sound Effects")]
    public AudioSource source;
    public List<AudioClip> soundEffects;

    [Header("Icons")]
    public Sprite[] icons;
    public Sprite[] statIcons;

    private void Start()
    {
        if(GameManager.Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
        Init();
    }

    public void Init()
    {
        settingsManager = new SettingsManager();


        settingsManager.Init();
        settingsManager.LoadSettings();
    }

    public void PlaySFX(SoundEffect effect)
    {
        source.PlayOneShot(soundEffects[(int)effect], 1f);
        Debug.Log($"Played Sound {soundEffects[(int)effect]}");
    }

    public Sprite GetSprite(int ID)
    {
        return icons.Length <= ID ? null : icons[ID];
    }

    public Sprite GetStatIcon(int ID)
    {
        return statIcons.Length <= ID ? null : statIcons[ID];
    }

    public GameObject GetEnemyWithID(int _ID)
    {
        if(_ID >= enemyPrefabs.Length)
        {
            return null;
        }

        return enemyPrefabs[_ID];
    }

    public void OnValidate()
    {
        if (soundEffects.Count < (int)SoundEffect.MaxSoundEffect)
        {
            for (int i = 0; i < ((int)SoundEffect.MaxSoundEffect - soundEffects.Count); i++)
            {
                soundEffects.Add(null);
            }
        }
    }
}

public class SettingsManager
{
    const string SettingsFilePath = "/Settings.data";

    //---------------- AUDIO ----------------

    //
    public float masterVolume;
    public float sfxVolume;
    public float musicVolume;

    //---------------- VIDEO ----------------
    public Resolution screenResolution;
    public bool fullscreen;
    public bool vsync;

    public Resolution[] resolutions;

    AudioMixer musicAudioMixer;
    AudioMixer sfxAudioMixer;

    public void Init()
    {
        if (musicAudioMixer == null)
        {
            musicAudioMixer = Resources.Load<AudioMixer>("MUSIC");
        }

        if (sfxAudioMixer == null)
        {
            sfxAudioMixer = Resources.Load<AudioMixer>("SFX");
        }

        resolutions = Screen.resolutions;
    }

    public void ResetSettings(bool applyAfter = false)
    {
        masterVolume = 0f;
        sfxVolume = 0f;
        musicVolume = 0f;

        screenResolution = new Resolution();
        screenResolution.width = Screen.width;
        screenResolution.height = Screen.height;

        fullscreen = false;
        vsync = true;

        if(applyAfter == true)
        {
            ApplySettings();
        }
    }

    public void LoadSettings()
    {
        string path = Application.persistentDataPath + SettingsFilePath;

        screenResolution = new Resolution();
        screenResolution.width = Screen.width;
        screenResolution.height = Screen.height;

        Debug.Log("Loading From " + path);

        if(!File.Exists(path))
        {
            ResetSettings(true);
            return;
        }

        string[] lines = File.ReadAllLines(path);

        for(int i = 0; i < lines.Length; i++) 
        {
            //Only Continue Loading If The Setting Value Exists
            if (lines[i].Split(" ").Length < 2)
            {
                continue;
            }

            string val = lines[i].Split(" ")[1];

            if (lines[i].Contains("MASTER_VOLUME"))
            {
                //Assign Master Volume
                try
                {
                    masterVolume = float.Parse(val);
                }
                catch (System.Exception)
                {
                    masterVolume = 10f;
                }
            }
            else if (lines[i].Contains("MUSIC_VOLUME"))
            {
                //Assign Music Volume
                try
                {
                    musicVolume = float.Parse(val);
                }
                catch (System.Exception)
                {
                    musicVolume = -20f;
                }
            }
            else if (lines[i].Contains("SFX_VOLUME"))
            {
                //Assign SFX Volume
                try
                {
                    sfxVolume = float.Parse(val);
                }
                catch (System.Exception)
                {
                    sfxVolume = -20f;
                }
            }
            else if (lines[i].Contains("SCREEN_RESOLUTION_WIDTH"))
            {
                //Assign SCREEN RESOLUTION WIDTH
                try
                {
                    screenResolution.width = int.Parse(val);
                }
                catch (System.Exception)
                {
                    screenResolution.width = Screen.width;
                }
            }
            else if (lines[i].Contains("SCREEN_RESOLUTION_HEIGHT"))
            {
                //Assign SCREEN RESOLUTION HEIGHT
                try
                {
                    screenResolution.height = int.Parse(val);
                }
                catch (System.Exception)
                {
                    screenResolution.height = Screen.height;
                }
            }
            else if (lines[i].Contains("FULLSCREEN"))
            {
                //Assign SCREEN RESOLUTION HEIGHT
                fullscreen = false;

                if(val == "TRUE")
                {
                    fullscreen = true;
                }

                if (val == "FALSE")
                {
                    fullscreen = false;
                }
            }
            else if (lines[i].Contains("VSYNC"))
            {
                //Assign SCREEN RESOLUTION HEIGHT

                vsync = false;

                if (val == "TRUE")
                {
                    vsync = true;
                }

                if (val == "FALSE")
                {
                    vsync = false;
                }
            }
        }

        ApplySettings();
    }

    public void SaveSettings() 
    {
        string path = Application.persistentDataPath + SettingsFilePath;

        string txt = "";

        txt = txt + "\nMASTER_VOLUME " + masterVolume;
        txt = txt + "\nMUSIC_VOLUME " + musicVolume;
        txt = txt + "\nSFX_VOLUME " + sfxVolume;

        txt = txt + "\n";

        txt = txt + "\nSCREEN_RESOLUTION_WIDTH " + screenResolution.width;
        txt = txt + "\nSCREEN_RESOLUTION_HEIGHT " + screenResolution.height;
        txt = txt + "\n";
        txt = txt + "\nFULLSCREEN " + (fullscreen ? "TRUE" : "FALSE");
        txt = txt + "\n";
        txt = txt + "\nVSYNC " + (vsync ? "TRUE" : "FALSE");

        Debug.Log("Saved All Settings To : " + path);
        File.WriteAllText(path, txt);
    }

    public void ApplySettings()
    {
        //Apply Audio Settings
        AudioListener.volume = masterVolume;
        musicAudioMixer.SetFloat("Volume", musicVolume);
        sfxAudioMixer.SetFloat("Volume", sfxVolume);

        //Apply Screen Settings
        Screen.SetResolution(screenResolution.width, screenResolution.height, fullscreen);
        QualitySettings.vSyncCount = vsync ? 1 : 0;

        SaveSettings();
    }
} 
