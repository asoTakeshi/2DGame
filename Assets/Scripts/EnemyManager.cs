using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("加算スコア")]
    public int myScore = 10;
    [Header("体力")]
    public int hp;
    public GameObject deathEffectPrefab;
    public float speed = 3.0f;             //移動速度
    public string directino = "left";      //向きright or left
    public float range = 0.0f;             //動く回る範囲
    Vector3 defPos;                        //初期位置
    Rigidbody2D rb;

    void Start()
    {
        // 速度を更新
        //Rigidbodyを取得
        rb = GetComponent<Rigidbody2D>();
        if (directino == "right")
        {
            transform.localScale = new Vector2(-1, 1);  //向きの変更
        }
        //初期位置
        defPos = transform.position;

    }

    void Update()
    {
        if (range > 0.0f)
        {
            if (transform.position.x < defPos.x - (range / 2))
            {
                directino = "right";
                transform.localScale = new Vector2(-1, 1);  //向きの変更
            }
            if (transform.position.x > defPos.x + (range / 2))
            {
                directino = "left";
                transform.localScale = new Vector2(1, 1);  //向きの変更
            }
        }
    }
    public void OnDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            //GameManager.instance.score += myScore;
            Instantiate(deathEffectPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {
        if (directino == "right")
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        //if (GManager.instance != null)
        //{

        //}    
    }

    /// <summary>
    /// 接触
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (directino == "right")
        {
            directino = "left";
            transform.localScale = new Vector2(1, 1);   //向きの変更
        }
        else
        {
            directino = "right";
            transform.localScale = new Vector2(-1, 1);   //向きの変更
        }
    }

}
