using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSliderStartValue : MonoBehaviour
{
    public ACGGroup ACGGroup;
    private void Start()
    {
        GetComponent<Slider>().value = ACGGroup.VolumeMin;
    }
}
