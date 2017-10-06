using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial_ScrollView_Video : MonoBehaviour
{
   
    public GameObject Button_Template_Videos;
	public char letter = 'A';
    
    private List<string> NameListVideos = new List<string>();
   
    // Use this for initialization
    void Start()
    {
        StartCoroutine(addVideos());
        
    }       
    
    IEnumerator addVideos()
    {
        yield return new WaitForSeconds(0.2f);        

        for (int z = 1; z < Globals.videoCount + 1; z++)
        {
			NameListVideos.Add(" " + z + ".\t" + "Video" + letter);
			letter++;
        }

        foreach (string str in NameListVideos)
        {
            GameObject go = Instantiate(Button_Template_Videos) as GameObject;
            go.SetActive(true);
            Buttons TB = go.GetComponent<Buttons>();
            TB.SetName(str);
            go.transform.SetParent(Button_Template_Videos.transform.parent);
        }
    }
    

    public void ButtonClicked(string str)
    {
        //Debug.Log(str + " button clicked.");
    }     
     
}
