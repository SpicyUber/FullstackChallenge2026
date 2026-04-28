using UnityEditor;
using UnityEngine;

public class CloseMenuOnClick : MonoBehaviour, IClickHandler
{
    [SerializeField] private MenuBase _menu;

    public int Priority => 99;

    public void Handle() => UIManager.Instance.CloseMenu(_menu.DataType);
}
