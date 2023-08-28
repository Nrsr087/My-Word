using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


[CustomEditor(typeof(BoardData), false)]
[CanEditMultipleObjects]
[System.Serializable]
public class BoardDataDrawer : Editor
{

    private BoardData GameDataInstance => target as BoardData;
    private ReorderableList _detaList;

    private void OnEnable()
    {
        instializedReordableList(ref _detaList, "searchwords", "searching words");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        GameDataInstance.TimeInSecconds = EditorGUILayout.FloatField("Game Time (in Seconds)", GameDataInstance.TimeInSecconds);

        DrawColumsRowsInputFields();
        EditorGUILayout.Space();
        ConvertToUpperButton();

        if (GameDataInstance.Board != null && GameDataInstance.colums > 0 && GameDataInstance.Rows > 0)
            DrawBoardTable();


        GUILayout.BeginHorizontal();
        clearBoardButton();
        FillUpWidthRandomLettersButton();
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
        _detaList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(GameDataInstance);
        }
    }

    private void DrawColumsRowsInputFields()
    {

        var columstemp = GameDataInstance.colums;
        var rowsTemp = GameDataInstance.Rows;

        GameDataInstance.colums = EditorGUILayout.IntField("Colums", GameDataInstance.colums);
        GameDataInstance.Rows = EditorGUILayout.IntField("Rows", GameDataInstance.Rows);

        if ((GameDataInstance.colums != columstemp || GameDataInstance.Rows != rowsTemp) && GameDataInstance.colums > 0 && GameDataInstance.Rows > 0)
        {
            GameDataInstance.creatNewBoard();
        }

    }

    private void DrawBoardTable()
    {
        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var headerColumStyle = new GUIStyle();
        headerColumStyle.fixedWidth = 35;


        var columStyle = new GUIStyle();
        columStyle.fixedWidth = 50;

        var RowStyle = new GUIStyle();
        RowStyle.fixedHeight = 25;
        RowStyle.fixedWidth = 40;
        RowStyle.alignment = TextAnchor.MiddleCenter;


        var textFieldStyle = new GUIStyle();
        textFieldStyle.normal.background = Texture2D.grayTexture;
        textFieldStyle.normal.textColor = Color.white;
        textFieldStyle.fontStyle = FontStyle.Bold;
        textFieldStyle.alignment = TextAnchor.MiddleCenter;


        EditorGUILayout.BeginHorizontal(tableStyle);
        for (var x = 0; x < GameDataInstance.colums; x++)
        {
            EditorGUILayout.BeginVertical(x == -1 ? headerColumStyle : columStyle);
            for (var y = 0; y < GameDataInstance.Rows; y++)
            {
                if (x >= 0 && y >= 0)
                {
                    EditorGUILayout.BeginHorizontal(RowStyle);
                    var character = (string)EditorGUILayout.TextArea(GameDataInstance.Board[x].Row[y], textFieldStyle);
                    if (GameDataInstance.Board[x].Row[y].Length > 1)
                    {
                        character = GameDataInstance.Board[x].Row[y].Substring(0, 1);
                    }
                    GameDataInstance.Board[x].Row[y] = character;
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }


    private void instializedReordableList(ref ReorderableList list, string proprtyName, string listlable)
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty(proprtyName), true
        , true, true, true);

        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, listlable);
        };

        var l = list;
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("word"), GUIContent.none);

        };
    }


    private void ConvertToUpperButton()
    {
        if (GUILayout.Button("To Upper"))
        {
            for (var i = 0; i < GameDataInstance.colums; i++)
            {
                for (var j = 0; j < GameDataInstance.Rows; j++)
                {
                    var errorcounter = Regex.Matches(GameDataInstance.Board[i].Row[j], @"[a-z]").Count;

                    if (errorcounter > 0)
                    {
                        GameDataInstance.Board[i].Row[j] = GameDataInstance.Board[i].Row[j].ToUpper();
                    }
                }
            }

            //for searching words list
            foreach (var searchword in GameDataInstance.searchwords)
            {
                var errorcounter = Regex.Matches(searchword.word, @"[a-z]").Count;

                searchword.word = Regex.Replace(searchword.word, @"\s", "");


                if (errorcounter > 0)
                {
                    searchword.word = searchword.word.ToUpper();
                }
            }


        }
    }


    // buttons to clear the board 
    private void clearBoardButton()
    {
        if (GUILayout.Button("Clear Board Puzzle"))
        {
            for (int i = 0; i < GameDataInstance.colums; i++)
            {
                for (int j = 0; j < GameDataInstance.Rows; j++)
                {
                    GameDataInstance.Board[i].Row[j] = " ";
                }
            }

        }
    }


    private void FillUpWidthRandomLettersButton()
    {
        if (GUILayout.Button("Fill Up With Random"))
        {
            for (int i = 0; i < GameDataInstance.colums; i++)
            {
                for (int j = 0; j < GameDataInstance.Rows; j++)
                {
                    int errorcounter = Regex.Matches(GameDataInstance.Board[i].Row[j], @"[a-zA-Z]").Count;
                    string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    int index = Random.Range(0, letters.Length);


                    if (errorcounter == 0)
                    {
                        GameDataInstance.Board[i].Row[j] = letters[index].ToString();
                    }
                }
            }
        }
    }
}


