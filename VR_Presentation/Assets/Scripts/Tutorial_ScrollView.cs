using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial_ScrollView : MonoBehaviour
{
    public GameObject Button_Template_Main;      

    private List<string> NameListMain = new List<string>();  

    // Use this for initialization
    void Start()
    {
        StartCoroutine(addMain());      
        
    }       

    IEnumerator addMain()
    {
        yield return new WaitForSeconds(0f);        

        NameListMain.Add(" 1.\tAdd Image");
        NameListMain.Add(" 2.\tAdd Audio");
        NameListMain.Add(" 3.\tAdd Video");
        NameListMain.Add(" 4.\tChange Skybox");
        NameListMain.Add(" 5.\tSpawn Object");
		NameListMain.Add(" 6.\tSpawn Complex Object");

        foreach (string str in NameListMain)
        {
            GameObject go = Instantiate(Button_Template_Main) as GameObject;
            go.SetActive(true);
            Buttons TB = go.GetComponent<Buttons>();
            TB.SetName(str);
            go.transform.SetParent(Button_Template_Main.transform.parent);
        }        
    }    

    public void ButtonClicked(string str)
    {
        //Debug.Log(str + " button clicked.");
    }     
     
}
