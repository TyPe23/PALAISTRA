using HighScore;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class EndOfGame : MonoBehaviour
{
    public GameObject keyboardUI;
    public GameObject playerUI;
    public GameObject keyboardFirstButton;

    public GameObject player;
    private PlayerInput playerInput;
    private PlayerInput UIinput;
    private PlayerStates state;
    private StarterAssetsInputs inputs;
    private PlayerStats stats;
    private MomentumManager momentum;
    private GameObject roomMan;

    private bool canEnter = true;
    private bool gameEnded = false;
    [SerializeField] private GameObject text;

    // Start is called before the first frame update
    void Start()
    {
        stats = player.GetComponent<PlayerStats>();
        state = player.GetComponent<PlayerStates>();
        momentum = player.GetComponent<MomentumManager>();
        playerInput = player.GetComponent<PlayerInput>();
        UIinput = GetComponent<PlayerInput>();
        inputs = player.GetComponent<StarterAssetsInputs>();
        state = player.GetComponent<PlayerStates>();
        playerUI = GameObject.FindGameObjectWithTag("UI");
        text.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            text.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && inputs.interact && !gameEnded)
        {
            endGame();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            text.SetActive(false);
        }
    }

    public void endGame()
    {
        gameEnded = true;

        keyboardUI.SetActive(true);
        playerUI.SetActive(false);
        playerInput.enabled = false;
        UIinput.enabled = true;

        EventSystem.current.SetSelectedGameObject(keyboardFirstButton);
    }

    public void enterFunct(string word)
    {
        stats.adjustScore(state.extraScore);
        stats.adjustScore(momentum.momentumScore);
        stats.adjustScore(1000 - (int)Time.time - PlayerPrefs.GetInt("StartTime"));

        if (canEnter)
        {
            canEnter = false;
            HS.SubmitHighScore(this, word, stats.score);
            StartCoroutine(waitThenChange());

            keyboardUI.SetActive(false);
        }
    }

    private IEnumerator waitThenChange()
    {

        yield return new WaitForSeconds(2);

        roomMan = GameObject.Find("RoomManager");
        roomMan.GetComponent<RoomManager>().changeRoomSpecific(1, direction.right);
    }
}
