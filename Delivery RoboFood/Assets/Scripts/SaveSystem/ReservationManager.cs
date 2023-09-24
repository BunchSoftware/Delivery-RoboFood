using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReservationManager : MonoBehaviour
{
    [SerializeField] private GameObject contentPage;
    [SerializeField] private GameObject DeleteAnswerPanel;
    [SerializeField] private Fade fade;
    [SerializeField][HideInInspector] private List<PageReservation> pageSafes;
    private ReservationManagerIO saveManagerIO;
    private List<ReservationElementUIData> reservationElementUIDatas;

    private string pathToApplicationFile;
    private ApplicationData applicationData;

    private bool isPageManagerActive = false;

    public event Action<PlayerData> OnInitializated;

    private void Awake()
    {
        Initialization();
    }

    private void Update()
    {
        if (transform.GetChild(0).gameObject.activeInHierarchy && isPageManagerActive == false)
        {
            UpdateData();
            DrawReservationElements();
            isPageManagerActive = true;
        }
        else if (transform.GetChild(0).gameObject.activeInHierarchy == false)
        {
            isPageManagerActive = false;
        }
    }

    private void Initialization()
    {
        pathToApplicationFile = Application.persistentDataPath + $"/ApplicationData.dap";
        saveManagerIO = new ReservationManagerIO(pathToApplicationFile);

        for (int i = 0; i < contentPage.transform.childCount; i++)
        {
            PageReservation pageReservation = contentPage.transform.GetChild(i).GetChild(0).GetComponent<PageReservation>();
            pageReservation.Initialization();
            pageSafes.Add(pageReservation);
        }

        if (saveManagerIO.LoadReservationApplicationData() == null)
        {
            applicationData = new ApplicationData();
            reservationElementUIDatas = new List<ReservationElementUIData>();
            for (int i = 0; i < pageSafes.Count; i++)
            {
                for (int j = 0; j < pageSafes[i].reservationElements.Count; j++)
                {
                    reservationElementUIDatas.Add(new ReservationElementUIData());
                }
            }

            OnInitializated?.Invoke(null);
        }
        else
        {
            applicationData = saveManagerIO.LoadReservationApplicationData();
            if (applicationData.playerData != null)
            {
                Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                applicationData.playerData.FillDataToPlayer(player);
                OnInitializated?.Invoke(applicationData.playerData);
               // applicationData.playerData = null;
                saveManagerIO = new ReservationManagerIO(pathToApplicationFile);
                saveManagerIO.CreateReservationApplicationData(applicationData);
            }
            reservationElementUIDatas = new List<ReservationElementUIData>(applicationData.reservationElementUIData);
        }

        DrawReservationElements();
    }

    private void UpdateData()
    {
        saveManagerIO = new ReservationManagerIO(pathToApplicationFile);

        if (saveManagerIO.LoadReservationApplicationData() == null)
        {
            applicationData = new ApplicationData();
            reservationElementUIDatas = new List<ReservationElementUIData>();
            for (int i = 0; i < pageSafes.Count; i++)
            {
                for (int j = 0; j < pageSafes[i].reservationElements.Count; j++)
                {
                    reservationElementUIDatas.Add(new ReservationElementUIData());
                }
            }
        }
        else
        {
            applicationData = saveManagerIO.LoadReservationApplicationData();
            reservationElementUIDatas = new List<ReservationElementUIData>(applicationData.reservationElementUIData);
        }
    }

    private void DrawReservationElements()
    {
            int temp = 0;
            for (int i = 0; i < pageSafes.Count; i++)
            {
                for (int j = 0; j < pageSafes[i].reservationElements.Count; j++)
                {
                    pageSafes[i].reservationElements[j].indexReservationElement = j + temp;

                    pageSafes[i].reservationElements[j].ClearSubscribersRemoveReservation();
                    pageSafes[i].reservationElements[j].OnClickRemoveReservationEvent += (index) =>
                    {
                        RemoveReservation(index);
                    };
                    if (pageSafes[i].reservationElements[j].stateReservationElementUI == StateReservationElementUI.LoadReservation)
                    {
                        pageSafes[i].reservationElements[j].ClearSubscribersLoadReservation();
                        pageSafes[i].reservationElements[j].OnClickLoadReservationEvent += (index) =>
                        {
                            LoadReservation(index);
                        };
                    }
                    else if (pageSafes[i].reservationElements[j].stateReservationElementUI == StateReservationElementUI.CreateReservation)
                    {
                        pageSafes[i].reservationElements[j].ClearSubscribersCreateReservation();
                        pageSafes[i].reservationElements[j].OnClickCreateReservationEvent += (index) =>
                        {

                            AddReservation(index);
                        };
                    }

                    if (FindReservationElementDataOfIndex(pageSafes[i].reservationElements[j].indexReservationElement) == null)
                    {
                        ReservationElementUIData reservationElementUIData = new ReservationElementUIData();
                        pageSafes[i].reservationElements[j].OnDrawReservationElement(reservationElementUIData.isFull);
                        pageSafes[i].reservationElements[j].textReservationData.text = reservationElementUIData.dateCreation;
                    }
                    else
                    {
                        ReservationElementUIData reservationElementUIData = FindReservationElementDataOfIndex(pageSafes[i].reservationElements[j].indexReservationElement);
                        pageSafes[i].reservationElements[j].OnDrawReservationElement(reservationElementUIData.isFull);
                        pageSafes[i].reservationElements[j].textReservationData.text = reservationElementUIData.dateCreation;
                    }
                }
                temp = pageSafes[i].reservationElements.Count;
            }
    }

    private ReservationElementUI FindReservationElementOfIndex(int indexReservation)
    {
        for (int i = 0; i < pageSafes.Count; i++)
        {
            for (int j = 0; j < pageSafes[i].reservationElements.Count; j++)
            {
                if (pageSafes[i].reservationElements[j].indexReservationElement == indexReservation)
                {
                    return pageSafes[i].reservationElements[j];
                }
            }
        }
        return null;
    }

    private void AddReservationElementDataOfIndex(int indexReservation, ReservationElementUIData reservationElementUIData)
    {
        int tempCount = 0;
        for (int i = 0; i < pageSafes.Count; i++)
        {
            tempCount += pageSafes[i].reservationElements.Count;
        }
        if (reservationElementUIDatas.Count != tempCount)
        {
            reservationElementUIDatas = new List<ReservationElementUIData>();
            for (int i = 0; i < pageSafes.Count; i++)
            {
                for (int j = 0; j < pageSafes[i].reservationElements.Count; j++)
                {
                    reservationElementUIDatas.Add(new ReservationElementUIData());
                }
            }

            for (int i = 0; i < pageSafes.Count; i++)
            {
                for (int j = 0; j < pageSafes[i].reservationElements.Count; j++)
                {
                    if (pageSafes[i].reservationElements[j].indexReservationElement == indexReservation)
                    {
                        reservationElementUIDatas[indexReservation] = reservationElementUIData;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < pageSafes.Count; i++)
            {
                for (int j = 0; j < pageSafes[i].reservationElements.Count; j++)
                {
                    if (pageSafes[i].reservationElements[j].indexReservationElement == indexReservation)
                    {
                        reservationElementUIDatas[indexReservation] = reservationElementUIData;
                    }
                }
            }
        }
    }

    private ReservationElementUIData FindReservationElementDataOfIndex(int indexReservation)
    {
        for (int i = 0; i < reservationElementUIDatas.Count; i++)
        {
            if (reservationElementUIDatas[i].indexReservationElementUI == indexReservation)
            {
                return reservationElementUIDatas[i];
            }
        }
        return null;
    }

    public void LoadReservation(int indexReservation)
    {
        if (FindReservationElementDataOfIndex(indexReservation) != null)
        {
            saveManagerIO = new ReservationManagerIO(pathToApplicationFile);
            applicationData = saveManagerIO.LoadReservationApplicationData();
            ReservationElementUIData reservationElementUIData = FindReservationElementDataOfIndex(indexReservation);
            saveManagerIO = new ReservationManagerIO(reservationElementUIData.path);
            applicationData.playerData = saveManagerIO.LoadReservationPlayerData();
            applicationData.currentReservationElementUIData = indexReservation;
            saveManagerIO = new ReservationManagerIO(pathToApplicationFile);
            saveManagerIO.CreateReservationApplicationData(applicationData);
            Time.timeScale = 1;
            fade.currentIndexScene = applicationData.playerData.level;
            fade.FadeBlack();
        }
    }

    public void AddReservation(int indexReservation)
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        PlayerData playerData = new PlayerData(player);
        playerData.level = SceneManager.GetActiveScene().buildIndex;

        string path =
            Application.persistentDataPath
            + $"/{DateTime.Now.ToString("MM/dd/yyyy")}_{DateTime.Now.ToString("HHmmss")}.dap";

        ReservationElementUI reservationElementUI = FindReservationElementOfIndex(indexReservation);
        reservationElementUI.OnDrawReservationElement(true);
        reservationElementUI.OnDrawAfterReservation
            ($"{DateTime.Now.ToString("MM/dd/yyyy")} {DateTime.Now.ToString("HH:mm:ss")}");

        saveManagerIO = new ReservationManagerIO(path);
        saveManagerIO.CreateReservationPlayerData(playerData);

        ReservationElementUIData reservationElementUIData = new ReservationElementUIData();

        reservationElementUIData.indexReservationElementUI = reservationElementUI.indexReservationElement;
        reservationElementUIData.isFull = reservationElementUI.isFull;
        reservationElementUIData.path = path;
        reservationElementUIData.dateCreation = $"{DateTime.Now.ToString("MM/dd/yyyy")} {DateTime.Now.ToString("HH:mm:ss")}";


        AddReservationElementDataOfIndex(indexReservation, reservationElementUIData);

        applicationData.reservationElementUIData = reservationElementUIDatas;

        saveManagerIO = new ReservationManagerIO(pathToApplicationFile);
        saveManagerIO.CreateReservationApplicationData(applicationData);
    }

    public void RemoveReservation(int indexReservation)
    {
        DeleteAnswerPanel.gameObject.SetActive(true);
        for (int i = 0; i < DeleteAnswerPanel.transform.GetChild(0).transform.childCount; i++)
        {
            if (DeleteAnswerPanel.transform.GetChild(0).transform.GetChild(i).name == "Yes")
            {
                DeleteAnswerPanel.
transform.GetChild(0).
transform.GetChild(i).
transform.GetComponent<Button>().
onClick.RemoveAllListeners();

                DeleteAnswerPanel.
                    transform.GetChild(0).
                    transform.GetChild(i).
                    transform.GetComponent<Button>().
                    onClick.AddListener(
                    () =>
                    {
                        for (int i = 0; i < pageSafes.Count; i++)
                        {
                            for (int j = 0; j < pageSafes[i].reservationElements.Count; j++)
                            {
                                if (pageSafes[i].reservationElements[j].indexReservationElement == indexReservation)
                                {
                                    pageSafes[i].reservationElements[j].OnDrawReservationElement(false);
                                    ReservationElementUIData reservationElementUIData = FindReservationElementDataOfIndex(pageSafes[i].reservationElements[j].indexReservationElement);
                                    saveManagerIO = new ReservationManagerIO(reservationElementUIData.path);
                                    saveManagerIO.Delete();

                                    saveManagerIO = new ReservationManagerIO(pathToApplicationFile);

                                    reservationElementUIData.Dispose();

                                    AddReservationElementDataOfIndex(indexReservation, reservationElementUIData);

                                    applicationData.reservationElementUIData = reservationElementUIDatas;

                                    saveManagerIO.CreateReservationApplicationData(applicationData);
                                }
                            }
                        }
                        DeleteAnswerPanel.gameObject.SetActive(false);
                    }
                    );
            }
        }
    }

    public void ClearAllReservation()
    {
        for (int i = 0; i < pageSafes.Count; i++)
        {
            for (int j = 0; j < pageSafes[i].reservationElements.Count; j++)
            {
                pageSafes[i].reservationElements[j].OnDrawReservationElement(false);
            }
        }
    }

    public void ClearAllData()
    {
        saveManagerIO = new ReservationManagerIO(Application.persistentDataPath);
        saveManagerIO.DeleteAll();

        fade.currentIndexScene = SceneManager.GetActiveScene().buildIndex;
        fade.FadeBlack();
    }

    public void AutoReservation()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        PlayerData playerData = new PlayerData(player);
        string path = Application.persistentDataPath
            + $"/{DateTime.Now.ToString("MM/dd/yyyy")}_{DateTime.Now.ToString("HHmmss")}.dap";
        saveManagerIO = new ReservationManagerIO(path);
        saveManagerIO.CreateReservationPlayerData(playerData);
    }
}
