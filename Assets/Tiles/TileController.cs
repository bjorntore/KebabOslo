using UnityEngine;
using System.Collections;
using System;

public class TileController : MonoBehaviour, IClickable
{
    public Tile tile;

    public void Click()
    {
        Debug.Log("Clicked " + tile.ToString());
        if (tile.type == TileType.Buildable)
        {
            GenericDialog panel = GenericDialog.Instance();
            panel.OpenDialog("Build Kebab Shop!", "Build a kebab shop and expand your empire!", BuildKebabBuilding, string.Format("Build (-{0} cash)!", Cost()), CloseKebabBuildingOverlay);

            OverlayHelper overlayHelper = OverlayHelper.Instance();
            KebabBuilding fakeKebabBuildingForOverlay = new KebabBuilding(null);
            fakeKebabBuildingForOverlay.tile = tile;
            overlayHelper.ActivateKebabBuildingOverlay(fakeKebabBuildingForOverlay);
        }
    }

    private void BuildKebabBuilding()
    {
        WorldController worldController = FindObjectOfType<WorldController>();
		worldController.AddAndSpawnKebabBuilding(new KebabBuilding(worldController.world), tile);
        worldController.world.player.ChangeCash(-Cost());
        CloseKebabBuildingOverlay();
    }

    private int Cost()
    {
        return tile.propertyValue;
    }

    private void CloseKebabBuildingOverlay()
    {
        OverlayHelper overlayHelper = OverlayHelper.Instance();
        overlayHelper.DeactivateKebabBuildingOverlay();
    }

}
