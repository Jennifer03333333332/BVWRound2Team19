using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureStore : MonoBehaviour
{
    public List<Vector3> ForwardPositionList = new List<Vector3>();
    public List<Vector3> BackwardPositionList = new List<Vector3>();

    public bool isDebug = true;

    public GameObject debugCube;

    public void Update()
    {
        if(isDebug && debugCube)
        {
            foreach(var item in BackwardPositionList)
            {
                Instantiate(debugCube, item, Quaternion.identity);
            }

        }    
    }





}
