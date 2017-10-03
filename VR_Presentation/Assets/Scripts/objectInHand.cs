using UnityEngine;
using System.Collections;

public class objectInHand {
	
	private static GameObject mainCam;
    private static GameObject pickedUpObject;
	
	private static bool snapToGrid;
	private static bool useRotationOffset;
	
    private static float distance;
    private static float smooth;
	
	public objectInHand() {
		mainCam = GameObject.FindWithTag("MainCamera");
		
		pickedUpObject = null;
		snapToGrid = false;
		useRotationOffset = false;
		distance = 0;
		smooth = 0;
	}
	
	public bool isCarrying() {
		return (pickedUpObject == null);
	}
	
	//Toggle snap to grid
	public void toggleSTG() {
		if (snapToGrid)
			snapToGrid = false;
		else
			snapToGrid = true;
	}
	//Toggle player rotation offset
	public void toggleURO() {
		if (useRotationOffset)
			useRotationOffset = false;
		else
			useRotationOffset = true;
	}
	
}

