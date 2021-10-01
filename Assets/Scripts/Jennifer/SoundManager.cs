using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Serializable]
    public struct SoundGroup
    {
        public AudioClip audioClip;
        public string soundName;
    }
    public List<SoundGroup> sound_List = new List<SoundGroup>();

    public AudioSource BGM1;
    public AudioSource BGM2;

    public static SoundManager instance;


    private float soundVolumn = 2f;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Use this for initialization
    void Start()
    {
        
    }




    //Playing sound on where player is.
    public void PlayingSound(string _soundName)
    {   //float soundVolumn
        //if (_soundName == "")
        //{

        //}
        AudioSource.PlayClipAtPoint(sound_List[FindSound(_soundName)].audioClip, Camera.main.transform.position, soundVolumn);
    }
    public int FindSound(string _soundName)
    {
        int i = 0;
        while (i < sound_List.Count)
        {
            if (sound_List[i].soundName == _soundName)
            {
                return i;
            }
            i++;
        }
        return i;
    }
}
