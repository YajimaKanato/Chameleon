using Unity.VisualScripting;
using UnityEngine;

public class Chameleon : MonoBehaviour
{
    [Header("GroundLayer")]
    [SerializeField]
    LayerMask _groundLayer;
    RaycastHit2D _hitGround;

    Rigidbody2D _rb2d;

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

        //Linecast
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f);
        _hitGround = Physics2D.Linecast(transform.position, transform.position + Vector3.down * 1.1f, _groundLayer);

        //�W�����v
        if (Input.GetKeyDown(KeyCode.Space)) _rb2d.AddForce(new Vector3(_speed, _jumpPower, 0),ForceMode2D.Impulse);

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
        
    }

    void ChangeAttack(int layer)
    {

    }

    /*SpriteRenderer sp;
    Rigidbody2D rigid2d;
    public float speed = 5;
    float move;
    float r, g, b, a;//�I�u�W�F�N�g�̐F�����߂�p�����[�^
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();//�F��ς������
        rigid2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        rigid2d.linearVelocity = Vector2.right * move * speed;//���ړ�

        if (Input.GetMouseButtonDown(1))//�E�N���b�N�ŐF��ς���
        {
            sp.color = new Color(r, g, b, a);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        r = collision.gameObject.GetComponent<Colors>().r;//�Փ˃I�u�W�F�N�g�̐F�p�����[�^���擾
        g = collision.gameObject.GetComponent<Colors>().g;
        b = collision.gameObject.GetComponent<Colors>().b;
        a = collision.gameObject.GetComponent<Colors>().a;
    }*/
}
