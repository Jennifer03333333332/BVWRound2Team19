using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> fireflies = new List<GameObject>();
    public int stage = 0;//控制玩家进行到哪一步
    /// <summary>
    /// control player game process and the things that showing up
    /// </summary>
    public class GameStage
    {
        public int stage;
        //public GameObject stage
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach(var item in fireflies)
        {
            item.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(stage < fireflies.Count)
        {
            fireflies[stage].SetActive(true);
        }
    }

    public void EndGame()
    {
        if(stage == 3)
        {
            SceneManager.LoadScene("EndScene");
        }
        
    }
}
