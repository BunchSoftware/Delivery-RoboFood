using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReservationElementUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image imageSafe;
    public Image imageNullSafe;
    public Text textReservationData;
    public Button buttonRemoveReservation;
    public Button buttonLoadReservation;
    public Button buttonCreateReservation;
    public StateReservationElementUI stateReservationElementUI;
    [HideInInspector] public bool isFull = false;

    public int indexReservationElement;

    public Action<int> OnClickRemoveReservationEvent;
    public Action<int> OnClickLoadReservationEvent;
    public Action<int> OnClickCreateReservationEvent;

    private Animator animator;

    private void Start()
    {
        buttonRemoveReservation.onClick.AddListener(OnClickRemoveReservation);
        animator = GetComponent<Animator>();
    }

    public void OnDrawReservationElement(bool isFull)
    {
        if (isFull)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            imageNullSafe.gameObject.SetActive(false);
            DetermineStateReservation(isFull);
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            imageNullSafe.gameObject.SetActive(true);
            DetermineStateReservation(isFull);
        }
    }

    public void OnDrawAfterReservation(string textReservation)
    {
        if (stateReservationElementUI == StateReservationElementUI.LoadReservation)
        {
            buttonLoadReservation.gameObject.SetActive(false);
            isFull = true;
            textReservationData.text = textReservation.ToString();
            buttonLoadReservation.onClick.RemoveAllListeners();
        }
        else if (stateReservationElementUI == StateReservationElementUI.CreateReservation)
        {
            buttonCreateReservation.gameObject.SetActive(false);
            isFull = true;
            textReservationData.text = textReservation.ToString();
            buttonCreateReservation.onClick.RemoveAllListeners();
        }
    }
    private void DetermineStateReservation(bool isFull)
    {
        if (stateReservationElementUI == StateReservationElementUI.LoadReservation)
        {
            if (isFull == true)
            {
                buttonCreateReservation.gameObject.SetActive(false);
                buttonLoadReservation.gameObject.SetActive(true);
            }
            else
            {
                buttonCreateReservation.gameObject.SetActive(false);
                buttonLoadReservation.gameObject.SetActive(true);               
            }

            buttonLoadReservation.onClick.RemoveAllListeners();
            buttonLoadReservation.onClick.AddListener(OnClickLoadReservation);
        }
        else if (stateReservationElementUI == StateReservationElementUI.CreateReservation)
        {
            if (isFull == true)
            {
                buttonCreateReservation.gameObject.SetActive(false);
                buttonLoadReservation.gameObject.SetActive(false);
            }
            else
            {
                buttonLoadReservation.gameObject.SetActive(false);
                buttonCreateReservation.gameObject.SetActive(true);
            }

            buttonCreateReservation.onClick.RemoveAllListeners();
            buttonCreateReservation.onClick.AddListener(OnClickCreateReservation);
        }
    }

    private void OnClickRemoveReservation()
    {
        OnClickRemoveReservationEvent?.Invoke(indexReservationElement);
    }

    public void ClearSubscribersRemoveReservation()
    {
        OnClickRemoveReservationEvent = null;
    }

    private void OnClickLoadReservation()
    {
        OnClickLoadReservationEvent?.Invoke(indexReservationElement);
    }

    public void ClearSubscribersLoadReservation()
    {
        OnClickLoadReservationEvent = null;
    }

    private void OnClickCreateReservation()
    {
        OnClickCreateReservationEvent?.Invoke(indexReservationElement);
    }

    public void ClearSubscribersCreateReservation()
    {
        OnClickCreateReservationEvent = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetTrigger("Highlighted");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetTrigger("Highlighted");
        animator.SetTrigger("Normal");
    }
}


public enum StateReservationElementUI
{
    LoadReservation,
    CreateReservation,
}
