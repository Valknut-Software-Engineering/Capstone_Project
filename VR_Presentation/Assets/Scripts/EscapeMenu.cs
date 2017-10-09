using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class EscapeMenu : MonoBehaviour {

	public GameObject escapeMenu;
	public GameObject mainMenuPanel;
	public GameObject optionsMenuPanel;
	public GameObject inGameUI;
	public GameObject taskbar;
	Button resume;
	public CharacterController characterController;
	public bool flag;

	// Use this for initialization
	void Start () {
		flag = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
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

	void findAll()
	{
		escapeMenu = GameObject.Find("Canvas").transform.Find("EscapeMenu").gameObject;
		inGameUI = GameObject.Find("Canvas").transform.Find("ContentMain").gameObject;
		taskbar = GameObject.Find("Canvas").transform.Find("Taskbar").gameObject;
		mainMenuPanel = escapeMenu.transform.Find("MainMenuPanel").gameObject;
		optionsMenuPanel = escapeMenu.transform.Find("OptionsPanel").gameObject;
		//resume = GameObject.Find("Resume").GetComponent<Button>();
	}

	public void onClickResume()
	{		
		findAll ();
		disableContentAll ();
	}

	public void onClickOptions()
	{	
		mainMenuPanel.SetActive (false);
		optionsMenuPanel.SetActive (true);
	}

	public void onClickBack()
	{		
		findAll ();
		disableContentAll ();
		enableContentAll ();
	}

	void enableContentOptions()
	{
		findAll ();
		mainMenuPanel.SetActive (true);
		optionsMenuPanel.SetActive (true);
		characterController.enabled = false;
		characterController.GetComponent<FirstPersonController>().enabled = false;
		Cursor.lockState = CursorLockMode.None;
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}

	void disableContentOptions()
	{
		findAll ();
		optionsMenuPanel.SetActive (false);
		enableContentAll ();
	}

	void enableContentAll()
	{	
		inGameUI.SetActive (false);	
		taskbar.SetActive (false);
		mainMenuPanel.SetActive (true);
		escapeMenu.SetActive(true);
		characterController.enabled = false;
		characterController.GetComponent<FirstPersonController>().enabled = false;
		Cursor.lockState = CursorLockMode.None;
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}

	void disableContentAll()
	{	
		inGameUI.SetActive (true);
		taskbar.SetActive (true);	
		mainMenuPanel.SetActive (false);
		escapeMenu.SetActive(false);
		optionsMenuPanel.SetActive (false);
		characterController.enabled = true;
		characterController.GetComponent<FirstPersonController>().enabled = true;
		Cursor.visible = false;
	}
}