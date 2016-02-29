using UnityEngine;
using System.Collections;

public class StatusPanelUpdater : MonoBehaviour
{

    private WorldController worldController;
    private WorldTimeController worldTimeController;

    public UnityEngine.UI.Text timeLabel;
    public UnityEngine.UI.Text cashLabel;
    public UnityEngine.UI.Text reputationLabel;
    public UnityEngine.UI.Text populationLabel;

    // Use this for initialization
    void Start ()
    {
        worldController = FindObjectOfType<WorldController>();
        worldTimeController = FindObjectOfType<WorldTimeController>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        timeLabel.text = worldTimeController.toString;
        cashLabel.text = "Cash: " + worldController.world.player.Cash;
        reputationLabel.text = "Reputation: " + worldController.world.player.Reputation;
        populationLabel.text = "Population: " + worldController.world.Customers.Count;
    }
}
