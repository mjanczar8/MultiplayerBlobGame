using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class PlayerScript : MonoBehaviour
{
	[SerializeField] public float speed = 3f;
	[SerializeField] public int mass = 5;
	[SerializeField] public TMP_Text massText;
	[SerializeField] float centerMassRadius = 1.5f;

	private static List<PlayerScript> allPlayers = new List<PlayerScript>();

	private void Start()
	{
		allPlayers.Add(this);
	}

	void Update()
	{

		MovePlayer();
		UpdateSize();
		UpdateLayerOrder();
		EatPlayerCheck();
		massText.text = mass.ToString();
	}
	void MovePlayer()
	{
		float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");

		Vector3 moveDir = new Vector3(moveX, moveY, 0).normalized;
		transform.position += moveDir * speed * Time.deltaTime;
	}

	void EatPlayerCheck()
	{
		foreach (PlayerScript otherPlayer in allPlayers)
		{
			if (otherPlayer == this) continue; // Skip self-check

			float distance = Vector2.Distance(this.transform.position, otherPlayer.transform.position);

			if (distance < centerMassRadius && this.mass > otherPlayer.mass) // Ensure the attacker is bigger
			{
				EatPlayer(otherPlayer);
			}
		}
	}

	void EatPlayer(PlayerScript target)
	{
		Debug.Log($"{target.name} was eaten by {this.name}!");

		
		this.mass += target.mass / 2;

		
		allPlayers.Remove(target);
		Destroy(target.gameObject);
	}

	void OnDestroy()
	{
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
		if(mass < 100)
		{
			mass += 1;
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
}
