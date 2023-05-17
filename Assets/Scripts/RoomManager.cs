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
    [SerializeField] private bool resetCounter;
    public bool entering;
    [SerializeField]private direction goDir;
    public int roomtoShop;
    [SerializeField] private GameObject player;
    [SerializeField] private List<Enemy> enemies;

    private PlayerStats stats;
    private PlayerStates states;
    private MomentumManager momentum;

    // Start is called before the first frame update
    void Start()
    {
        stats = player.GetComponent<PlayerStats>();
        momentum = player.GetComponent<MomentumManager>();
        states = player.GetComponent<PlayerStates>();
        roomCount = PlayerPrefs.GetInt("roomCount");
        var enemy = GameObject.FindGameObjectsWithTag("enemyNoHit");
        foreach(GameObject e in enemy)
        {
            enemies.Add(e.GetComponent<Enemy>());
        }

        //print("Room Count: " + roomCount);
        StartCoroutine(enter());
    }

    // Update is called once per frame
    void Update()
    {
        int enemiesLeft = 0;
        foreach(var enemy in enemies){
            if(enemy.health > 0)
            {
                enemiesLeft++;
            }
        }
        if(enemiesLeft == 0)
        {
            levelComplete = true;
        }
        /*if (entering)
        {
            
            if (goDir == direction.left)
            {
                player.GetComponent<StarterAssetsInputs>().move = new Vector2(-0.7f, 0.7f);
                player.GetComponent<StarterAssetsInputs>().moveDir = new Vector2(-0.7f, 0.7f);
            }
            else
            {
                player.GetComponent<StarterAssetsInputs>().move = new Vector2(0.7f, 0.7f);
                player.GetComponent<StarterAssetsInputs>().moveDir = new Vector2(0.7f, 0.7f);
            }
        }*/
    }

    public void changeRoom(direction dir)
    {
        if (resetCounter)
        {
            PlayerPrefs.SetInt("StartTime", (int)Time.time);
            resetRoomCounter();
        }

        stats.adjustScore(momentum.momentumScore);
        stats.adjustScore(states.extraScore);

        if (roomCount < roomtoShop)
        {
            //randomly choose next room
            var chance = Random.Range(bottomIndex, topIndex + 1);
            PlayerPrefs.SetInt("roomCount", roomCount + 1);
            SceneManager.LoadScene(chance);
            print(PlayerPrefs.GetInt("roomCount"));
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

    private IEnumerator enter()
    {
        entering = true;
        yield return new WaitForSeconds(1);
        entering = false;
    }
}
