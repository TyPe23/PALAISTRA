using System.Collections;
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
    private RoomManager roomMan;
    private GameObject player;
    private StarterAssetsInputs sai;

    [Tooltip("Put at -1 to randomly choose room, otherwise, input buildIndex of next room")]
    [SerializeField] int nextRoom;

    [SerializeField] private Collider topCollider;
    [SerializeField] private Collider bottomCollider;
    [SerializeField] private direction goDir;
    [SerializeField] private MeshRenderer text;
    private bool leaving;
    [SerializeField] private bool enemiesOption;
    
    // Start is called before the first frame update
    void Start()
    {
        roomMan = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        player = GameObject.FindWithTag("Player");
        sai = player.GetComponent<StarterAssetsInputs>();

        text.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (leaving) {
            if (goDir == direction.left)
            {
                sai.move = new Vector2(-0.7f, 0.7f);
                sai.moveDir = new Vector2(-0.7f, 0.7f);
            }
            else if(goDir == direction.downleft){
                sai.move = new Vector2(-0.7f, -0.7f);
                sai.moveDir = new Vector2(-0.7f, -0.7f);
            }
            else if(goDir == direction.downright)
            {
                sai.move = new Vector2(0.7f, -0.7f);
                sai.moveDir = new Vector2(0.7f, -0.7f);
            }
            else
            {
                sai.move = new Vector2(0.7f, 0.7f);
                sai.moveDir = new Vector2(0.7f, 0.7f);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        text.enabled = true;
    }
    private void OnTriggerStay(Collider other)
    {
        
        if (other.transform.CompareTag("Player")&& roomMan.levelComplete && sai.interact)
        {
            topCollider.enabled = false;
            bottomCollider.enabled = false;
            StartCoroutine(BeforeSceneChange());
        }
        else if(other.transform.CompareTag("Player") && enemiesOption && sai.interact)
        {
            topCollider.enabled = false;
            bottomCollider.enabled = false;
            StartCoroutine(BeforeSceneChange());
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
            roomMan.changeRoom(goDir);
        }
        else
        {
            print("going to room" + nextRoom);
            roomMan.changeRoomSpecific(nextRoom,goDir);
        }
    }
}
