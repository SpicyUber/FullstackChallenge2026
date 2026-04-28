using UnityEngine;

public class MainMenuData
{   
    public MainMenuData(bool doesSaveExist)
    {
        DoesSaveExist = doesSaveExist;
    }

    public bool DoesSaveExist { get; private set; }
}
