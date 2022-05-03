using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class ConfirmDialog : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private TMP_Text acceptText;
    [SerializeField] private TMP_Text cancelText;

    private UnityAction onAccept;
    private UnityAction onCancel;

    public void Show(UnityAction callOnAccept, string prompt)
    {
        Show(callOnAccept, () => { }, prompt);
    }

    public void Show(UnityAction callOnAccept, UnityAction callOnCancel, string prompt)
    {
        Show(callOnAccept, callOnCancel, prompt, "Accept", "Cancel");
    }

    public void Show(UnityAction callOnAccept, UnityAction callOnCancel, string prompt, string acceptText, string cancelText)
    {
        onAccept = callOnAccept;
        onCancel = callOnCancel;

        promptText.text = prompt;

        transform.parent.gameObject.SetActive(true);

        this.acceptText.text = acceptText;
        this.cancelText.text = cancelText;
    }

    public void Accept()
    {
        onAccept.Invoke();

        Hide();
    }

    public void Cancel()
    {
        if(onCancel != null)
        {
            onCancel.Invoke();
        }
        
        Hide();
    }

    public void Hide()
    {
        transform.parent.gameObject.SetActive(false);

        onAccept = null;
        onCancel = null;
    }
}
