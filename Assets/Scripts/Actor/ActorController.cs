using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour 
{
	public float distanceValue;
	public GameObject effect;
	//public Animator anim;

	public enum GetActionType { Position, SetImage }
	public GetActionType getTypeObject;

	public Item GetItem; // Получаем дату обьекта

	// Use this for initialization
	void Awake () 
	{
		//anim = GetComponent<Animator>();

		GetItem = Instantiate(GetItem);	//Инициализируем дату
		GetItem.Setup(transform);		// Настраиваем дату обьекта
	}

	// Update is called once per frame
	void Update()
	{
		//anim.SetBool("IsOpen", true);

		// Если тень обьекта отключена, то мы перестаем делать прощёты
		if (!GetItem.getItemDataShadow.setGameObject.activeSelf)	
			return;

		// Получаем позицию обьекта
		Vector2 objectPosition = GetItem.getItemDataObject.setGameObject.transform.position;
		// Получаем позицию тени
		Vector2 shadowPosition = GetItem.getItemDataShadow.setGameObject.transform.position;

		// Получаем значения между тенью и обьектом
		float distance = Vector2.Distance(objectPosition, shadowPosition);

		// Если позиция между тенья и обьектом меньше
		if (distance <= distanceValue)
		{
            switch (getTypeObject)
            {
                case GetActionType.Position:
					// Присваиваем позицию тени обьекту
					GetItem.getItemDataObject.setGameObject.transform.position = shadowPosition;
					break;
                case GetActionType.SetImage:
					GetItem.getItemDataObject.setGameObject.SetActive(false);
					GetComponentInParent<Pets>().NextShape();
                    break;
                default:
                    break;
            }
			Complite();
		}
	}

	void Complite()
    {
		CreateEffect();

		Destroy(GetItem.getItemDataObject.setGameObject.GetComponent<InputController>());
		Destroy(GetItem.getItemDataObject.setGameObject.GetComponent<ActorMovement>());
		Destroy(GetItem.getItemDataObject.setGameObject.GetComponent<SortingLayerController>());
		Destroy(GetItem.getItemDataObject.setGameObject.GetComponent<Actor>());
		Destroy(GetComponent<ActorController>());
	}

	void CreateEffect()
    {
		GameObject EFX = Instantiate(effect);
		EFX.transform.position = GetItem.getItemDataObject.setGameObject.transform.position;
	}

	void OnEnable()
    {
		if (GameObject.FindObjectOfType<Manager>())
		{
			GameObject.FindObjectOfType<Manager>().Add(this);
		}
    }

	void OnDisable()
    {
		if (GameObject.FindObjectOfType<Manager>())
			GameObject.FindObjectOfType<Manager>().Remove(this);
	}
}
