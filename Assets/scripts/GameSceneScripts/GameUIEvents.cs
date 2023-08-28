using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIEvents : MonoBehaviour
{
    public static GameUIEvents instance;
    public GeneralGameSettings m_generalSettings;
    public GameData currentGameData;

    public GameLevelData m_gamelevelData;

    public GameObject pausePanel, skipPuzzle_Panel, countDownTimePanel, win_panel, lsoe_panel, notEnoughCoinsPanel, startPuzzleAnimation;
    public Button sound_btn, clockSound_btn;
    public Sprite sound_ON_sp, sound_OF_sp, clockSound_on_btn, clockSound_of_btn, redTime_backsp, normleTime_backsp;

    public Text SkipPuzzleTextMessage, categoryNmaeTxt, startAnime_categroyname, startAnime_puzzleProgress;
    public AudioSource gameAudioSource, countDownAudioSource;


    public Text winPanel_coinsNumber;


    private string currentCategoryName;

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
        Screen.orientation = ScreenOrientation.Portrait;

        pausePanel.SetActive(false);
        notEnoughCoinsPanel.SetActive(false);
        skipPuzzle_Panel.SetActive(false);
        lsoe_panel.SetActive(false);
        win_panel.SetActive(false);





        SkipPuzzleTextMessage.text = "skip current puzzle board for  " + m_generalSettings.CoinsNumberToSkipPuzzle + "  coins";

        currentCategoryName = m_gamelevelData.gameCategories[PlayerPrefs.GetInt("currentCategorySelected", 0)].category_name;
        categoryNmaeTxt.text = currentCategoryName;


        updateStartAnimationText();


        checkGameSoundStatus();
        checkClockSoundStatus();


    }



    void checkToshowInterstitialAds()
    {
        int N_Wins = PlayerPrefs.GetInt("numberOfWins", 0);
        N_Wins++;

        PlayerPrefs.SetInt("numberOfWins", N_Wins);
    }

    void updateStartAnimationText()
    {
        int puzzleProgres = PuzzleProgressSaver.instance.retriveSelectedPuzzle(currentCategoryName);
        int allcategory_count = m_gamelevelData.gameCategories[PlayerPrefs.GetInt("currentCategorySelected", 0)].PuzzleBoards.Count;

        startAnime_categroyname.text = currentCategoryName;
        startAnime_puzzleProgress.text = (puzzleProgres + 1) + "/" + allcategory_count;

        startPuzzleAnimation.SetActive(true);

        Invoke("destroyStartAnimation", 4.0f);
    }
    void destroyStartAnimation()
    {
        Destroy(startPuzzleAnimation);
    }



    #region  game panels, buttons, events functions

    //***************************************************************
    // sound status events

    private void checkClockSoundStatus()
    {
        if (PlayerPrefs.GetInt("gameClock_sound_status", 0) == 0)
        {

            // sound is off
            clockSound_btn.gameObject.GetComponent<Image>().sprite = clockSound_of_btn;
            countDownAudioSource.mute = true;

        }
        else
        {
            //sound is on 

            // sound is off
            clockSound_btn.gameObject.GetComponent<Image>().sprite = clockSound_on_btn;
            countDownAudioSource.mute = false;


        }
    }

    private void checkGameSoundStatus()
    {

        if (PlayerPrefs.GetInt("game_sound_status1", 0) == 0)   // sound off =0 :: sound on=1
        {
            // the sound is off 
            // change the idon
            sound_btn.gameObject.GetComponent<Image>().sprite = sound_OF_sp;

            // mute the audio 
            gameAudioSource.mute = true;
            countDownAudioSource.mute = true;

        }
        else
        {
            sound_btn.gameObject.GetComponent<Image>().sprite = sound_ON_sp;

            //unmute the audio
            gameAudioSource.mute = false;
            countDownAudioSource.mute = false;

        }
        checkClockSoundStatus();

    }


    public void click_clocksound_btn()
    {
        if (PlayerPrefs.GetInt("gameClock_sound_status", 0) == 0)
        {
            //clock sound on
            PlayerPrefs.SetInt("gameClock_sound_status", 1);
        }

        else
        if (PlayerPrefs.GetInt("gameClock_sound_status", 0) == 1)
        {
            //clock sund off
            PlayerPrefs.SetInt("gameClock_sound_status", 0);
        }
        checkClockSoundStatus();

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
            PlayerPrefs.SetInt("gameClock_sound_status", 0);
        }
        checkGameSoundStatus();

    }



    public void setCountDownPanelToNormale()
    {
        countDownTimePanel.GetComponent<Image>().sprite = normleTime_backsp;

    }
    public void setCountDownPanelToRed()
    {
        countDownTimePanel.GetComponent<Image>().sprite = redTime_backsp;
    }




    //***************************************************************
    // pause continue game
    public void pauseGame()
    {
        pausePanel.SetActive(true);
        timeScaleStatus(0);
    }
    public void resume_game()
    {
        pausePanel.SetActive(false);
        timeScaleStatus(1);

    }


    private void timeScaleStatus(int sta)
    {
        if (sta == 1)
        {
            // the game is running
            Time.timeScale = 1.0f;
        }
        else if (sta == 0)
        {
            // the game is stoped
            Time.timeScale = 0.0f;
        }

    }

    public void openCategoriesList()
    {
        timeScaleStatus(1);
        SceneManager.LoadScene(1);

    }
    public void go_homeMainMenu()
    {
        timeScaleStatus(1);
        SceneManager.LoadScene(0);
    }


    //***************************************************************
    // skip puzzle
    public void skipPuzzleClicked()
    {
        // open skip puzzle panel
        skipPuzzle_Panel.SetActive(true);
        timeScaleStatus(0);
    }
    public void CancelSkipPuzzle()
    {
        timeScaleStatus(1);
        skipPuzzle_Panel.SetActive(false);

        Debug.Log("skip puzzle cancled");
    }
    public void confirmSkipPuzzle()
    {
        timeScaleStatus(1);
        int playreCoins = PlayerPrefs.GetInt("game_coins_number", 0);

        if (playreCoins > m_generalSettings.CoinsNumberToSkipPuzzle)
        {

            playreCoins -= m_generalSettings.CoinsNumberToSkipPuzzle;
            PlayerPrefs.SetInt("game_coins_number", playreCoins);

            nextboard_Puzzle();
        }
        else
        {
            notEnoughCoinsPanel.SetActive(true);
        }


    }
    int getCurrentCategoryIndex()
    {
        return PlayerPrefs.GetInt("currentCategorySelected", 0);
    }



    #endregion

    #region  win lose events
    //***************************************************************
    // win lose events

    public void player_win()
    {

        CountDownTimer.instance.player_win_lose(true);   // to stop the countdown time
                                                         // show the wining panel
        win_panel.SetActive(true);


        //update win ui data
        winPanel_coinsNumber.text = m_generalSettings.collectedCoinsEch_level.ToString();
        int current_coins = PlayerPrefs.GetInt("game_coins_number", 0);
        current_coins += m_generalSettings.collectedCoinsEch_level;
        PlayerPrefs.SetInt("game_coins_number", current_coins);

    }

    public void player_lose()
    {
        CountDownTimer.instance.player_win_lose(true);   // to stop the countdown time

        lsoe_panel.SetActive(true);
    }

    // lsoe panel buttons events

    public void addextraTimeCLidked()
    {
        // if the conditions off adding extra time avilibal coins number
        int playreCoins = PlayerPrefs.GetInt("game_coins_number", 0);

        if (playreCoins > m_generalSettings.CoinsNumberToAddExtraTime)
        {
            playreCoins -= m_generalSettings.CoinsNumberToAddExtraTime;
            CountDownTimer.instance.addExtratimewhwilePlayerLose();

            PlayerPrefs.SetInt("game_coins_number", playreCoins);


            lsoe_panel.SetActive(false);
        }
        else
        {
            //show not enough coins
            notEnoughCoinsPanel.SetActive(true);
        }

    }


    public void nextboard_Puzzle()
    {
        // update the current game data to the next puzzle then load the scnee again 

        if (isLastBoardOnPuzzle() == true)
        {
            int currentcatIndex = PlayerPrefs.GetInt("currentCategorySelected", 0);
            m_gamelevelData.gameCategories[currentcatIndex + 1].IscategroyOpned = true;

            //save the category status on player prefs
            PuzzleProgressSaver.instance.saveCategoryStatus(m_gamelevelData.gameCategories[currentcatIndex + 1].category_name, true);

            PlayerPrefs.SetInt("currentCategorySelected", currentcatIndex + 1);


            //set the selected puzzle board
            PuzzleProgressSaver.instance.saveSelectedPuzzle(m_gamelevelData.gameCategories[currentcatIndex + 1].category_name, 0);
        }
        else
        {
            int cinex = PuzzleProgressSaver.instance.retrevePuzzleProgress(currentCategoryName);
            //  PuzzleProgressSaver.instance.savePuzzleData(currentCategoryName, cinex + 1);


            int selectd_PZZ_index = PuzzleProgressSaver.instance.retriveSelectedPuzzle(currentCategoryName);
            selectd_PZZ_index++;
            PuzzleProgressSaver.instance.saveSelectedPuzzle(currentCategoryName, selectd_PZZ_index);


            if (selectd_PZZ_index > cinex)
            {
                PuzzleProgressSaver.instance.savePuzzleData(currentCategoryName, selectd_PZZ_index);

            }

        }
        // then load the same scene again 
        SceneManager.LoadScene(2);
    }

    private bool isLastBoardOnPuzzle()
    {
        int currentcatIndex = PlayerPrefs.GetInt("currentCategorySelected", 0);
        Category curretncat = m_gamelevelData.gameCategories[currentcatIndex];
        int solvedindex = PuzzleProgressSaver.instance.retrevePuzzleProgress(curretncat.category_name);

        //when the category is completed jump to the next one 
        if (solvedindex == curretncat.PuzzleBoards.Count - 1)
        {
            return true;
        }
        return false;
    }

    #endregion





}
