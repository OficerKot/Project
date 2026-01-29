using UnityEngine;

/// <summary>
/// Абстрактный класс для всех меню.
/// </summary>
public abstract class Menu : MonoBehaviour
{
    public abstract void Open();
    public abstract void Close();
}
