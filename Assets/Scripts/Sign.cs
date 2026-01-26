using System.Collections;
using UnityEngine;

public class Sign : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private GameObject HolePrefab;
    private RaycastHit2D[] dominos;
    bool RayCrossed = false;

    private void Awake()
    {
        SignsManager.Instance.PutInList(this);
    }

    public void CastRay()
    {
        if (Physics2D.Raycast(transform.position, direction, 5f, LayerMask.GetMask("Player")))
        {
            RayCrossed = true;
            Debug.Log("Player found!");
            return;
        }
        else if (Physics2D.Raycast(transform.position, direction, 5f, LayerMask.GetMask("DominoBase")))
        {
            dominos = Physics2D.RaycastAll(transform.position, direction, 5f, LayerMask.GetMask("DominoBase"));
            Debug.Log($"Ray touched {dominos.Length} dominos");
        }
        if (RayCrossed)
        {
            StartCoroutine(SpawnHoles());
            this.enabled = false;
        }
    }

    IEnumerator SpawnHoles()
    {
        int i = 0;
        Transform holetf = this.transform;
        foreach (RaycastHit2D domino in dominos)
        {
            Destroy(domino.transform.gameObject);
            holetf.transform.position += direction;
            GameObject hole = Instantiate(HolePrefab, holetf);
            ++i;
            yield return new WaitForSeconds(0.5f);
        }
        if (i < 5)
        {
            holetf.transform.position += direction;
            GameObject hole = Instantiate(HolePrefab, holetf);
            ++i;
            yield return new WaitForSeconds(0.5f);
        }
        else StopAllCoroutines();
    }
}
