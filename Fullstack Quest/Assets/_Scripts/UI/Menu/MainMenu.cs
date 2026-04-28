using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu<MainMenuData>
{
    [SerializeField] private Button _loadButton;
    
    public override void Load(MainMenuData data)
    {
        _loadButton.interactable = data.DoesSaveExist;
    }
}
