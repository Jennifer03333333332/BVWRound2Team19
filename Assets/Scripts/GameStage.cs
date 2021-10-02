using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStage : MonoBehaviour
{
    public int stageNum = 0;
    public string stageName;//stagename
    public bool passThisStage = false;
    public bool isGenerate = false;
    public GameObject lantern;
    public float LanternIntensity = 30;
    public GameManager gm;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    //成功通过这个阶段
    public void SuccessPass()
    {
        gm.NowStage++;
        LanternFireActive();
    }

    public void RegenerateThisStage()
    {
        //后续可加
    }
    public void LanternFireActive()
    {
        if(lantern) lantern.SetActive(true);
    }
    public void LanternFireInactive()
    {
        if (lantern)  lantern.SetActive(false);
    }
    /// <summary>
    /// 通过关卡后放剧情语音
    /// </summary>
    public void PassStageVoice()
    {
        SoundManager.instance.PlayingSound(stageName);
    }
}
