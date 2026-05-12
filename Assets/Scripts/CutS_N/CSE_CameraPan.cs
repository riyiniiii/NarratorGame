using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CSE_CameraPan : CutsceneElementBase
{
    [SerializeField] private Transform panTarget; // where camera should move
    [SerializeField] private Transform player; // to return after

    private CinemachineCamera vCam;
    private Transform originalFollow;

    public override void Execute()
    {
        vCam = (CinemachineCamera)cutsceneHandler.vCam;

        // Store original follow (player)
        originalFollow = vCam.Follow;

        StartCoroutine(PanCoroutine());
    }

    private IEnumerator PanCoroutine()
    {
        // 🔥 PAN TO TARGET
        vCam.Follow = panTarget;

        // Wait for the pan (Cinemachine handles movement)
        yield return new WaitForSeconds(duration);

        // 🔥 PAN BACK TO PLAYER
        vCam.Follow = originalFollow != null ? originalFollow : player;
        vCam.LookAt = player;

        yield return new WaitForSeconds(duration);

        cutsceneHandler.PlayNextElement();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}    

