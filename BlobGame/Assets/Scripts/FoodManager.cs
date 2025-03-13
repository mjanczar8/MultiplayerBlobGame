using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [SerializeField]
    public GameObject Food;
    [SerializeField]
    public int maxFood = 100;

    public Vector2 spawnRangeX = new Vector2(-8f, 8f);
	public Vector2 spawnRangeY = new Vector2(-5f, 5f);


	// Start is called before the first frame update
	void Start()
    {
        InitializeFood();
    }
    
    void InitializeFood()
    {
        for (int i = 0; i < maxFood; i++)
        {
            SpawnFood();
        }
    }

    public void SpawnFood()
    {
        //random pos
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnRangeX.x, spawnRangeX.y),
            Random.Range(spawnRangeY.x, spawnRangeY.y),
            0
        );

        //random color
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        Food.GetComponent<SpriteRenderer>().color = randomColor;

        //instantiate
        GameObject food = Instantiate(Food, randomPosition, Quaternion.identity);
	}
}
