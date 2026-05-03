using System;
using TMPro;
using UnityEngine;


public class MessageBox : Menu<MessageBoxData>
{
    [SerializeField] TextMeshProUGUI _tmp;
    private string[] _messages;

    private Action _onClose;
    private int index = 0;

    public override void Load(MessageBoxData data)
    {
        index = 0;
        _messages = data.Messages;

        _onClose = data.OnClose;
        _tmp.SetText(_messages[0]);
    }

    public void Ok()
    {
        if(index + 1 >= _messages.Length)
        {
            _onClose?.Invoke();
            CloseSelf();
        }
        else
        {
            _tmp.SetText(_messages[++index]);
        }
    }
}
