using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClassicInGame : MonoBehaviour
{
    public static ClassicInGame instance;
    [SerializeField] private GameObject scorePanel, timePanel;
    private int _score;
    private List<string> letterList;
    private List<string> wordList;
    private List<string> letterwordList;
    [SerializeField] private Transform parentLetters;
    
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
    private void Start()
    {
        if (PlayerPrefs.GetInt("ClassicMode") == 1)
        {
            scorePanel.SetActive(true);
            timePanel.SetActive(false);
        }
        _score = PlayerPrefs.GetInt("ClassicMode_score");
        letterList = new List<string>();
        wordList = new List<string>();
        letterwordList = new List<string>();
        UpdateScore();
    }
    public void UpdateScore()
    {
        _score = PlayerPrefs.GetInt("ClassicMode_score");
        scorePanel.transform.GetChild(0).GetComponent<Text>().text = _score.ToString();
    }
    public void GetLetters(string letter)
    {
        letterList.Add(letter);
    }
    public void GetWordList()
    {
        foreach (var item in WordsGrid.instance.currentBoardData.searchwords)
        {
            wordList.Add(item.word);
        }
        SetUpHint();
    }
    private void SetUpHint()
    {
        foreach (var words in wordList)
        {
            foreach (var item in words.ToCharArray())
            {
                if(item.ToString() != " ") letterwordList.Add(item.ToString());
                print(item.ToString());
            }
        }
    }
    public void ClickHint()
    {
        for (int i = 0; i < letterList.Count; i++)
        {
            foreach (var item in letterwordList)
            {
                if (item == letterList[i])
                {
                    print(item + " " + letterList[i]);
                    parentLetters.GetChild(i + 1).GetComponent<Animator>().enabled = true;
                    print(i);
                }
            }
        }
    }
    public void ClickNext()
    {
        PG.ClassicSelected.instance.NextRandom();
    }
}
