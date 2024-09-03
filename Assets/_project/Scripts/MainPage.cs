using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPage : MonoBehaviour
{
    public Button StartButton;
    public Canvas MainCanvas;

    private void Start()
    {
        ShipController.instance.disableText();
    }
    public void OnStartButtonClick()
    {
        CameraFollow.instance.GameStart();
        MainCanvas.gameObject.SetActive(false);

    }

}
