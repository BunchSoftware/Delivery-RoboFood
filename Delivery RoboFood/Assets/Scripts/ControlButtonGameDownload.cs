 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlButtonGameDownload : MonoBehaviour
{
    [SerializeField] private UIController UIController;
    [SerializeField] private int gameSceneNumber;
    [Header("Button")]
    [SerializeField] private Button buttonBegin;
    [SerializeField] private Button buttonContinue;
    [SerializeField] private Button buttonNewGame;

    private ReservationManagerIO saveManagerIO;
    private string pathToApplicationFile;
    private ApplicationData applicationData;

    private void Start()
    {
        buttonNewGame.onClick.AddListener(
            () => 
            { 
                NewGame();
                UIController.soundManager.OnPlayOneShotAndEndLast(0);
            }
            );

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Button>().onClick.AddListener(
            () =>
            {
                UIController.soundManager.OnPlayOneShotAndEndLast(0);
            }
            );
        }

        pathToApplicationFile = Application.persistentDataPath + $"/ApplicationData.dap";
        saveManagerIO = new ReservationManagerIO(pathToApplicationFile);
        applicationData = saveManagerIO.LoadReservationApplicationData();

        if (applicationData != null)
        {
            if (IsThereSave(applicationData))
            {
                buttonContinue.gameObject.SetActive(true);
                buttonNewGame.gameObject.SetActive(true);
                buttonBegin.gameObject.SetActive(false);
            }
            else
            {
                buttonContinue.gameObject.SetActive(false);
                buttonNewGame.gameObject.SetActive(false);
                buttonBegin.gameObject.SetActive(true);
            }
        }
    }

    public bool IsThereSave(ApplicationData applicationData)
    {
        pathToApplicationFile = Application.persistentDataPath + $"/ApplicationData.dap";
        saveManagerIO = new ReservationManagerIO(pathToApplicationFile);
        applicationData = saveManagerIO.LoadReservationApplicationData();

        if (applicationData != null)
        {
            for (int i = 0; i < applicationData.reservationElementUIData.Count; i++)
            {
                if (applicationData.reservationElementUIData[i] != null)
                {
                    return true;
                }
                return false;
            }
        }
        return false;
    }


    public void NewGame()
    {
        saveManagerIO = new ReservationManagerIO(Application.persistentDataPath);
        saveManagerIO.DeleteAll();
        UIController.LoadLevel(gameSceneNumber);
    }
}
