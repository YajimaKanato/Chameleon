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
    int _attackSelect = 1;//攻撃切り替え
    int _nowLayer = 0;

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
        if (_speed != 0 && !_isTongueAttacking)//方向転換（舌伸ばし中は方向転換しない）
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
            Debug.Log("ColorChange");
            Colors c = _hitGround.collider.GetComponent<Colors>();
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, c.a);
            _nowLayer = 1 << _hitGround.collider.gameObject.layer;
        }
        else if (Input.GetKeyDown(KeyCode.C) && !_hitGround.collider)
        {
            Debug.Log("ColorReset");
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            _nowLayer = 0;
        }

        //攻撃切り替え
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            Debug.Log("攻撃切り替え：１");
            _attackSelect = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            Debug.Log("攻撃切り替え：２");
            _attackSelect = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            Debug.Log("攻撃切り替え：３");
            _attackSelect = 3;
        }

        //攻撃
        if (Input.GetMouseButtonDown(0))
        {
            ColorAttack();
        }
    }

    /// <summary>
    /// 色に応じた攻撃をする関数
    /// </summary>
    void ColorAttack()
    {
        if ((_nowLayer & 1 << LayerMask.NameToLayer("Red")) == 1 << LayerMask.NameToLayer("Red"))
        {
            Debug.Log("RedAttack");
            switch (_attackSelect)
            {
                case 1:
                    _tongueRange = 1;
                    TongueAttack();
                    break;
                case 2:
                    FireCharge();
                    break;
            }
        }
        else if ((_nowLayer & 1 << LayerMask.NameToLayer("Blue")) == 1 << LayerMask.NameToLayer("Blue"))
        {
            Debug.Log("BlueAttack");
            switch (_attackSelect)
            {
                case 1:
                    _tongueRange = 1;
                    TongueAttack();
                    break;
                case 2:
                    BubbleCharge();
                    break;
            }
        }
        else if ((_nowLayer & 1 << LayerMask.NameToLayer("Yellow")) == 1 << LayerMask.NameToLayer("Yellow"))
        {
            Debug.Log("YellowAttack");
            switch (_attackSelect)
            {
                case 1:
                    _tongueRange = 1;
                    TongueAttack();
                    break;
                case 2:
                    ThunderCharge();
                    break;
            }
        }
        else if ((_nowLayer & 1 << LayerMask.NameToLayer("Purple")) == 1 << LayerMask.NameToLayer("Purple"))
        {
            Debug.Log("PurpleAttack");
            switch (_attackSelect)
            {
                case 1:
                    _tongueRange = 3;
                    TongueAttack();
                    break;
                case 2:
                    FireCharge();
                    break;
                case 3:
                    BubbleCharge();
                    break;
            }
        }
        else if ((_nowLayer & 1 << LayerMask.NameToLayer("Green")) == 1 << LayerMask.NameToLayer("Green"))
        {
            Debug.Log("GreenAttack");
            switch (_attackSelect)
            {
                case 1:
                    _tongueRange = 3;
                    TongueAttack();
                    break;
                case 2:
                    BubbleCharge();
                    break;
                case 3:
                    ThunderCharge();
                    break;
            }
        }
        else if ((_nowLayer & 1 << LayerMask.NameToLayer("Orange")) == 1 << LayerMask.NameToLayer("Orange"))
        {
            Debug.Log("OrangeAttack");
            switch (_attackSelect)
            {
                case 1:
                    _tongueRange = 3;
                    TongueAttack();
                    break;
                case 2:
                    ThunderCharge();
                    break;
                case 3:
                    FireCharge();
                    break;
            }
        }
        else if (_nowLayer == 0)
        {
            Debug.Log("NormalAttack");
            switch (_attackSelect)
            {
                case 1:
                    _tongueRange = 1;
                    TongueAttack();
                    break;
            }
        }
    }

    //舌攻撃に関するフィールド
    [Header("TongueRange")]
    [Tooltip("舌の長さ")]
    [SerializeField]
    float _tongueRange;

    bool _isTongueAttacking = false;//攻撃中かどうか

    const float _tongueRangeMaxTime = 0.3f;//舌が伸びきるまでの時間

    /// <summary>
    /// 舌攻撃を行う関数
    /// </summary>
    void TongueAttack()
    {
        if (!_isTongueAttacking)
        {
            Debug.Log("舌伸ばし攻撃");
            StartCoroutine(LengthenTongueCoroutine());
        }
    }

    /// <summary>
    /// 舌を伸ばすほうのコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator LengthenTongueCoroutine()
    {
        _isTongueAttacking = true;
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
                _isTongueAttacking = false;
                Debug.Log("舌クールタイム終了");
                yield break;
            }
            _cc2d.offset = nowRange;
            _ec2d.SetPoints(new List<Vector2>() { Vector3.zero, nowRange });
        }
    }


    //炎攻撃
    [Header("FireCharge")]
    [SerializeField]
    GameObject _fireCharge;

    bool _isFireAttacking = false;

    /// <summary>
    /// 炎攻撃をする関数
    /// </summary>
    void FireCharge()
    {
        if (!_isFireAttacking)
        {
            Debug.Log("炎攻撃");
            Instantiate(_fireCharge, transform.position + Vector3.right * 0.3f * transform.localScale.x, Quaternion.identity);
            StartCoroutine(FireCoolTimeCoroutine());
        }
    }

    /// <summary>
    /// 属性攻撃のクールタイムに関するコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator FireCoolTimeCoroutine()
    {
        _isFireAttacking = true;
        yield return new WaitForSeconds(1.0f);
        _isFireAttacking = false;
        Debug.Log("炎クールタイム終了");
        yield break;
    }

    //水攻撃
    [Header("BubbleCharge")]
    [SerializeField]
    GameObject _bubbleCharge;

    bool _isBubbleAttacking = false;

    /// <summary>
    /// 水攻撃をする関数
    /// </summary>
    void BubbleCharge()
    {
        if (!_isBubbleAttacking)
        {
            Debug.Log("水攻撃");
            Instantiate(_bubbleCharge, transform.position + Vector3.right * 0.3f * transform.localScale.x, Quaternion.identity);
            StartCoroutine(BubbleCoolTimeCoroutine());
        }
    }

    /// <summary>
    /// 属性攻撃のクールタイムに関するコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator BubbleCoolTimeCoroutine()
    {
        _isBubbleAttacking = true;
        yield return new WaitForSeconds(1.0f);
        _isBubbleAttacking = false;
        Debug.Log("水クールタイム終了");
        yield break;
    }

    //雷攻撃
    [Header("ThunderCharge")]
    [SerializeField]
    GameObject _thunderCharge;

    bool _isThunderAttacking = false;

    //雷攻撃をする関数
    void ThunderCharge()
    {
        if (!_isThunderAttacking)
        {
            Debug.Log("雷攻撃");
            Instantiate(_thunderCharge, transform.position + Vector3.right * 0.3f * transform.localScale.x, Quaternion.identity);
            StartCoroutine(ThunderCoolTimeCoroutine());
        }
    }

    /// <summary>
    /// 属性攻撃のクールタイムに関するコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator ThunderCoolTimeCoroutine()
    {
        _isThunderAttacking = true;
        yield return new WaitForSeconds(1.0f);
        _isThunderAttacking = false;
        Debug.Log("雷クールタイム終了");
        yield break;
    }
}
