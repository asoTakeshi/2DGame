using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController2 : MonoBehaviour
{
    private bool isGameOver;
    public float knockbackPower;              // �G�ƐڐG�����ۂɐ�����΂�����
    public int coinPoint;                       // �R�C�����l������Ƒ�����|�C���g�̑���
    //public UIManager uiManager;
    public GameObject bulletPrefab;
    public Transform shotPoint;
    float coolTime = 0.3f;                       //�ҋ@����
    float leftCoolTime;�@�@�@�@�@�@�@�@�@�@ // �ҋ@���Ă��鎞��
    bool isRight;
    [SerializeField]
    //private StartChecker StartChecker;

    //[SerializeField]
    private AudioClip knockbackSE;                    // �G�ƐڐG�����ۂɖ炷SE�p�̃I�[�f�B�I�t�@�C�����A�T�C������

    [SerializeField]
    private GameObject knockbackEffectPrefab;         // �G�ƐڐG�����ۂɐ�������G�t�F�N�g�p�̃v���t�@�u�̃Q�[���I�u�W�F�N�g���A�T�C������

    [SerializeField]
    private AudioClip coinSE;                    // �R�C���ɐڐG�����ۂɖ炷SE�p�̃I�[�f�B�I�t�@�C�����A�T�C������

    [SerializeField]
    private GameObject coinEffectPrefab; �@�@�@�@�@�@//�R�C���ƐڐG�����ۂɐ�������G�t�F�N�g�p�̃v���t�@�u�̃Q�[���I�u�W�F�N�g���A�T�C������

    void Update()
    {
        Shot();
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
    void FixedUpdate()
    {
        if (isGameOver == true)
        {
            return;
        }
        // �ړ�
        //Move();

    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        // �ڐG�����R���C�_�[�����Q�[���I�u�W�F�N�g��Tag��Enemy�Ȃ� 
        if (col.gameObject.tag == "Enemy")
        {
            // �L�����ƓG�̈ʒu���狗���ƕ������v�Z
            Vector3 direction = (transform.position - col.transform.position).normalized;

            // �G�̔��Α��ɃL�����𐁂���΂�
            transform.position += direction * knockbackPower;

            // �G�Ƃ̐ڐG�p��SE(AudioClip)���Đ�����
            AudioSource.PlayClipAtPoint(knockbackSE, transform.position);

            // �ڐG�����ۂ̃G�t�F�N�g���A�G�̈ʒu�ɁA�N���[���Ƃ��Đ�������B�������ꂽ�Q�[���I�u�W�F�N�g��ϐ��֑��
            GameObject knockbackEffect = Instantiate(knockbackEffectPrefab, col.transform.position, Quaternion.identity);

            // �G�t�F�N�g�� 0.5 �b��ɔj��
            Destroy(knockbackEffect, 0.5f);
        }
    }




    /// <summary>
    /// �o���[���j��
    /// </summary>

    public void DestroyBallon()
    {
        // TODO ����A�o���[�����j�󂳂��ۂɁu���ꂽ�v�悤�Ɍ�����A�j�����o��ǉ�����
        //if (ballons[1] != null)
        //{
        //    Destroy(ballons[1]);
        //}
       //else if (ballons[0] != null)
        //{
            //Destroy(ballons[0]);
        //}
    }
    ////* �V�������\�b�h���P�ǉ��@�������� *////

    // IsTrigger���I���̃R���C�_�[�����Q�[���I�u�W�F�N�g��ʉ߂����ꍇ�ɌĂяo����郁�\�b�h
    private void OnTriggerEnter2D(Collider2D col)
    {
        // �ʉ߂����R���C�_�[�����Q�[���I�u�W�F�N�g�� Tag �� Coin �̏ꍇ
        if (col.gameObject.tag == "Coin")
        {
            // �ʉ߂����R�C���̃Q�[���I�u�W�F�N�g�̎��� Coin �X�N���v�g���擾���Apoint �ϐ��̒l���L�����̎��� coinPoint �ϐ��ɉ��Z
            coinPoint += col.gameObject.GetComponent<Coin>().point;

            //uiManager.UpdateDisplayScore(coinPoint);
            // �ʉ߂����R�C���̃Q�[���I�u�W�F�N�g��j�󂷂�
            Destroy(col.gameObject);

            //�R�C���Ƃ̐ڐG�p��SE(AudioClip)���Đ�����
            AudioSource.PlayClipAtPoint(coinSE, transform.position);

            // �ڐG�����ۂ̃G�t�F�N�g���A�R�C���̈ʒu�ɁA�N���[���Ƃ��Đ�������B�������ꂽ�Q�[���I�u�W�F�N�g��ϐ��֑��
            GameObject coinEffect = Instantiate(coinEffectPrefab, transform.position, Quaternion.identity);

            // �G�t�F�N�g�� 0.3 �b��ɔj��
            Destroy(coinEffect, 0.3f);

        }
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
}

