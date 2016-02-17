using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GenericDialogPanel : MonoBehaviour
{

    public GameObject panel;
    public UnityEngine.UI.Text title;
    public UnityEngine.UI.Text description;
    public UnityEngine.UI.Button button;
    public UnityEngine.UI.Text buttonText;
    public UnityEngine.UI.Button cancel;

    private static GenericDialogPanel genericDialogPanel;

    public static GenericDialogPanel Instance()
    {
        if (!genericDialogPanel)
        {
            genericDialogPanel = FindObjectOfType(typeof(GenericDialogPanel)) as GenericDialogPanel;
            if (!genericDialogPanel)
                Debug.LogError("There needs to be one active GenericDialogPanel script on a GameObject in your scene.");
        }

        return genericDialogPanel;
    }

    public void SetPanel(string title, string description, UnityAction buttonAction, string buttonText, UnityAction cancelEvent = null)
    {
        panel.SetActive(true);

        this.title.text = title;
        this.description.text = description;

        this.button.onClick.RemoveAllListeners();
        this.button.onClick.AddListener(buttonAction);
        this.button.onClick.AddListener(ClosePanel);
        this.buttonText.text = buttonText;

        this.cancel.onClick.RemoveAllListeners();
        this.cancel.onClick.AddListener(ClosePanel);
        if (cancelEvent != null)
            this.cancel.onClick.AddListener(cancelEvent);
    }

    void ClosePanel()
    {
        panel.SetActive(false);
    }
}
