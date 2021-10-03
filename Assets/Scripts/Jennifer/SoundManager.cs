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
    public List<SoundGroup> dialog_List = new List<SoundGroup>();

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

    public void StopPlayingMainBGM()
    {
        BGM1.Stop();//destroy会出bug
        BGM2.volume = 0.1f;
    }


    //Playing sound on where player is.
    public void PlayingSound(string _soundName)
    {
        AudioSource.PlayClipAtPoint(sound_List[FindSound(_soundName, sound_List)].audioClip, Camera.main.transform.position, soundVolumn);
    }

    public void PlayingSound(string _soundName, float volume)
    {
        AudioSource.PlayClipAtPoint(sound_List[FindSound(_soundName, sound_List)].audioClip, Camera.main.transform.position, volume);
    }

    public void PlayingDialog(string _soundName)
    {
        AudioSource.PlayClipAtPoint(dialog_List[FindSound(_soundName, dialog_List)].audioClip, Camera.main.transform.position, soundVolumn);
    }

    public int FindSound(string _soundName, List<SoundGroup> list)
    {
        int i = 0;
        while (i < list.Count)
        {
            if (list[i].soundName == _soundName)
            {
                return i;
            }
            i++;
        }
        return i;
    }

    public float SoundClipsLong(string _soundName)
    {

        float ClipsLong = dialog_List[FindSound(_soundName, dialog_List)].audioClip.length;
        print(_soundName + "声音长度" + ClipsLong);
        return ClipsLong;
    }
}
