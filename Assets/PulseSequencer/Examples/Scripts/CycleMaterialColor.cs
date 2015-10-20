using UnityEngine;
using System.Collections;

public class CycleMaterialColor : MonoBehaviour
{
    [SerializeField] private Color[] _colors;

    [SerializeField] private Renderer _renderer;

    private int _currentColorIdx;

    private void OnEnable()
    {
        _currentColorIdx = 0;
        CycleColor();
    }

    private void OnMouseUpAsButton()
    {
        CycleColor();
    }

    private void CycleColor()
    {
        if (_colors == null || _colors.Length == 0 || _renderer == null)
        {
            return;
        }

        _renderer.material.color = _colors[_currentColorIdx];
        _currentColorIdx = (_currentColorIdx + 1) % _colors.Length;
    }
}
