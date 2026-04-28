using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonClickDispatcher : MonoBehaviour
{
    private IClickHandler[] _handlers;

    private void Start() => _handlers = GetComponents<IClickHandler>().OrderBy(h=>h.Priority).ToArray();

    public void OnClick()
    {
        foreach(var h in _handlers)
        {
            h.Handle();
        }
    }
}
