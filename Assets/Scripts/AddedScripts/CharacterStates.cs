using UnityEngine;

/// <summary>
/// Скрипт, отвечающий за состояния, в которых находится персонаж игрока (тонет в луже, застрял в паутине)
/// Метод CheckState() проверяет, в каком именно состоянии оказался игрок, и запускает нужным метод: PutInPond() запускает анимацию с лужей, PutInWeb() - анимацию с паутиной (пока пустует), DefaultAnim() возвращает стандартную анимацию игрока.
/// Дополнительная настройка: Указать слои, которые будут изменять состояние игрока.
/// </summary>
public class CharacterStates : MonoBehaviour
{

    private Collider2D stopperCol;
    private SpriteAnimator SprAnim;
    bool crawledOut = true;
    public LayerMask StopperLayers;

    void Awake()
    {
        SprAnim = GetComponent<SpriteAnimator>();
    }
    public void CheckState()
    {
        //stopperCol = Physics2D.OverlapCircle(transform.position, .01f, StopperLayers);
        //if (stopperCol != null && crawledOut == true)
        //{
        //    if (stopperCol.gameObject.layer == LayerMask.NameToLayer("Pond"))
        //    {
        //        PutInPond();
        //        crawledOut = false;
        //    }
        //    else if (stopperCol.gameObject.layer == LayerMask.NameToLayer("Web"))
        //    {
        //        PutInWeb();
        //        crawledOut = false;
        //    }
        //}
        //else
        //{
        //    DefaultAnim();
        //    crawledOut = true;
        //}

    }

    public void PutInPond()
    {
        SprAnim.ForcePlay("InPond");
    }
    public void PutInWeb()
    {

    }
    public void DefaultAnim()
    {
        SprAnim.ForcePlay("Player");
    }
    
}
