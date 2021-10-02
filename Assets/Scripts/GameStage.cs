using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStage : MonoBehaviour
{
    public int stage;
    public Transform generatePlace;
    public bool passThisStage = false;
    public bool isGenerate = false;
    public GameObject lantern;
    public float LanternIntensity = 30;

    //成功通过这个阶段
    public void SuccessPass()
    {
        passThisStage = true;
    }

    public void RegenerateThisStage()
    {
        //后续可加
    }
    public void LanternFireActive()
    {
        lantern.SetActive(true);
    }
    public void LanternFireInactive()
    {
        lantern.SetActive(false);
    }
    /// <summary>
    /// 通过关卡后放剧情语音
    /// </summary>
    public void PassStageVoice()
    {
        SoundManager.instance.PlayingSound(stage.ToString());
    }
}
