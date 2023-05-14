using System.Collections;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class StarterAssetsInputs : MonoBehaviour, PlayerInputs.INewActions
{
    #region FIELDS AND PROPERTIES
    public PlayerStates states;

	[Header("Character Input Values")]
	public Vector2 move;
	public Vector2 moveDir;
	public Vector2 look;
	public bool pileDriver;
	public bool sprint;
	public bool spin;
	public bool lariat;
	public bool interact;

	[Header("Movement Settings")]
	public bool analogMovement;

	[Header("Mouse Cursor Settings")]
	public bool cursorLocked = true;
	public bool cursorInputForLook = true;
    #endregion

    #region INPUT
    public void OnMove(InputAction.CallbackContext value)
	{
		MoveInput(value.ReadValue<Vector2>());
	}

	public void OnLook(InputAction.CallbackContext value)
	{
		if(cursorInputForLook)
		{
			LookInput(value.ReadValue<Vector2>());
		}
	}

	public void OnPileDriver(InputAction.CallbackContext value)
	{
		PileDriverInput(value.ReadValueAsButton());
		StartCoroutine(pileDriverTimeout());
	}

	public void OnSprint(InputAction.CallbackContext value)
	{
		SprintInput(value.ReadValueAsButton());
	}

    public void OnSpin(InputAction.CallbackContext value)
    {
		SpinInput(value.ReadValueAsButton());
        StartCoroutine(spinTimeout());
    }

    public void OnLariat(InputAction.CallbackContext value)
    {
		LariatInput(value.ReadValueAsButton());
        StartCoroutine(lariatTimeout());
    }

    public void OnInteract(InputAction.CallbackContext value)
    {
		InteractInput(value.ReadValueAsButton());
    }
    #endregion

    #region VAR MODS
    public void MoveInput(Vector2 newMoveDirection)
	{
		move = newMoveDirection;

		if (newMoveDirection != Vector2.zero)
		{
			moveDir = newMoveDirection;
		}
	} 

	public void LookInput(Vector2 newLookDirection)
	{
		look = newLookDirection;
	}

	public void PileDriverInput(bool newPDState)
	{
		pileDriver = newPDState;
	}

	public void SprintInput(bool newSprintState)
	{
		sprint = newSprintState;
	}

    public void SpinInput(bool newSpinState)
    {
        spin = newSpinState;
    }
    public void LariatInput(bool newLariatState)
    {
        lariat = newLariatState;
    }

	public void InteractInput(bool newInteractState)
    {
		interact = newInteractState;
    }

    private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(cursorLocked);
	}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
    #endregion

    #region HELPER FUNCTS
	private IEnumerator lariatTimeout()
	{
		yield return new WaitForSeconds(0.25f);
		lariat = false;
	}

    private IEnumerator spinTimeout()
    {
        yield return new WaitForSeconds(0.25f);
		if (states.state != playerStates.SPIN)
		{
			spin = false;
		}
    }

    private IEnumerator pileDriverTimeout()
    {
        yield return new WaitForSeconds(0.25f);
        pileDriver = false;
    }
    #endregion
}