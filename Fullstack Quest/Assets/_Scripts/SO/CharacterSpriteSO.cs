using Shared.Enumerators;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSpriteSO", menuName = "Scriptable Objects/CharacterSpriteSO")]
public class CharacterSpriteSO : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private CharacterType _characterType;

    public Sprite Sprite => _sprite;
    public CharacterType CharacterType => _characterType;

}
