using UnityEngine;
using System.Collections;
using System.IO;

public class GenericObject {
	
	private GameObject mainCam;
    private GameObject myObject;
	
	private bool snapToGrid;
	private bool useRotationOffset;
	
    private float distance;
    private float smooth;
	
	public GenericObject() {
		mainCam = GameObject.FindWithTag("MainCamera");
		
		myObject = null;
		snapToGrid = false;
		useRotationOffset = false;
		distance = 10;
		smooth = 10;
	}
	
	public GenericObject(bool stg, bool uro, float d, float s) {
		mainCam = GameObject.FindWithTag("MainCamera");
		
		snapToGrid = stg;
		useRotationOffset = uro;
		distance = d;
		smooth = s;
	}
	
	public bool isCarrying() {
		return (myObject != null);
	}
	
	public GameObject getObject() {
		return myObject;
	}

    //Reset distance
    public void reset_Distance()
    {
        distance = 10;
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
	
	public void updateObjPosition(GameObject player) {
		if(!snapToGrid) {
			myObject.transform.position = Vector3.Lerp(myObject.transform.position, mainCam.transform.position + mainCam.transform.forward * distance, Time.deltaTime * smooth);
		} else {
			//snap object to grid when moving
			Vector3 curPosInGrid = mainCam.transform.position + mainCam.transform.forward * distance;
			Vector3 newPosInGrid = new Vector3(Mathf.Round(curPosInGrid.x), Mathf.Round(curPosInGrid.y), Mathf.Round(curPosInGrid.z));
			myObject.transform.position = newPosInGrid;
		}
		
		//Rotate object as player rotates
		myObject.transform.parent = player.transform; // Make the object that collided with the player a child of the player
		if(useRotationOffset) {
			myObject.transform.localRotation = Quaternion.Euler(Vector3.forward); 	// Not exactly sure what this does but if I leave it out it becomes random
			myObject.transform.localRotation = Quaternion.Euler(90,0,90); 			// offset the rotation to stay in relation to player
		}
	}
	
	public void increaseOverallSize() {
		myObject.transform.localScale += new Vector3(0.01F, 0.01F, 0.01F);
	}
	public void decreaseOverallSize() {
		myObject.transform.localScale -= new Vector3(0.01F, 0.01F, 0.01F);
	}
	public void increaseX() {
		myObject.transform.localScale += new Vector3(0.01F, 0, 0);
	}
	public void increaseY() {
		myObject.transform.localScale += new Vector3(0, 0.01F, 0);
	}
	public void increaseZ() {
		myObject.transform.localScale += new Vector3(0, 0, 0.01F);
	}
	
	public void decreaseX() {
		myObject.transform.localScale -= new Vector3(0.01F, 0, 0);
	}
	public void decreaseY() {
		myObject.transform.localScale -= new Vector3(0, 0.01F, 0);
	}
	public void decreaseZ() {
		myObject.transform.localScale -= new Vector3(0, 0, 0.01F);
	}
	
	public void rotateX() {
		myObject.transform.Rotate(0.5F, 0, 0);
	}
	public void rotateY() {
		myObject.transform.Rotate(0, 0.5F, 0);
	}
	public void rotateZ() {
		myObject.transform.Rotate(0, 0, 0.5F);
	}
	
	public void rotateReset() {
		useRotationOffset = false;
		myObject.transform.rotation = Quaternion.Euler(0, 0, 0);
		Rigidbody rb = myObject.GetComponent<Rigidbody>();
		rb.angularVelocity = Vector3.zero;
	}
	
	public bool pickUpWorldObject() {
		int x = Screen.width / 2;
		int y = Screen.height / 2;
		
		Ray myRay = mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
		RaycastHit hit;
		
		if (Physics.Raycast(myRay, out hit)) {
			Pickupable isPickUpable = hit.collider.GetComponent<Pickupable>();
			
			//if the object the RayCast hit has the Pickupable script:
			//if (isPickUpable != null && gameObject.GetComponent<Rigidbody>() != null) {
			if (isPickUpable != null) {
				if(isPickUpable.gameObject.GetComponent<Rigidbody>() == null)
					isPickUpable.gameObject.AddComponent<Rigidbody>();
				
				isPickUpable.gameObject.GetComponent<Rigidbody>().useGravity = false;
				isPickUpable.gameObject.GetComponent<Rigidbody>().isKinematic = true;
				myObject = isPickUpable.gameObject;
			}
		} else {
			return true;
		}
		
		return true;
	}
	
	public bool deleteWorldObject() {
		int x = Screen.width /2;
		int y = Screen.height /2;
		
		Ray myRay = mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
		RaycastHit hit;
		
		if(Physics.Raycast(myRay, out hit)) {
			Pickupable isPickUpable = hit.collider.GetComponent<Pickupable>();
			
			//if the object the RayCast hit has the canPickup script:
			if(isPickUpable != null) {
				MonoBehaviour.Destroy(isPickUpable.gameObject);
			}
		} else {
			return false;
		}
		
		return true;
	}
	
	public bool duplicateObject() {
		int x = Screen.width / 2;
		int y = Screen.height / 2;
		
		Ray myRay = mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
		RaycastHit hit;
		
		if (Physics.Raycast(myRay, out hit)) {
			Pickupable isPickUpable = hit.collider.GetComponent<Pickupable>();
			
			//if the object the RayCast hit has the canPickup script:
			if (isPickUpable != null) {
				GameObject sourceObj = isPickUpable.gameObject;
				GameObject cloneObj;
				
				cloneObj = MonoBehaviour.Instantiate(sourceObj, new Vector3(1F, 1F, 1F), Quaternion.identity);
				cloneObj.transform.localScale = sourceObj.transform.localScale*3;
				//isCarrying = true;
				if (cloneObj.gameObject.GetComponent<Rigidbody>() == null)
					cloneObj.gameObject.AddComponent<Rigidbody>();
				
				//isPickUpable.gameObject.GetComponent<Rigidbody>().useGravity = false;
				cloneObj.gameObject.GetComponent<Rigidbody>().useGravity = false;
				cloneObj.gameObject.GetComponent<Rigidbody>().isKinematic = true;
				
				myObject.GetComponent<Rigidbody>().useGravity = true;
				myObject.GetComponent<Rigidbody>().isKinematic = false;
				myObject.transform.parent = null;
				
				myObject = cloneObj;
			}
		} else {
			return false;
		}
		
		return true;
	}
	
	public bool toggleCanInteract() {
		if(myObject) { floatObject(); }
		
		int x = Screen.width / 2;
		int y = Screen.height / 2;
		
		Ray myRay = mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
		RaycastHit hit;
		
		if (Physics.Raycast(myRay, out hit))
		{
			GameObject gameObj = hit.collider.gameObject;
			Pickupable isPickUpable = gameObj.GetComponent<Pickupable>();
			Debug.Log(hit.collider.gameObject.name);
			
			if (hit.collider.gameObject.GetComponent<TerrainCollider>() == true)
			{
				return false;
			} else if(isPickUpable != null) {
				if (gameObj.GetComponent<Rigidbody>() != null) {
                    MonoBehaviour.Destroy(gameObj.GetComponent<Rigidbody>());
                }
                //MonoBehaviour.Destroy(gameObj.GetComponent<MeshCollider>());
                MonoBehaviour.Destroy(isPickUpable);
                //gameObj.AddComponent<MeshCollider>();
                //gameObj.GetComponent<MeshCollider>().convex = false;
            } else {
				
				gameObj.AddComponent<Pickupable>();
			
				if (gameObj.GetComponent<BoxCollider>() == null) {
					gameObj.AddComponent<BoxCollider>();
				}
				if (gameObj.GetComponent<MeshCollider>() == null) {
					gameObj.AddComponent<MeshCollider>().convex = true;
				} else {
					MonoBehaviour.Destroy(gameObj.GetComponent<MeshCollider>());
					gameObj.AddComponent<MeshCollider>().convex = true;
				}
			}
		} else {
			return false;
		}
		return true;
	}
	
	//Drop the object being held by the user
	public void dropObject() {
		myObject.transform.parent = null;
		myObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
		myObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		myObject = null;
	}
	//Float the object being held by the user
	public void floatObject() {
		myObject.transform.parent = null;
        MonoBehaviour.Destroy(myObject.gameObject.GetComponent<Rigidbody>());
		myObject = null;
	}

    //Increase the objects distance 
    public void increase_Distance()
    {
        /*Vector3 pos =  myObject.transform.position;
        pos.x += 1;
        pos.z -= 1;
        myObject.transform.position = pos;*/
        distance += 1;
    }

    //Decrease the objects distance 
    public void decrease_Distance()
    {
        /*Vector3 pos = myObject.transform.position;
        pos.x -= 1;
        pos.z += 1;
        myObject.transform.position = pos;*/
        distance -= 1;
    }

    //// Spawn new primitive objects with preset properties and place in hand \\\\
    public void spawnCube() {
		PrimitiveObject myCube = new CubeObject();
		if (myObject) { dropObject(); }
        myObject = myCube.getPrimitiveObj();
	}
	public void spawnSphere()
    {
		PrimitiveObject mySphere = new SphereObject();
        if (myObject) { dropObject(); }
        myObject = mySphere.getPrimitiveObj();
    }
	public void spawnCapsule()
    {
		PrimitiveObject myCapsule = new CapsuleObject();
        if (myObject) { dropObject(); }
		myObject = myCapsule.getPrimitiveObj();
    }
	public void spawnCylinder()
    {
		PrimitiveObject myCylinder = new CylinderObject();
        if (myObject) { dropObject(); }
		myObject = myCylinder.getPrimitiveObj();
    }
	//// End of spawn methods \\\\
}

