using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PG
{
    public class ClassicSelected : MonoBehaviour
    {
        public static ClassicSelected instance;
        [SerializeField] private DiffData diffData;
        private bool next;
        private string firstValue;
        private string secondValue;
        private string levelName;

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
        public void Click_Classic(int diff)
        {
            PlayerPrefs.SetInt("ClassicMode", 1);
            switch (diff)
            {
                case 0:
                    SetDataToPlay(diffData.easyList);
                    PlayerPrefs.SetInt("ClassicMode_selected", 1);
                    break;
                case 1:
                    SetDataToPlay(diffData.normalList);
                    PlayerPrefs.SetInt("ClassicMode_selected", 2);
                    break;
                case 2:
                    SetDataToPlay(diffData.hardList);
                    PlayerPrefs.SetInt("ClassicMode_selected", 3);
                    break;
                case 3:
                    SetDataToPlay(diffData.veryhardList);
                    PlayerPrefs.SetInt("ClassicMode_selected", 4);
                    break;
            }
        }
        private void SetDataToPlay(List<string> list)
        {
            int num = 0;
            num = UnityEngine.Random.Range(0, list.Capacity);
            foreach (var item in list[num].ToCharArray())
            {
                if (item != ',') 
                {
                    if (!next) firstValue += item;
                    else secondValue += item;
                }
                else next = true;
            }
            PlayerPrefs.SetInt("currentCategorySelected", System.Convert.ToInt32(firstValue));
            levelName = diffData.gameLevelData.gameCategories[System.Convert.ToInt32(firstValue)].category_name;
            PlayerPrefs.SetInt(levelName + "_selectedPuzzleInCategory", System.Convert.ToInt32(secondValue));
            SceneManager.LoadSceneAsync(3);
            print(firstValue + " : " + secondValue);
        }
        public void NextRandom()
        {
            int score = PlayerPrefs.GetInt("ClassicMode_score");
            print(PlayerPrefs.GetInt("ClassicMode_selected"));
            switch (PlayerPrefs.GetInt("ClassicMode_selected"))
            {
                case 1:
                    score += Random.Range(150, 200);
                    SetDataToPlay(diffData.easyList);
                    break;
                case 2:
                    score += Random.Range(250, 300);
                    SetDataToPlay(diffData.normalList);
                    break;
                case 3:
                    score += Random.Range(350, 400);
                    SetDataToPlay(diffData.hardList);
                    break;
                case 4:
                    score += Random.Range(450, 500);
                    SetDataToPlay(diffData.veryhardList);
                    break;
            }
            PlayerPrefs.SetInt("ClassicMode_score", score);
            ClassicInGame.instance.UpdateScore();
        }
        public void Click_RePrefab()
        {
            PlayerPrefs.DeleteAll();
        }

    }
}
