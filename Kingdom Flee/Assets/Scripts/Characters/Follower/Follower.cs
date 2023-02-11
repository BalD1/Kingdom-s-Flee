using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : Character
{
    [SerializeField] private SO_FollowerColors colors;

    [SerializeField] private SpriteRenderer headRenderer;
    [SerializeField] private SpriteRenderer bodyRenderer;

    private bool isBehindKing = false;

    protected override void Awake()
    {
        base.Awake();

        headRenderer.color = colors.GetRandomHeadColor();
        bodyRenderer.color = colors.GetRandomBodyColor();

        GameManager.Instance.Player.D_movementStateChange += SetMovement;
    }

    public void SetMovement(bool state)
    {
        this.motor.SetDirection(state ? 1 : 0);
    }
}
