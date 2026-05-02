using Newtonsoft.Json;
using UnityEngine;

public class SaveManager : SingletonPersistent<SaveManager>
{
    private const string SaveKey = "Save";

    public bool HasSave => PlayerPrefs.HasKey(SaveKey);

    public void Delete() => PlayerPrefs.DeleteKey(SaveKey);

    public void Save(GameState state)
    {
        PlayerPrefs.SetString(SaveKey, JsonConvert.SerializeObject(state));
        PlayerPrefs.Save();
    }

    public GameState Load() => JsonConvert.DeserializeObject<GameState>(PlayerPrefs.GetString(SaveKey));

}
