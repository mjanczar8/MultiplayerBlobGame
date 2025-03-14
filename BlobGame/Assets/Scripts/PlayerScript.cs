using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.TopDownShooter;

public class PlayerScript : NetworkBehaviour
{
	[SerializeField] public float startSpeed = 5f;
	[SerializeField] public int mass = 5;
	[SerializeField] public TMP_Text massText;
	[SerializeField] public TMP_Text totalMassText;
	[SerializeField] float centerMassRadius = 1.5f;
	public string username = "";
	public int totalMass = 5;
	public int gamesPlayed;
	public int kills;
	private float speed;
	private Rigidbody2D rb;
	private Camera mainCam;
	private CameraTopDown topDown;
	private Transform plrTransform;

	private static List<PlayerScript> allPlayers = new List<PlayerScript>();

	private void Start()
	{
		allPlayers.Add(this);

		plrTransform = GetComponent<Transform>();
		mainCam = GameObject.Find("MainCamera").GetComponent<Camera>();
		topDown = mainCam.GetComponent<CameraTopDown>();
		rb = GetComponent<Rigidbody2D>();

		rb.gravityScale = 0;
		rb.freezeRotation = true;
		rb.velocity = Vector2.zero;

		topDown.playerTransform = plrTransform;
		
	}

	void Update()
	{
		if (isLocalPlayer)
		{
			MovePlayer();
			UpdateSize();
			UpdateLayerOrder();
			EatPlayerCheck();
			massText.text = mass.ToString();
			totalMassText.text = totalMass.ToString();
		}
	}

	void MovePlayer()
	{
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");

		Vector3 moveDir = new Vector3(moveX, moveY, 0).normalized;

		speed = Mathf.Max(1f, startSpeed - (mass * 0.02f)); //logarithmic slow based on mass

		CmdMovePlayer(transform.position += moveDir * speed * Time.deltaTime);
	}

	void EatPlayerCheck()
	{
		foreach (PlayerScript otherPlayer in allPlayers)
		{
			if (otherPlayer == this) continue;

			float distance = Vector2.Distance(this.transform.position, otherPlayer.transform.position);

			if (distance < centerMassRadius && this.mass > otherPlayer.mass) //ensure the attacker is bigger
			{
				EatPlayer(otherPlayer);
			}
		}
	}

	void EatPlayer(PlayerScript target)
	{
		Debug.Log($"{target.name} was eaten by {this.name}!");

		kills++;
		this.mass += target.mass / 2;

		allPlayers.Remove(target);
		Destroy(target.gameObject);
	}

	void OnDestroy()
	{
		//save kills
		//save total mass
		//count rounds played

		allPlayers.Remove(this);
	}

	void UpdateLayerOrder()
	{
		this.GetComponent<SpriteRenderer>().sortingOrder = mass;
	}

	void UpdateSize()
	{
		float newSize = 0.1f * mass;
		transform.localScale = new Vector3(newSize, newSize, 1);
	}

	public void EatFood()
	{
		if (mass < 100)
		{
			mass += 1;
			totalMass += 1;
			UpdateSize();
		}
		else if (mass >= 100)
		{
			StartCoroutine(WeightLoss());
			this.GetComponent<SpriteRenderer>().color = Color.yellow;
		}
	}

	IEnumerator WeightLoss()
	{
		do
		{
			mass--;
			yield return new WaitForSeconds(0f);
		} while (mass > 50);
	}

	[Command]
	void CmdMovePlayer(Vector3 newPosition)
	{
		transform.position = newPosition;
	}
}
