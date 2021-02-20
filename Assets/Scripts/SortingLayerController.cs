using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerController : MonoBehaviour 
{
	public enum SortingLayer { Object, Shadow }
	public SortingLayer sortingLayer;

	private SpriteRenderer spriteRenderer;

	public int sortingOrder;

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void OnMouseDown()
	{
		sortingLayer = SortingLayer.Object;
		spriteRenderer.sortingLayerName = sortingLayer.ToString();
	}

	void OnMouseUp()
	{
		sortingLayer = SortingLayer.Shadow;
		spriteRenderer.sortingLayerName = sortingLayer.ToString();
	}

	void OnDestroy()
	{
		sortingLayer = SortingLayer.Shadow;
		spriteRenderer.sortingLayerName = sortingLayer.ToString();
		spriteRenderer.sortingOrder = sortingOrder;
	}
}
