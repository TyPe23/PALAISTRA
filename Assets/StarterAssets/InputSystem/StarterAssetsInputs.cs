using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		public ThirdPersonController charCon;

		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool pileDriver;
		public bool sprint;
		public bool spin;
		public bool lariat;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnPileDriver(InputValue value)
		{
			if (!lariat && !spin)
			{
				PileDriverInput(value.isPressed);
			}
		}

		public void OnSprint(InputValue value)
		{
			if (value.isPressed)
			{
				SprintInput(true);
			}
		}

        public void OnSpin(InputValue value)
        {
			if (!lariat && !pileDriver)
			{
				SpinInput(value.isPressed);
				if (!value.isPressed)
				{
					SprintInput(false);
					charCon.letGo = true;
					StartCoroutine(charCon.resetTimeout());
				}
			}
        }

        public void OnLariat(InputValue value)
        {
			if (!spin && !pileDriver)
			{
				LariatInput(value.isPressed);
			}
        }
#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
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

        private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
}