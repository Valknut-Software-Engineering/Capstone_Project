using UnityEngine;
using System.Collections;

public class CubeObject : PrimitiveObject {
	public CubeObject() {
		this.generic = GameObject.CreatePrimitive(PrimitiveType.Cube);
		this.setAttributes();
	}
}