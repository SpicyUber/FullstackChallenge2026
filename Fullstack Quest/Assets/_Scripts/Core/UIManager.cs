using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonPersistent<UIManager>
{
    [Header("Menu SOs")]
    [SerializeField]
    private MenuSO[] _menuSOs;

    [Header("Attach Menus To")]
    [SerializeField]
    private Transform _transform;

    private Dictionary<Type, MenuBase> _menuDictionary;

    private void OnEnable() => LoadMenus();

    private void LoadMenus()
    {
        _menuDictionary = new Dictionary<Type, MenuBase>();

        foreach(MenuSO so in _menuSOs)
        {
            so.LoadMenuIntoDictionary(_menuDictionary, attachToParent: _transform);
        }
    }

    public void CloseMenu(Type menuDataType) => CloseMenu(_menuDictionary[menuDataType]);

    public void OpenMenu<TData>(TData data) => OpenMenu(_menuDictionary[typeof(TData)], data);


    private void CloseMenu(MenuBase menu)
    {
        if(menu && menu.gameObject.activeInHierarchy)
        {
            menu.gameObject.SetActive(false);

        }
    }

    private void OpenMenu<TData>(MenuBase menu, TData data)
    {
        if(!menu) return;
        menu.gameObject.SetActive(true);
        menu.Load(data);
    }

}
