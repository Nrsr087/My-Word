using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class levelsList : MonoBehaviour
{

    public static levelsList instance;

    private List<Button> levelsListbuttons;
    public Button levelListButton_prefab;
    public GameObject levellist_parent, categoryListGameObject;


    private Category current_Selected_category;

    public GameLevelData m_levels;

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

    }


    public void setupLevelLIst(Category selectedCategory)
    {

        current_Selected_category = selectedCategory;
        levelsListbuttons = new List<Button>();

        for (int i = 0; i < selectedCategory.PuzzleBoards.Count; i++)
        {
            int copy = i + 1;
            Button currentButton = Instantiate(levelListButton_prefab, levelListButton_prefab.transform.position, Quaternion.identity);
            currentButton.transform.SetParent(levellist_parent.transform, false);

            currentButton.onClick.AddListener(() => level_puzzleSelectedToOpen(copy));



            levelsListbuttons.Add(currentButton);


        }

        setupButtonsList(levelsListbuttons);
    }

    void level_puzzleSelectedToOpen(int index)
    {
 

        PuzzleProgressSaver.instance.saveSelectedPuzzle(current_Selected_category.category_name, index - 1);
        SceneManager.LoadScene(2);
    }

    public void setupButtonsList(List<Button> buttons)
    {

        Button[] newArray = buttons.ToArray();


        for (int i = 0; i < buttons.Count; i++)
        {
            newArray[i].transform.GetChild(0).GetComponentInChildren<Text>().text = "level " + (i + 1);

            float solvedPuzzles = PuzzleProgressSaver.instance.retrevePuzzleProgress(current_Selected_category.category_name);  

            if (i <= solvedPuzzles)
            {

                newArray[i].gameObject.transform.GetChild(1).gameObject.SetActive(false);

            }
            else
            {
                newArray[i].interactable = false;

                newArray[i].gameObject.transform.GetChild(1).gameObject.SetActive(true);   


            }

        }


    }


    public void ClickBackToCategories()
    {
        for (int i = 0; i < levelsListbuttons.Count; i++)
        {
            Destroy(levelsListbuttons[i].gameObject);
        }

        this.gameObject.SetActive(false);

        categoryListGameObject.SetActive(true);


    }


}
