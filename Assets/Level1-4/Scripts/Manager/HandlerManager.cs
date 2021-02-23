using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlerManager : MonoBehaviour 
{
	public float sleepTime = 5f;
	public float actionTime;

	public GameObject handler;
	private GameObject hand;

	// Use this for initialization
	void Start()
	{
		actionTime = sleepTime;
	}

	// Update is called once per frame
	public void UpdateHandler(Item item)
	{
		if (Input.GetMouseButton(0))
		{
			ResetTimer();
			Destroy(hand);
		}

		if (actionTime <= 0)
		{
			CreateHandler(item);
			ResetTimer();
		}
		else
		{
			actionTime -= Time.deltaTime;
		}
	}


	public void ResetTimer()
	{
		actionTime = sleepTime;
	}

	void CreateHandler(Item item)
    {
		hand = Instantiate(handler);
		hand.GetComponent<Handler>().Setup(item);

		hand.name = "Handler";
    }
}
