using System;
using UnityEngine;

public class MessageBoxData
{
    public string[] Messages { get; private set; }
    public Action OnClose { get; private set; }

    public MessageBoxData(string[] messages, Action onClose)
    {
        Messages = messages;
        OnClose = onClose;
    }

}
