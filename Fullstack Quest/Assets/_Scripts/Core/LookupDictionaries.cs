using Shared.Enumerators;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LookupDictionaries : SingletonPersistent<LookupDictionaries>
{
    [SerializeField] private SoundEffectSO[] _sounds;
    [SerializeField] private ElementSO[] _elements;
    [SerializeField] private CharacterSpriteSO[] _characterSprites;

    private Dictionary<MoveSFXType, AudioClip> _soundDictionary;
    private Dictionary<Element, Color> _colorDictionary;
    private Dictionary<CharacterType, Sprite> _spriteDictionary;

    private void OnEnable()
    {
        InitializeDictionaries();
    }

    private void InitializeDictionaries()
    {
        _soundDictionary = new();
        _colorDictionary = new();
        _spriteDictionary = new();

        foreach(var sound in _sounds)
            _soundDictionary.Add(sound.SoundType,sound.Clip);

        foreach(var element in _elements)
            _colorDictionary.Add(element.Element, element.Color);

        foreach(var characterSprite in _characterSprites)
            _spriteDictionary.Add(characterSprite.CharacterType, characterSprite.Sprite); 
        
    }

    public AudioClip GetSound(MoveSFXType type) => _soundDictionary[type];
    public Color GetColor(Element element) => _colorDictionary[element];
    public Sprite GetCharacterSprite(CharacterType type) => _spriteDictionary[type];
}
