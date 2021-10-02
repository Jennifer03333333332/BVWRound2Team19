using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// control player game process and the things that showing up
    /// </summary>
    public List<GameObject> gameStagesPrefab = new List<GameObject>();
    private List<GameStage> gameStages = new List<GameStage>();
    public List<Transform> StageGenerateTransforms = new List<Transform>();
    public List<GameObject> Lanterns = new List<GameObject>();
    public int nowStage = 0;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i < gameStagesPrefab.Count; i++)
        {
            
            gameStages[i] = gameStagesPrefab[i].GetComponent<GameStage>();
            gameStages[i].generatePlace = StageGenerateTransforms[1];
            gameStages[i].lantern = Lanterns[i];
            gameStages[i].LanternFireInactive();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var item in gameStages)
        {
            if (item.passThisStage)
            {
                nowStage = item.stage;
                continue;
            }
            else if (!item.passThisStage && !item.isGenerate)
            {
                if (item)
                {
                    Instantiate(item.gameObject, item.generatePlace.position, Quaternion.identity);
                    item.isGenerate = true;
                }
            }
        }
    }

    public void EndGame()
    {
        if(nowStage
            == (gameStages.Count - 1))
        {
            SceneManager.LoadScene("EndScene");
        }
        
    }
}
