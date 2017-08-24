using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour {
    
    // Use this for initialization
    private string Name;
    public Text ButtonText;
    public Tutorial_ScrollView ScrollView;

    public void SetName(string name)
    {
        Name = name;
        ButtonText.text = name;
    }
    public void Button_Click()
    {
        ScrollView.ButtonClicked(Name);
    }
}
