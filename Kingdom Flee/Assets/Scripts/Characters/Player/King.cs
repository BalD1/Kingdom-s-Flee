using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : MonoBehaviour
{
    [field: SerializeField] public PlayerInputs playerInputs { get; private set; }

    [field: SerializeField] public PlayerMotor playerMotor { get; private set; }

    [field: SerializeField] public Rigidbody2D playerBody { get; private set; }

    [field: SerializeField] public SO_EntityStats kingStats { get; private set; }

    private void Awake()
    {
#if UNITY_EDITOR
        ComponentsNullCheck();
#endif

        playerInputs.D_movementInputs += playerMotor.SetDirection;
    }

    private void ComponentsNullCheck()
    {
#if UNITY_EDITOR
        if (playerInputs == null) Debug.LogError(playerInputs + " was not set to Player.", this.gameObject);
        if (playerMotor == null) Debug.LogError(playerMotor + " was not set to Player.", this.gameObject);
#endif
    }
}
