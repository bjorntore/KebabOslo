using UnityEngine;
using System.Collections;

public class ReputationLabelUpdater : MonoBehaviour
{

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
        textLabel.text = "Reputation: " + worldController.world.player.Reputation;
    }

}
