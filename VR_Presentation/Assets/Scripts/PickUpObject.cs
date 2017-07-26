using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public GameObject thecamera;
    public GameObject mainCam;
    public GameObject pickedUpObject;
    
	public bool isCarrying = false;
	public bool snapToGrid = false;
	public bool useRotationOffset = false;
	
    public float distance;
    public float smooth;
	
	
	// Use this for initialization
    void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera");
        thecamera = GameObject.Find("360Capture");
    }

    // Update is called once per frame
    void Update()
    {
        thecamera.transform.position = mainCam.transform.position;
       
      
        //Check for Escape and change back to menu
      
        //Spawn new object and place in hand
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray myRay = mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit;
            spawnCube();
        }
		
		//Toggle snap to grid
		if(Input.GetKeyDown(KeyCode.G)) {
			if(snapToGrid)
				snapToGrid = false;
			else
				snapToGrid = true;
		}
		
		//Toggle player rotation offset
		if(Input.GetKeyDown(KeyCode.Q))
		{
			if(useRotationOffset)
				useRotationOffset = false;
			else
				useRotationOffset = true;
		}
		
		
        if (isCarrying)
        {
            carry(pickedUpObject);
            checkDrop();
        }
        else
        {
            interact();
        }
    }

    void carry(GameObject obj)
    {
        if(!snapToGrid) {
			obj.transform.position = Vector3.Lerp(obj.transform.position, mainCam.transform.position + mainCam.transform.forward * distance, Time.deltaTime * smooth);
		}
		else //snap object to grid when moving
		{
			Vector3 curPosInGrid = mainCam.transform.position + mainCam.transform.forward * distance;
			Vector3 newPosInGrid = new Vector3(Mathf.Round(curPosInGrid.x), Mathf.Round(curPosInGrid.y), Mathf.Round(curPosInGrid.z));
			
			//obj.transform.position = Vector3.Lerp(obj.transform.position, newPosInGrid, Time.deltaTime * smooth);
			obj.transform.position = newPosInGrid;
		}
		
		//Rotate object as player rotates

		obj.transform.parent = this.transform; // Make the object that collided with the player a child of the player
		if(useRotationOffset) {
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
            useRotationOffset = false;
            obj.transform.rotation = Quaternion.Euler(0, 0, 0);
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.angularVelocity = Vector3.zero;
        }

        //Duplicate current object
        if (Input.GetKeyDown(KeyCode.C))
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray myRay = mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
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
                    isCarrying = true;
                    if (cloneObj.gameObject.GetComponent<Rigidbody>() == null)
                        cloneObj.gameObject.AddComponent<Rigidbody>();

                    //isPickUpable.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    cloneObj.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    cloneObj.gameObject.GetComponent<Rigidbody>().isKinematic = true;

                    pickedUpObject.GetComponent<Rigidbody>().useGravity = true;
                    pickedUpObject.GetComponent<Rigidbody>().isKinematic = false;

                    pickedUpObject.transform.parent = null;

                    pickedUpObject = cloneObj;
                }
            }
        }

    }

    void spawnCube()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.AddComponent<Rigidbody>(); // Add the rigidbody.
        cube.AddComponent<Pickupable>(); // Add the canPickup script.
		
        if (pickedUpObject) { dropObject(); }
        
        isCarrying = true;
        cube.GetComponent<Rigidbody>().useGravity = false;
        cube.GetComponent<Rigidbody>().isKinematic = true;
		pickedUpObject = cube;
    }
	

    //// Spawn other primitive objects \\\\

    void spawnSphere()
    {
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        
		sphere.AddComponent<Rigidbody>(); // Add the rigidbody.
        sphere.AddComponent<Pickupable>(); // Add the canPickup script.
		
        if (pickedUpObject) { dropObject(); }
        
        isCarrying = true;
        sphere.GetComponent<Rigidbody>().useGravity = false;
        sphere.GetComponent<Rigidbody>().isKinematic = true;
		pickedUpObject = sphere;
    }

    void spawnCapsule()
    {
		GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        
		capsule.AddComponent<Rigidbody>(); // Add the rigidbody.
        capsule.AddComponent<Pickupable>(); // Add the canPickup script.
		
        if (pickedUpObject) { dropObject(); }
        
		isCarrying = true;
        capsule.GetComponent<Rigidbody>().useGravity = false;
		capsule.GetComponent<Rigidbody>().isKinematic = true;
        pickedUpObject = capsule;
    }

    void spawnCylinder()
    {
		GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        
		cylinder.AddComponent<Rigidbody>(); // Add the rigidbody.
        cylinder.AddComponent<Pickupable>(); // Add the canPickup script.

        if (pickedUpObject) { dropObject(); }
        
		isCarrying = true;
        cylinder.GetComponent<Rigidbody>().useGravity = false;
        cylinder.GetComponent<Rigidbody>().isKinematic = true;
		pickedUpObject = cylinder;
    }


    void interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray myRay = mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit;

            if (Physics.Raycast(myRay, out hit))
            {
               Pickupable isPickUpable = hit.collider.GetComponent<Pickupable>();

                //if the object the RayCast hit has the canPickup script:
                if (isPickUpable != null)
                {
                    isCarrying = true;
					if(isPickUpable.gameObject.GetComponent<Rigidbody>() == null)
						isPickUpable.gameObject.AddComponent<Rigidbody>();
					
					isPickUpable.gameObject.GetComponent<Rigidbody>().useGravity = false;
					isPickUpable.gameObject.GetComponent<Rigidbody>().isKinematic = true;
					pickedUpObject = isPickUpable.gameObject;
                }
            }
        }
		
		//Delete object
		if(Input.GetKey(KeyCode.Delete)) {
			int x = Screen.width /2;
			int y = Screen.height /2;
			
			Ray myRay = mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
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

   
    void checkDrop()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
			dropObject();
		}
		if (Input.GetKeyDown(KeyCode.F)) {
			floatObject();
		}
		
    }

    //Drop the object being held by the user
	void dropObject() {
		isCarrying = false;
		pickedUpObject.transform.parent = null;
		pickedUpObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
		pickedUpObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		pickedUpObject = null;
	}
	void floatObject() {
		isCarrying = false;
		pickedUpObject.transform.parent = null;
		
		Destroy(pickedUpObject.gameObject.GetComponent<Rigidbody>());
		pickedUpObject = null;
	}
	
	
}







