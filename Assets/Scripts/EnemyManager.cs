using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("‘Ì—Í")]
    public int hp;
    public GameObject deathEffectPrefab;

    public void OnDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Instantiate(deathEffectPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
