using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidGenerator : MonoBehaviour
{
    public string guidString;
    private Guid guidId;

    public void GenGuid()
    {
        guidId = Guid.NewGuid();
        guidString = guidId.ToString();
    }
}
