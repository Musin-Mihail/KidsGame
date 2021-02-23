using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
	public bool isMove;

	public Vector2 startPosition;
	public Vector2 directionPosition;

	void OnMouseDrag()
	{
		isMove = true;
		directionPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	void OnMouseUp()
	{
		directionPosition = startPosition;
		isMove = false;
	}

	public void SetPosition(Vector2 position)
	{
		if (!isMove)
		{
			startPosition = position;
			directionPosition = startPosition;
		}
	}
}
