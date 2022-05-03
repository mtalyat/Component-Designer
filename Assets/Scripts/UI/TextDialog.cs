using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class TextDialog : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button doneButton;

    private UnityAction<string> onDone;

    private void Start()
    {
        doneButton.interactable = false;

        inputField.onValueChanged.AddListener((string s) =>
        {
            doneButton.interactable = !string.IsNullOrWhiteSpace(s);
        });
    }

    public void Show(UnityAction<string> callOnDone, string startingText = "")
    {
        onDone = callOnDone;

        inputField.text = startingText;

        transform.parent.gameObject.SetActive(true);
    }

    public void Done()
    {
        onDone.Invoke(inputField.text);

        Hide();
    }

    public void Hide()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
