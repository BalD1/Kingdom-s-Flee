using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private King owner;

    [SerializeField] [ReadOnly] private Vector2 inputDirection;
    [SerializeField] [ReadOnly] private Vector2 lastDirection;

    [SerializeField] [ReadOnly] private Vector2 currentVelocity;

    private void FixedUpdate()
    {
        currentVelocity.x = inputDirection.x * owner.kingStats.speed * Time.deltaTime;
        currentVelocity.y = owner.playerBody.velocity.y;

        owner.playerBody.velocity = currentVelocity;
    }

    public void SetDirection(float _xDirection)
    {
        inputDirection.x = _xDirection;
    }
}
