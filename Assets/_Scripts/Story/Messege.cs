using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Messege
{
    [SerializeField] private TMP_Text _storyText;
    [SerializeField] private GameObject _messagePanel;

    public void Show(string storyText)
    {
        _messagePanel.SetActive(true);
        _storyText.text = storyText;
    }

    public void Hide()
    {
        _messagePanel.SetActive(false);
        _storyText.text = "";
    }

}
