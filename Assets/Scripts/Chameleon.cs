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
    int _attackSelect = 1;//�U���؂�ւ�
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
        if (_speed != 0 && !_isTongueAttacking)//�����]���i��L�΂����͕����]�����Ȃ��j
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

        //�U���؂�ւ�
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            Debug.Log("�U���؂�ւ��F�P");
            _attackSelect = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            Debug.Log("�U���؂�ւ��F�Q");
            _attackSelect = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            Debug.Log("�U���؂�ւ��F�R");
            _attackSelect = 3;
        }

        //�U��
        if (Input.GetMouseButtonDown(0))
        {
            ColorAttack();
        }
    }

    /// <summary>
    /// �F�ɉ������U��������֐�
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

    //��U���Ɋւ���t�B�[���h
    [Header("TongueRange")]
    [Tooltip("��̒���")]
    [SerializeField]
    float _tongueRange;

    bool _isTongueAttacking = false;//�U�������ǂ���

    const float _tongueRangeMaxTime = 0.3f;//�オ�L�т���܂ł̎���

    /// <summary>
    /// ��U�����s���֐�
    /// </summary>
    void TongueAttack()
    {
        if (!_isTongueAttacking)
        {
            Debug.Log("��L�΂��U��");
            StartCoroutine(LengthenTongueCoroutine());
        }
    }

    /// <summary>
    /// ���L�΂��ق��̃R���[�`��
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
                _isTongueAttacking = false;
                Debug.Log("��N�[���^�C���I��");
                yield break;
            }
            _cc2d.offset = nowRange;
            _ec2d.SetPoints(new List<Vector2>() { Vector3.zero, nowRange });
        }
    }


    //���U��
    [Header("FireCharge")]
    [SerializeField]
    GameObject _fireCharge;

    bool _isFireAttacking = false;

    /// <summary>
    /// ���U��������֐�
    /// </summary>
    void FireCharge()
    {
        if (!_isFireAttacking)
        {
            Debug.Log("���U��");
            Instantiate(_fireCharge, transform.position + Vector3.right * 0.3f * transform.localScale.x, Quaternion.identity);
            StartCoroutine(FireCoolTimeCoroutine());
        }
    }

    /// <summary>
    /// �����U���̃N�[���^�C���Ɋւ���R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator FireCoolTimeCoroutine()
    {
        _isFireAttacking = true;
        yield return new WaitForSeconds(1.0f);
        _isFireAttacking = false;
        Debug.Log("���N�[���^�C���I��");
        yield break;
    }

    //���U��
    [Header("BubbleCharge")]
    [SerializeField]
    GameObject _bubbleCharge;

    bool _isBubbleAttacking = false;

    /// <summary>
    /// ���U��������֐�
    /// </summary>
    void BubbleCharge()
    {
        if (!_isBubbleAttacking)
        {
            Debug.Log("���U��");
            Instantiate(_bubbleCharge, transform.position + Vector3.right * 0.3f * transform.localScale.x, Quaternion.identity);
            StartCoroutine(BubbleCoolTimeCoroutine());
        }
    }

    /// <summary>
    /// �����U���̃N�[���^�C���Ɋւ���R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator BubbleCoolTimeCoroutine()
    {
        _isBubbleAttacking = true;
        yield return new WaitForSeconds(1.0f);
        _isBubbleAttacking = false;
        Debug.Log("���N�[���^�C���I��");
        yield break;
    }

    //���U��
    [Header("ThunderCharge")]
    [SerializeField]
    GameObject _thunderCharge;

    bool _isThunderAttacking = false;

    //���U��������֐�
    void ThunderCharge()
    {
        if (!_isThunderAttacking)
        {
            Debug.Log("���U��");
            Instantiate(_thunderCharge, transform.position + Vector3.right * 0.3f * transform.localScale.x, Quaternion.identity);
            StartCoroutine(ThunderCoolTimeCoroutine());
        }
    }

    /// <summary>
    /// �����U���̃N�[���^�C���Ɋւ���R���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator ThunderCoolTimeCoroutine()
    {
        _isThunderAttacking = true;
        yield return new WaitForSeconds(1.0f);
        _isThunderAttacking = false;
        Debug.Log("���N�[���^�C���I��");
        yield break;
    }
}
