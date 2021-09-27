using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;

[System.Serializable]
public class MyStringEvent : UnityEvent<string> { }
public class BoatMoving : MonoBehaviour
{
    public InputDevice rightHand;
    public InputDevice leftHand;
    public Transform leftT;
    public Transform rightT;
    public Transform rigT;
    public Vector3 ReversoRightT;//相对右手动作
    public Vector3 ReversoLeftT;//相对左手动作
    public GameObject debugCubePrefab;
    public GameObject debugCubePrefab2;
    private Vector3 planeNormal;
    private List<Gesture> gesturesList = new List<Gesture>();
    public float recordInterval = 0.05f;
    public float recognitionThreshold = 0.6f;

    
    public MyStringEvent onRecognized;

    public bool creatMode = true;
    public string newGestureName;
    public bool RisMove = false;
    public bool RisPress = false;
    public bool LisMove = false;
    public bool LisPress = false;
    public List<Vector3> rightPositionList = new List<Vector3>();
    public List<Vector3> leftPositionList = new List<Vector3>();
    //jennifer
    private GameObject RingStick;
    private bool rightTriggerValue;
    //jennifer ends
    // Start is called before the first frame update
    void Start()
    {
        //Gesture init
        planeNormal = new Vector3(1, 0, 0);
        string[] gesturefile = Directory.GetFiles(Application.dataPath+"/Gesture/", "*.xml");
        foreach(var gfile in gesturefile)
        {
            gesturesList.Add(GestureIO.ReadGestureFromFile(gfile));
        }
        //get Input device
        List<InputDevice> rightdevices = new List<InputDevice>();
        List<InputDevice> leftdevices = new List<InputDevice>();
        InputDeviceRole righthand = InputDeviceRole.RightHanded;
        InputDeviceRole lefthand = InputDeviceRole.LeftHanded;
        InputDevices.GetDevicesWithRole(righthand, rightdevices);
        if (rightdevices.Count > 0)
        {
            rightHand = rightdevices[0];
        }
        InputDevices.GetDevicesWithRole(lefthand, leftdevices);
        if(leftdevices.Count > 0)
        {
            leftHand = leftdevices[0]; 
        }
        //jennifer
        RingStick = GameObject.FindGameObjectWithTag("RingStick");
        RingStick.SetActive(false);
        rightTriggerValue = false;
        //jennifer ends
    }

    // Update is called once per frame
    void Update()
    {
        if(rightHand.TryGetFeatureValue(CommonUsages.gripButton, out bool rgb))
        {
            RisPress = rgb;
        }
       
        if(leftHand.TryGetFeatureValue(CommonUsages.gripButton, out bool lgb))
        {
            //print("左手按下:" + lgb);
            LisPress = lgb;
        }
        //jennifer
        if (rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out bool test))
        {
            rightTriggerValue = test;
            if (rightTriggerValue)
            {
                RingStick.SetActive(true);
            }
            else
            {
                RingStick.SetActive(false);
            }
        }
        //Jennifer end


        ReversoRightT = rightT.position - rigT.position;
        ReversoLeftT = leftT.position - rigT.position;
        if(!RisMove && RisPress)
        {
            RightStartMovement();
        }
        else if(RisMove && !RisPress)
        {
            RightEndMovement();
        }
        else if(RisMove && RisPress)
        {
            RightUpdateMovement();
        }

