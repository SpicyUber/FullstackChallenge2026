using UnityEngine;

public interface IHUDElement
{
    public void Initialize(BaseCharacter hero, BaseCharacter monster);
    public void Refresh();
}
