using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomWindRotator : MonoBehaviour
{
    private CustomWind _cw;
    // Relies on a specific bone in the armature
    private Transform bone;
    private Vector3 startPos;
    private Vector3 amplitude;
    private float offset;

    private void Start()
    {
        _cw = FindObjectOfType<CustomWind>();
        bone = transform.Find("Armature").GetChild(1).GetChild(0);//.GetChild(0);
        startPos = bone.position + new Vector3(0f, 1f, 0f);
        offset = Random.Range(0, 1f);
    }

    private void Update()
    {
        if (_cw)
        {
            amplitude = _cw.direction * Vector3.forward * _cw.main * 2;
            Vector3 swayPosition = Vector3.Lerp(startPos - amplitude, startPos - amplitude - _cw.main * Vector3.up, (Mathf.Sin(Time.time + offset) + 1.01f)/2f);

            bone.localEulerAngles = Vector3.Cross(transform.up, Vector3.ProjectOnPlane(transform.up, transform.InverseTransformPoint(swayPosition) - bone.localPosition)) * 15;
        }
    }
}
