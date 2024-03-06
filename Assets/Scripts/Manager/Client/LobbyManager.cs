using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class LobbyManager : MonoBehaviour
{
    #region # Variable
    [Header("Rect Transform")]
    [SerializeField] private RectTransform UIPreset;

    [Header("TextMeshPro UGUI")]
    [SerializeField] private TextMeshProUGUI PlayerListText;

    [Header("TMP_InputField")]
    [SerializeField] private TMP_InputField nameInputField;

    [Header("Button")]
    [SerializeField] private Button nameEnterButton;
    #endregion

    #region # Unity_Function
    private void Start()
    {
        nameEnterButton.onClick.AddListener(() =>
        {
            NetworkManager.SetNickName(nameInputField.text);
            UIPreset.DOLocalMoveX(-1920, 0.5f).SetEase(Ease.OutQuad);
        });
    }
    private void Update()
    {

    }
    #endregion
}
