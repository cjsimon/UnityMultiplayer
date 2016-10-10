using UnityEngine;

public class Rotate:MonoBehaviour {
	void Start() {
	}

	void Update() {
		transform.Rotate(15 * Vector3.right * Time.deltaTime, Space.World);
		transform.Rotate(10 * Vector3.up * Time.deltaTime, Space.World);
	}
}
