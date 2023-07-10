using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class BattlefieldScene : BaseScene
{
    private IObjectPool<Soldier> soldierPool;
    private Transform soldierPoolRoot;
    private bool deployingAllySoldier;

    protected override IEnumerator LoadingRoutine()
    {
        yield return StartCoroutine(InitRoutine());
        progress = 1f;
    }
    // TODO: subdivide Init
    IEnumerator InitRoutine()
    {
        // temporary loading time
        yield return new WaitForSecondsRealtime(1f);
        progress = 0.7f;
        yield return new WaitForSecondsRealtime(2f);
        yield return StartCoroutine("DeploySoldierRoutine");
        yield return null;
    }
    IEnumerator DeploySoldierRoutin()
    {
        soldierPoolRoot = new GameObject("SoldierPoolRoot").transform;
        soldierPool = new ObjectPool<Soldier>(
            CreateSoldier,
            OnGet,
            OnRelease,
            OnDistroy
            );
        StartCoroutine("DeployAllySoldierRoutine");
        StartCoroutine("DeployEnemySoldierRoutine");
        yield return null;
    }
    IEnumerable DeployAllySoldierRoutine()
    {        
        Soldier deployedSolder;
        for (int i = 0; i < GameManager.Data.NumberOfTroops; i++)
        {
            deployingAllySoldier = true;
            soldierPool.Get(out deployedSolder);
            if (i >= GameManager.Data.SoldierCapacityInBattle)
            {
                // TODO: yield return new WaitWhile (() => ������� ���� �̻� ¿���������� ���)
                //GameManager.Data.NumberOfTroops - ������ �����
                //i = 0;
            }
            yield return null;
        }
    }
    IEnumerable DeployEnemySoldierRoutine()
    {
        Soldier deployedSolder;
        for (int i = 0; i < GameManager.Data.EncounteredNumberOfEnemyTroops; i++)
        {
            deployingAllySoldier = false;
            soldierPool.Get(out deployedSolder);
            if (i >= GameManager.Data.SoldierCapacityInBattle)
            {
                // TODO: yield return new WaitWhile (() => ������� ���� �̻� ¿���������� ���)
                // GameManager.Data.EncounteredNumberOfEnemyTroops - ������ �����
                // ���߿� EncounteredNumberOfEnemyTroops�� ���� �� �ִ� �屺���� ���� ���� �ٲ㼭 ����
                // �屺���� ������� Data�� Dictionary�� ����
                //i = 0;
            }
            yield return null;
        }
    }
    private Soldier CreateSoldier()
    {
        Soldier soldier = Instantiate(Resources.Load("Prefabs/Soldier").GetComponent<Soldier>());
        soldier.SetPool(soldierPool);
        soldier.transform.parent = soldierPoolRoot;
        return soldier;
    }
    private void OnGet(Soldier soldier)
    {
        soldier.Initialize(deployingAllySoldier);
    }
    private void OnRelease(Soldier soldier)
    {
        soldier.gameObject.SetActive(false);
    }
    private void OnDistroy(Soldier soldier)
    {
        Destroy(soldier.gameObject);
    }
}
