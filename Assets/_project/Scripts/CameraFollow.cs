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
    public float slowMoScale = 0.2f;  // Time scale during slow-motion
    public float slowMoDuration = 1f; // Duration of the slow-motion effect
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

        // Apply slow-motion effect
        Time.timeScale = slowMoScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Adjust physics time step

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime for smooth slow-motion
            yield return null;
        }

        // Revert time scale
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f; // Reset physics time step to normal

        callback?.Invoke();
    }

    public void OnDestruction()
    {
        StartCoroutine(SwitchCamera(cam2, cam1, transitionDuration));
    }

    private void OnTransitionComplete()
    {
        Debug.Log("Transition Complete");

        // Perform the next action here
        PerformNextAction();
    }

    private void PerformNextAction()
    {
        // ShipController.instance.IsActivated = true;
        ShipController.instance.StartShipMovement();
        // Your next action logic here
        Debug.Log("Performing next action...");
    }
}
