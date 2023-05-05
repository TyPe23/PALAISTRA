using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public float timeStart;
    public float timeEnd;
    public bool levelComplete;
    [SerializeField] private int bottomIndex;
    [SerializeField] private int topIndex;
    private int roomCount;
    [SerializeField] private int finalIndex;
    // Start is called before the first frame update
    void Start()
    {
        roomCount = GameObject.FindWithTag("Player").GetComponent<PlayerStats>().roomCount;
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
        if (roomCount < 2)
        {
            //randomly choose next room
            var chance = Random.Range(bottomIndex, topIndex + 1);
            SceneManager.LoadScene(chance);
            PlayerPrefs.SetInt("roomCount", roomCount + 1);
        }
        else
        {
            //go to final room
            SceneManager.LoadScene(finalIndex);
        }
    }
}
