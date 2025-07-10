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
    Vector _vector;//����N���X

    [Header("Muzzle"), Tooltip("�e�̎ˏo��")]
    [SerializeField]
    Transform _muzzle;

    [Header("Status(ScriptableObject)")]
    [SerializeField]
    PlayerElement _playerElement;

    //Status
    public int _element;
    float _chargePower;
    float _tonguePower;
    [Header("StatusCheck")]
    [SerializeField] float _walk = 0.2f;
    [SerializeField] float _run = 0.5f;
    [SerializeField] float _jump = 5.0f;
    float _tongueRange;

    float _speed;//�L�[���͂��擾
    int _attackSelect = 1;//�U���؂�ւ�
    int _nowLayer = 6;

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _cc2d = GetComponent<CircleCollider2D>();
        _ec2d = GetComponent<EdgeCollider2D>();
        _vector = new Vector();
        StatusSetting();
    }


    private void Update()
    {
        //�_�b�V��
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            _speed = Input.GetAxisRaw("Horizontal");
            transform.position += Vector3.right * _speed * _run;
            if (_speed != 0 && !_isTongueAttacking)//�����]���i��L�΂����͕����]�����Ȃ��j
            {
                transform.localScale = new Vector3(_speed, 1, 1);
            }
        }
        else
        {
            _speed = Input.GetAxisRaw("Horizontal");
            transform.position += Vector3.right * _speed * _walk;
            if (_speed != 0 && !_isTongueAttacking)//�����]���i��L�΂����͕����]�����Ȃ��j
            {
                transform.localScale = new Vector3(_speed, 1, 1);
            }
        }

        //���ړ�


        //Linecast
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f);
        _hitGround = Physics2D.Linecast(transform.position, transform.position + Vector3.down * 1.1f, _groundLayer);

        //�W�����v
        if (Input.GetKeyDown(KeyCode.Space) && _hitGround)
        {
            _rb2d.AddForce(new Vector3(_speed, _jump, 0), ForceMode2D.Impulse);
        }

        //�F�ς�
        if (Input.GetKeyDown(KeyCode.C) && _hitGround.collider)
        {
            Debug.Log("ColorChange");
            Color c = _hitGround.collider.GetComponent<SpriteRenderer>().color;
            this.gameObject.GetComponent<SpriteRenderer>().color = c;
            _nowLayer = _hitGround.collider.gameObject.layer - LayerMask.NameToLayer("Red");//�C���f�b�N�X����
            StatusSetting();
        }
        else if (Input.GetKeyDown(KeyCode.C) && !_hitGround.collider)
        {
            Debug.Log("ColorReset");
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            _nowLayer = 6;
            StatusSetting();
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

    void StatusSetting()
    {
        _element = (int)_playerElement._elementData[_nowLayer]._element;
        _chargePower = _playerElement._elementData[_nowLayer]._chargePower;
        _tonguePower = _playerElement._elementData[_nowLayer]._tonguePower;
        _tongueRange = _playerElement._elementData[_nowLayer]._tongueRange;
    }

    /// <summary>
    /// �F�ɉ������U��������֐�
    /// </summary>
    void ColorAttack()
    {
        Debug.Log(_element);
        if (_element == 0)
        {
            Debug.Log("RedAttack");
            switch (_attackSelect)
            {
                case 1:
                    TongueAttack();
                    break;
                case 2:
                    FireCharge();
                    break;
            }
        }
        else if (_element == 1)
        {
            Debug.Log("BlueAttack");
            switch (_attackSelect)
            {
                case 1:
                    TongueAttack();
                    break;
                case 2:
                    BubbleCharge();
                    break;
            }
        }
        else if (_element == 2)
        {
            Debug.Log("YellowAttack");
            switch (_attackSelect)
            {
                case 1:
                    TongueAttack();
                    break;
                case 2:
                    ThunderCharge();
                    break;
            }
        }
        else if (_element == 3)
        {
            Debug.Log("PurpleAttack");
            switch (_attackSelect)
            {
                case 1:
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
        else if (_element == 4)
        {
            Debug.Log("GreenAttack");
            switch (_attackSelect)
            {
                case 1:
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
        else if (_element == 5)
        {
            Debug.Log("OrangeAttack");
            switch (_attackSelect)
            {
                case 1:
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
        else if (_element == 6)
        {
            Debug.Log("NormalAttack");
            switch (_attackSelect)
            {
                case 1:
                    TongueAttack();
                    break;
            }
        }
    }

    //��U���Ɋւ���t�B�[���h
    Vector3 _tongueVector;
    Vector3 _mousePoint;

    bool _isTongueAttacking = false;//�U�������ǂ���
    const float _tongueRangeMaxTime = 0.3f;//�オ�L�т���܂ł̎���

    /// <summary>
    /// ��U�����s���֐�
    /// </summary>
    void TongueAttack()
    {
        if (!_isTongueAttacking)
        {
            //�}�E�X�̍��W���擾
            _mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _mousePoint.z = 0;

            _tongueVector = _vector.Multiple((_mousePoint - transform.position).normalized, transform.localScale);
            if ((_mousePoint.x - transform.position.x) * transform.localScale.x <= 0)//��̉��͈͂𐧌�
            {
                _tongueVector = new Vector3(0, 1, 0);
            }
            else if (_mousePoint.y - transform.position.y <= -1 / 2f)
            {
                _tongueVector = new Vector3(Mathf.Sqrt(3) / 2f, -1 / 2f, 0);
            }
            Debug.Log(_tongueVector);
            //_tongueVector = _vector.Multiple((_mousePoint - transform.position).normalized, transform.localScale);

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
                nowRange += _tongueVector * _tongueRange * Time.deltaTime / _tongueRangeMaxTime;
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
            if (Vector3.Distance(nowRange, Vector3.zero) > 0.0f)
            {
                nowRange -= _tongueVector * _tongueRange * Time.deltaTime / _tongueRangeMaxTime;
                if (nowRange.x * _tongueVector.x <= 0 && nowRange.y * _tongueVector.y <= 0)//�k�݂�������
                {
                    nowRange = Vector3.zero;
                }
                yield return null;
            }
            else if (Vector3.Distance(nowRange, Vector3.zero) <= 0.0f)
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
            Instantiate(_fireCharge, _muzzle.transform.position, Quaternion.identity);
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
            Instantiate(_bubbleCharge, _muzzle.transform.position, Quaternion.identity);
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
            Instantiate(_thunderCharge, _muzzle.transform.position, Quaternion.identity);
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
