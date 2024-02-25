using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomWind : MonoBehaviour
{
    private WindZone _wz;
    public Quaternion direction;
    public float main;
    public float turbulence;

    private void Awake()
    {
        _wz = GetComponent<WindZone>();

    }

    private void Update()
    {
        direction = _wz.transform.rotation;
        main = _wz.windMain;
        turbulence = _wz.windTurbulence;
    }
}
