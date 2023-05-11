using HighScore;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class EndOfGame : MonoBehaviour
{
    public GameObject keyboardUI;
    public GameObject playerUI;
    public GameObject keyboardFirstButton;

    private GameObject player;
    private PlayerInput playerInput;
    private PlayerInput UIinput;
    private PlayerStates state;
    private StarterAssetsInputs inputs;
    private PlayerStats stats;
    private MomentumManager momentum;
    private GameObject roomMan;

    private bool canEnter = true;
    private bool gameEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
        momentum = player.GetComponent<MomentumManager>();
        playerInput = player.GetComponent<PlayerInput>();
        UIinput = GetComponent<PlayerInput>();
        inputs = player.GetComponent<StarterAssetsInputs>();
        state = player.GetComponent<PlayerStates>();
        playerUI = GameObject.FindGameObjectWithTag("UI");
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && inputs.interact && !gameEnded)
        {
            endGame();
        }
    }

    public void endGame()
    {
        gameEnded = true;

        state.ChangeState(playerStates.IDLE);

        keyboardUI.SetActive(true);
        playerUI.SetActive(false);
        inputs.cursorLocked = false;
        playerInput.enabled = false;
        UIinput.enabled = true;

        EventSystem.current.SetSelectedGameObject(keyboardFirstButton);
    }

    public void enterFunct(string word)
    {
        stats.adjustScore(momentum.momentumScore);

        stats.adjustScore(-(int)Time.time);

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
