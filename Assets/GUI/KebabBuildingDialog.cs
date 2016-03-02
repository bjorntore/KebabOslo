using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class KebabBuildingDialog : MonoBehaviour 
{
	public GameObject panel;
	public Text title;
    public Text employeeLabel;
    public Text customersLabel;
    public Button addEmployeeButton;
    public Button fireEmployeeButton;
    public Button cancelButton;

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

	public void SetPanel(KebabBuilding kebabBuilding, UnityAction addEmployeeAction, UnityAction fireEmployeeAction, UnityAction cancelEvent = null)
	{
		this.kebabBuilding = kebabBuilding;
		panel.SetActive(true);

        title.text = kebabBuilding.ToString();

        addEmployeeButton.onClick.RemoveAllListeners();
        addEmployeeButton.onClick.AddListener(addEmployeeAction);

        fireEmployeeButton.onClick.RemoveAllListeners();
        fireEmployeeButton.onClick.AddListener(fireEmployeeAction);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(ClosePanel);
        if (cancelEvent != null)
            cancelButton.onClick.AddListener(cancelEvent);
    }

	void ClosePanel()
	{
		panel.SetActive(false);
	}
	
}
