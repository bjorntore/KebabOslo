using UnityEngine;
using System.Collections;

public class CustomerHelpButton : MonoBehaviour {

    public void OpenInfoPane()
    {
        GenericDialog panel = GenericDialog.Instance();
        panel.SetPanel("Customer Icon Help", "Red circle = Angry, \nOrange circle = Did not bother with kebab", null, "");
    }

}
