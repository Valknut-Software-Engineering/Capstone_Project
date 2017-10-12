using UnityEngine;
using System.Collections;

public class CylinderObject : PrimitiveObject {
	public CylinderObject() {
		this.generic = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		this.setAttributes();
	}
}