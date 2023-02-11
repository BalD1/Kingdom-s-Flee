using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [field: SerializeField] public CharacterMotor motor { get; protected set; }

    [field: SerializeField] public Rigidbody2D body { get; protected set; }

    [field: SerializeField] public SO_EntityStats stats { get; protected set; }

    protected virtual void Awake()
    {
#if UNITY_EDITOR
        ComponentsNullCheck();
#endif
    }

    protected virtual void ComponentsNullCheck()
    {
#if UNITY_EDITOR
        if (motor == null) motor = SearchComponent(typeof(CharacterMotor)) as CharacterMotor;
        if (body == null) body = SearchComponent(typeof(Rigidbody2D)) as Rigidbody2D;
#endif
    }

    protected Component SearchComponent(Type comp)
    {
        Debug.LogError(comp + " was not set to Player. Please set it in editor.", this.gameObject);

        Component res = this.GetComponent(comp);
        if (res == null) res = this.GetComponentInChildren(comp);
        if (res == null) res = this.GetComponentInParent(comp);

        return res;
    }
}
