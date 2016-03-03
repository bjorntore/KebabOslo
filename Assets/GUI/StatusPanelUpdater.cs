using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusPanelUpdater : MonoBehaviour
{

    private WorldController worldController;
    private WorldTimeController worldTimeController;

    public Text timeLabel;
    public Text gameSpeedLabel;
    public Text cashLabel;
    public Text reputationLabel;
    public Text populationLabel;

    // Use this for initialization
    private void Start ()
    {
        worldController = FindObjectOfType<WorldController>();
        worldTimeController = FindObjectOfType<WorldTimeController>();
    }

    // Update is called once per frame
    private void Update ()
    {
        timeLabel.text = worldTimeController.toString;
        gameSpeedLabel.text = GetTimeScaleUnderscores();
        cashLabel.text = "Cash: " + worldController.world.player.Cash;
        reputationLabel.text = "Reputation: " + worldController.world.player.Reputation;
        populationLabel.text = "Population: " + worldController.world.Customers.Count;
    }

    private string GetTimeScaleUnderscores()
    {
        string str = "";
        for (int i = 0; i < Time.timeScale; i++)
            str += "_";
        return str;
    }
}
