using Unity.VisualScripting;
using UnityEngine;

public class Chameleon : MonoBehaviour
{
    [Header("GroundLayer")]
    [SerializeField]
    LayerMask _groundLayer;
    RaycastHit2D _hitGround;

    float _speed;

    private void Start()
    {

    }


    private void Update()
    {
        _speed = Input.GetAxisRaw("Horizontal");
        transform.position += Vector3.right * _speed * 0.01f;

        Debug.DrawLine(transform.position, transform.position + Vector3.down);
        _hitGround = Physics2D.Linecast(transform.position, transform.position + Vector3.down, _groundLayer);

        if (_hitGround)
        {
            Debug.Log("a");
        }
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && _hitGround)
        {
            Debug.Log("ColorChange");
            Colors c = _hitGround.collider.GetComponent<Colors>();
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, c.a);
        }
    }

    /*SpriteRenderer sp;
    Rigidbody2D rigid2d;
    public float speed = 5;
    float move;
    float r, g, b, a;//オブジェクトの色を決めるパラメータ
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();//色を変えるもの
        rigid2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        rigid2d.linearVelocity = Vector2.right * move * speed;//横移動

        if (Input.GetMouseButtonDown(1))//右クリックで色を変える
        {
            sp.color = new Color(r, g, b, a);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        r = collision.gameObject.GetComponent<Colors>().r;//衝突オブジェクトの色パラメータを取得
        g = collision.gameObject.GetComponent<Colors>().g;
        b = collision.gameObject.GetComponent<Colors>().b;
        a = collision.gameObject.GetComponent<Colors>().a;
    }*/
}
