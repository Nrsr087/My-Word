using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordChecker : MonoBehaviour
{

    public GameData currentGameData;
    private string _word;
    private int _assignedPints = 0;
    private int _completedwords = 0;
    private Ray _rayUp, _rayDown;
    private Ray _rayLeft, _rayRight;
    private Ray _rayDiagonalLeftUp, _rayDiagonalLeftDown;
    private Ray _rayDiagonalRightUp, _rayDiagonalRightDown;
    private Ray _currentRay = new Ray();
    private Vector3 _rayStartPosition;
    private List<int> _correctSquareList = new List<int>();

    private List<string> solvedWords;
    private void OnEnable()
    {
        GameEvents.OnCheckSquare += squareSelected;
        GameEvents.OnClearSelection += clearSelection;

    }

    private void OnDisable()
    {
        GameEvents.OnCheckSquare -= squareSelected;
        GameEvents.OnClearSelection -= clearSelection;


    }
    void Start()
    {
        solvedWords = new List<string>();
        _assignedPints = 0;
        _completedwords = 0;

    }

    void Update()
    {
        if (_assignedPints > 0 && Application.isEditor)
        {

            Debug.DrawRay(_rayUp.origin, _rayUp.direction * 4);
            Debug.DrawRay(_rayDown.origin, _rayDown.direction * 4);

            Debug.DrawRay(_rayLeft.origin, _rayLeft.direction * 4);
            Debug.DrawRay(_rayRight.origin, _rayRight.direction * 4);

            Debug.DrawRay(_rayDiagonalLeftUp.origin, _rayDiagonalLeftUp.direction * 4);
            Debug.DrawRay(_rayDiagonalLeftDown.origin, _rayDiagonalLeftDown.direction * 4);

            Debug.DrawRay(_rayDiagonalRightUp.origin, _rayDiagonalRightUp.direction * 4);
            Debug.DrawRay(_rayDiagonalRightDown.origin, _rayDiagonalRightDown.direction * 4);
        }

    }


    private void squareSelected(string letter, Vector3 squarePosition, int squareIndex)
    {
        if (_assignedPints == 0)
        {
            _rayStartPosition = squarePosition;
            _correctSquareList.Add(squareIndex);
            _word += letter;

            _rayUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(0f, 1));
            _rayDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(0f, -1));
            _rayLeft = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1f, 0f));
            _rayRight = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1f, 0f));
            _rayDiagonalLeftUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1f, 1f));
            _rayDiagonalLeftDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1f, -1f));
            _rayDiagonalRightUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1f, 1f));
            _rayDiagonalRightDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1f, -1f));
        }

        else if (_assignedPints == 1)
        {
            _correctSquareList.Add(squareIndex);
            _currentRay = selectRay(_rayStartPosition, squarePosition);
            GameEvents.SelectSquareMethod(squarePosition);
            _word += letter;
            checkWord();
        }

        else
        {
            if (IsPointOnTheRay(_currentRay, squarePosition))
            {
                _correctSquareList.Add(squareIndex);
                GameEvents.SelectSquareMethod(squarePosition);
                _word += letter;
                checkWord();

                // ther thirs seleciton
            }

        }
        _assignedPints++;
    }
    private void checkWord()
    {
        foreach (var searchingword in WordsGrid.instance.currentBoardData.searchwords)
        {
            if (_word == searchingword.word && it_IS_Solved(_word)==false)
            {
                GameEvents.correctWordMethod(_word, _correctSquareList);

                _completedwords++;  // add a word completed to the gourp of completed words
                solvedWords.Add(_word);
                CheckBoardCompleted();  //check if all the board puzzle are solved or not


                _word = string.Empty;
                _correctSquareList.Clear();
                return;
            }

        }
    }
    private bool it_IS_Solved(string word)
    {
        for (int i = 0; i < solvedWords.Count; i++)
        {

            if (solvedWords[i] == word)
            {
                return true;
            }

        }
        return false;

    }

    private bool IsPointOnTheRay(Ray currentRay, Vector3 point)
    {
        var hits = Physics.RaycastAll(currentRay, 500.0f);

        for (int i = 0; i < hits.Length; i++)
        {

            if (hits[i].transform.position == point)
                return true;
        }

        return false;
    }


    private Ray selectRay(Vector2 firstPosition, Vector2 secocndePosition)
    {
        var direction = (secocndePosition - firstPosition).normalized;
        float tolerance = 0.01f;

        if (Mathf.Abs(direction.x) < tolerance && Mathf.Abs(direction.y - 1f) < tolerance)
        {
            return _rayUp;
        }

        if (Mathf.Abs(direction.x) < tolerance && Mathf.Abs(direction.y - (-1f)) < tolerance)
        {
            return _rayDown;
        }


        if (Mathf.Abs(direction.x - (-1f)) < tolerance && Mathf.Abs(direction.y) < tolerance)
        {
            return _rayLeft;
        }

        if (Mathf.Abs(direction.x - 1f) < tolerance && Mathf.Abs(direction.y) < tolerance)
        {
            return _rayRight;

        }



        if (direction.x < 0f && direction.y > 0f)
        {
            return _rayDiagonalLeftUp;
        }
        if (direction.x < 0f && direction.y < 0f)
        {
            return _rayDiagonalLeftDown;
        }

        if (direction.x > 0f && direction.y > 0f)
        {
            return _rayDiagonalRightUp;
        }


        if (direction.x > 0f && direction.y < 0f)
        {
            return _rayDiagonalRightDown;
        }

        return _rayDown;
    }
    private void clearSelection()
    {
        _assignedPints = 0;
        _correctSquareList.Clear();
        _word = string.Empty;
    }

    private void CheckBoardCompleted()
    {
        if (WordsGrid.instance.currentBoardData.searchwords.Count == _completedwords)
        {
            if (PlayerPrefs.GetInt("ClassicMode") == 0)
            {
                if (CountDownTimer.instance.isIt_On_Time())
                {
                    GameUIEvents.instance.player_win();

                    var categoryName = WordsGrid.instance.currentBoardData;
                }
            }

            else
            {
                PG.ClassicSelected.instance.NextRandom();
            }
        }
    }
}
