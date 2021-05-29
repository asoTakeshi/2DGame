using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    public GameObject impactPrefab;
    float span = 1.5f;
    float delta = 0;
    public float speed = 10f;  //スピードの代入
    Rigidbody2D rb;
    [Header("攻撃力")]
    public int at;
    public GameObject bulletPrefab;
    public Transform shotPoints;

    void Update()
    {
        Shot();
    }

    void Shot()
    {
        this.delta += Time.deltaTime;
        if (this.delta > this.span)
        {
            this.delta = 0;
            GameObject bullet = Instantiate(bulletPrefab, shotPoints.position, transform.rotation);
            //bullet.GetComponent<BulletManeger>().Shot(transform.localScale.x - 4);

        }
    }
}
