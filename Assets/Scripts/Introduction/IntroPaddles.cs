using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPaddles : MonoBehaviour
{
    public Animator rightHand;
    public Animator LeftHand;

    private void OnEnable()
    {
        StartCoroutine(SetAnimStart());
    }

    IEnumerator SetAnimStart()
    {
        yield return new WaitForSeconds(0.4f);
        if (rightHand) { rightHand.SetBool("StartIntro", true); }
        if (LeftHand) { LeftHand.SetBool("StartIntro", true); }
    }
}
