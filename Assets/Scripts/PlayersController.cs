using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersController : MonoBehaviour
{
    Rigidbody2D rb;                  //Rigidbody2D�ϐ�
    float axisH = 0.0f;              //����
    public float speed = 3.0f;      // �ړ����x  
    public float jump = 9.0f;       //�W�����v��
    public LayerMask groundLayer;   //���n�o���郌�C���[
    bool goJump = false;            //�W�����v�J�n�t���O
    bool onGround = false;          //�n�ʂɗ����Ă���t���O
    public GameObject bulletPrefab;
    public Transform shotPoint;
    float coolTime = 0.3f;                       //�ҋ@����
    float leftCoolTime;�@�@�@�@�@�@�@�@�@�@ // �ҋ@���Ă��鎞��
    private string enemyTag = "Enemy";
    [SerializeField]
    private GameObject knockbackEffectPrefab;         // �G�ƐڐG�����ۂɐ�������G�t�F�N�g�p�̃v���t�@�u�̃Q�[���I�u�W�F�N�g���A�T�C������

    //[SerializeField]
    //private AudioClip coinSE;                    // �R�C���ɐڐG�����ۂɖ炷SE�p�̃I�[�f�B�I�t�@�C�����A�T�C������

    //[SerializeField]
    //private GameObject coinEffectPrefab; �@�@�@�@�@�@//�R�C���ƐڐG�����ۂɐ�������G�t�F�N�g�p�̃v���t�@�u�̃Q�[���I�u�W�F�N�g���A�T�C������
    public GameObject impactPrefab;
    [Header("�U����")]
    public int at;

    //�A�j���[�V�����Ή�
    Animator animator;     //�A�j���[�^�[
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    //public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    //public string jumpDownAnime = "PlayerJumpFall";
    string nowAnime = "";
    string oldAnime = "";
    public static string gameStste = "playing";   //�Q�[���̏��
    public int score = 0;            // �X�R�A
    bool isMoving = false;           //�^�b�`�X�N���[���Ή��ǉ�

    void Start()
    {
        //Rigidbody2D�擾
        rb = GetComponent<Rigidbody2D>();
        //Animator���擾
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;
        gameStste = "playing";   //�Q�[�����ɂ���
    }

    void Update()
    {
        Shot();
        if (gameStste != "playing")
        {
            return;
        };
        //�ړ�
        if (isMoving == false)
        {
            //���������̓��͂��`�F�b�N����
            axisH = Input.GetAxisRaw("Horizontal");
        }
        //�����̒���
        if (axisH > 0.0f)
        {
            //�E�ړ�
            //Debug.Log("�E�ړ�");
            transform.localScale = new Vector3(3, 3 ,3);
        }
        else if (axisH < 0.0f)
        {
            //���ړ�
            //Debug.Log("���ړ�");
            transform.localScale = new Vector3(-3, 3, 3);     //���E���]
        }
        //�L�����N�^�[���W�����v������
        if (Input.GetButtonDown("Jump"))
        {
            Jump();  //�W�����v
        }
        

    }

    private void FixedUpdate()
    {
        if (gameStste != "playing")
        {
            return;
        }
        //�n�㔻��
        onGround = Physics2D.Linecast(transform.position,
                                      transform.position - (transform.up * 0.1f),
                                      groundLayer);
        if (onGround || axisH != 0)
        {
            //�n�ʂ̏�or���x���O�łȂ�
            //���x��ύX����
            rb.velocity = new Vector2(speed * axisH, rb.velocity.y);
        }
        if (onGround && goJump)
        {
            //�n�ʂ̏�ŃW�����v�L�[�������ꂽ
            //�W�����v������
            //Debug.Log("�W�����v�I");
            Vector2 jumpPw = new Vector2(0, jump);    //�W�����v������x�N�g�������
            rb.AddForce(jumpPw, ForceMode2D.Impulse);   //�u�ԓI�ȗ͂�������
            goJump = false;                           //�W�����v�t���O�����낷
        }
        if (onGround)
        {
            //�n�ʂ̏�
            if (axisH == 0)
            {
                nowAnime = stopAnime;    //��~��
            }
            else
            {
                nowAnime = moveAnime;    //�ړ�
            }
        }
        else
        {
            //��
            nowAnime = jumpAnime;
        }
        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime);      //�A�j���[�V�����Đ�
        }
    }
    /// <summary>
    /// �W�����v
    /// </summary>
    public void Jump()
    {
        goJump = true;      //�W�����v�t���O�𗧂Ă�
        //Debug.Log("�W�����v�{�^����������");
    }

    /// <summary>
    /// �ڐG
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Goal")
        {
            //Debug.Log("�S�[��");
            Goal();   //�S�[��
        }
        else if (col.gameObject.tag == "Dead")
        {
            //Debug.Log("GameOver");
            GameOver();    //�Q�[���I�[�o�[
        }
        else if (col.gameObject.tag == "Enemy")
        {
            //Debug.Log("GameOver");
            GameOver();    //�Q�[���I�[�o�[
        }
        else if (col.gameObject.tag == "ScoreItem")
        {
            //�X�R�A�A�C�e��
            //ItenData���擾
            ItemData item = col.gameObject.GetComponent<ItemData>();
            //�X�R�A���擾
            score = item.value;

            //�A�C�e���폜
            Destroy(col.gameObject);
        }
    }

    /// <summary>
    /// �S�[��
    /// </summary>
    public void Goal()
    {
        Debug.Log("�S�[��");
        //animator.Play(goalAnime);
        gameStste = "gameclear";
        GameStop();       //�Q�[����~
    }

    /// <summary>
    /// �Q�[���I�[�o�[
    /// </summary>
    public void GameOver()
    {
        //Debug.Log("GameOver");
        animator.Play(deadAnime);
        gameStste = "gameover";
        Debug.Log("GameOver");
        GameStop();       //�Q�[����~
        //===================
        //�Q�[���I�[�o�[���o
        //===================
        //�v���C���[�����������
        GetComponent<CapsuleCollider2D>().enabled = false;
        //�v���C���[����ɏ������ˏグ�鉉�o
        rb.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }
    /// <summary>
    /// �Q�[����~
    /// </summary>

    void GameStop()
    {
        //Rigidbody2D�擾
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        //���x��0�ɂ��ċ�����~
        rb.velocity = new Vector2(0, 0);
    }

    /// <summary>
    /// �^�b�`�X�N���[���Ή��ǉ�
    /// </summary>
    /// <param name="h"></param>
    /// <param name="v"></param>
    public void SetAxis(float h, float v)
    {
        axisH = h;
        if (axisH == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }
    void Shot()
    {
        leftCoolTime -= Time.deltaTime;
        if (leftCoolTime <= 0)
        {
            
            if (Input.GetButtonDown("Attack"))
            {
                
                GameObject bullet = Instantiate(bulletPrefab, shotPoint.position, transform.rotation);

                bullet.GetComponent<BulletManeger>().Shot(transform.localScale.x / 3);
                leftCoolTime = coolTime;
            }
        }
        
    }
    //private void OnCollisionEnter2D(Collision2D col)
    //{
    //    //�G�ɓ���������
    //    if (col.gameObject.tag == "Enemy")
    //    {

    //        //�_���[�W��^����
    //        EnemyManager enemy = col.gameObject.GetComponent<EnemyManager>();
    //        enemy.OnDamage(at);

    //        GameObject effect = Instantiate(impactPrefab, transform.position, transform.rotation);

    //        Destroy(effect, 1.0f);
    //        //�j��
    //        //Destroy(col.gameObject);

    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }

    //}

}
