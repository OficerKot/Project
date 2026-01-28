using UnityEngine;

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
        stopperCol = Physics2D.OverlapCircle(transform.position, .01f, StopperLayers);
        if (stopperCol != null && crawledOut == true)
        {
            if (stopperCol.gameObject.layer == LayerMask.NameToLayer("Pond"))
            {
                PutInPond();
                crawledOut = false;
            }
            else if (stopperCol.gameObject.layer == LayerMask.NameToLayer("Web"))
            {
                PutInWeb();
                crawledOut = false;
            }
        }
        else
        {
            DefaultAnim();
            crawledOut = true;
        }

    }

    void PutInPond()
    {
        SprAnim.ForcePlay("InPond");
    }
    void PutInWeb()
    {

    }
    void DefaultAnim()
    {
        SprAnim.ForcePlay("Player");
    }
    
}
