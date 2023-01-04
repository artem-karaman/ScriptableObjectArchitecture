using UnityEngine;

public enum MenuType
{
    MAIN_MENU,
    PAUSE_MENU
}

[CreateAssetMenu(fileName = "NewMenu", menuName = "Scene Data/Menu")]
public class MenuSO : GameSceneSO
{
    [Header("Menu specific")]
    public MenuType menuType;
}