using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial_ScrollView_SpawnObjects : MonoBehaviour
{
    public GameObject Button_Template_Objects;     
   
    private List<string> NameListObjects = new List<string>();

    // Use this for initialization
    void Start()
    {
        StartCoroutine(addObjects());
    }       
    
    IEnumerator addObjects()
    {
        yield return new WaitForSeconds(0.2f);     

        NameListObjects.Add(" 1.\tCube");
        NameListObjects.Add(" 2.\tSphere");
        NameListObjects.Add(" 3.\tCapsule");
        NameListObjects.Add(" 4.\tCylinder");

        foreach (string str in NameListObjects)
        {
            GameObject go = Instantiate(Button_Template_Objects) as GameObject;
            go.SetActive(true);
            Buttons TB = go.GetComponent<Buttons>();
            TB.SetName(str);
            go.transform.SetParent(Button_Template_Objects.transform.parent);
        }
    }

    public void ButtonClicked(string str)
    {
        //Debug.Log(str + " button clicked.");
    }     
     
}
