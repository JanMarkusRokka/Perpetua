using UnityEngine;
using UnityEngine.UI;

public class FPSLimiter : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Slider>().value = 3;
    }
}
