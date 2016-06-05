using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GenericDialog : Dialog
{
    public UnityEngine.UI.Text title;
    public UnityEngine.UI.Text description;
    public UnityEngine.UI.Button button;
    public UnityEngine.UI.Text buttonText;
    public UnityEngine.UI.Button cancel;

    private static GenericDialog genericDialog;
    public static GenericDialog instance;
    
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
    

    public void OpenDialog(string title, string description, UnityAction buttonAction = null, string buttonText = "", UnityAction cancelEvent = null)
    {
        Display();

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
}
