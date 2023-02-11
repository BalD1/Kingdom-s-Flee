using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowersManager : Singleton<FollowersManager>
{
    [SerializeField] [Range(0,50)] private int startFollowersCount;


    [SerializeField] private Transform followersDirectory;

    [field: SerializeField] public GameObject[] followers { get; private set; }

    [SerializeField] private float randomRange = 1;
    [SerializeField] private float startDistanceBehindKing = 3;

    [InspectorButton(nameof(SpawnFollowers), ButtonWidth = 150)]
    [SerializeField] private bool respawnFollowers;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        SpawnFollowers();
    }

    private void SpawnFollowers()
    {
        if (startFollowersCount <= 0) return;

        if (followers.Length > 0)
        {
            foreach (var item in followers) Destroy(item);
        }
        GameObject followersPF = GameAssets.Instance.FollowersPF;

        Vector2 playerPos = GameManager.Instance.Player.transform.position;

        followers = new GameObject[startFollowersCount];

        Vector2 spawnPos;

        for (int i = 0; i < startFollowersCount; i++)
        {
            spawnPos = playerPos;
            spawnPos.x += Random.Range(-randomRange, randomRange) - startDistanceBehindKing - randomRange;
            spawnPos.y = 0;

            GameObject gO = Instantiate(followersPF, spawnPos, Quaternion.identity);
            gO.transform.SetParent(followersDirectory, true);

            followers[i] = gO;
        }

        GameManager.AddFollowers(startFollowersCount);
    }
}
