using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Image img;
    private Color originalColor;
    private Color hoverColor;

    private void Awake()
    {
        img = GetComponent<Image>();
        originalColor = img.color;
        hoverColor = new Color(img.color.r, img.color.g, img.color.b, 0.25f);
    }

    public void OnClick()
    {
        SceneChanger.Instance.OnStartGame();
    }

    public void OnPointerEnter()
    {
        img.color = hoverColor;
    }

    public void OnPointerExit()
    {
        img.color = originalColor;
    }
}
