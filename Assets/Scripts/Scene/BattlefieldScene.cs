using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlefieldScene : BaseScene
{
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
        yield return null;
    }
}
