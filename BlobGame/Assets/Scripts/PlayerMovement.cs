using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
	public float speed;
	public float startSpeed = 5f;
	public int mass;
	private Rigidbody2D rb;
	private PlayerScript ps;

	void Start()
	{
		ps = GetComponent<PlayerScript>();
		rb = GetComponent<Rigidbody2D>();

		rb.gravityScale = 0;
		rb.freezeRotation = true;
		rb.velocity = Vector2.zero;
	}
	// Update is called once per frame
	void Update()
    {
		mass = ps.mass;

		if (isLocalPlayer)
		{
			float moveX = Input.GetAxisRaw("Horizontal");
			float moveY = Input.GetAxisRaw("Vertical");

			Vector3 moveDir = new Vector3(moveX, moveY, 0).normalized;


			speed = Mathf.Max(1f, startSpeed - (mass * 0.02f)); //logarithmic slow based on mass

			CmdMovePlayer(transform.position += moveDir * speed * Time.deltaTime);
		}

	}

	[Command]
	void CmdMovePlayer(Vector3 newPosition)
	{
		transform.position = newPosition;
	}
}
