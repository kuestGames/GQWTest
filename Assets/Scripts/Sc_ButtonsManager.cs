using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GQW;
public class Sc_ButtonsManager : MonoBehaviour{
    public GameObject inGamePanel;
    public GameObject startPanel;
    public GameObject finishPanel;
    public GameObject pausePanel;
    public GameObject placementManager;

    public void PlayPressed() {
        startPanel.SetActive(false);
        inGamePanel.SetActive(true);
        finishPanel.SetActive(false);
        pausePanel.SetActive(false);
        Sc_GameManager.gameManager.BotonInicio();
    }

    public void PausePressed() {
        pausePanel.SetActive(true);
    }

    public void ResumePressed() {
        pausePanel.SetActive(false);

    }

    public void RetryPressed() {
        startPanel.SetActive(false);
        inGamePanel.SetActive(true);
        finishPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    public void ExitPressed() {
        Application.Quit();

    }
    public void PlacementFinish() {
        placementManager.SetActive(false);

    }

    public void PlacementStart() {
        placementManager.GetComponent<Sc_PlacementManager>().objToPlace = this.transform;
        placementManager.SetActive(true);
    }

    public void ConfirmUser(Text inputtxt) {
        string resulttxt = inputtxt.text.Trim();
        if (resulttxt!= "") {
            Sc_MainManager.manager.PressedConfirmUser(resulttxt);
        }
        else {
            Debug.Log("vacio");
        }

    }

}
