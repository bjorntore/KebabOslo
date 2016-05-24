using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using System.Linq;

public class KebabBuildingDialog : Dialog
{
    public Text title;
    public Text employeeLabel;
    public Text customersLabel;
    public Button addEmployeeButton;
    public Button fireEmployeeButton;
    public Button cancelButton;
    public Button deleteButton;
    public KebabMenuController kebabMenuController;

    public KebabBuilding kebabBuilding;

    private static KebabBuildingDialog kebabBuildingDialog;
    public static KebabBuildingDialog Instance()
    {
        if (!kebabBuildingDialog)
        {
            kebabBuildingDialog = FindObjectOfType(typeof(KebabBuildingDialog)) as KebabBuildingDialog;
            if (!kebabBuildingDialog)
                Debug.LogError("There needs to be one active KebabBuildingDialog script on a GameObject in your scene.");
        }

        return kebabBuildingDialog;
    }

    void Update()
    {
        if (kebabBuilding != null)
        {
            employeeLabel.text = "Employees: " + kebabBuilding.employees.Count + "/" + Settings.KebabBuilding_MaxEmployees;
            customersLabel.text = "Customers: " + kebabBuilding.customers.Count + "/" + kebabBuilding.GetCurrentCapacity();
            if (kebabBuilding.customersInQueue.Count > 0)
                customersLabel.text = string.Format("{0} (Queue: {1})", customersLabel.text, kebabBuilding.customersInQueue.Count);
        }
    }

    public void OpenDialog(KebabBuildingController kebabBuildingController, UnityAction cancelEvent = null)
    {
        Display();

        kebabBuilding = kebabBuildingController.building;
        title.text = kebabBuilding.ToString();

        addEmployeeButton.onClick.RemoveAllListeners();
        addEmployeeButton.onClick.AddListener(kebabBuilding.HireEmployee);

        fireEmployeeButton.onClick.RemoveAllListeners();
        fireEmployeeButton.onClick.AddListener(kebabBuilding.FireEmployee);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(ClosePanel);
        cancelButton.onClick.AddListener(kebabMenuController.DiscardChanges);

        deleteButton.onClick.RemoveAllListeners();
        deleteButton.onClick.AddListener(kebabBuildingController.Delete);
        deleteButton.onClick.AddListener(FlushDialogData);
        deleteButton.onClick.AddListener(ClosePanel);

        if (cancelEvent != null)
        {
            cancelButton.onClick.AddListener(cancelEvent);
            deleteButton.onClick.AddListener(cancelEvent);
        }

        kebabMenuController.SetupKebabMenu(kebabBuilding.kebabMenu);
    }

    private void FlushDialogData()
    {
        kebabBuilding = null;
    }
}
