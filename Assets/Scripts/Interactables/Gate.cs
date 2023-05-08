using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum direction
{
    left,
    right,
}
public class Gate : MonoBehaviour
{
    private GameObject roomMan;

    [Tooltip("Put at -1 to randomly choose room, otherwise, input buildIndex of next room")]
    [SerializeField] int nextRoom;

    [Tooltip("Determines the hightest point on the gate.")]
    [SerializeField] private int peak;

    [SerializeField] private direction goDir;
    private bool leaving;
    
    // Start is called before the first frame update
    void Start()
    {
        roomMan = GameObject.Find("RoomManager");
    }

    // Update is called once per frame
    private void Update()
    {
        if (leaving) {
            var player = GameObject.FindWithTag("Player");
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
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (other.transform.CompareTag("Player")&& roomMan.GetComponent<RoomManager>().levelComplete)
        {
            
            StartCoroutine(BeforeSceneChange());
            
        }
    }

    
    private IEnumerator BeforeSceneChange()
    {

        //TODO fake player movement
        //var player = GameObject.FindWithTag("Player");
        //player.GetComponent<CharacterController>().enabled = false;
        leaving = true;
        
        
        yield return new WaitForSeconds(2);
        if (nextRoom <=-1)
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
