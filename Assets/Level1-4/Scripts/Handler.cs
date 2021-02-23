using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler : MonoBehaviour 
{
	public Vector2 startPosition;
	public Vector2 targetPosition;

	public float actionTime;
	public float distance;

	public float speed;

	public void Setup(Item item)
	{
		startPosition = item.getItemDataObject.setGameObject.transform.position;
		targetPosition = item.getItemDataShadow.setGameObject.transform.position;

		transform.position = startPosition;

		distance = Vector2.Distance(transform.position, targetPosition);
		StartCoroutine(StartMovement());
	}

	IEnumerator StartMovement()
	{
		// Wait for time, in movement target
		yield return new WaitForSeconds(actionTime);

		distance = Vector2.Distance(transform.position, targetPosition);

		//Start to circle is move
		while (distance > 0.1f)
		{
			distance = Vector2.Distance(transform.position, targetPosition);
			Vector2 pos = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
			transform.position = pos;

			yield return null;
		}

		Debug.Log("End Move");

		yield return new WaitForSeconds(actionTime);

		Destroy(gameObject);

		yield break;
	}
}
