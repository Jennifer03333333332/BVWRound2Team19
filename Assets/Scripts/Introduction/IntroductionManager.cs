using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionManager : MonoBehaviour
{
   [System.Serializable]
   public class IntroductionStage
    {
        public GameObject ShowingThings;
        public string DialogName;
    }

    public IntroBoatMoving introBoatMoving;
    public IntroBoatController introBoatController;

    public int generateStage = 0;
    public int nowStage = 0;
    public List<IntroductionStage> introStages = new List<IntroductionStage>();

    private void Start()
    {
        foreach(var item in introStages)
        {
            if (item.ShowingThings) item.ShowingThings.SetActive(false);
        }
    }

    private void Update()
    {
        if(generateStage == nowStage && generateStage < introStages.Count)
        {
            StartCoroutine(SetIntroActive(generateStage));
            SoundManager.instance.PlayingDialog(introStages[generateStage].DialogName);
            generateStage++;

        }
    }

    IEnumerator SetIntroActive(int _generateStage)
    {
        yield return new WaitForSeconds(SoundManager.instance.SoundClipsLong(introStages[_generateStage].DialogName));
        introStages[_generateStage].ShowingThings.SetActive(true);
    }

}
