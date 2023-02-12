using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncountersManager : Singleton<EncountersManager>
{
    [field: SerializeField] public SO_Encounter firstEncounter { get; private set; }

    [field: SerializeField] public SO_Encounter[] EncountersData { get; private set; }
    private SO_Encounter lastEncounter;

    [field: SerializeField] public float encounterSpawnDistanceFromPlayer { get; private set; }

    [field: SerializeField] public int encountersSpawnCount { get; private set; }

    private int skipEncounterCount = 1;

    private const int WHILE_LOOP_FAIL_SAFE_COUNT = 10;
    private int whileFailSafe;

    private void Start()
    {
        return;
        Vector2 spawnPos = GameManager.Instance.Player.transform.position;

        spawnPos.x += encounterSpawnDistanceFromPlayer;
        spawnPos.y = 0;
        Encounter.Create(firstEncounter, spawnPos);

        for (int i = 0; i < encountersSpawnCount; i++)
        {
            spawnPos = GameManager.Instance.Player.transform.position;

            spawnPos.x += encounterSpawnDistanceFromPlayer * (i + 2);
            spawnPos.y = 0;

            SpawnEncounter(spawnPos);
        }
    }

    public void SpawnNewEncounter()
    {
        if (skipEncounterCount > 0)
        {
            skipEncounterCount--;
            return;
        }

        Vector2 spawnPos = GameManager.Instance.Player.transform.position;

        spawnPos.x += encounterSpawnDistanceFromPlayer * encountersSpawnCount;
        spawnPos.y = 0;

        SpawnEncounter(spawnPos);
    }

    private void SpawnEncounter(Vector2 spawnPos)
    {
        SO_Encounter randEncounter = null;

        do
        {
            randEncounter = EncountersData.RandomElement();
            whileFailSafe++;

        } while (randEncounter == lastEncounter && whileFailSafe < WHILE_LOOP_FAIL_SAFE_COUNT);

        lastEncounter = randEncounter;
        Encounter.Create(randEncounter, spawnPos);
    }
}
