using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary {
  public float xMin, xMax, zMin, zMax;
}


public class PlayerController : MonoBehaviour {
  private Rigidbody rb;
  private AudioSource aud;
  public float speed, tilt;
  public Boundary boundary;
  public GameObject shot;
  public Transform[] shotSpawns;
  public float fireRate;

  private float nextFire;
  private GameController gameController;

  void Start () {
    rb  = GetComponent<Rigidbody>();
    aud = GetComponent<AudioSource>();

    GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
    if (gameControllerObject != null) {
      gameController = gameControllerObject.GetComponent <GameController>();
    } 
    if (gameController == null) {
      Debug.Log("Cannot find 'GameController' script");
    }
  }

	void FixedUpdate () {
	  float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical   = Input.GetAxis("Vertical");

    Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

    rb.velocity = movement * speed;

    rb.position = new Vector3 (
      Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax),
      0.0f,
      Mathf.Clamp (rb.position.z, boundary.zMin, boundary.zMax)
    );

    rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}

  void Update () {
    if (Input.GetButton("Fire1") && Time.time > nextFire) {
      nextFire = Time.time + fireRate;

      if (gameController.GetScore () >= 20 && gameController.GetScore () < 50) {
        Instantiate(shot, shotSpawns[1].position, shotSpawns[1].rotation);
        Instantiate(shot, shotSpawns[2].position, shotSpawns[2].rotation);
      } else if (gameController.GetScore () >= 50) {
        Instantiate(shot, shotSpawns[0].position, shotSpawns[0].rotation);
        Instantiate(shot, shotSpawns[3].position, shotSpawns[3].rotation);
        Instantiate(shot, shotSpawns[4].position, shotSpawns[4].rotation);
      } else {
        Instantiate(shot, shotSpawns[0].position, shotSpawns[0].rotation);
      }

      aud.Play ();
    }
  }
}
