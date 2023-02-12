using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowersManager : Singleton<FollowersManager>
{
    [SerializeField] [Range(0,50)] private int startFollowersCount;


    [SerializeField] private Transform followersDirectory;

    [field: SerializeField] public List<GameObject> followers { get; private set; }

    [SerializeField] private float randomRange = 1;
    [SerializeField] private float startDistanceBehindKing = 3;

    [InspectorButton(nameof(InitiateFollowers), ButtonWidth = 150)]
    [SerializeField] private bool respawnFollowers;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitiateFollowers();
    }

    private void InitiateFollowers()
    {
        if (startFollowersCount <= 0) return;

        if (followers.Count > 0)
        {
            foreach (var item in followers) Destroy(item);
        }

        followers = new List<GameObject>();

        SpawnFollowers(startFollowersCount);

        GameManager.AddFollowers(startFollowersCount);
    }

    public void SpawnFollowers(int count)
    {
        Vector2 spawnPos;
        Vector2 playerPos = GameManager.Instance.Player.transform.position;

        for (int i = 0; i < count; i++)
        {
            spawnPos = playerPos;
            spawnPos.x += Random.Range(-randomRange, randomRange) - startDistanceBehindKing - randomRange;
            spawnPos.y = 0;

            GameObject gO = Instantiate(GameAssets.Instance.FollowersPF, spawnPos, Quaternion.identity);
            gO.transform.SetParent(followersDirectory, true);

            followers.Add(gO);
        }
    }

    public void KillFollowers(int count)
    {
        if (count <= 0) return;

        int idx = Random.Range(0, followers.Count);
        GameObject f = followers[idx];
        followers.RemoveAt(idx);

        Destroy(f);

        KillFollowers(count - 1);
    }
}
