using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Button[] transportButton;
    public Action<int> SelectTranportButtonClicked;

    private void Awake()
    {
        HideTransportButtons();
        UnitController.ShowTransportButtons = ShowTransportButtons;
        SelectTranportButtonClicked = UnitController.SelectUnit;
        for (int i = 0; i < transportButton.Length; i++)
        {
            int index = i;
            transportButton[i].onClick.AddListener(() => SelectTranportButtonClicked?.Invoke(index - 1));
        }
    }

    public void ShowTransportButtons(UnitObject unit)
    {
        if (unit != null && unit.boardedUnits.Count >0)
        {
            //TODO:redo to disable rendering
            transportButton[0].transform.parent.gameObject.SetActive(true);
            transportButton[0].gameObject.SetActive(true);
            for (int i = 0; i < transportButton.Length - 1; i++)
            {
                transportButton[i + 1].gameObject.SetActive(i < unit.boardedUnits.Count);
            }
        }
        else
        {
            HideTransportButtons();
        }
    }

    public void OnTransportSelectButtonClick(int index)
    {
        SelectTranportButtonClicked?.Invoke(index);
    }

    void HideTransportButtons()
    {
        //TODO:redo to disable rendering
        transportButton[0].transform.parent.gameObject.SetActive(false);
        for (int i = 0; i < transportButton.Length; i++)
        {
            transportButton[i].gameObject.SetActive(false);
        }
    }
}
