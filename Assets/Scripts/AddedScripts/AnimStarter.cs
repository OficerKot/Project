using UnityEngine;

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
