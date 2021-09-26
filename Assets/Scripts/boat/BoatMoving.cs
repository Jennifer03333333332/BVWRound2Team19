using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using PDollarGestureRecognizer;
using System.IO;
public class BoatMoving : MonoBehaviour
{
    public InputDevice rightHand;
    public InputDevice leftHand;
    public Transform leftT;
    public Transform rightT;
    public GameObject debugCubePrefab;
    public GameObject debugCubePrefab2;
    private Vector3 planeNormal;
    private List<Gesture> gesturesList = new List<Gesture>();
    public float recordInterval = 0.05f;


    public bool creatMode = true;
    public string newGestureName;
    public bool RisMove = false;
    public bool RisPress = false;
    public List<Vector3> rightPositionList = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        //Gesture init
        planeNormal = new Vector3(1, 0, 0);
        string[] gesturefile = Directory.GetFiles(Application.persistentDataPath, "*.xml");
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
            if (lgb)
            {
                print("开始左手划船");
                print("lgb:"+lgb);
            }
        }
      
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

    }


    void RightStartMovement()
    {
        Debug.Log("开始手动");
        rightPositionList.Clear();
        RisMove = true;
        rightPositionList.Add(rightT.position);

        if (debugCubePrefab) Destroy(Instantiate(debugCubePrefab, rightT.position, Quaternion.identity), 3);
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
            Debug.Log(result.GestureClass + result.Score);
        }
    }
    void RightUpdateMovement()
    {
        Debug.Log("正在手动");
        Vector3 latestPos = rightPositionList[rightPositionList.Count - 1];
        if(Vector3.Distance(latestPos,leftT.position) > recordInterval)
        {
            if (debugCubePrefab) Destroy(Instantiate(debugCubePrefab, rightT.position, Quaternion.identity), 1);
            rightPositionList.Add(rightT.position);
        }
        
    }
}
