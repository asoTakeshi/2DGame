using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("‰ÁŽZƒXƒRƒA")]
    public int myScore = 10;
    [Header("‘Ì—Í")]
    public int hp;
    public GameObject deathEffectPrefab;

    public void OnDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            GManager.instance.score += myScore;
            Instantiate(deathEffectPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {
        if (GManager.instance != null)
        {

        }    
    }
}
