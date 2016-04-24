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
    public KebabMenuController kebabMenuController;

    private KebabBuilding kebabBuilding;

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
        }
    }

	public void OpenDialog(KebabBuilding kebabBuilding, UnityAction cancelEvent = null)
	{
        Display(lockKeyboard: true);
		this.kebabBuilding = kebabBuilding;

        title.text = kebabBuilding.ToString();

        addEmployeeButton.onClick.RemoveAllListeners();
        addEmployeeButton.onClick.AddListener(kebabBuilding.HireEmployee);

        fireEmployeeButton.onClick.RemoveAllListeners();
        fireEmployeeButton.onClick.AddListener(kebabBuilding.FireEmployee);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(ClosePanel);
        if (cancelEvent != null)
            cancelButton.onClick.AddListener(cancelEvent);

        kebabMenuController.SetupKebabMenu(kebabBuilding.kebabMenu);
    }	
}
