using Shared.Enumerators;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundEffectSO", menuName = "Scriptable Objects/SoundEffectSO")]
public class SoundEffectSO : ScriptableObject
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private MoveSFXType _soundType;

    public AudioClip Clip => _clip;
    public MoveSFXType SoundType => _soundType;
}
