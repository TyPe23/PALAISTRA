using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;


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
    private PlayerStats stats;
    private MomentumManager momentum;
    private PlayerStates states;

    [Tooltip("Put at -1 to randomly choose room, otherwise, input buildIndex of next room")]
    [SerializeField] int nextRoom;

    [SerializeField] private Collider topCollider;
    [SerializeField] private Collider bottomCollider;
    [SerializeField] private direction goDir;
    [SerializeField] private MeshRenderer text;
    private bool leaving;
    [SerializeField] private bool enemiesOption;
    private Animator fade;
    
    // Start is called before the first frame update
    void Start()
    {
        roomMan = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        print(roomMan);
        player = GameObject.FindWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
        states = player.GetComponent<PlayerStates>();
        momentum = player.GetComponent<MomentumManager>();
        sai = player.GetComponent<StarterAssetsInputs>();
        fade = GameObject.FindWithTag("LevelChanger").GetComponent<Animator>();

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
        
        yield return new WaitForSeconds(1);
        fade.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1);

        stats.adjustScore(momentum.momentumScore);
        stats.adjustScore(states.extraScore);
        
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
