using UnityEngine;

public class HoverHighlight : MonoBehaviour
{
    private SpriteRenderer _sr;
    private Color defaultColor;
    public bool active;
    //
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        defaultColor = _sr.color;
    }
    private void OnMouseOver()
    {
        if (active)
        _sr.color = Color.green;
    }

    private void OnMouseExit()
    {
        if (active)
        _sr.color = defaultColor;
    }

    public void SetActiveValue(bool value)
    {
        active = value;
        if (!value)
        _sr.color = defaultColor;
    }
}
