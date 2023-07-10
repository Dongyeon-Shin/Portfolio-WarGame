using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private int soldierCapacityInBattle;
    public int SoldierCapacityInBattle { get { return soldierCapacityInBattle;} set { soldierCapacityInBattle = value; } }
    private int numberOfTroops;
    public int NumberOfTroops { get { return numberOfTroops; } }
    private int encounteredNumberOfEnemyTroops;
    public int EncounteredNumberOfEnemyTroops;
    private Vector3 allySpawnPoint;
    public Vector3 AllySpawnPoint { get { return allySpawnPoint; } }
    private Vector3 enemySpawnPoint;
    public Vector3 EnemySpawnPoint { get { return enemySpawnPoint; } }
    private string pathOfMapResource;

    private void Awake()
    {
        pathOfMapResource = "Prefabs/BattleField";

        //temp
        soldierCapacityInBattle = 30;
    }
    public void Encounter(int encounteredNumberOfEnemyTroops, int tileNumber)
    {
        StringBuilder stringBuilder = new StringBuilder(25);
        stringBuilder.Append(pathOfMapResource);
        stringBuilder.Append(tileNumber);
        Instantiate(Resources.Load(stringBuilder.ToString()));       
        allySpawnPoint = GameObject.FindGameObjectsWithTag("Respawn")[0].transform.position;
        enemySpawnPoint = GameObject.FindGameObjectsWithTag("Respawn")[1].transform.position;
        this.encounteredNumberOfEnemyTroops = encounteredNumberOfEnemyTroops;
    }
}
