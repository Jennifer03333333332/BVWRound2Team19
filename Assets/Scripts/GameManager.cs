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
    public int nowStage = 0;
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
        if(!gameStageStructs[nowStage].gameStage.passThisStage && !gameStageStructs[nowStage].gameStage.isGenerate)
        {
            GameStageStruct item = gameStageStructs[nowStage];
            print("stage:" + nowStage);
            print(gameStageStructs[nowStage].gameStagesPrefab);
            if (item.gameStagesPrefab)
            {
               GameObject go = Instantiate(item.gameStagesPrefab, item.StageGenerateTransforms.position, Quaternion.identity);

            }
        }
    }

    public void EndGame()
    {
        if(nowStage == (gameStageStructs.Count - 1))
        {
            SceneManager.LoadScene("EndScene");
        }
        
    }
}
