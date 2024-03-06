using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class LobbyManager : MonoBehaviour
{
    #region # Variable
    [SerializeField] private TMP_InputField nameInputField;

    [SerializeField] private Button nameEnterButton;
    #endregion

    #region # Unity_Function
    private void Start()
    {
        nameEnterButton.onClick.AddListener(() => NetworkManager.SetNickName(nameInputField.text));
    }
    #endregion
}
