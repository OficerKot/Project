using UnityEngine;


/// <summary>
/// Скрипт-стартер для анимации объекта на канвасе.
/// Устанавливается на Image-объект, состояние enabled которого в процессе работы игры планируется переключать.
///Дополнительная настройка не требуется. Установить скрипт на объект с компонентом ImageAnimator и не трогать.
/// </summary>
public class AnimStarter : MonoBehaviour
{
    [SerializeField] ImageAnimator imageAnimator;
    private string anim;

    void Awake()
    {
        imageAnimator = this.GetComponent<ImageAnimator>();
        anim = imageAnimator.playAnimationOnStart;
    }

    void OnEnable()
    {
        imageAnimator.ForcePlay(anim);
    }
}
