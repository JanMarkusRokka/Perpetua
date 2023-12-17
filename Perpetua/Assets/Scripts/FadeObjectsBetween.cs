using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FadeObjectsBetween : MonoBehaviour
{
    public GameObject target;
    private Ray ray;
    private RaycastHit[] raycastHits;
    private Collider[] previousColliders;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        raycastHits = Physics.RaycastAll(transform.position, transform.forward, Vector3.Distance(transform.position, target.transform.position));

        foreach(RaycastHit hit in raycastHits)
        {
            TransparencyController tp = hit.collider.GetComponent<TransparencyController>();
            if (tp)
            {
                tp.enabled = true;
                tp.MakeTransparent();
            }
        }
    }
}
