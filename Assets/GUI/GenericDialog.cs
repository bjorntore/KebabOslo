using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GenericDialog : Dialog
{

    public GameObject panel;
    public UnityEngine.UI.Text title;
    public UnityEngine.UI.Text description;
    public UnityEngine.UI.Button button;
    public UnityEngine.UI.Text buttonText;
    public UnityEngine.UI.Button cancel;

    private static GenericDialog genericDialog;
    public static GenericDialog Instance()
    {
        if (!genericDialog)
        {
            genericDialog = FindObjectOfType(typeof(GenericDialog)) as GenericDialog;
            if (!genericDialog)
                Debug.LogError("There needs to be one active GenericDialog script on a GameObject in your scene.");
        }

        return genericDialog;
    }

    public void OpenDialog(string title, string description, UnityAction buttonAction, string buttonText, UnityAction cancelEvent = null)
    {
        if (IsAnyDialogOpen())
            return;

        panel.SetActive(true);

        this.title.text = title;
        this.description.text = description;


        if (buttonAction == null)
            this.button.gameObject.SetActive(false);
        else
        {
            this.button.gameObject.SetActive(true);
            this.button.onClick.RemoveAllListeners();
            this.button.onClick.AddListener(buttonAction);
            this.button.onClick.AddListener(ClosePanel);
            this.buttonText.text = buttonText;
        }

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
