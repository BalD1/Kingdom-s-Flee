using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowersManager : Singleton<FollowersManager>
{
    [SerializeField] [Range(0,50)] private int neededFollowers;

    [field: SerializeField] public Transform[] followers { get; private set; }

    public float followersSpeed;
    //private float followersSpeed;
    private float followersVelocity;

    [SerializeField] private float randomRange = 1;


    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        SpawnFollowers();

        //followersSpeed = GameManager.Instance.Player.kingStats.speed;

        GameManager.Instance.Player.playerInputs.D_movementInputs += SetFollowersVelocity;
    }

    private void FixedUpdate()
    {
        foreach (var item in followers)
        {
            Vector2 pos = item.transform.position;
            pos.x += followersVelocity * Time.deltaTime;
            item.transform.position = pos;
        }
    }

    private void SpawnFollowers()
    {
        GameObject followersPF = GameAssets.Instance.FollowersPF;

        followers = new Transform[neededFollowers];

        Vector2 playerPos = GameManager.Instance.Player.transform.position;

        Vector2 spawnPos;

        for (int i = 0; i < neededFollowers; i++)
        {
            spawnPos = playerPos;
            spawnPos.x += Random.Range(-randomRange, randomRange);

            GameObject gO = Instantiate(followersPF, spawnPos, Quaternion.identity);

            followers[i] = gO.transform;
        }
    }

    private void SetFollowersVelocity(float x)
    {
        followersVelocity = x * followersSpeed;
    }
}
