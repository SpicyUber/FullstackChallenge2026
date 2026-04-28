using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "MenuSO", menuName = "Scriptable Objects/MenuSO")]
public class MenuSO : ScriptableObject
{
    [SerializeField] private GameObject _menuPrefab;

    public void LoadMenuIntoDictionary(Dictionary<Type, MenuBase> dictionary, Transform attachToParent)
    {

        var menuComponent = LoadMenu(attachToParent);
        menuComponent.gameObject.SetActive(false);

        dictionary.Add(menuComponent.DataType, menuComponent);
    }

    public MenuBase LoadMenu(Transform attachToParent)
    {
        GameObject menuInstance = Instantiate(_menuPrefab, attachToParent);
        return menuInstance.GetComponent<MenuBase>();
    }
}
