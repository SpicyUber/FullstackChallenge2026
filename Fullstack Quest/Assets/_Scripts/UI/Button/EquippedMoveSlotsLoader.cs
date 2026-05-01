using UnityEngine;

public class EquippedMoveSlotsLoader : MonoBehaviour
{
    [SerializeField]
    private EquippedMoveSlotDisplay[] _slotDisplays;

    public void Load(Hero hero) 
    {
        for(int i = 0; i < Hero.MoveCount; i++)
            _slotDisplays[i].Initialize(hero,hero.Moves[i],i);
    
    }
}
