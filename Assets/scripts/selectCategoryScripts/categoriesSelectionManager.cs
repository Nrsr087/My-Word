using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class categoriesSelectionManager : MonoBehaviour
{

    public AudioSource audioSource;
    public GeneralGameSettings generalGameSettings;
    public GameLevelData m_gameCategories;
    public GameData currentGameData;
    public GameObject listCategoriesParent, UnlockCategoryPanel, noEnoughStars, levelList_GameObject;
    public Button categoryButtonPrefab;

    public Text coinsNumberToOpen;

    private List<Button> categoriesButtons;
    private int solvedPuzzles, categoryWantedUnlockIndex;




    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        noEnoughStars.SetActive(false);
        levelList_GameObject.SetActive(false);

        setupCategoriesList();

        checkSoundStatus();


    }

    void checkSoundStatus()
    {
        if (PlayerPrefs.GetInt("game_sound_status1", 0) == 0)
        {
            audioSource.mute = true;

        }
        else
        {
            audioSource.mute = false;

        }

    }

    public void playclickAudio()
    {
        audioSource.clip = generalGameSettings.click_sound;
        audioSource.loop = false;
        audioSource.Play();
    }



    public void setupCategoriesList()
    {
        categoriesButtons = new List<Button>();

        for (int i = 0; i < m_gameCategories.gameCategories.Count; i++)
        {
            int copy = i + 1;
            Button currentButton = Instantiate(categoryButtonPrefab, categoryButtonPrefab.transform.position, Quaternion.identity);
            currentButton.transform.SetParent(listCategoriesParent.transform, false);

            currentButton.onClick.AddListener(() => categorySelectedToOpen(copy));


            categoriesButtons.Add(currentButton);


        }

        settupTheLivelListButtons(categoriesButtons);



    }

    void settupTheLivelListButtons(List<Button> buttons)
    {

        Button[] newArray = buttons.ToArray();


        for (int i = 0; i < m_gameCategories.gameCategories.Count; i++)
        {
            newArray[i].transform.GetChild(0).GetComponentInChildren<Text>().text = m_gameCategories.gameCategories[i].category_name;

            float solvedPuzzles = PuzzleProgressSaver.instance.retrevePuzzleProgress(m_gameCategories.gameCategories[i].category_name);  



            if (PuzzleProgressSaver.instance.retreveCatgeoryStatus(m_gameCategories.gameCategories[i].category_name) == true)
            {
    

                m_gameCategories.gameCategories[i].IscategroyOpned = true;

            }


            if (m_gameCategories.gameCategories[i].IscategroyOpned == true)
            {
                newArray[i].gameObject.transform.GetChild(2).gameObject.SetActive(false);   
                PuzzleProgressSaver.instance.saveCategoryStatus(m_gameCategories.gameCategories[i].category_name, true);
            }


            newArray[i].transform.GetChild(1).GetComponent<Slider>().value
           = getCategoryProgress(solvedPuzzles, m_gameCategories.gameCategories[i].PuzzleBoards.Count);


            if (solvedPuzzles == 0)
            {
                newArray[i].transform.GetChild(1).transform.GetChild(2).GetComponent<Text>().text
                = solvedPuzzles + "/" + m_gameCategories.gameCategories[i].PuzzleBoards.Count;
            }
            else
            {

                newArray[i].transform.GetChild(1).transform.GetChild(2).GetComponent<Text>().text
                = (solvedPuzzles + 1) + "/" + m_gameCategories.gameCategories[i].PuzzleBoards.Count;
            }



        }

    }

    float getCategoryProgress(float currentPuzzle, float allPuzzles)
    {
        if (currentPuzzle == 0)
        {
            return currentPuzzle / allPuzzles;
        }
        else
        {
            return (currentPuzzle + 1) / allPuzzles;

        }


    }

    public void categorySelectedToOpen(int categoryIndex)
    {
        playclickAudio();

        int currentCategorySelectedIndex = categoryIndex - 1;

        if (m_gameCategories.gameCategories[currentCategorySelectedIndex].IscategroyOpned == true)
        {

            Category currentCategory = m_gameCategories.gameCategories[currentCategorySelectedIndex];


            PlayerPrefs.SetInt("currentCategorySelected", currentCategorySelectedIndex);     
            this.gameObject.SetActive(false);
            levelList_GameObject.SetActive(true);

            levelsList.instance.setupLevelLIst(currentCategory);


        }
        else
        {

            categoryWantedUnlockIndex = currentCategorySelectedIndex;
            showUnoackPanel();

        }



    }



    #region  unlock categories events
    private void showUnoackPanel()
    {
        UnlockCategoryPanel.SetActive(true);

        coinsNumberToOpen.text = "unock the categroy for " + m_gameCategories.gameCategories[categoryWantedUnlockIndex].coinsToUnlock + " coins";


    }

    public void unlockcategory()
    {
        int playercoins = PlayerPrefs.GetInt("game_coins_number", 0);

        if (playercoins > m_gameCategories.gameCategories[categoryWantedUnlockIndex].coinsToUnlock)
        {
            playercoins -= m_gameCategories.gameCategories[categoryWantedUnlockIndex].coinsToUnlock;

            PuzzleProgressSaver.instance.saveCategoryStatus(m_gameCategories.gameCategories[categoryWantedUnlockIndex].category_name, true);

            m_gameCategories.gameCategories[categoryWantedUnlockIndex].IscategroyOpned = true;
            PlayerPrefs.SetInt("game_coins_number", playercoins);
            SceneManager.LoadScene(1);
        }

        else
        {

            UnlockCategoryPanel.SetActive(false);
            noEnoughStars.SetActive(true);
            categoryWantedUnlockIndex = 0;
        }

    }



    #endregion



}
