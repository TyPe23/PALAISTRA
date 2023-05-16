using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioBase;
    public AudioSource audioCombat;
    public AudioSource audioLoop;

    public Enemy[] enemies;

    private bool startedLoop = false;
    private bool inCombat = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (enemies != null)
        {
            if (enemies[0].state == enemyStates.MOVE && !inCombat)
            {
                inCombat = true;
            }
        }

        if (!audioBase.isPlaying && inCombat && !startedLoop)
        {
            audioCombat.Play();
        }
        else if (!inCombat && !audioBase.isPlaying && !audioLoop.isPlaying)
        {
            audioBase.Play();
        }

        if (!audioCombat.isPlaying && !startedLoop && inCombat)
        {
            startedLoop = true;
            audioLoop.Play();
        }

        if (inCombat)
        {
            foreach  (Enemy enemy in enemies)
            {
                if (enemy.health <= 0)
                {
                    inCombat = false;
                }
                else
                {
                    inCombat = true;
                }
            }
            if (!inCombat)
            {
                audioLoop.loop = false;
            }
        }
    }
}
