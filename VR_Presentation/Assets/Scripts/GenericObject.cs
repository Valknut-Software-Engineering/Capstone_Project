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
		distance = 6;
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
				MonoBehaviour.Destroy(isPickUpable);
			} else {
				
				gameObj.AddComponent<Pickupable>();
				
				/*if (gameObj.GetComponent<Rigidbody>() == null) {
					gameObj.AddComponent<Rigidbody>();
				}*/
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
	
	//// Spawn new objects and place in hand \\\\
	public void spawnCube() {
		//GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		PrimitiveObject myCube = new CubeObject();
		/*
		cube.AddComponent<Rigidbody>(); // Add the rigidbody.
        cube.AddComponent<Pickupable>(); // Add the canPickup script.
		cube.GetComponent<Rigidbody>().useGravity = false;
        cube.GetComponent<Rigidbody>().isKinematic = true;
		*/
        if (myObject) { dropObject(); }
        
        myObject = myCube.getPrimitiveObj();
	}
	
	public void spawnSphere()
    {
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        
		sphere.AddComponent<Rigidbody>(); // Add the rigidbody.
        sphere.AddComponent<Pickupable>(); // Add the canPickup script.
		sphere.GetComponent<Rigidbody>().useGravity = false;
        sphere.GetComponent<Rigidbody>().isKinematic = true;
		
        if (myObject) { dropObject(); }
        
        myObject = sphere;
    }

    public void spawnCapsule()
    {
		GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        
		capsule.AddComponent<Rigidbody>(); // Add the rigidbody.
        capsule.AddComponent<Pickupable>(); // Add the canPickup script.
		capsule.GetComponent<Rigidbody>().useGravity = false;
		capsule.GetComponent<Rigidbody>().isKinematic = true;
		
        if (myObject) { dropObject(); }
        
		myObject = capsule;
    }

    public void spawnCylinder()
    {
		GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        
		cylinder.AddComponent<Rigidbody>(); // Add the rigidbody.
        cylinder.AddComponent<Pickupable>(); // Add the canPickup script.
		cylinder.GetComponent<Rigidbody>().useGravity = false;
        cylinder.GetComponent<Rigidbody>().isKinematic = true;
		
        if (myObject) { dropObject(); }
        
		myObject = cylinder;
    }
	
}

