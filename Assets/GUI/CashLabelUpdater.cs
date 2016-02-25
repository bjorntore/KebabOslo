using UnityEngine;
using System.Collections;

public class CashLabelUpdater : MonoBehaviour {

    WorldController worldController;
    UnityEngine.UI.Text textLabel;

    // Use this for initialization
    void Start ()
    {
        worldController = FindObjectOfType<WorldController>();
        textLabel = GetComponent<UnityEngine.UI.Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        textLabel.text = "Cash: " + worldController.world.player.Cash;
    }

}
