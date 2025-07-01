using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chameleon : MonoBehaviour
{
    [Header("GroundLayer")]
    [SerializeField]
    LayerMask _groundLayer;
    RaycastHit2D _hitGround;

    Rigidbody2D _rb2d;
    CircleCollider2D _cc2d;
    EdgeCollider2D _ec2d;

    [Header("Status")]
    [SerializeField]
    float _move;
    [SerializeField]
    float _run;
    [SerializeField]
    float _jumpPower;

    float _speed;//�L�[���͂��擾
    float _baseMove;//�ړ��Ɏg��

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _cc2d = GetComponent<CircleCollider2D>();
        _ec2d = GetComponent<EdgeCollider2D>();
        _baseMove = _move;
    }


    private void Update()
    {
        //�_�b�V��
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            _baseMove = _run;
        }
        else
        {
            _baseMove = _move;
        }

        //���ړ�
        _speed = Input.GetAxisRaw("Horizontal");
        transform.position += Vector3.right * _speed * _baseMove;
        if (_speed != 0 && !_isAttacking)//�����]���i��L�΂����͕����]�����Ȃ��j
        {
            transform.localScale = new Vector3(_speed, 1, 1);
        }

        //Linecast
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f);
        _hitGround = Physics2D.Linecast(transform.position, transform.position + Vector3.down * 1.1f, _groundLayer);

        //�W�����v
        if (Input.GetKeyDown(KeyCode.Space)) _rb2d.AddForce(new Vector3(_speed, _jumpPower, 0), ForceMode2D.Impulse);

        //�F�ς�
        if (Input.GetKeyDown(KeyCode.C) && _hitGround.collider)
        {
            Debug.Log(_hitGround.collider.gameObject.layer);
            Debug.Log("ColorChange");
            Colors c = _hitGround.collider.GetComponent<Colors>();
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, c.a);
        }
        else if (Input.GetKeyDown(KeyCode.C) && !_hitGround.collider)
        {
            Debug.Log("ColorReset");
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }

        //�U��
        if (Input.GetMouseButtonDown(0) && !_isAttacking)
        {
            TongueAttack();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    //��U���Ɋւ���t�B�[���h
    [Header("TongueRange")]
    [Tooltip("��̒���")]
    [SerializeField]
    float _tongueRange;

    bool _isAttacking;//�U�������ǂ���

    const float _tongueRangeMaxTime = 0.3f;//�オ�L�т���܂ł̎���

    /// <summary>
    /// ��U�����s���֐�
    /// </summary>
    void TongueAttack()
    {
        _isAttacking = true;
        StartCoroutine(LengthenTongueCoroutine());
    }

    /// <summary>
    /// ���L�΂��ق��̃R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator LengthenTongueCoroutine()
    {
        Vector3 nowRange = Vector3.zero;
        while (true)
        {
            if (Vector3.Distance(Vector3.zero, nowRange) < _tongueRange)
            {
                nowRange += Vector3.right * _tongueRange * Time.deltaTime / _tongueRangeMaxTime;
                yield return null;
            }
            else if (Vector3.Distance(Vector3.zero, nowRange) >= _tongueRange)
            {
                StartCoroutine(ShortenTongueCoroutine(nowRange));
                yield break;
            }
            _cc2d.offset = nowRange;
            _ec2d.SetPoints(new List<Vector2>() { Vector3.zero, nowRange });
        }
    }

    /// <summary>
    /// ����k�߂�ق��̃R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator ShortenTongueCoroutine(Vector3 nowRange)
    {
        while (true)
        {
            if (nowRange.x - 0.0f > 0.0f)
            {
                nowRange -= Vector3.right * _tongueRange * Time.deltaTime / _tongueRangeMaxTime;
                yield return null;
            }
            else if (nowRange.x - 0.0f <= 0.0f)
            {
                _isAttacking = false;
                yield break;
            }
            _cc2d.offset = nowRange;
            _ec2d.SetPoints(new List<Vector2>() { Vector3.zero, nowRange });
        }
    }

    void FireCharge()
    {

    }

    void BubbleCharge()
    {

    }

    void ThunderCharge()
    {

    }
}
