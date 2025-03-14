using Mirror;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
	private FoodManager _foodManager;

	private void Start()
	{
		_foodManager = FindObjectOfType<FoodManager>();
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<PlayerScript>().EatFood();
			_foodManager.SpawnFood();
			Destroy(gameObject);
		}
	}
}

