using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InfoPresenter : MonoBehaviour
{
    void Awake()
    {
        if (FindObjectsOfType(typeof(InfoPresenter)).Count() > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }
}
