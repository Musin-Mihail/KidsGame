using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Menu
{
    public class Protection : MonoBehaviour
    {
        public GameObject _question;
        public GameObject _protection;
        public GameObject _purchaseManager;
        public GameObject _payment;
        public GameObject Eng;
        public GameObject Rus;
        public List<GameObject> _protectionList;
        public List<GameObject> _lockList;

        public void Exit()
        {
            _protection.SetActive(false);
        }

        public void OpenQuestion()
        {
            _question.SetActive(true);
            if (Application.systemLanguage.ToString() == "Russian")
            {
                Eng.SetActive(false);
                Rus.SetActive(true);
            }
            else
            {
                Eng.SetActive(true);
                Rus.SetActive(false);
            }
        }

        public void ExitQuestion()
        {
            _question.SetActive(false);
        }

        public void OpenProtection()
        {
            _question.SetActive(false);
            _protection.SetActive(true);
            var _random = Random.Range(0, 4);
            for (var i = 0; i < _protectionList.Count; i++)
            {
                if (i == _random)
                {
                    _protectionList[i].SetActive(true);
                }
                else
                {
                    _protectionList[i].SetActive(false);
                }
            }
        }

        public void Payment()
        {
            // _purchaseManager.GetComponent<PurchaseManager>().BuyProductID("open.all1");
        }

        public void ExitPayment()
        {
            _payment.SetActive(false);
        }

        public void SuccessfulPayment()
        {
            _payment.SetActive(false);
            var filePath = Application.persistentDataPath + @"/open.all1";
            var fi = new FileInfo(filePath);
            fi.Create();
            foreach (var item in _lockList)
            {
                item.SetActive(false);
            }
        }
    }
}