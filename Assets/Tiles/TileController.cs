using UnityEngine;
using System.Collections;
using System;

public class TileController : MonoBehaviour, IClickable
{

    public Tile tile;

    public void Click()
    {
        Debug.Log("Clicked " + tile.ToString());
        if (tile.CanBuildOn())
        {
            GenericDialogPanel panel = GenericDialogPanel.Instance();
            panel.SetPanel("Build Kebab Shop!", "Build a kebab shop and expand your empire!", BuildKebabBuilding, string.Format("Build (-{0} cash)!", Cost()));
        }
    }

    public void BuildKebabBuilding()
    {
        WorldController wc = FindObjectOfType<WorldController>();
        wc.AddAndSpawnBuilding(new KebabBuilding(tile.x, tile.z), tile);
        wc.player.ChangeCash(-Cost());
    }

    public int Cost()
    {
        return 1000;
    }

}
