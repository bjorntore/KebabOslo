using UnityEngine;
using System.Collections;

public class CustomerHelpButton : MonoBehaviour {

    public void OpenInfoPane()
    {
        GenericDialog panel = GenericDialog.instance;
        panel.OpenDialog("Customer Icon Help", "Red circle = Angry, \nOrange circle = Did not bother with kebab");
    }

}
