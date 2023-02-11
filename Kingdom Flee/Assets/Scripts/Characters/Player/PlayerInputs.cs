using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] private King owner;
    [SerializeField] private PlayerInput inputs;

    public delegate void D_MovementInputs(float xInput);
    public D_MovementInputs D_movementInputs;

    public delegate void D_AnyKeyPressed();
    public D_AnyKeyPressed D_anyKeyPressed;

    public void ReadMovementsInputs(InputAction.CallbackContext _context)
    {
        D_movementInputs?.Invoke(_context.ReadValue<Vector2>().x);
    }

    public void AnykeyPressed(InputAction.CallbackContext _context)
    {
        if (_context.performed) D_anyKeyPressed?.Invoke();
    }
}
