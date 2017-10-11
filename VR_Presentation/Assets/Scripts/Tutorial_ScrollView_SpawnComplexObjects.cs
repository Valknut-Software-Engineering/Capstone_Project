using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial_ScrollView_SpawnComplexObjects : MonoBehaviour
{

	public GameObject Button_Template_ComplexObjects;
	public char letter = 'A';

	private List<string> NameListComplexObjects = new List<string>();

	// Use this for initialization
	void Start()
	{
		StartCoroutine(addPrefabs());

	}       

	IEnumerator addPrefabs()
	{
		yield return new WaitForSeconds(0.2f);        

		for (int z = 1; z < Globals.prefabCount + 1; z++)
		{
			NameListComplexObjects.Add(" " + z + ".\t" + "Complex" + letter);
			letter++;
		}

		foreach (string str in NameListComplexObjects)
		{
			GameObject go = Instantiate(Button_Template_ComplexObjects) as GameObject;
			go.SetActive(true);
			Buttons TB = go.GetComponent<Buttons>();
			TB.SetName(str);
			go.transform.SetParent(Button_Template_ComplexObjects.transform.parent);
		}
	}


	public void ButtonClicked(string str)
	{
		//Debug.Log(str + " button clicked.");
	}     

}
