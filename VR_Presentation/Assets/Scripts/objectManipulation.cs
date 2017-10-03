using UnityEngine;
using System.Collections;

public class objectManipulation : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//Toggle snap to grid
        if (Input.GetKeyDown(KeyCode.G)) {
			Globals.carriedObj.toggleSTG();
        }
        //Toggle player rotation offset
        if (Input.GetKeyDown(KeyCode.Q)) {
			Globals.carriedObj.toggleURO();
        }
        if (Globals.carriedObj.isCarrying()) {
            carry(Globals.pickedUpObject);
            checkDrop();
        } else {
            interact();
        }
		
	}
	
	void interact()
    {
        //Pick up object being aimed at
		if (Input.GetKeyDown(KeyCode.E))
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray myRay = Globals.mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit;

            if (Physics.Raycast(myRay, out hit))
            {
               Pickupable isPickUpable = hit.collider.GetComponent<Pickupable>();

                //if the object the RayCast hit has the canPickup script:
                if (isPickUpable != null && gameObject.GetComponent<Rigidbody>() != null)
                {
                    Globals.isCarrying = true;
					if(isPickUpable.gameObject.GetComponent<Rigidbody>() == null)
						isPickUpable.gameObject.AddComponent<Rigidbody>();
					
					isPickUpable.gameObject.GetComponent<Rigidbody>().useGravity = false;
					isPickUpable.gameObject.GetComponent<Rigidbody>().isKinematic = true;
					Globals.pickedUpObject = isPickUpable.gameObject;
                }
            }
        }
		
		//Delete object
		if(Input.GetKey(KeyCode.Delete)) {
			int x = Screen.width /2;
			int y = Screen.height /2;
			
			Ray myRay = Globals.mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
			RaycastHit hit;
			
			if(Physics.Raycast(myRay, out hit)) {
				Pickupable isPickUpable = hit.collider.GetComponent<Pickupable>();
				
				//if the object the RayCast hit has the canPickup script:
				if(isPickUpable != null) {
					Destroy(isPickUpable.gameObject);
				}
			}
		}
    }
	
	void carry(GameObject obj)
    {
        if(!Globals.snapToGrid) {
			obj.transform.position = Vector3.Lerp(obj.transform.position, Globals.mainCam.transform.position + Globals.mainCam.transform.forward * Globals.distance, Time.deltaTime * Globals.smooth);
		}
		else //snap object to grid when moving
		{
			Vector3 curPosInGrid = Globals.mainCam.transform.position + Globals.mainCam.transform.forward * Globals.distance;
			Vector3 newPosInGrid = new Vector3(Mathf.Round(curPosInGrid.x), Mathf.Round(curPosInGrid.y), Mathf.Round(curPosInGrid.z));
			
			//obj.transform.position = Vector3.Lerp(obj.transform.position, newPosInGrid, Time.deltaTime * Globals.smooth);
			obj.transform.position = newPosInGrid;
		}
		
		//Rotate object as player rotates
		obj.transform.parent = this.transform; // Make the object that collided with the player a child of the player
		if(Globals.useRotationOffset) {
			obj.transform.localRotation = Quaternion.Euler(Vector3.forward); // Not exactly sure what this does but if I leave it out it becomes random
			obj.transform.localRotation = Quaternion.Euler(90,0,90); // offset the rotation to stay in relation to player
		}

        //ALL AXIS MAKE LARGER
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            obj.transform.localScale += new Vector3(0.01F, 0.01F, 0.01F);
        }
        //ALL AXIS MAke SMALler
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            obj.transform.localScale -= new Vector3(0.01F, 0.01F, 0.01F);
        }
        // AXIS MAKE LARGER
        if (Input.GetKey(KeyCode.Keypad1))
        {
            obj.transform.localScale += new Vector3(0.01F, 0, 0);
        }
        if (Input.GetKey(KeyCode.Keypad2))
        {
            obj.transform.localScale += new Vector3(0, 0.01F, 0);
        }
        if (Input.GetKey(KeyCode.Keypad3))
        {
            obj.transform.localScale += new Vector3(0, 0, 0.01F);
        }

        //AXIS MAKE SMALLER
        if (Input.GetKey(KeyCode.Keypad4))
        {
            obj.transform.localScale -= new Vector3(0.01F, 0, 0);
        }
        if (Input.GetKey(KeyCode.Keypad5))
        {
            obj.transform.localScale -= new Vector3(0, 0.01F, 0F);
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            obj.transform.localScale -= new Vector3(0, 0, 0.01F);
        }

        //Rotation
        if (Input.GetKey(KeyCode.Keypad7))
        {
            obj.transform.Rotate(0.5F, 0, 0);
        }
        if (Input.GetKey(KeyCode.Keypad8))
        {
            obj.transform.Rotate(0, 0.5F, 0F);
        }
        if (Input.GetKey(KeyCode.Keypad9))
        {
            obj.transform.Rotate(0, 0, 0.5F);
        }

        //RESET Rotation
        if (Input.GetKey(KeyCode.R))
        {
            Globals.useRotationOffset = false;
            obj.transform.rotation = Quaternion.Euler(0, 0, 0);
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.angularVelocity = Vector3.zero;
        }

        //Duplicate current object
        if (Input.GetKeyDown(KeyCode.C))
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray myRay = Globals.mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit;

            if (Physics.Raycast(myRay, out hit))
            {
                Pickupable isPickUpable = hit.collider.GetComponent<Pickupable>();

                //if the object the RayCast hit has the canPickup script:
                if (isPickUpable != null)
                {
                  
                    GameObject sourceObj = isPickUpable.gameObject;
                    GameObject cloneObj;
                  
                    cloneObj = Instantiate(sourceObj, new Vector3(1F, 1F, 1F), Quaternion.identity);
                    cloneObj.transform.localScale = sourceObj.transform.localScale*3;
                    Globals.isCarrying = true;
                    if (cloneObj.gameObject.GetComponent<Rigidbody>() == null)
                        cloneObj.gameObject.AddComponent<Rigidbody>();

                    //isPickUpable.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    cloneObj.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    cloneObj.gameObject.GetComponent<Rigidbody>().isKinematic = true;

                    Globals.pickedUpObject.GetComponent<Rigidbody>().useGravity = true;
                    Globals.pickedUpObject.GetComponent<Rigidbody>().isKinematic = false;

                    Globals.pickedUpObject.transform.parent = null;

                    Globals.pickedUpObject = cloneObj;
                }
            }
        }
    }
	
	//Test if player is trying to drop of float object
	void checkDrop() {
        if (Input.GetKeyDown(KeyCode.E)) {
			dropObject();
		}
		if (Input.GetKeyDown(KeyCode.F)) {
			floatObject();
		}
    }
	
	//Drop the object being held by the user
	void dropObject() {
		Globals.isCarrying = false;
		Globals.pickedUpObject.transform.parent = null;
		Globals.pickedUpObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
		Globals.pickedUpObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		Globals.pickedUpObject = null;
	}
	//Float the object being held by the user
	void floatObject() {
		Globals.isCarrying = false;
		Globals.pickedUpObject.transform.parent = null;
        Destroy(Globals.pickedUpObject.gameObject.GetComponent<Rigidbody>());
		Globals.pickedUpObject = null;
	}
	
}
