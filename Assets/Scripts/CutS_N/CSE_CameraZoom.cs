using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class CSE_CameraZoom : CutsceneElementBase
{
    [SerializeField] private float targetSize = 3f; // smaller = more zoom
    [SerializeField] private Transform player; // assign in inspector
    

    private float originalSize;
    private Transform originalFollow;
    public CinemachineCamera vCam;
    

    public override void Execute()
    {
        vCam = (CinemachineCamera)cutsceneHandler.vCam;

        // Store original values
        originalSize = vCam.Lens.OrthographicSize;
        originalFollow = vCam.Follow;

        StartCoroutine(ZoomCamera());
    }

    private IEnumerator ZoomCamera()
    {
        float elapsedTime = 0;

        // ZOOM IN
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            vCam.Lens.OrthographicSize = Mathf.Lerp(originalSize, targetSize, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        vCam.Lens.OrthographicSize = targetSize;

        // Wait (for dialogue moment)
        yield return new WaitForSeconds(1f);

        elapsedTime = 0;

        // ZOOM OUT
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            vCam.Lens.OrthographicSize = Mathf.Lerp(targetSize, originalSize, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Restore original zoom
        vCam.Lens.OrthographicSize = originalSize;

        // Restore follow (IMPORTANT)
        vCam.Follow = originalFollow != null ? originalFollow : player;
        vCam.LookAt = player;

        cutsceneHandler.PlayNextElement();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}