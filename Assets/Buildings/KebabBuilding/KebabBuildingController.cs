using UnityEngine;
using System.Collections;
using System;

public class KebabBuildingController : BuildingController, IClickable
{
    private WorldTimeController worldTimeController;

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
        worldTimeController = FindObjectOfType<WorldTimeController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (building.cashEarnedTrigger)
        {
            RunCashEarnedAnimationAndSound();
            building.cashEarnedTrigger = false;
        }
        building.TriggerExpences(worldTimeController.day);
    }

    public void Click()
    {
        KebabBuildingDialog.Instance().OpenDialog(this, CloseKebabBuildingOverlay);

        OverlayHelper overlayHelper = OverlayHelper.Instance();
        overlayHelper.ActivateKebabBuildingOverlay(building);
    }

    private void RunCashEarnedAnimationAndSound()
    {
        cashEarnedAnimator.enabled = true;
        cashEarnedAnimator.Play("CashGainedAnimation", -1, 0);
        AudioSource.PlayOneShot(cashEarnedAudioClip, 0.1f);
    }

    public void Delete()
    {
        building.RejectCustomers();
        WorldController.instance.DeleteKebabBuilding(building);
        Destroy(gameObject);
    }

    private void CloseKebabBuildingOverlay()
    {
        OverlayHelper overlayHelper = OverlayHelper.Instance();
        overlayHelper.DeactivateKebabBuildingOverlay();
    }

}
