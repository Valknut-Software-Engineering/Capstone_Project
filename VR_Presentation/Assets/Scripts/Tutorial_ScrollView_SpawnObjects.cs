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
        Debug.Log("SKYBOX COUNTER = " + Globals.objectCount);

        //for (int z = 1; z < Globals.objectCount + 1; z++)
        //{
        //    NameListObjects.Add("Object " + z);
        //}

        NameListObjects.Add("Cube");
        NameListObjects.Add("Sphere");
        NameListObjects.Add("Capsule");
        NameListObjects.Add("Cylinder");

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
