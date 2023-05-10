using HighScore;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInput = player.GetComponent<PlayerInput>();
        UIinput = GetComponent<PlayerInput>();
        inputs = player.GetComponent<StarterAssetsInputs>();
        state = player.GetComponent<PlayerStates>();
        playerUI = GameObject.FindGameObjectWithTag("UI");
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            state.ChangeState(playerStates.IDLE);

            keyboardUI.SetActive(true);
            playerUI.SetActive(false);
            inputs.cursorLocked = false;
            playerInput.enabled = false;
            UIinput.enabled = true;

            EventSystem.current.SetSelectedGameObject(keyboardFirstButton);
        }
    }
}
