using Shared.DataTransferObjects;
using Shared.Enumerators;
using UnityEngine;

public class Monster : BaseCharacter
{
    public override int Attack => CharacterInfo.Attack + GetEffectValue(EffectType.MODIFY_ATTACK);

    public override int Defense => CharacterInfo.Defense + GetEffectValue(EffectType.MODIFY_DEFENSE);

    public override int Magic => CharacterInfo.Magic + GetEffectValue(EffectType.MODIFY_MAGIC);

    public override int Health => CharacterInfo.Health;

    public override int Mana => CharacterInfo.Mana;

    protected override void InitializeSpecific(CharacterDto characterDto)
    {
        _spriteRenderer.flipX = false;
    }

    protected override void OnDeath() => BattleEvents.InvokeMonsterDied();

    protected override void OnDisableSpecific()
    {
        GameEvents.EncounterMonsterSelected -= Initialize;
        BattleEvents.MonsterAttacked += TakeDamageAndApplyEffects;
    }

    protected override void OnEnableSpecific()
    {
        GameEvents.EncounterMonsterSelected += Initialize;
        BattleEvents.MonsterAttacked -= TakeDamageAndApplyEffects;
    }
}
