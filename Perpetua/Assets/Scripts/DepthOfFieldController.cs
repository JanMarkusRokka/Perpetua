using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;

public class DepthOfFieldController : MonoBehaviour
{
    /*
    float playerDistance = 10f;
    public GameObject target;

    private PostProcessVolume volume;
    DepthOfField depthOfField;

    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out depthOfField);

        DepthOfField tmp;

        if (volume.profile.TryGetSettings<DepthOfField>(out tmp))
        {
            depthOfField = tmp;
        }

        depthOfField.aperture.value = 0.9f;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            playerDistance = Vector3.Distance(transform.position, target.transform.position) * 0.7f;
        }

        SetFocus();
    }

    void SetFocus()
    {
        depthOfField.focusDistance.value = playerDistance;
    }*/
}
