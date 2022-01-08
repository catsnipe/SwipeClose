using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window1 : MonoBehaviour
{
    [SerializeField]
    GameObject  Window2;

    void Awake()
    {
        GetComponentInChildren<Button>().onClick.AddListener(pushButton);
    }

    void pushButton()
    {
        Window2.SetActive(true);
    }
}
