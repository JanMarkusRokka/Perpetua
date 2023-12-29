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
    void Start()
    {
        snowTracks = SnowTracksObject.GetComponent<ParticleSystem>();
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (SnowTrackEnabled) ProcessSnowTracks();
    }

    // Enables/Disables snow tracks based on rigidbody's velocity
    private void ProcessSnowTracks()
    {
        var emission = snowTracks.emission;
        if (Vector3.Magnitude(_rb.velocity) > 1) emission.enabled = true;
        else emission.enabled = false;
    }

    private void DisableSnowTracks()
    {
        var emission = snowTracks.emission;
        SnowTrackEnabled = false;
        emission.enabled = false;
    }

}
