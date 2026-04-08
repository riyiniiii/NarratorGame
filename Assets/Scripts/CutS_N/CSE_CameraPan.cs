using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CSE_CameraPan : CutsceneElementBase
{ 
    private CinemachineVirtualCameraBase vCam;
    [SerializeField] private Vector2 distanceToMove;

    public override void Execute()
    {
        vCam = cutsceneHandler.vCam;
        vCam.Follow = null;
        StartCoroutine(PanCoroutine());
    }

    private IEnumerator PanCoroutine()
    {
        Vector3 originalPosition = vCam.transform.position;
        Vector3 targetPosition = originalPosition + new Vector3(distanceToMove.x, distanceToMove.y, 0);
        
        float startTime = Time.time;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            vCam.transform.position = Vector3.Lerp(originalPosition, targetPosition, t);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        vCam.transform.position = targetPosition;
        
        cutsceneHandler.PlayNextElement();
    }

    private void OnDestroy()
    {
      StopAllCoroutines();  
    }
    
}
