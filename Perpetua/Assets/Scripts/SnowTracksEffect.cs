using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SnowTracksEffect : MonoBehaviour
{
    public bool SnowTrackEnabled;
    public GameObject SnowTracksObject;
    private ParticleSystem snowTracks;
    private Rigidbody _rb;
    private Vector3 previousPosition;
    void Start()
    {
        snowTracks = SnowTracksObject.GetComponent<ParticleSystem>();
        _rb = GetComponent<Rigidbody>();
        previousPosition = Vector3.zero;
    }

    void FixedUpdate()
    {
        if (SnowTrackEnabled) ProcessSnowTracks();
    }

    // Enables/Disables snow tracks based on rigidbody's velocity
    private void ProcessSnowTracks()
    {
        var emission = snowTracks.emission;
        // Using transform, because some objects are moved manually via transform and thus don't have a velocity.
        //if (Vector3.Magnitude(_rb.velocity) > 1) emission.enabled = true;
        if (Vector3.Magnitude((transform.position - previousPosition) / Time.fixedDeltaTime) > 0.1) emission.enabled = true;
        else emission.enabled = false;
        previousPosition = transform.position;
    }

    private void DisableSnowTracks()
    {
        var emission = snowTracks.emission;
        SnowTrackEnabled = false;
        emission.enabled = false;
    }

}
