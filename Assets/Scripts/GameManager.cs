using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// control player game process and the things that showing up
    /// </summary>
    /// 
    [System.Serializable]
    public struct GameStageStruct
    {
        public GameObject gameStagesPrefab;
        public GameStage gameStage;
        public Transform StageGenerateTransforms;
        public GameObject lantern;
    
    }

    public List<GameStageStruct> gameStageStructs = new List<GameStageStruct>();
    public int GenerateStage = 0;
    public int NowStage = 0;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i < gameStageStructs.Count; i++)
        {
            
            if (gameStageStructs[i].lantern) { gameStageStructs[i].gameStage.lantern = gameStageStructs[i].lantern; }
            gameStageStructs[i].gameStage.LanternFireInactive();
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GenerateStage == NowStage && GenerateStage < (gameStageStructs.Count-1))
        {
            if (gameStageStructs[GenerateStage].gameStagesPrefab)
            {
                Instantiate(gameStageStructs[GenerateStage].gameStagesPrefab, gameStageStructs[GenerateStage].StageGenerateTransforms.position, Quaternion.Euler(0, 90, 0));
                GenerateStage++;
            }
        }
        
    }

    public void EndGame()
    {
        if(NowStage == (gameStageStructs.Count))
        {
            SceneManager.LoadScene("EndScene");
        }
        
    }
}
