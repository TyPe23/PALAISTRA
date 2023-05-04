using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public float timeStart;
    public float timeEnd;
    public bool levelComplete;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        int enemiesLeft = 0;
        var enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach(var enemy in enemies){
            if(enemy.GetComponent<PlayerEnemyInteraction>().health > 0)
            {
                enemiesLeft++;
            }
        }
        if(enemiesLeft == 0)
        {
            levelComplete = true;
        }
    }

    public void changeRoom()
    {
        
    }
}
