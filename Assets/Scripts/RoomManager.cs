using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private bool resetCounter;
    public bool entering;
    [SerializeField]private direction goDir;
    public int roomtoShop;
    [SerializeField] private GameObject player;

    private PlayerStats stats;
    private MomentumManager momentum;
    private inputs input;

    // Start is called before the first frame update
    void Start()
    {
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        momentum = GameObject.FindGameObjectWithTag("Player").GetComponent<MomentumManager>();
        roomCount = PlayerPrefs.GetInt("roomCount");
        //print("Room Count: " + roomCount);
        StartCoroutine(enter());
        input = player.GetComponent<inputs>();
    }

    // Update is called once per frame
    void Update()
    {
        int enemiesLeft = 0;
        var enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach(var enemy in enemies){
            if(enemy.GetComponent<Enemy>().health > 0)
            {
                enemiesLeft++;
            }
        }
        if(enemiesLeft == 0)
        {
            levelComplete = true;
        }
        if (entering)
        {
            
            if (goDir == direction.left)
            {
                input.move = new Vector2(-0.7f, 0.7f);
                input.moveDir = new Vector2(-0.7f, 0.7f);
            }
            else
            {
                input.move = new Vector2(0.7f, 0.7f);
                input.moveDir = new Vector2(0.7f, 0.7f);
            }
        }
    }

    public void changeRoom(direction dir)
    {
        stats.adjustScore(momentum.momentumScore);

        stats.adjustScore(-(int)Time.time);

        if (resetCounter)
        {
            resetRoomCounter();
        }

        if (roomCount < roomtoShop)
        {
            //randomly choose next room
            var chance = Random.Range(bottomIndex, topIndex + 1);
            PlayerPrefs.SetInt("roomCount", roomCount + 1);
            SceneManager.LoadScene(chance);
            print(PlayerPrefs.GetInt("roomCount"));
            goDir = dir;
            StartCoroutine(movementClock(chance));
        }
        else
        {
            //go to final room
            SceneManager.LoadScene(finalIndex);
        }
    }

    public void changeRoomSpecific(int nextRoom,direction dir)
    {
        if (resetCounter)
        {
            resetRoomCounter();
        }

        if (roomCount < roomtoShop)
        {
            //randomly choose next room
            PlayerPrefs.SetInt("roomCount", roomCount + 1);
            SceneManager.LoadScene(nextRoom);
            print(PlayerPrefs.GetInt("roomCount"));
            goDir = dir;

            
            //TODO player moving into room
            //do coroutine for moving into room duration, then when out stop forced movement.
        }
        else
        {
            //go to final room
            SceneManager.LoadScene(finalIndex);
        }
    }

    private void resetRoomCounter() {
        roomCount = 0;
    }

    private IEnumerator movementClock(int nextRoom)
    {
        while(SceneManager.GetActiveScene().buildIndex != nextRoom)
        {
            yield return null;
        }
        if(SceneManager.GetActiveScene().buildIndex == nextRoom)
        {
            var rm = GameObject.Find("RoomManager");
            rm.GetComponent<RoomManager>().entering = true;
            entering = true;
            yield return new WaitForSeconds(1.5f);
            entering = false;
            rm.GetComponent<RoomManager>().entering = false;
        }
    }
    private IEnumerator enter()
    {
        entering = true;
        yield return new WaitForSeconds(2);
        entering = false;
    }
}
