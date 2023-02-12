using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Streetlight : MonoBehaviour
{
    [SerializeField] private float spawnDistFromPlayer = 20;

    [SerializeField] private float maxDistanceFailSafe = 60;

    private bool flag = false;

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameManager.Instance.Player.transform;
    }

    private void Update()
    {
        if (playerTransform.position.x > this.transform.position.x + maxDistanceFailSafe) 
            TeleportAheadPlayer();
    }

    private void TeleportAheadPlayer()
    {
        Vector2 pos = playerTransform.position;
        pos.x += spawnDistFromPlayer;
        pos.y = this.transform.position.y;
        this.transform.position = pos;
    }

    private void OnBecameVisible()
    {
        flag = true;
    }

    private void OnBecameInvisible()
    {
        if (!flag) return;

        TeleportAheadPlayer();

        flag = false;
    }
}
