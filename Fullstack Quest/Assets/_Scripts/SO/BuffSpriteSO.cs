using Shared.Enumerators;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffSpriteSO", menuName = "Scriptable Objects/BuffSpriteSO")]
public class BuffSpriteSO : ScriptableObject
{
    [SerializeField] private Sprite _buffSprite, _debuffSprite;
    [SerializeField] private EffectType _type;

    public EffectType Type => _type;
    public Sprite GetBuffSprite(bool isDebuff) => isDebuff ? _debuffSprite : _buffSprite;
}
