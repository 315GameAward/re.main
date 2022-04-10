using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private int __healthpoits;

    private void Awake()
    {
        __healthpoits = 30;
    }

    public bool TakeHit()
    {
        __healthpoits -= 10;
        bool isDead = __healthpoits <= 0;
        if (isDead) __Die();
        return isDead;
    }

    private void __Die()
    {
        Destroy(gameObject);
    }

}
