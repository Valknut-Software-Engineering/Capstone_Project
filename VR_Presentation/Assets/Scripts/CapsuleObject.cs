using UnityEngine;
using System.Collections;

public class CapsuleObject : PrimitiveObject {
	public CapsuleObject() {
		this.generic = GameObject.CreatePrimitive(PrimitiveType.Capsule);
		this.setAttributes();
	}
}