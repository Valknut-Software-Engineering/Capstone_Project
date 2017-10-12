using UnityEngine;
using System.Collections;

public class ObjectManipulation : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//Toggle snap to grid
        if (Input.GetKeyDown(KeyCode.G)) {
			Globals.genericObj.toggleSTG();
        }
        //Toggle player rotation offset
        if (Input.GetKeyDown(KeyCode.Q)) {
			Globals.genericObj.toggleURO();
        }
        if (Globals.genericObj.isCarrying()) {
            carry();
        } else {
            interact();
        }	
	}
	
	private void interact()
    {
		if (Input.GetKeyDown(KeyCode.E)) {
            //Pick up object being aimed at
			Globals.genericObj.pickUpWorldObject();
        } else if(Input.GetKey(KeyCode.Delete)) {
			//Delete object
			Globals.genericObj.deleteWorldObject();
		}
    }
	
	private void carry() {
        Globals.genericObj.updateObjPosition(gameObject);
		checkKeyPressOnCarry();
		checkDrop();
    }
	
	private void checkKeyPressOnCarry() {
        //Variable to check mouse scroll  
        var wheel = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetKey(KeyCode.KeypadPlus)) {
            //ALL AXIS MAKE LARGER
			Globals.genericObj.increaseOverallSize();
        } else if (Input.GetKey(KeyCode.KeypadMinus)) {
            //ALL AXIS MAke SMALler
			Globals.genericObj.decreaseOverallSize();
        } else if (Input.GetKey(KeyCode.Keypad1)) {
            // X AXIS MAKE LARGER
			Globals.genericObj.increaseX();
        } else if (Input.GetKey(KeyCode.Keypad2)) {
            // Y AXIS MAKE LARGER
			Globals.genericObj.increaseY();
        } else if (Input.GetKey(KeyCode.Keypad3)) {
            // Z AXIS MAKE LARGER
			Globals.genericObj.increaseZ();
        } else if (Input.GetKey(KeyCode.Keypad4)) {
            // X AXIS MAKE SMALLER
			Globals.genericObj.decreaseX();
        } else if (Input.GetKey(KeyCode.Keypad5)) {
            // Y AXIS MAKE SMALLER
			Globals.genericObj.decreaseY();
        } else if (Input.GetKey(KeyCode.Keypad6)) {
            // Z AXIS MAKE SMALLER
			Globals.genericObj.decreaseZ();
        } else if (Input.GetKey(KeyCode.Keypad7)) {
            //Rotate X
			Globals.genericObj.rotateX();
        } else if (Input.GetKey(KeyCode.Keypad8)) {
            //Rotate Y
			Globals.genericObj.rotateY();
        } else if (Input.GetKey(KeyCode.Keypad9)) {
            //Rotate Z
			Globals.genericObj.rotateZ();
        } else if (Input.GetKey(KeyCode.R)) {
            //RESET Rotation
			Globals.genericObj.rotateReset();
        } else if (Input.GetKeyDown(KeyCode.C)) {
            //Duplicate current object
			Globals.genericObj.duplicateObject();
        }
        //Scroll up
        else if (wheel > 0f)
        {
            Globals.genericObj.increase_Distance();
        }
        //Scroll down 
        else if (wheel < 0f)
        {
            Globals.genericObj.decrease_Distance();
        }
	}
	
	//Test if player is trying to drop of float object
	void checkDrop() {
        if (Input.GetKeyDown(KeyCode.E)) {
			Globals.genericObj.dropObject();
            Globals.genericObj.reset_Distance();
		} else if (Input.GetKeyDown(KeyCode.F)) {
			Globals.genericObj.floatObject();
		}
    }
	
	
}
