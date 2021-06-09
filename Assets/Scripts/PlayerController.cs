using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private string horizontal = "Horizontal";    // �L�[���͗p�̕�����w��(InputManager �� Horizontal �̓��͂𔻒肷�邽�߂̕�����)
    private string jump = "Jump";�@�@�@�@�@�@�@�@// �L�[���͗p�̕�����w��
    private Rigidbody2D rb;                      // �R���|�[�l���g�̎擾�p
    private Animator anim;
    private float limitPosX = 100.5f;           // �������̐����l
    private float limitPosY = 11.45f;          // �c�����̐����l
    private float scale;                         // �����̐ݒ�ɗ��p����
    public float moveSpeed;                      // �ړ����x
    public float jumpPower;                      // �W�����v�E���V��
    public bool isGrounded;
    bool gojump = false;
    private bool isGameOver = false;                    // GameOver��Ԃ̔���p�Btrue �Ȃ�Q�[���I�[�o�[�B
    public float knockbackPower;              // �G�ƐڐG�����ۂɐ�����΂�����
    public int gemPoint;                       // �R�C�����l������Ƒ�����|�C���g�̑���
    public GameObject bulletPrefab;
    public Transform shotPoint;
    float coolTime = 0.3f;                       //�ҋ@����
    float leftCoolTime;�@�@�@�@�@�@�@�@�@�@ // �ҋ@���Ă��鎞��
    bool isRight;
    public int coinPoint;
    public GManager gManager;
    private string enemyTag = "Enemy";
    private bool isDown = false;
    private string deadAreaTag = "DeadArea";
    public UIManager uiManager;
    private bool isJump = false;
    private bool isOtherJump = false;
    private bool isRun = false; 
    private bool nonDownAnim = false;
    private bool isClearMotion = false;
    //[Header("�W�����v���鎞�ɖ炷SE")] public AudioClip jumpSE; 
    //[Header("���ꂽ�炷SE")] public AudioClip downSE; 
    //[Header("�R���e�B�j���[���ɖ炷SE")] public AudioClip continueSE;  







    [SerializeField, Header("Linecast�p �n�ʔ��背�C���[")]
    private LayerMask groundLayer;
    //[SerializeField]
    // private AudioClip knockbackSE;                    // �G�ƐڐG�����ۂɖ炷SE�p�̃I�[�f�B�I�t�@�C�����A�T�C������

    [SerializeField]
    private GameObject knockbackEffectPrefab;         // �G�ƐڐG�����ۂɐ�������G�t�F�N�g�p�̃v���t�@�u�̃Q�[���I�u�W�F�N�g���A�T�C������

    //[SerializeField]
    //private AudioClip coinSE;                    // �R�C���ɐڐG�����ۂɖ炷SE�p�̃I�[�f�B�I�t�@�C�����A�T�C������

    [SerializeField]
    private GameObject coinEffectPrefab; �@�@�@�@�@�@//�R�C���ƐڐG�����ۂɐ�������G�t�F�N�g�p�̃v���t�@�u�̃Q�[���I�u�W�F�N�g���A�T�C������

    void Start()
    {
        // �K�v�ȃR���|�[�l���g���擾���ėp�ӂ����ϐ��ɑ��
        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        scale = transform.localScale.x;

    }
    void Update()
    {
        Shot();
        // �n�ʐڒn  Physics2D.Linecast���\�b�h�����s���āAGround Layer�ƃL�����̃R���C�_�[�Ƃ��ڒn���Ă��鋗�����ǂ������m�F���A�ڒn���Ă���Ȃ� true�A�ڒn���Ă��Ȃ��Ȃ� false ��߂�
        isGrounded = Physics2D.Linecast(transform.position + transform.up * 0.4f, transform.position - transform.up * 0.9f, groundLayer);

        // Scene�r���[�� Physics2D.Linecast���\�b�h��Line��\������
        Debug.DrawLine(transform.position + transform.up * 0.4f, transform.position - transform.up * 0.9f, Color.red, 1.0f);
        //if (ballons.Length > 0)
        //{


        // �W�����v
        if (Input.GetButtonDown(jump) && this.rb.velocity.y == 0)
        {
            Jump();
        }

        if (isGrounded == true)
        {
            anim.ResetTrigger("Fall");

        }
        // �ڒn���Ă��Ȃ�(�󒆂ɂ���)�ԂŁA�������̏ꍇ
        if (isGrounded == false && rb.velocity.y < 0.15f)
        {

            //if(anim.GetCurrentAnimatorStateInfo(0).IsName ("Jump_Up"))
            //{

            //}

            // �����A�j�����J��Ԃ�
            anim.SetTrigger("Fall");
        }

    }



    //���ꂽ���̏��� 
    private void ReceiveDamage(bool downAnim)
    {
        if (isDown)
        {
            return;
        }
        else
        {
            if (downAnim)
            {
                anim.Play("Damage_Down");
            }
            else
            {
                nonDownAnim = true;
            }
            isDown = true;
            //GManager.instance.PlaySE(downSE);  
            GManager.instance.SubLifeNum();
        }
    }
    void Shot()
    {
        leftCoolTime -= Time.deltaTime;
        if (leftCoolTime <= 0)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject bullet = Instantiate(bulletPrefab, shotPoint.position, transform.rotation);

                bullet.GetComponent<BulletManeger>().Shot(transform.localScale.x / 3);
                leftCoolTime = coolTime;
            }
        }
    }

    /// <summary>
    /// �W�����v�Ƌ󒆕��V
    /// </summary>
    private void Jump()
    {


        // �L�����̈ʒu��������ֈړ�������(�W�����v�E���V)
        rb.AddForce(transform.up * jumpPower);

        // Jump(Up + Mid) �A�j���[�V�������Đ�����
        anim.SetTrigger("Jump");
    }

    void FixedUpdate()
    {
        if (!isDown && !GManager.instance.isGameOver && !GManager.instance.isStageClear)
        {

        }
        else
        {
            if (!isClearMotion && GManager.instance.isStageClear)
            {
                anim.Play("player_clear");
                isClearMotion = true;
            }
           
        }

        // �ړ�
        Move();
    }

    /// <summary>
    /// �ړ�
    /// </summary>
    private void Move()
    {

        // ����(��)�����ւ̓��͎�t
        float x = Input.GetAxis(horizontal);�@�@�@// InputManager �� Horizontal �ɓo�^����Ă���L�[�̓��͂����邩�ǂ����m�F���s��

        // x �̒l�� 0 �ł͂Ȃ��ꍇ = �L�[���͂�����ꍇ
        if (x != 0)
        {

            // velocity(���x)�ɐV�����l�������Ĉړ�
            rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);

            // temp �ϐ��Ɍ��݂� localScale �l����
            Vector3 temp = transform.localScale;

            // ���݂̃L�[���͒l x �� temp.x �ɑ��
            temp.x = x;

            // �������ς��Ƃ��ɏ����ɂȂ�ƃL�������k��Ō����Ă��܂��̂Ő����l�ɂ���            
            if (temp.x > 0)
            {

                //  ������0�����傫����΂��ׂ�1�ɂ���
                temp.x = scale;

            }
            else
            {
                //  ������0������������΂��ׂ�-1�ɂ���
                temp.x = -scale;
            }

            // �L�����̌������ړ������ɍ��킹��
            transform.localScale = temp;
            // �ҋ@��Ԃ̃A�j���̍Đ����~�߂āA����A�j���̍Đ��ւ̑J�ڂ��s��
            anim.SetBool("Idle", false);
            anim.SetFloat("Run", 0.5f);
        }
        else
        {
            //  ���E�̓��͂��Ȃ������牡�ړ��̑��x��0�ɂ��ăs�^�b�Ǝ~�܂�悤�ɂ���
            rb.velocity = new Vector2(0, rb.velocity.y);
            //  ����A�j���̍Đ����~�߂āA�ҋ@��Ԃ̃A�j���̍Đ��ւ̑J�ڂ��s��
            anim.SetFloat("Run", 0.0f);     // ���@�ǉ�  Run �A�j���[�V�����ɑ΂��āA0.f �̒l�����Ƃ��ēn���B�J�ڏ����� less 0.1 �Ȃ̂ŁA0.1 �ȉ��̒l��n���Ə�������������Run �A�j���[�V��������~�����
            anim.SetBool("Idle", true);     // ���@�ǉ��@Idle �A�j���[�V������ true �ɂ��āA�ҋ@�A�j���[�V�������Đ�����S
        }
        // ���݂̈ʒu��񂪈ړ��͈͂̐����͈͂𒴂��Ă��Ȃ����m�F����B�����Ă�����A�����͈͓��Ɏ��߂�
        float posX = Mathf.Clamp(transform.position.x, -limitPosX, limitPosX);
        float posY = Mathf.Clamp(transform.position.y, -limitPosY, limitPosY);

        // ���݂̈ʒu���X�V(�����͈͂𒴂����ꍇ�A�����ňړ��͈̔͂𐧌�����)
        transform.position = new Vector2(posX, posY);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        // �ڐG�����R���C�_�[�����Q�[���I�u�W�F�N�g��Tag��Enemy�Ȃ� 
        if (col.gameObject.tag == "Enemy")
        {
            //anim.Play("Damage_Down");
            //isDown = true;
            ReceiveDamage(true);
        }

    }

    /// <summary>
    /// �R���e�B�j���[�ҋ@��Ԃ�    
    /// </summary>    
    /// <returns></returns>  

    public bool IsContinueWaiting()
    {
        if (GManager.instance.isGameOver)  
        {
            return false;
        }
        else
        {
            return IsDownAnimEnd() || nonDownAnim;  
        }
        
    }

    private bool IsDownAnimEnd()
    {
        //Debug.Log(isDown);
        if (isDown && anim != null)
        {
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
            if (currentState.IsName("Damage_Down"))
            {
                //Debug.Log("�ʉ�");
                if (currentState.normalizedTime >= 1)
                {
                    Debug.Log("�ʉ�");
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// �Q�[���I�[�o�[
    /// </summary>
    public void GameOver()
    {
        isGameOver = true;

        // Console �r���[�� isGameOver �ϐ��̒l��\������B���������s������ true �ƕ\�������
        Debug.Log(isGameOver);

        // ��ʂɃQ�[���I�[�o�[�\�����s��
        //uiManager.DisplayGameOverInfo();
    }
    /// <summary>
    /// �R���e�B�j���[����
    /// </summary>
    public void ContinuePlayer()
    {
        //GManager.instance.PlaySE(continueSE);  //New! 
        isDown = false;
        anim.Play("Cindy_Idle");
        isJump = false;
        isOtherJump = false;
        isRun = false;
        nonDownAnim = false;    
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == deadAreaTag)
        {
            ReceiveDamage(false);
        }
    }
}
