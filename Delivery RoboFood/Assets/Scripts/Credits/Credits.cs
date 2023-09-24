using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;

    private void Update()
    {
        if (IsAnimationPlaying("CreditsAnimation") == false)
        {
            creditsPanel.gameObject.SetActive(false);
        }
    }

    public bool IsAnimationPlaying(string animationName)
    {
        Animator creditsText = transform.GetChild(0).GetComponent<Animator>();
        var animatorStateInfo = creditsText.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.IsName(animationName))
        {
            return true;
        }
        return false;
    }

    public void EndAnimation()
    {
        creditsPanel.gameObject.SetActive(false);
    }
}
