using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DebugScene : MonoBehaviour
{
    public int numberOfTroop;
    public int numberOfEnemyTroop;
    [SerializeField]
    private Transform allySpawnPoint;
    [SerializeField]
    private Transform enemySpawnPoint;

    private void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnAllyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < numberOfTroop; i++)
        {
            yield return new WaitForSeconds(2f);
            Soldier solder = Instantiate(Resources.Load("Prefabs/Soldier").GetComponent<Soldier>());
            solder.Initialize(true);
        }      
    }
    IEnumerator SpawnAllyRoutine()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < numberOfTroop; i++)
        {
            yield return new WaitForSeconds(2f);
            Soldier solder = Instantiate(Resources.Load("Prefabs/Soldier").GetComponent<Soldier>());
            solder.Initialize(false);
        }
    }
}
