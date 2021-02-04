using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour {

	Rigidbody2D rb;
	float dirX;
	public float jumpPower = 1500f, playerSpeed = 15f;
	int playerLayer, platformLayer;
	bool jumpOffCoroutineIsRunning = false;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		playerLayer = LayerMask.NameToLayer ("Player");
		platformLayer = LayerMask.NameToLayer ("Ground");
	}
	
	// Update is called once per frame
	void Update () {
		dirX = Input.GetAxis ("Horizontal");

		if (Input.GetButtonDown ("Jump") && !Input.GetKey (KeyCode.S) && rb.velocity.y == 0) {
			rb.AddForce (Vector2.up * jumpPower, ForceMode2D.Force);

		} else if (Input.GetButtonDown ("Jump") && Input.GetKey (KeyCode.S) && !jumpOffCoroutineIsRunning) {
			StartCoroutine ("JumpOff");
		}
		
		if (rb.velocity.y > 0)
			Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, true);
		
		else if (rb.velocity.y <= 0 && !jumpOffCoroutineIsRunning)
			Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, false);			
	}

	void FixedUpdate()
	{
		rb.velocity = new Vector2 (dirX * playerSpeed, rb.velocity.y);
	}

	IEnumerator JumpOff()
	{
		jumpOffCoroutineIsRunning = true;
		Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, true);
		yield return new WaitForSeconds (0.5f);
		Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, false);
		jumpOffCoroutineIsRunning = false;
	}

}
