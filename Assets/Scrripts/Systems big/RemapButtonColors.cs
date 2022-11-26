using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemapButtonColors : MonoBehaviour
{
    [SerializeField] Color _highlightColor;
    [SerializeField] Color _pressedColor;

    [ContextMenu("highlight")]
    public void HighlightButton()
    {
        var buttons = FindObjectsOfType<Button>(true);

        foreach (var button in buttons)
        {
            var colors = button.colors;
            colors.highlightedColor = _highlightColor;
            colors.pressedColor = _pressedColor;
            button.colors = colors;
        }
    }
}
