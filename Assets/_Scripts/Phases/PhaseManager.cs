using UnityEngine;
using CustomInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseManager : MonoBehaviour
{
    [HorizontalLine("기본속성"), HideField] public bool _l0;
    [SerializeField] float updateInterval = 1f;

    [HorizontalLine("트랙속성"), HideField] public bool _l1;
    [SerializeField] List<Phase> mileageList = new List<Phase>();

    private TrackManager trkMgr;
    private ObstacleManager obsMgr;
    private InGameUI uiInGame;

    IEnumerator Start()
    {
        trkMgr = FindFirstObjectByType<TrackManager>();
        obsMgr = FindFirstObjectByType<ObstacleManager>();
        uiInGame = FindFirstObjectByType<InGameUI>();

        GetEndLine();

        yield return new WaitUntil(() => GameManager.IsPlaying);
        StartCoroutine(IntervalUpdate());
    }

    IEnumerator IntervalUpdate()
    {
        if (mileageList == null || mileageList.Count <= 0)
            yield break;

        int i = 0;

        while (true)
        {
            Phase phase = mileageList[i];
            if (GameManager.mileage > phase.Mileage)
            {
                SetPhase(phase);
                i++;
            }

            if (i == mileageList.Count)
            {
                GameClear(phase);
                yield break;
            }

            yield return new WaitForSeconds(updateInterval);
        }
    }

    void GetEndLine()
    {
        Phase phaseEnd = mileageList.LastOrDefault();
        GameManager.mileageEnd = phaseEnd.Mileage;
    }

    void SetPhase(Phase phase)
    {
        uiInGame?.ShowInfo(phase.Name);
        trkMgr?.SetPhase(phase);
        obsMgr?.SetPhase(phase);
    }

    void GameClear(Phase phase)
    {
        SetPhase(phase);

        GameManager.IsPlaying = false;
        GameManager.IsGameover = true;
    }
}