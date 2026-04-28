using UnityEngine;

public class SingletonPersistent<T> : MonoBehaviour where T : SingletonPersistent<T>
{
    public static T Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)this;
            DontDestroyOnLoad(Instance.gameObject);
        }
        else Destroy(this.gameObject);
    }

}
