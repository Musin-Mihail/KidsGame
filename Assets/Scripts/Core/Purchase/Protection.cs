using System.Collections.Generic;
using UnityEngine;

namespace Core.Purchase
{
    public class Protection : MonoBehaviour
    {
        public static Protection instance { get; private set; }
        public GameObject question;
        public GameObject protection;
        public GameObject rus;
        public GameObject eng;
        public List<GameObject> protectionList;

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Exit()
        {
            protection.SetActive(false);
        }

        public void OpenQuestion()
        {
            question.SetActive(true);
            if (Application.systemLanguage.ToString() == "Russian")
            {
                eng.SetActive(false);
                rus.SetActive(true);
            }
            else
            {
                eng.SetActive(true);
                rus.SetActive(false);
            }
        }

        public void ExitQuestion()
        {
            question.SetActive(false);
        }

        public void OpenProtection()
        {
            question.SetActive(false);
            protection.SetActive(true);
            var random = Random.Range(0, protectionList.Count);
            for (var i = 0; i < protectionList.Count; i++)
            {
                protectionList[i].SetActive(i == random);
            }
        }
    }
}