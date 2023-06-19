using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScene : BaseScene
{
    protected override IEnumerator LoadingRoutine()
    {
        progress = 1f;
        yield return null;
    }
    public void GameStart()
    {
        GameManager.Scene.LoadScene("BattlefieldScene", 0);
    }
    public void GameQuit()
    {
        Application.Quit();
    }
}
