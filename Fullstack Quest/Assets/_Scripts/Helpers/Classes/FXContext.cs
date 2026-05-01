using Shared.Enumerators;
using UnityEngine;

public class FXContext
{
    public MoveSFXType SFXType { get; private set; }
    public MoveVFXType VFXType { get; private set; }

    public Element Element { get; private set; }
    public bool IsSelfCast { get; private set; }

    public bool FlipSprite { get; private set; }
    public Vector3 CastPosition { get; private set; }

    public FXContext(MoveSFXType sFXType, MoveVFXType vFXType, Element element, bool isSelfCast, bool flipSprite, Vector3 castPosition)
    {
        SFXType = sFXType;
        VFXType = vFXType;

        Element = element;
        IsSelfCast = isSelfCast;

        FlipSprite = flipSprite;
        CastPosition = castPosition;
    }
}
