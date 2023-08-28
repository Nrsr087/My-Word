using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    public static CountDownTimer instance;

    public GameLevelData currentCategory_puzzle;

    public GeneralGameSettings m_generalGameSettings;
    public GameObject extraSecondsButton, notEnoughCoinsPanel;

    public Sprite normaleCountDown_backspt, redCountDown_backspt;

    public Text timer_txt;

    private float puzzleSeconds;
    private bool isOnTime = true;
    private bool playeon_rwin_lose_events = false;
    private bool extraTimeBtnShowed = false;




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
        notEnoughCoinsPanel.SetActive(false);
        extraSecondsButton.SetActive(false);

        GetComponent<Image>().sprite = normaleCountDown_backspt;
        int currentCategoryIndex = PlayerPrefs.GetInt("currentCategorySelected", 0);
        Category curretnCategory = currentCategory_puzzle.gameCategories[currentCategoryIndex];


        puzzleSeconds = curretnCategory.PuzzleBoards[PuzzleProgressSaver.instance.retriveSelectedPuzzle(curretnCategory.category_name)].TimeInSecconds;

        if (puzzleSeconds == null && puzzleSeconds == 0)
        {
            puzzleSeconds = 35;
        }

        if (PlayerPrefs.GetInt("ClassicMode") == 0) StartCoroutine(startcountDownTimer());
        else timer_txt.text = "-:-";
    }



    IEnumerator startcountDownTimer()
    {

        yield return new WaitForSeconds(1.0f);

        while (puzzleSeconds >= 0 && playeon_rwin_lose_events == false)
        {

            float minutes = Mathf.FloorToInt(puzzleSeconds / 60);
            float seconds = Mathf.FloorToInt(puzzleSeconds % 60);
            timer_txt.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            GameSoundManager.instance.PlayCountDownSound();

            yield return new WaitForSeconds(1.0f);




            if (puzzleSeconds > 0 && puzzleSeconds <= 10)
            {


                GetComponent<Image>().sprite = redCountDown_backspt;
            }

            if (puzzleSeconds < 10 && extraTimeBtnShowed == false)
            {
                showExtraTimeBtn();
            }

            puzzleSeconds--;
        }



        if (puzzleSeconds == 0 || puzzleSeconds < 0)
        {
            puzzleSeconds = 0;
            isOnTime = false;

            GameUIEvents.instance.player_lose();
        }
    }

    public void player_win_lose(bool b)
    {
        playeon_rwin_lose_events = b;
    }

    public bool isIt_On_Time()
    {
        return isOnTime;
    }


    private void showExtraTimeBtn()
    {
        extraSecondsButton.SetActive(true);
        extraTimeBtnShowed = true;
    }


    public void addExtratimewhwilePlayerLose()
    {
        int playreCoins = PlayerPrefs.GetInt("game_coins_number", 0);

        if (playreCoins > m_generalGameSettings.CoinsNumberToAddExtraTime)
        {
            puzzleSeconds += 10;
            playeon_rwin_lose_events = false;
            isOnTime = true;

            StartCoroutine(startcountDownTimer());

        }
        else
        {
            notEnoughCoinsPanel.SetActive(true);

        }

    }

    public void addExtraTime_WhileGame_Runing()
    {
        int playreCoins = PlayerPrefs.GetInt("game_coins_number", 0);

        if (playreCoins > m_generalGameSettings.CoinsNumberToAddExtraTime)
        {
            playreCoins -= m_generalGameSettings.CoinsNumberToAddExtraTime;
            PlayerPrefs.SetInt("game_coins_number", playreCoins);

            puzzleSeconds += 10;

        }
        else
        {
            notEnoughCoinsPanel.SetActive(true);

        }
    }
}
