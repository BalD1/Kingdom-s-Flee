using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Character
{
    [field: SerializeField] public PlayerInputs playerInputs { get; private set; }

    private bool move = false;

    public delegate void D_MovementStateChange(bool newState);
    public D_MovementStateChange D_movementStateChange;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        SetMovementState(true);
    }

    public void SetMovementState(bool _move)
    {
        move = _move;
        this.motor.SetDirection(_move ? 1 : 0);

        D_movementStateChange?.Invoke(_move);
    }

    protected override void ComponentsNullCheck()
    {
#if UNITY_EDITOR
        base.ComponentsNullCheck();

        if (playerInputs == null) playerInputs = SearchComponent(typeof(PlayerInputs)) as PlayerInputs; 
#endif
    }
}
