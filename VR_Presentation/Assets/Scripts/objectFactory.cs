public class objectFactory {
	//use getObject method to get object of type PrimitiveObject 
	public PrimitiveObject getObject(string objType) {
		if(objType == null || objType == "")
			return null;
		
		if(objType == "Cube") {
			return new CubeObject();
		} else if (objType == "Sphere") {
			return new SphereObject();
		} else if (objType == "Capsule") {
			return new CapsuleObject();
		} else if (objType == "Cylinder") {
			return new CylinderObject();
		}
		
		return null;
	}
}