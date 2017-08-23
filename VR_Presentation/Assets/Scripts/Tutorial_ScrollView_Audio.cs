using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial_ScrollView_Audio : MonoBehaviour
{
    
    public GameObject Button_Template_Audio;
    
    private List<string> NameListAudio = new List<string>();    

    // Use this for initialization
    void Start()
    {
        StartCoroutine(addAudio());       
    }       
    

    IEnumerator addAudio()
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("AUDIO COUNTER = " + Globals.audioCount);

        for (int z = 1; z < Globals.audioCount + 1; z++)
        {
            NameListAudio.Add("Audio " + z);
        }

        foreach (string str in NameListAudio)
        {
            GameObject go = Instantiate(Button_Template_Audio) as GameObject;
            go.SetActive(true);
            Buttons TB = go.GetComponent<Buttons>();
            TB.SetName(str);
            go.transform.SetParent(Button_Template_Audio.transform.parent);
        }
    }
   
    public void ButtonClicked(string str)
    {
        //Debug.Log(str + " button clicked.");
    }     
     
}
