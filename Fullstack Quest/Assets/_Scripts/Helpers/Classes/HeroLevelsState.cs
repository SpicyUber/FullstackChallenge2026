using Newtonsoft.Json;
using UnityEngine;

public class HeroLevelsState
{
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public int Magic { get; private set; }
    public int Health{ get; private set; }
    public int Mana { get; private set; }

    [JsonConstructor]
    public HeroLevelsState(int attack, int defense, int magic, int health, int mana)
    {
        Attack = attack;
        Defense = defense;
        Magic = magic;
        Health = health;
        Mana = mana;
    }
}
