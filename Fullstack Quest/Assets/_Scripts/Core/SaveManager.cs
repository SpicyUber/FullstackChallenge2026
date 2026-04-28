using UnityEngine;

public class SaveManager : SingletonPersistent<SaveManager>
{
    public bool HasSave() => false;
}
