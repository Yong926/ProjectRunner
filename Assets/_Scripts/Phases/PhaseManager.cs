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
    [SerializeField, Foldout] List<PhaseSO> phaseList = new List<PhaseSO>();

    private TrackManager trkMgr;
    private ObstacleManager obsMgr;
    private InGameUI uiInGame;

    IEnumerator Start()
    {
        trkMgr = FindFirstObjectByType<TrackManager>();
        obsMgr = FindFirstObjectByType<ObstacleManager>();
        uiInGame = FindFirstObjectByType<InGameUI>();

        GetEndLine();

        uiInGame.SetMileage(phaseList);

        yield return new WaitUntil(() => GameManager.IsPlaying);
        StartCoroutine(IntervalUpdate());
    }

    IEnumerator IntervalUpdate()
    {
        if (phaseList == null || phaseList.Count <= 0)
            yield break;

        int i = 0;

        while (true)
        {
            PhaseSO phase = phaseList[i];
            if (GameManager.mileage > phase.Mileage)
            {
                SetPhase(phase);
                i++;
            }

            if (i == phaseList.Count)
            {
                GameClear(phase);
                yield break;
            }

            yield return new WaitForSeconds(updateInterval);
        }
    }

    void GetEndLine()
    {
        PhaseSO phaseEnd = phaseList.LastOrDefault();
        GameManager.mileageEnd = phaseEnd.Mileage;
    }

    void SetPhase(PhaseSO phase)
    {
        uiInGame?.SetPhase(phase);
        trkMgr?.SetPhase(phase);
        obsMgr?.SetPhase(phase);
    }

    void GameClear(PhaseSO phase)
    {
        SetPhase(phase);

        GameManager.IsPlaying = false;
        GameManager.IsGameover = true;
    }
}