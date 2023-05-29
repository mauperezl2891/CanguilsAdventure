using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipBox : MonoBehaviour
{
    public static TooltipBox _instance;
    public TextMeshProUGUI textComponent;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
