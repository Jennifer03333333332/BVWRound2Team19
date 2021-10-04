using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToRightPosition : MonoBehaviour
{
    private bool OnlyOnce = false;
    public Transform mainCamera;
    public float focusTime = 3f;
    private float time = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!OnlyOnce)
        {
            if(mainCamera.transform.localPosition.x != 0 || mainCamera.transform.localPosition.z != 0 || mainCamera.transform.rotation != Quaternion.identity)
            {
                
                this.transform.localPosition = new Vector3(-mainCamera.transform.localPosition.x, -mainCamera.transform.localPosition.y, -mainCamera.transform.localPosition.z);
                //mainCamera.transform.rotation = Quaternion.identity;
                this.transform.localRotation = Quaternion.LookRotation(new Vector3(0f, 0f, 1f),new Vector3(0f, 0f, 0f)) * Quaternion.Inverse(mainCamera.transform.localRotation);
            }
        }
        time += Time.deltaTime;
        if (time > 3f)
        {
            OnlyOnce = true;
        }

    }
}
