using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour 
{
	public List<GameObject> shapes = new List<GameObject>();
	public List<Shell> GetShellArray = new List<Shell>();

	public Animator anim;

	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator>();

		GetShellArray.AddRange(GameObject.FindObjectsOfType<Shell>());

		ActorController[] pathTransform = GameObject.FindObjectsOfType<ActorController>();
	
        foreach (var item in pathTransform)
        {
			shapes.Add(item.gameObject);
        }
	}

	public IEnumerator DisableObject()
    {
		foreach (var item in shapes)
		{
			yield return new WaitForSeconds(0.2f);
			item.gameObject.SetActive(false);
		}

		yield return new WaitForSeconds(0.2f);
		anim.SetTrigger("IsOpen");

		yield return new WaitForSeconds(2f);
	}

	public IEnumerator DisableShell()
    {
		foreach (var item in GetShellArray)
		{
			yield return new WaitForSeconds(0.2f);
			item.gameObject.SetActive(false);
		}
	}
}
