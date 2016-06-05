using UnityEngine;
using System.Collections;

public class OverlayHelper : MonoBehaviour
{

    private KebabBuilding curKebabBuilding;
    public GameObject kebabBuildingReputationOverlay;
    public GameObject kebabBuildingHungerOverlay;

    private int CameraAngleAdjustmentX = 2;
    private int CameraAngleAdjustmentZ = 2;

    private static OverlayHelper overlayHelper;
    public static OverlayHelper instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
        {
            Debug.LogError("Tried to created another instance of " + GetType() + ". Destroying.");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (curKebabBuilding != null)
            UpdateKebabBuildingOverlay();
    }

    public void ActivateKebabBuildingOverlay(KebabBuilding kebabBuilding)
    {
        curKebabBuilding = kebabBuilding;
        UpdateKebabBuildingOverlay();
    }

    public void DeactivateKebabBuildingOverlay()
    {
        curKebabBuilding = null;
        kebabBuildingReputationOverlay.SetActive(false);
        kebabBuildingHungerOverlay.SetActive(false);
    }

    private void UpdateKebabBuildingOverlay()
    {
        OverlayScaleAndActivate(kebabBuildingReputationOverlay, curKebabBuilding.tile.x, curKebabBuilding.tile.z, curKebabBuilding.Reputation * 2);
        OverlayScaleAndActivate(kebabBuildingHungerOverlay, curKebabBuilding.tile.x, curKebabBuilding.tile.z, (curKebabBuilding.Reputation + 100) * 2); // 100 is max hunger
    }

    private void OverlayScaleAndActivate(GameObject overlayGameObject, int x, int z, int dimension)
    {
        if (dimension < 0)
            dimension = 0;

        float currentScaleY = overlayGameObject.transform.localScale.y;
        overlayGameObject.transform.localScale = new Vector3(dimension, currentScaleY, dimension);

        float currentPositionY = overlayGameObject.transform.localPosition.y;
        overlayGameObject.transform.position = new Vector3(x - CameraAngleAdjustmentX, currentPositionY, z - CameraAngleAdjustmentZ);

        overlayGameObject.SetActive(true);
    }

}