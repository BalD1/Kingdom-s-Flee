using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncountersManager : Singleton<EncountersManager>
{
    [field: SerializeField] public SO_Encounter[] EncountersData { get; private set; }

    [field: SerializeField] public float encounterSpawnDistanceFromPlayer { get; private set; }

    [field: SerializeField] public int encountersSpawnCount { get; private set; }

    private void Start()
    {
        for (int i = 0; i < encountersSpawnCount; i++)
        {
            Vector2 spawnPos = GameManager.Instance.Player.transform.position;

            spawnPos.x += encounterSpawnDistanceFromPlayer * (i + 1);
            spawnPos.y = .5f;

            Encounter.Create(EncountersData.RandomElement(), spawnPos);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) UIManager.Instance.SetupEncounterWindow(EncountersData[0]);
    }
}
