using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial_ScrollView_Images : MonoBehaviour
{

    public GameObject Button_Template_Images;
   
    private List<string> NameListImages = new List<string>();

    // Use this for initialization
    void Start()
    {
        StartCoroutine(addImages());
    }       

    IEnumerator addImages()
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("IMAGE COUNTER = " + Globals.imageCount);

        for (int z = 1; z < Globals.imageCount + 1; z++)
        {
            NameListImages.Add("Image " + z);
        }

        foreach (string str in NameListImages)
        {
            GameObject go = Instantiate(Button_Template_Images) as GameObject;
            go.SetActive(true);
            Buttons TB = go.GetComponent<Buttons>();
            TB.SetName(str);
            go.transform.SetParent(Button_Template_Images.transform.parent);
        }
         
    }    

    public void ButtonClicked(string str)
    {
        //Debug.Log(str + " button clicked.");
    }     
     
}
