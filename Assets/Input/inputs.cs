using UnityEngine;
using System;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class inputs : MonoBehaviour
    {
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


#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnPileDriver(InputValue value)
        {
            PileDriverInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        public void OnSpin(InputValue value)
        {
            SpinInput(value.isPressed);
        }

        public void OnLariat(InputValue value)
        {
            LariatInput(value.isPressed);
        }

        public void OnInteract(InputValue value)
        {
            InteractInput(value.isPressed);
        }
#endif

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
    }
}
