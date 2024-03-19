using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
// Somewhat inspired by Game Dev Guide https://www.youtube.com/watch?v=HXFoUGw7eKk
public class Tooltip : MonoBehaviour
{
    private RectTransform rect;
    private void Awake()
    {
        if (FindObjectsOfType(typeof(Tooltip)).Count() > 1)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        rect.pivot = new Vector2(mousePos.x / Screen.width, mousePos.y / Screen.height);
        transform.position = mousePos;
    }
}
