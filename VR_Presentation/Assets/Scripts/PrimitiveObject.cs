using UnityEngine;
using System.Collections;

public abstract class PrimitiveObject {
	protected GameObject generic;
	
	public PrimitiveObject() {
		
	}
	public void setAttributes() {
		generic.AddComponent<Rigidbody>(); // Add the rigidbody.
        generic.AddComponent<Pickupable>(); // Add the canPickup script.
		generic.GetComponent<Rigidbody>().useGravity = false;
        generic.GetComponent<Rigidbody>().isKinematic = true;
	}
	public GameObject getPrimitiveObj() {
		return generic;
	}
}