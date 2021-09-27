using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> fireflies = new List<GameObject>();
    public int stage = 0;//控制玩家进行到哪一步
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
}
