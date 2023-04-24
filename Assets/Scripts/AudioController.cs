using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource _source;

    private bool IsPaused;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsPaused)
            _source.UnPause();

        if (Input.GetKeyDown(KeyCode.Escape) && !IsPaused)
            _source.Pause();

        if (Input.GetKeyDown(KeyCode.Escape))
            IsPaused = !IsPaused;
    }
}
