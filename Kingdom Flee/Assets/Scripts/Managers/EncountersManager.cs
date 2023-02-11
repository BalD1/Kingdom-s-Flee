using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncountersManager : Singleton<EncountersManager>
{
    [field: SerializeField] public SO_Encounter firstEncounter { get; private set; }

    [field: SerializeField] public SO_Encounter[] EncountersData { get; private set; }

    [field: SerializeField] public float encounterSpawnDistanceFromPlayer { get; private set; }

    [field: SerializeField] public int encountersSpawnCount { get; private set; }

    private void Start()
    {
        Vector2 spawnPos = GameManager.Instance.Player.transform.position;

        spawnPos.x += encounterSpawnDistanceFromPlayer;
        spawnPos.y = 0;

        Encounter.Create(firstEncounter, spawnPos);

        for (int i = 0; i < encountersSpawnCount; i++)
        {
            spawnPos = GameManager.Instance.Player.transform.position;

            spawnPos.x += encounterSpawnDistanceFromPlayer * (i + 2);
            spawnPos.y = 0;

            Encounter.Create(EncountersData.RandomElement(), spawnPos);
        }
    }

    public void SpawnNewEncounter()
    {
        Vector2 spawnPos = GameManager.Instance.Player.transform.position;

        spawnPos.x += encounterSpawnDistanceFromPlayer;
        spawnPos.y = 0;

        Encounter.Create(EncountersData.RandomElement(), spawnPos);
    }
}
