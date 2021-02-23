using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbles : MonoBehaviour 
{
	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector2 pos = transform.position;
		pos.y += speed * Time.deltaTime;
		transform.position = pos;
	}

	void OnMouseDown()
	{
		GetComponent<Animator>().SetBool("IsOpen", true);
		Destroy(gameObject, 0.2f);
	}
}
