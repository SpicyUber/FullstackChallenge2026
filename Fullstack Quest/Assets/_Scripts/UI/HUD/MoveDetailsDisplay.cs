using Shared.DataTransferObjects;
using Shared.Enumerators;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class MoveDetailsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tmp;

    public void Display(MoveDto move)
    {
        string moveInfo = "";

        string damageStat = (move.DamageType == DamageType.PHYSICAL) ? "Attack" : "Magic";

        if(move.DamageType != DamageType.NONE)
            moveInfo += $">Deals {move.DamageScaling}% of user's " +
            $"{damageStat} as <color=orange>damage</color>.\n";

        if(move.SelfHealingScaling > 0)
            moveInfo += $"><color=green>Heals</color> {move.SelfHealingScaling}% of user's " +
                $"{damageStat}.\n";

        if(move.Effect != null)
            moveInfo += $">{(!move.IsVFXAndEffectSelfCast ? "Inflicts" : "Self inflicts")} effect <color=purple>{move.Effect.Type.ToString().Replace("_", " ")}</color>." +
                $"\nAmount: {((move.Effect.IsDebuff) ? -1 : 1) * move.Effect.Amount} " +
                $"\nDuration: {move.Effect.Duration}\n";

        moveInfo += $"><color=#66B2FF>Mana</color> cost: {move.ManaCost}\n><color=red>Health</color> cost: {move.HealthCost}";

        _tmp.SetText(moveInfo);
    }

    public void Clear() => _tmp.SetText("");
}
