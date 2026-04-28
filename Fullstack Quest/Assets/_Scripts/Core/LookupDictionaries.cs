using Shared.Enumerators;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LookupDictionaries : SingletonPersistent<LookupDictionaries>
{
    [SerializeField] private SoundEffectSO[] _sounds;
    [SerializeField] private ElementSO[] _elements;
    [SerializeField] private CharacterSpriteSO[] _characterSprites;
    [SerializeField] private BuffSpriteSO[] _buffSprites;

    private Dictionary<MoveSFXType, AudioClip> _soundDictionary;
    private Dictionary<Element, Color> _colorDictionary;
    private Dictionary<CharacterType, Sprite> _characterSpriteDictionary;
    private Dictionary<EffectType, BuffSpriteSO> _effectSpriteDictionary;

    private void OnEnable()
    {
        InitializeDictionaries();
    }

    private void InitializeDictionaries()
    {
        _soundDictionary = new();
        _colorDictionary = new();
        _characterSpriteDictionary = new();
        _effectSpriteDictionary = new();

        foreach(var sound in _sounds)
            _soundDictionary.Add(sound.SoundType,sound.Clip);

        foreach(var element in _elements)
            _colorDictionary.Add(element.Element, element.Color);

        foreach(var characterSprite in _characterSprites)
            _characterSpriteDictionary.Add(characterSprite.CharacterType, characterSprite.Sprite);

        foreach(var buffSprite in _buffSprites)
            _effectSpriteDictionary.Add(buffSprite.Type, buffSprite);
        
    }

    public AudioClip GetSound(MoveSFXType type) => _soundDictionary[type];
    public Color GetColor(Element element) => _colorDictionary[element];
    public Sprite GetCharacterSprite(CharacterType type) => _characterSpriteDictionary[type];
    public Sprite GetBuffSprite(EffectType type, bool isDebuff) => _effectSpriteDictionary[type].GetBuffSprite(isDebuff);
}
