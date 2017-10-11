using UnityEngine;
using System.Collections;

public class SphereObject : PrimitiveObject {
	public SphereObject() {
		this.generic = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		this.setAttributes();
	}
}