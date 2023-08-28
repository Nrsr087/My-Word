using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsGrid : MonoBehaviour
{

    public static WordsGrid instance;

    public GameLevelData levelsData;
    public GameObject GridSquirePrefab;
    public AlphabetData alphabetData;

    public float squareOffset = 0.0f;
    public float TopPosition = 0.0f;
    private List<GameObject> squareList = new List<GameObject>();

    public BoardData currentBoardData;


    private void Awake()
    {
        currentBoardData = getcurretnGameBoardData();
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

        spawnGridSquares();
        setSquaresPosition();
        ClassicInGame.instance.GetWordList();
    }
    public BoardData getcurretnGameBoardData()
    {
        int currentCategoryIndex = PlayerPrefs.GetInt("currentCategorySelected", 0);
        Category curretnCategory = levelsData.gameCategories[currentCategoryIndex];
        int currentBoardIndex = PuzzleProgressSaver.instance.retriveSelectedPuzzle(curretnCategory.category_name);
        return curretnCategory.PuzzleBoards[currentBoardIndex];


    }


    private void setSquaresPosition()
    {
        var squareRect = squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
        var squarTransform = squareList[0].GetComponent<Transform>();



        var offset = new Vector2
        {

            x = (squareRect.width * squarTransform.localScale.x + squareOffset) * 0.01f,
            y = (squareRect.height * squarTransform.localScale.y + squareOffset) * 0.01f,
        };

        var startPosition = GetfirstSquarePosition();

        int columeNumber = 0;
        int rowNumber = 0;

        foreach (var square in squareList)
        {
            if (rowNumber + 1 > currentBoardData.Rows)
            {
                columeNumber++;
                rowNumber = 0;

            }

            var PositionX = startPosition.x + offset.x * columeNumber;
            var positiony = startPosition.y - offset.y * rowNumber;


            square.GetComponent<Transform>().position = new Vector2(PositionX, positiony);
            rowNumber++;

        }


    }


    private Vector2 GetfirstSquarePosition()
    {
        var startPosition = new Vector2(0f, transform.position.y);
        var squareRect = squareList[0].GetComponent<SpriteRenderer>().sprite.rect;

        var squareTransform = squareList[0].GetComponent<Transform>();
        var squareSize = new Vector2(0f, 0f);


        squareSize.x = squareRect.width * squareTransform.localScale.x;
        squareSize.y = squareRect.height * squareTransform.localScale.y;

        var midwithPosition = (((currentBoardData.colums - 1) * squareSize.x) / 2) * 0.01f;
        var midwithdHight = (((currentBoardData.Rows - 1) * squareSize.y) / 2) * 0.01f;

        startPosition.x = (midwithPosition != 0) ? midwithPosition * -1 : midwithPosition;

        startPosition.y += midwithdHight;

        return startPosition;


    }

    private void spawnGridSquares()
    {

        if (currentBoardData != null)
        {
            var squareScale = getSquareScale(new Vector3(1.5f, 1.5f, 0.1f));
            foreach (var squares in currentBoardData.Board)
            {
                foreach (var squreletter in squares.Row)
                {
                    ClassicInGame.instance.GetLetters(squreletter);
                    var normaleletterData = alphabetData.alphabetNormale.Find(data => data.letter == squreletter);
                    var selectedLetterData = alphabetData.alphabetHighlited.Find(data => data.letter == squreletter);
                    var correctLetterData = alphabetData.alphabetCorrect.Find(data => data.letter == squreletter);

                    if (normaleletterData.Image == null || selectedLetterData.Image == null)
                    {
                        Debug.LogError(" all fields in your array should have some letters. press fill up with random button in you board data to add random letter.letter" + squreletter);

#if UNITY_EDITOR

                        if (UnityEditor.EditorApplication.isPlaying)
                        {
                            UnityEditor.EditorApplication.isPlaying = false;

                        }

#endif

                    }
                    else
                    {
                        // if we found the letter
                        squareList.Add(Instantiate(GridSquirePrefab));
                        squareList[squareList.Count - 1].GetComponent<GridSquar>().setSprite(normaleletterData, selectedLetterData, correctLetterData); 
                        squareList[squareList.Count - 1].transform.SetParent(this.transform);
                        squareList[squareList.Count - 1].GetComponent<Transform>().position = new Vector3(0f, 0f, 0f);
                        squareList[squareList.Count - 1].transform.localScale = squareScale;
                        squareList[squareList.Count - 1].GetComponent<GridSquar>().set_index(squareList.Count - 1);
                    }
                }
            }

        }
    }

    private Vector3 getSquareScale(Vector3 defaultScale)
    {

        var finalScale = defaultScale;
        var adjustment = 0.01f;

        while (shouldScaleDown(finalScale))
        {
            finalScale.x -= adjustment;
            finalScale.y -= adjustment;

            if (finalScale.x <= 0 || finalScale.y <= 0)
            {
                finalScale.x = adjustment;
                finalScale.y = adjustment;
                return finalScale;
            }
        }
        return finalScale;


    }


    private bool shouldScaleDown(Vector3 targetScale)
    {
        var squareRect = GridSquirePrefab.GetComponent<SpriteRenderer>().sprite.rect;
        var squaresize = new Vector2(0f, 0f);
        var startPosition = new Vector2(0f, 0f);

        squaresize.x = (squareRect.width * targetScale.x) + squareOffset;
        squaresize.y = (squareRect.height * targetScale.y) + squareOffset;

        var midwithPosition = ((currentBoardData.colums * squaresize.x) / 2) * 0.01f;
        var midwithdHight = ((currentBoardData.Rows * squaresize.y) / 2) * 0.01f;


        startPosition.x = (midwithPosition != 0) ? midwithPosition * -1 : midwithPosition;

        startPosition.y = midwithdHight;


        return (startPosition.x < getHalfScreenWith() * -1 || startPosition.y > TopPosition);


    }

    private float getHalfScreenWith()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = (1.7f * height) * Screen.width / Screen.height;

        return width / 2;

    }



}
