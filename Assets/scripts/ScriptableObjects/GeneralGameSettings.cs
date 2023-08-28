using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu]
public class GeneralGameSettings : ScriptableObject
{

    [Header("social media accounts")]
    public string moreGamesLink;
    public string PrivacyPolicyLink;

    [Header("other settings")]
    [Space(30)]
    public int CoinsNumberToSkipPuzzle;
    public int CoinsNumberToAddExtraTime;
    public int collectedCoinsEch_level;        
    public string Gdpr_message;



    [Header("sounds")]
    [Space(30)]
    public AudioClip click_sound;
    public AudioClip countDownSoundClock;


}
