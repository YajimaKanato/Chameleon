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

    float _speed;//キー入力を取得
    float _baseMove;//移動に使う

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _cc2d = GetComponent<CircleCollider2D>();
        _ec2d = GetComponent<EdgeCollider2D>();
        _baseMove = _move;
    }


    private void Update()
    {
        //ダッシュ
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            _baseMove = _run;
        }
        else
        {
            _baseMove = _move;
        }

        //横移動
        _speed = Input.GetAxisRaw("Horizontal");
        transform.position += Vector3.right * _speed * _baseMove;
        if (_speed != 0 && !_isAttacking)//方向転換（舌伸ばし中は方向転換しない）
        {
            transform.localScale = new Vector3(_speed, 1, 1);
        }

        //Linecast
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f);
        _hitGround = Physics2D.Linecast(transform.position, transform.position + Vector3.down * 1.1f, _groundLayer);

        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space)) _rb2d.AddForce(new Vector3(_speed, _jumpPower, 0), ForceMode2D.Impulse);

        //色変え
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

        //攻撃
        if (Input.GetMouseButtonDown(0) && !_isAttacking)
        {
            TongueAttack();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    //舌攻撃に関するフィールド
    [Header("TongueRange")]
    [Tooltip("舌の長さ")]
    [SerializeField]
    float _tongueRange;

    bool _isAttacking;//攻撃中かどうか

    const float _tongueRangeMaxTime = 0.3f;//舌が伸びきるまでの時間

    /// <summary>
    /// 舌攻撃を行う関数
    /// </summary>
    void TongueAttack()
    {
        _isAttacking = true;
        StartCoroutine(LengthenTongueCoroutine());
    }

    /// <summary>
    /// 舌を伸ばすほうのコルーチン
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
    /// 舌を縮めるほうのコルーチン
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
