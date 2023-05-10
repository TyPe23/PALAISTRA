using HighScore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HS.Clear(this);
        HS.Init(this, "PALAISTRA");
    }
}
