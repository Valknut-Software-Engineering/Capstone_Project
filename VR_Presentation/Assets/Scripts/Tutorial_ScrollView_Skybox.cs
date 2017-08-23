using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial_ScrollView_Skybox : MonoBehaviour
{
    public GameObject Button_Template_Skybox;     
   
    private List<string> NameListSkybox = new List<string>();

    // Use this for initialization
    void Start()
    {
        StartCoroutine(addSkybox());
    }       
    
    IEnumerator addSkybox()
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("SKYBOX COUNTER = " + Globals.skyBoxCount);

        for (int z = 1; z < Globals.skyBoxCount + 1; z++)
        {
            NameListSkybox.Add("Skybox " + z);
        }

        foreach (string str in NameListSkybox)
        {
            GameObject go = Instantiate(Button_Template_Skybox) as GameObject;
            go.SetActive(true);
            Buttons TB = go.GetComponent<Buttons>();
            TB.SetName(str);
            go.transform.SetParent(Button_Template_Skybox.transform.parent);
        }
    }

    public void ButtonClicked(string str)
    {
        //Debug.Log(str + " button clicked.");
    }     
     
}
