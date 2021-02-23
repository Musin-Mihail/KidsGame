using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerBtn : MonoBehaviour 
{
	public float timeDelay;
	public float timeBase;

	public int currentAnimators;

	public List<Animator> animators = new List<Animator>();
	// Use this for initialization
	void Start () 
	{
		animators.AddRange(GameObject.FindObjectsOfType<Animator>());
		currentAnimators = 0;
		timeDelay = timeBase;
	}

	void Update()
    {
		if(timeDelay <= 0)
        {
			if (currentAnimators >= animators.Count-1)
				currentAnimators = 0;
			else
				currentAnimators++;

			timeDelay = timeBase;
			animators[currentAnimators].SetTrigger("IsOpen");
		}
		else
        {
			timeDelay -= Time.deltaTime;
        }
    }
}
