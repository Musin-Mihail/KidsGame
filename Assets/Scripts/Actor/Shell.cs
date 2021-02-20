using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour 
{
    public Animator anim;
    public List<Actor> actorArray = new List<Actor>();
    public bool isEnable;

    void Awake()
    {
        anim = GetComponent<Animator>();
        actorArray.AddRange(transform.GetComponentsInChildren<Actor>());
    }

    void Start()
    {
        DisableObect();
        StartCoroutine(StartMovement());
    }

    public IEnumerator StartMovement()
    {
        yield return StartCoroutine(ShowAnimation());

        while (actorArray[0] != null)
        {
            yield return null;
        }

        yield return StartCoroutine(HideAnimation());
    }

    IEnumerator ShowAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("IsOpen", true);

        yield return new WaitForSeconds(1f);
        actorArray[0].gameObject.SetActive(true);
        actorArray[0].gameObject.GetComponentInParent<Animator>().SetBool("IsOpen", true);
    }

    IEnumerator HideAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("IsOpen", false);

        yield return new WaitForSeconds(0.1f);
        actorArray.RemoveAt(0);

        if (actorArray.Count > 0)
            StartCoroutine(StartMovement());
        else if (actorArray.Count == 0)
            GameObject.FindObjectOfType<ChestManager>().GetShells.Remove(this);
    }

    void DisableObect()
    {
        foreach (var item in actorArray)
        {
            item.gameObject.SetActive(false);
        }
    }
}
