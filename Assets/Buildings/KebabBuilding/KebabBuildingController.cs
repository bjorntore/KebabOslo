﻿using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class KebabBuildingController : BuildingController, IClickable
{
    public Animator cashEarnedAnimator;
    public AudioClip cashEarnedAudioClip;
    private AudioSource AudioSource;

    public KebabBuilding building;

    public override void SetBuilding(Building building)
    {
        this.building = (KebabBuilding)building;
    }

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (building.cashEarnedTrigger)
        {
            RunCashEarnedAnimationAndSound();
            building.cashEarnedTrigger = false;
        }
    }

    public void Click()
    {
        //Make me a dialog.
    }

    private void RunCashEarnedAnimationAndSound()
    {
        cashEarnedAnimator.enabled = true;
        cashEarnedAnimator.Play("CashGainedAnimation", -1, 0);
        AudioSource.PlayOneShot(cashEarnedAudioClip, 0.1f);
    }

}
