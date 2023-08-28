using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleProgressSaver : MonoBehaviour
{
    public static PuzzleProgressSaver instance;

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

    public void savePuzzleData(string categoryName, int SolvedPuzzles)
    {
        PlayerPrefs.SetInt(categoryName, SolvedPuzzles);
    }




    public int retrevePuzzleProgress(string catgoryName)
    {
        return PlayerPrefs.GetInt(catgoryName, 0);

    }


    public void saveCategoryStatus(string categoryName, bool isOpene)
    {
        if (isOpene == true)
        {
            PlayerPrefs.SetInt(categoryName + "_locked_status", 1);
        }
        else
        {
            PlayerPrefs.SetInt(categoryName + "_locked_status", 0);
        }
    }

    public bool retreveCatgeoryStatus(string categoryName)
    {
        if (PlayerPrefs.GetInt(categoryName + "_locked_status", 0) == 0)
        {
            return false;

        }
        else
        {
            return true;
        }
    }




    public void saveSelectedPuzzle(string CategoyrName, int puzzleIndex)
    {
        PlayerPrefs.SetInt(CategoyrName + "_selectedPuzzleInCategory", puzzleIndex);


    }
    public int retriveSelectedPuzzle(string CategoryNam)
    {
        return PlayerPrefs.GetInt(CategoryNam + "_selectedPuzzleInCategory", 0);


    }

}
