using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pets : MonoBehaviour 
{
    public List<ActorController> controllers = new List<ActorController>();
    public List<GameObject> child = new List<GameObject>();

    public ItemPonels itemPonels;

    [Header("Animator's")]
    public Animator animDialog;

    public SpriteRenderer render;

    private float distance;
    private bool isOpen;

    public float timeDelay;

    public ActorMovement GetMovementController;

    void Awake()
    {
        GetMovementController = GetComponent<ActorMovement>();

        ActorController[] pathTransform = transform.Find("Shapes").GetComponentsInChildren<ActorController>();

        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (pathTransform[i].transform != transform.Find("Shapes"))
            {
                if (pathTransform[i].transform.name != "Empty")
                {
                    controllers.Add(pathTransform[i]);
                }
                child.Add(pathTransform[i].gameObject);
            }
        }

        animDialog = GetComponent<Animator>();

        itemPonels = GameObject.FindObjectOfType<ItemPonels>();

        DisableObect();
    }

    public IEnumerator StartMovement()
    {
        yield return new WaitForSeconds(timeDelay);

        yield return StartCoroutine(Movement(Vector2.zero));

        yield return StartCoroutine(Enable());

        yield return new WaitForSeconds(timeDelay);

        isOpen = true;

        GameObject.Find("GameManager").GetComponent<HandlerManager>().ResetTimer();

        while (controllers.Count > 0)
        {
            animDialog.SetBool("IsOpen", isOpen);
            render.sprite = controllers[0].GetItem.getItemDataShadow.setGameObject.GetComponent<SpriteRenderer>().sprite;
            controllers[0].GetItem.getItemDataShadow.setGameObject.SetActive(true);

            yield return null;
        }

        yield return StartCoroutine(Disable());
        DisableObect();

        yield return new WaitForSeconds(timeDelay);

        yield return StartCoroutine(Movement(new Vector2(20, 0)));

        itemPonels.ResetImage();


        PetsController controller = GetComponentInParent<PetsController>();
        controller.actors.Remove(this);
        controller.StartMovement();

        GameObject.Find("GameManager").GetComponent<HandlerManager>().ResetTimer();
        GameObject.Find("GameManager").GetComponent<HandlerManager>().enabled = false;
    }

    IEnumerator Movement(Vector2 direction)
    {
        distance = Vector2.Distance(transform.position, direction);

        while (distance > 0.1f)
        {
            distance = Vector2.Distance(transform.position, direction);
            GetMovementController.MoveTo(direction);
            yield return null;
        }
    }

    IEnumerator Disable()
    {
        foreach (var item in child)
        {
            item.GetComponent<Animator>().SetBool("IsOpen", false);
            yield return new WaitForSeconds(0.2f);
            item.gameObject.SetActive(false);
        }
    }

    IEnumerator Enable()
    {
        foreach (var item in child)
        {
            item.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            item.GetComponent<Animator>().SetBool("IsOpen", true);
        }
    }

    public void NextShape()
    {
        isOpen = false;
        Invoke("ResetAnimation", 1f);
    }

    void ResetAnimation()
    {
        if (controllers.Count <= 0)
        {
            isOpen = false;
        }
        else
        {
            isOpen = true;

            itemPonels.SetImage(controllers[0].GetItem.getItemDataShadow.setGameObject.GetComponent<SpriteRenderer>().sprite);
            controllers.RemoveAt(0);
        }
    }

    void DisableObect()
    {
        foreach (var item in child)
        {
            item.gameObject.SetActive(false);
        }
    }
}
