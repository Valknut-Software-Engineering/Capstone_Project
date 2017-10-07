

using UnityEngine;
using System.Collections;

public class PrimitiveObject {
	public GameObject generic;
	
	public PrimitiveObject() {
		generic = new GameObject();
		
		generic.AddComponent<Rigidbody>(); // Add the rigidbody.
        generic.AddComponent<Pickupable>(); // Add the canPickup script.
		generic.GetComponent<Rigidbody>().useGravity = false;
        generic.GetComponent<Rigidbody>().isKinematic = true;
		
	}
	public GameObject getPrimitiveObj() {
		return generic;
	}
}