        if (!LisMove && LisPress)
        {
            LeftStartMovement();
        }
        else if (LisMove && !LisPress)
        {
            leftEndMovement();
        }
        else if (LisMove && LisPress)
        {
            LeftUpdateMovement();
        }

    }

    /// <summary>
    /// RightHandMovement
    /// </summary>
    void RightStartMovement()
    {
        Debug.Log("开始手动");
        rightPositionList.Clear();
        RisMove = true;
        rightPositionList.Add(ReversoRightT);

        if (debugCubePrefab) Destroy(Instantiate(debugCubePrefab, ReversoRightT, Quaternion.identity), 3);
    }
    void RightEndMovement()
    {
        Debug.Log("结束手动");
        RisMove = false;
        //创造手势
        Point[] pointArray = new Point[rightPositionList.Count];
        
        for(int i = 0;i < rightPositionList.Count; i++)
        {
            Vector3 rightPoint = Vector3.ProjectOnPlane(rightPositionList[i], GameObject.Find("Main Camera").transform.right);
            //Vector3 rightPoint = Vector3.ProjectOnPlane(rightPositionList[i], planeNormal);
            
            if (debugCubePrefab2) Destroy(Instantiate(debugCubePrefab2, rightPoint, Quaternion.identity), 3);
            pointArray[i] = new Point(rightPoint.z, rightPoint.y, 0);

            
        }
        Gesture newGesture = new Gesture(pointArray);

        if (creatMode)
        {
            newGesture.Name = newGestureName;
            gesturesList.Add(newGesture);

            //string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
            string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);
        }
        else
        {
            Result result = PointCloudRecognizer.Classify(newGesture, gesturesList.ToArray());
            string movementName = result.GestureClass;
            Debug.Log(result.GestureClass + "" + result.Score);
            Debug.Log(movementName);
            
            if (result.Score > recognitionThreshold)
            {
                onRecognized.Invoke("r");
            }
        }
    }
    void RightUpdateMovement()
    {
        Debug.Log("正在手动");
        
        Vector3 latestPos = rightPositionList[rightPositionList.Count - 1];
        if(Vector3.Distance(latestPos, ReversoRightT) > recordInterval)
        {
            if (debugCubePrefab) Destroy(Instantiate(debugCubePrefab, ReversoRightT, Quaternion.identity), 1);
            rightPositionList.Add(ReversoRightT);
        }
        
    }


    /// <summary>
    /// RightHandMovement
    /// </summary>
    void LeftStartMovement()
    {
        Debug.Log("开始手动");
        leftPositionList.Clear();
        LisMove = true;
        leftPositionList.Add(ReversoLeftT);

        if (debugCubePrefab) Destroy(Instantiate(debugCubePrefab, ReversoLeftT, Quaternion.identity), 3);
    }
    void leftEndMovement()
    {
        Debug.Log("结束手动");
        LisMove = false;
        //创造手势
        Point[] pointArray = new Point[leftPositionList.Count];

        for (int i = 0; i < leftPositionList.Count; i++)
        {
            Vector3 leftPoint = Vector3.ProjectOnPlane(leftPositionList[i], GameObject.Find("Main Camera").transform.right);
            //Vector3 rightPoint = Vector3.ProjectOnPlane(rightPositionList[i], planeNormal);

            if (debugCubePrefab2) Destroy(Instantiate(debugCubePrefab2, leftPoint, Quaternion.identity), 3);
            pointArray[i] = new Point(leftPoint.z, leftPoint.y, 0);


        }
        Gesture newGesture = new Gesture(pointArray);

        if (creatMode)
        {
            newGesture.Name = newGestureName;
            gesturesList.Add(newGesture);

            //string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
            string fileName = Application.dataPath + "/Gesture/" + newGestureName + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);
        }
        else
        {
            Result result = PointCloudRecognizer.Classify(newGesture, gesturesList.ToArray());
            string movementName = result.GestureClass;
            Debug.Log(result.GestureClass + "" + result.Score);
            Debug.Log(movementName);

            if (result.Score > recognitionThreshold)
            {
                onRecognized.Invoke("l");
            }
        }
    }
    void LeftUpdateMovement()
    {
        Debug.Log("正在手动");

        Vector3 latestPos = leftPositionList[leftPositionList.Count - 1];
        if (Vector3.Distance(latestPos, ReversoLeftT) > recordInterval)
        {
            if (debugCubePrefab) Destroy(Instantiate(debugCubePrefab, ReversoLeftT, Quaternion.identity), 1);
            leftPositionList.Add(ReversoLeftT);
        }

    }



}
