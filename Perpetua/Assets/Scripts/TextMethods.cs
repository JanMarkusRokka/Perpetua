using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextMethods : MonoBehaviour
{
    public static IEnumerator RevealText(string text, TextMeshProUGUI textMesh, float speed)
    {
        textMesh.text = "";
        foreach (char character in text)
        {
            yield return new WaitForSecondsRealtime(speed);
            textMesh.text += character;
        }
    }
}
