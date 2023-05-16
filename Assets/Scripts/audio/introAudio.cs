using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class introAudio : MonoBehaviour
{
    public AudioSource audioIn;
    public AudioSource audioLoop;

    private bool startedLoop = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioIn.isPlaying && !startedLoop)
        {
            startedLoop = true;
            audioLoop.Play();
        }
    }
}
