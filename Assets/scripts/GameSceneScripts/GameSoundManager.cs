using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundManager : MonoBehaviour
{

    public static GameSoundManager instance;   
    public GeneralGameSettings m_gameSettings;
    

    public AudioSource General_audioSource,countDownAudioSource;

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



    public void PlayClickSound()
    {
        General_audioSource.clip = m_gameSettings.click_sound;
        General_audioSource.loop = false;
        General_audioSource.Play();

    }
    public void PlayCountDownSound()
    {
        countDownAudioSource.clip=m_gameSettings.countDownSoundClock;
        countDownAudioSource.loop=false;
        countDownAudioSource.Play();

    }
    void Start()
    {

    }

    void Update()
    {

    }
}
