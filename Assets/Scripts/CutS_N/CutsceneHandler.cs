using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;

public class CutsceneHandler : MonoBehaviour
{
    public Camera cam;
    public CinemachineVirtualCameraBase vCam;
    
    private CutsceneElementBase[] cutsceneElements;
    private int index = -1;

    public void Start()
    {
        cutsceneElements = GetComponentsInChildren<CutsceneElementBase>();
    }

    private void ExecuteCurrentElement()
    {
        if(index >=0 && index < cutsceneElements.Length)
            cutsceneElements[index].Execute();
    }

    public void PlayNextElement()
    {
        index++;
        ExecuteCurrentElement();
    }
}
