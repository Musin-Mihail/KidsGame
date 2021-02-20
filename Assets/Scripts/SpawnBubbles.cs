using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnBubbles : MonoBehaviour 
{
	public GameObject[] spawnBubbles;
	public int counter;

	IEnumerator Spawn()
    {
		Debug.Log("Start");
		yield return new WaitForSeconds(1f);

        while (counter > 0)
        {
			Debug.Log("While started");
			GameObject obj = Instantiate(spawnBubbles[Random.Range(0, spawnBubbles.Length)]);

			Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
			Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

			min.x += 0.75f;
			min.y += 1.5f;

			max.x -= 0.75f;
			max.y -= 1.5f;

			obj.transform.position = new Vector2(Random.Range(min.x, max.x), min.y);

			counter--;
			yield return new WaitForSeconds(Random.Range(0.3f, 0.5f));
        }

		yield return new WaitForSeconds(3f);
		SceneManager.LoadScene("SellectScene");
    }

	void Start()
	{
		StartCoroutine(Spawn());
	}
}
