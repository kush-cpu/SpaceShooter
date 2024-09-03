using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
    public CinemachineVirtualCamera cam1;
    public CinemachineVirtualCamera cam2;
    public float transitionDuration = 1f;
    public System.Action onTransitionComplete;

    private CinemachineBrain cinemachineBrain;

    private void Awake()
    {
        instance = this;
    }
    public void GameStart()
    {
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        StartCoroutine(SwitchCamera(cam1, cam2, transitionDuration, OnTransitionComplete));
    }

    private IEnumerator SwitchCamera(CinemachineVirtualCamera fromCam, CinemachineVirtualCamera toCam, float duration, System.Action callback = null)
    {
        fromCam.Priority = 10;
        toCam.Priority = 20;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        callback?.Invoke();
    }

    public void OnDestruction()
    {
        StartCoroutine(SwitchCamera(cam2, cam1, 0.5f));
    }

    private void OnTransitionComplete()
    {
        Debug.Log("Transition Complete");
        // Perform the next action here
        PerformNextAction();
    }

    private void PerformNextAction()
    {
        //ShipController.instance.IsActivated = true;
        ShipController.instance.StartShipMovement();
        // Your next action logic here
        Debug.Log("Performing next action...");
    }
}
