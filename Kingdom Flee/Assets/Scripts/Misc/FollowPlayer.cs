using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameManager.Instance.Player.transform;
    }

    private void Update()
    {
        Vector3 selfPos = this.transform.position;
        selfPos.x = playerTransform.position.x;
        this.transform.position = selfPos;
    }
}
