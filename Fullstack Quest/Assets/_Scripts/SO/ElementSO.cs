using Shared.Enumerators;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementSO", menuName = "Scriptable Objects/ElementSO")]
public class ElementSO : ScriptableObject
{
    [SerializeField] private Color _color;
    [SerializeField] private Element _element;

    public Color Color => _color;
    public Element Element => _element;

}
