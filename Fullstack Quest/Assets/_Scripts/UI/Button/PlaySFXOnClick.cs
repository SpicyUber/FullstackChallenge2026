using UnityEngine;

public class PlaySFXOnClick : MonoBehaviour, IClickHandler
{
    [SerializeField] private AudioClip _sfx;

    public int Priority => 0;

    public void Handle() => AudioSource.PlayClipAtPoint(_sfx, Vector3.zero);
}
