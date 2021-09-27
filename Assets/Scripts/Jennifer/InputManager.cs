using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

[System.Serializable]
//public class TriggerButtonEvent : UnityEvent<bool> { }

public class InputManager : MonoBehaviour
{
    public InputDevice rightHand;
    public InputDevice leftHand;

    private GameObject RingStick;
    //public TriggerButtonEvent triggerButtonPress;

    //private List<InputDevice> devicesWithTriggerButton;
    private bool triggerValue;
    private void Awake()
    {
        List<InputDevice> rightdevices = new List<InputDevice>();
        List<InputDevice> leftdevices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, rightdevices);
        if (rightdevices.Count > 0)
        {
            rightHand = rightdevices[0];
        }
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left, leftdevices);
        if (leftdevices.Count > 0)
        {
            leftHand = leftdevices[0];
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        RingStick = GameObject.FindGameObjectWithTag("RingStick");
    }

    // Update is called once per frame
    void Update()
    {
        print(rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue));
        print(triggerValue);
        if (rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue)
        {
            RingStick.SetActive(true);
        }
        else {
            RingStick.SetActive(false);
        }
    }
}
