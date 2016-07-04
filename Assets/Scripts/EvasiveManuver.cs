using UnityEngine;
using System.Collections;

//[System.Serializable]
//public class Boundary {
//  public float xMin, xMax, zMin, zMax;
//}


public class EvasiveManuver : MonoBehaviour {
  public float dodge, smoothing, tilt;
  public Vector2 startWait, manuverWait, manuverTime;
  public Boundary boundary;

  private float targetManuver, currentSpeed;
  private Rigidbody rb;

	void Start () {
    rb           = GetComponent <Rigidbody> ();
    currentSpeed = rb.velocity.z;
    StartCoroutine (Evade ());
	}
	
  IEnumerator Evade () {
    yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));

    while (true) {
      targetManuver = Random.Range (1, dodge) * -Mathf.Sign (transform.position.x);
      yield return new WaitForSeconds (Random.Range(manuverTime.x, manuverTime.y));
      targetManuver = 0;
      yield return new WaitForSeconds (Random.Range(manuverWait.x, manuverWait.y));
    }
  }

	void FixedUpdate () {
    float newManuver = Mathf.MoveTowards (rb.velocity.x, targetManuver, Time.deltaTime * smoothing);
    rb.velocity = new Vector3 (newManuver, 0.0f, currentSpeed);
    rb.position = new Vector3 (
      Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax),
      0.0f,
      Mathf.Clamp (rb.position.z, boundary.zMin, boundary.zMax)
    );

    rb.rotation  = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}
}
