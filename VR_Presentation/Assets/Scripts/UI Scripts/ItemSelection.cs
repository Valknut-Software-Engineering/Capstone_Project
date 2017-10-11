using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour {

    public Image SelectionImage;
    public Image currentImage;    
    public List<Sprite> ItemList = new List<Sprite>();    
    private int index = 0;

	public GameObject sceneSelection;
	public GameObject text;
	public Text realText;

	void findAll(){
		sceneSelection = GameObject.Find("Canvas").transform.Find("SceneSelection").gameObject;
		text = sceneSelection.transform.Find("Text").gameObject;
		realText = text.GetComponent<Text> ();
	}     

    public void RightSelection()
    {		
        if (index < ItemList.Count - 1)
        {
            index++;
            SelectionImage.sprite = ItemList[index];
			if (index == 0) {
				findAll ();
				realText.text = "TUTORIAL SCENE";

			} else {
				findAll ();
				realText.text = "OFFICE SCENE";
			}          
        }
    }


    public void LeftSelection()
    {
        if (index > 0)
        {
            index--;
            SelectionImage.sprite = ItemList[index];
            SelectionImage.sprite = ItemList[index];
			if (index == 0) {
				findAll ();
				realText.text = "TUTORIAL SCENE";

			} else {
				findAll ();
				realText.text = "OFFICE SCENE";
			}            
        }
    }

}
