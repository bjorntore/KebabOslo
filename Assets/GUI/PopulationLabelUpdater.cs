using UnityEngine;
using System.Collections;

public class PopulationLabelUpdater : MonoBehaviour {

    /* Created this mainly for debug/development purposes. At the time the class was created it showed that when max population was reached(spawning completed), FPS Increased by 150. */

    WorldController worldController;
    UnityEngine.UI.Text textLabel;

    // Use this for initialization
    void Start()
    {
        worldController = FindObjectOfType<WorldController>();
        textLabel = GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        textLabel.text = "Population: " + worldController.world.Customers.Count;
    }

}
