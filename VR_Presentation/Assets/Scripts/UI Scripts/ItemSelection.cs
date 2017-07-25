using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour {

    public Image SelectionImage;
    public Image currentImage;    
    public List<Sprite> ItemList = new List<Sprite>();    
    private int index = 0;

    public void RightSelection()
    {
        if (index < ItemList.Count - 1)
        {
            index++;
            SelectionImage.sprite = ItemList[index];            
            //currentImage = GameObject.Find("SceneSelection").GetComponent<Image>();
            //currentImage.sprite = SelectionImage.sprite;
        }
    }

    public void LeftSelection()
    {
        if (index > 0)
        {
            index--;
            SelectionImage.sprite = ItemList[index];
            SelectionImage.sprite = ItemList[index];
            //GameObject.Find("Canvas").GetComponent<Image>().sprite = SelectionImage.sprite;
            //currentImage.sprite = SelectionImage.sprite;
        }

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
