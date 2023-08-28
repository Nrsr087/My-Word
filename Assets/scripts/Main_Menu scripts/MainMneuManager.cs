using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMneuManager : MonoBehaviour
{
    public static MainMneuManager instance;
    public GeneralGameSettings m_generalSettings;
    public AudioSource mainmenuAudioSource;

    public GameObject GDPR_panel;
    public Text GDPR_Message_txt, homeCoinsNumber;
    public Button sound_btn;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        else
        {
            instance = this;
        }
    }


    void Start()
    {
        PlayerPrefs.SetInt("ClassicMode", 0);
        PlayerPrefs.SetInt("ClassicMode_score", 0);
        Screen.orientation = ScreenOrientation.Portrait;

        confirmGdprStatus();
        checksoundStatus();
        updateCoinsValue();

    }


    // audio functions
    public void playCLick_Sound()
    {
        mainmenuAudioSource.clip = m_generalSettings.click_sound;
        mainmenuAudioSource.loop = false;
        mainmenuAudioSource.Play();
    }



    //******************************************************************************
    //home game events 
    public void quitGame()
    {
        Application.Quit();
    }

    public void updateCoinsValue()
    {

        homeCoinsNumber.text = PlayerPrefs.GetInt("game_coins_number", 0).ToString();
    }




    //******************************************************************************
    //gdpr events
    #region GDPR Events
    // there are 3 status for the gdpr status player prefs
    // -1 theres is no data about the player if he accept or refuse yet
    // 0 the player decline the request of collecting data
    // 1 the player accept the request of collecting data 

    public void confirmGdprStatus()
    {
        GDPR_Message_txt.text = m_generalSettings.Gdpr_message;

        int gpdr_status = PlayerPrefs.GetInt("gdpr_status", -1);

        if (gpdr_status == -1)
        {
            showGdprPanel();    // shows for the first time
        }
        else
        {
            return;
        }


    }


    public void showGdprPanel()
    {
        GDPR_Message_txt.text = m_generalSettings.Gdpr_message;
        //PlayerPrefs.SetInt("gdpr_status", 0);
        //  0 : accept data usage
        //  1 : desmiss data usage
        //  -1 : doesnt Opened yet
        GDPR_panel.SetActive(true);
    }
    public void closeGdprPanel()
    {
        GDPR_panel.SetActive(false);
    }

    public void openPrivacyPolicy()
    {
        Application.OpenURL(m_generalSettings.PrivacyPolicyLink);
    }

    public void acceptDatausage()
    {
        PlayerPrefs.SetInt("gdpr_status", 0);   // the palyer accept then show the personalized ads 
        // GameUnityAds.instance.useraccept();
        closeGdprPanel();


    }
    public void DesmissDataUsage()
    {
        PlayerPrefs.SetInt("gdpr_status", 1);   // the user desmmiss then show the non personalized ads 
        //  GameUnityAds.instance.userDessmis();
        closeGdprPanel();
    }

    #endregion


    //******************************************************************************
    // social media events
    public void openMoreGamesLink()
    {
        Application.OpenURL(m_generalSettings.moreGamesLink);
    }

    //******************************************************************************
    // sound events
    #region  sound events
    public Sprite sound_On_sp, sound_Off_sp;
    void checksoundStatus()
    {
        if (PlayerPrefs.GetInt("game_sound_status1", 0) == 0)   // sound off =0 :: sound on=1
        {
            // the sound is off 
            // change the idon
            sound_btn.gameObject.GetComponent<Image>().sprite = sound_Off_sp;

            // mute the audio 
            mainmenuAudioSource.mute = true;

        }
        else
        {
            sound_btn.gameObject.GetComponent<Image>().sprite = sound_On_sp;

            //unmute the audio
            mainmenuAudioSource.mute = false;
        }

    }
    public void click_soundBtn()
    {
        if (PlayerPrefs.GetInt("game_sound_status1", 0) == 0)
        {
            PlayerPrefs.SetInt("game_sound_status1", 1);
        }

        else
        if (PlayerPrefs.GetInt("game_sound_status1", 0) == 1)
        {
            PlayerPrefs.SetInt("game_sound_status1", 0);
        }
        checksoundStatus();

    }
    #endregion


}
