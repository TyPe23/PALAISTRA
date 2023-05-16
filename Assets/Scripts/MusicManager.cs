using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioBase;
    public AudioSource audioCombat;
    public AudioSource audioLoop;

    private bool startedLoop = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!audioCombat.isPlaying && !startedLoop)
        {
            startedLoop = true;
            audioLoop.Play();
        }
    }
}
