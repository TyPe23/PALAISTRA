using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum direction
{
    left,
    right,
    downright,
    downleft,
}
public class Gate : MonoBehaviour
{
    private GameObject roomMan;
    private GameObject player;

    [Tooltip("Put at -1 to randomly choose room, otherwise, input buildIndex of next room")]
    [SerializeField] int nextRoom;

    [SerializeField] private Collider topCollider;
    [SerializeField] private Collider bottomCollider;
    [SerializeField] private direction goDir;
    [SerializeField] private MeshRenderer text;
    private bool leaving;
    
    // Start is called before the first frame update
    void Start()
    {
        roomMan = GameObject.Find("RoomManager");
        player = GameObject.FindWithTag("Player");
        text.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (leaving) {
            if (goDir == direction.left)
            {
                player.GetComponent<StarterAssetsInputs>().move = new Vector2(-0.7f, 0.7f);
                player.GetComponent<StarterAssetsInputs>().moveDir = new Vector2(-0.7f, 0.7f);
            }
            else if(goDir == direction.downleft){
                player.GetComponent<StarterAssetsInputs>().move = new Vector2(-0.7f, -0.7f);
                player.GetComponent<StarterAssetsInputs>().moveDir = new Vector2(-0.7f, -0.7f);
            }
            else if(goDir == direction.downright)
            {
                player.GetComponent<StarterAssetsInputs>().move = new Vector2(0.7f, -0.7f);
                player.GetComponent<StarterAssetsInputs>().moveDir = new Vector2(0.7f, -0.7f);
            }
            else
            {
                player.GetComponent<StarterAssetsInputs>().move = new Vector2(0.7f, 0.7f);
                player.GetComponent<StarterAssetsInputs>().moveDir = new Vector2(0.7f, 0.7f);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        text.enabled = true;
    }
    private void OnTriggerStay(Collider other)
    {
        
        if (other.transform.CompareTag("Player")&& roomMan.GetComponent<RoomManager>().levelComplete && player.GetComponent<StarterAssetsInputs>().interact)
        {
            bottomCollider.enabled = false;
            StartCoroutine(BeforeSceneChange());
            topCollider.enabled = false;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        text.enabled = false;
    }


    private IEnumerator BeforeSceneChange()
    {

        //TODO fake player movement
        //var player = GameObject.FindWithTag("Player");
        //player.GetComponent<CharacterController>().enabled = false;
        leaving = true;
        
        yield return new WaitForSeconds(2);
        if (nextRoom <= -1)
        {
            print("random room");
            roomMan.GetComponent<RoomManager>().changeRoom(goDir);
        }
        else
        {
            print("going to room" + nextRoom);
            roomMan.GetComponent<RoomManager>().changeRoomSpecific(nextRoom,goDir);
        }
    }
}
