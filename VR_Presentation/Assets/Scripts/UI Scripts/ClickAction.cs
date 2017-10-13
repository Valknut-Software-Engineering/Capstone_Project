using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;

[RequireComponent(typeof(AudioSource))]
public class ClickAction : MonoBehaviour, IPointerClickHandler
{
    public Image currentImage;
    public AudioSource audio;

    //public Animation animation;

    public void OnPointerClick(PointerEventData eventData)
    {         
        currentImage = GetComponent<Image>();        
        
        if(currentImage.sprite.name == "Office")
        {
			
            audio.Play();
            Thread.Sleep(2000);
            //animation.Play();            
            LoadByIndex(1);
        }
		else if(currentImage.sprite.name == "Forest")
        {
            audio.Play();
            Thread.Sleep(2000);
            //animation.Play();            
            LoadByIndex(2);
        }
		else
		{
			audio.Play();
			Thread.Sleep(2000);
			//animation.Play();            
			LoadByIndex(3);
		}
    }
	 
    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
