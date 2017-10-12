using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class ControlText : MonoBehaviour {

	public GameObject controls;
	public GameObject buttonsPanel;
	public GameObject taskbar;
	public GameObject image;
	public GameObject text;
	public Text realText;
	public CharacterController characterController;
	public bool flag;
	public bool flagE;
	public bool flagF;
	public bool flagG;
	public bool flagShift;
	public bool flagTab;
	public bool numpadA;
	public bool numpadB;
	public bool flagV;
	public bool flagMouseWheel;

	// Use this for initialization
	void Start () {
		flag = false;
		flagE = false;
		flagF = false;
		flagG = false;
		flagShift = false;
		flagTab = false;
		flagV = false;
		flagMouseWheel = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F1))
        {
			findAll ();
			if (flag) {
				disableContentAll ();
				flag = false;
			} else {
				enableContentAll ();
				flag = true;
			}
        }
    }

	public void setAll(){
		taskbar.SetActive (true);
		buttonsPanel.SetActive (true);
		controls.SetActive (true);
		image.SetActive (true);
		text.SetActive (true);
	}

	public void setFlags(){
		flag = true;
		flagE = true;
		flagF = true;
		flagG = true;
		flagShift = true;
		flagTab = true;
		numpadA = true;
		numpadB = true;
		flagV = true;
		flagMouseWheel = true;
	}

	public void onClickF1(){		
		findAll ();
		if (flag) {
			disableContentAll ();
			flag = false;
		} else {
			enableContentAll ();
			flag = true;
		}		
	}

	public void onClickE(){
		findAll ();
		if (flagE) {
			image.SetActive (false);
			flagE = false;
		} else {
			setAll ();
			setFlags ();
			realText.text = "The ' E ' key is used to pick up an object.";
		}
	}

	public void onClickF(){
		findAll ();
		if (flagF) {
			image.SetActive (false);
			flagF = false;
		} else {
			setAll ();
			setFlags ();
			realText.text = "The ' F ' key is used to freeze objects that you are holding.";
		}
	}

	public void onClickG(){
		findAll ();
		if (flagG) {
			image.SetActive (false);
			flagG = false;
		} else {
			setAll ();
			setFlags ();
			realText.text = "The ' G ' key is used to toggle snap to grid on objects you are holding.";
		}
	}

	public void onClickV(){
		findAll ();
		if (flagV) {
			image.SetActive (false);
			flagV = false;
		} else {
			setAll ();
			setFlags ();
			realText.text = "The ' V ' key is used to pause and restart videos.";
		}
	}

	public void onClickMouseWheel(){
		findAll ();
		if (flagMouseWheel) {
			image.SetActive (false);
			flagMouseWheel = false;
		} else {
			setAll ();
			setFlags ();
			realText.text = "The ' Mouse Wheel ' is used to push or pull the object in hand.";
		}
	}

	public void onClickShift(){
		findAll ();
		if (flagShift) {
			image.SetActive (false);
			flagShift = false;
		} else {
			setAll ();
			setFlags ();
			realText.text = "The ' Shift ' key is used to sprint.";
		}
	}

	public void onClickTab(){
		findAll ();
		if (flagTab) {
			image.SetActive (false);
			flagTab = false;
		} else {
			setAll ();
			setFlags ();
			realText.text = "The ' Tab ' key is used on complex objects, enabling you to pick them up.";
		}
	}

	public void onClickNumpadA(){
		findAll ();
		if (numpadA) {
			image.SetActive (false);
			numpadA = false;
		} else {
			setAll ();
			setFlags ();
			realText.text = "The ' Numpad 1 - 6, +, - ' keys are used to resize objects in your hand.";
		}
	}

	public void onClickNumpadB(){
		findAll ();
		if (numpadB) {
			image.SetActive (false);
			numpadB = false;
		} else {
			setAll ();
			setFlags ();
			realText.text = "The ' Numpad 7 - 9 ' keys are used to rotate objects in your hand.";
		}
	}

	void findAll()
	{
		taskbar = GameObject.Find("Canvas").transform.Find("Taskbar").gameObject;
		buttonsPanel = taskbar.transform.Find("ButtonsPanel").gameObject;
		controls = buttonsPanel.transform.Find("DisplayText").gameObject;
		image = controls.transform.Find("DisplayImage").gameObject;
		text = image.transform.Find("KeyText").gameObject;
		realText = text.GetComponent<Text> ();
	}

	void enableContentAll()
	{		
		buttonsPanel.SetActive (true);
		//controls.SetActive (true);
		characterController.enabled = false;
		characterController.GetComponent<FirstPersonController>().enabled = false;
		Cursor.lockState = CursorLockMode.None;
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}

	void disableContentAll()
	{	
		buttonsPanel.SetActive (false);	
		controls.SetActive (false);
		characterController.enabled = true;
		characterController.GetComponent<FirstPersonController>().enabled = true;
		Cursor.visible = false;
	}
}