using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotor : MonoBehaviour
{
    [SerializeField] private Character owner;

    [SerializeField][ReadOnly] private Vector2 inputDirection;
    [SerializeField][ReadOnly] private Vector2 lastDirection;

    [SerializeField][ReadOnly] private Vector2 currentVelocity;

    [SerializeField] private bool allowLeftMovements = false;

    protected virtual void FixedUpdate()
    {
        currentVelocity.x = inputDirection.x * owner.stats.speed * Time.deltaTime;
        currentVelocity.y = owner.body.velocity.y;

        owner.body.velocity = currentVelocity;
    }

    public virtual void SetDirection(float _xDirection)
    {
        if (!allowLeftMovements && _xDirection < 0) return;

        inputDirection.x = _xDirection;
    }
}
