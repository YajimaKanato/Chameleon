using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("PlayerMask")]
    [SerializeField]
    LayerMask _playerMask;
    RaycastHit2D _hitPlayer;

    [Header("HP")]
    [SerializeField]
    float _hp;

    [Header("Target Range"), Tooltip("ターゲット検知の範囲")]
    [SerializeField]
    float _targetRange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Linecast
        Debug.DrawLine(transform.position, transform.position + Vector3.right * _targetRange);
        _hitPlayer = Physics2D.Linecast(transform.position, transform.position + Vector3.right * _targetRange, _playerMask);

        //移動

        //死亡判定
        if (_hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    float DamageCalculation(string tag)
    {
        float damage = 0;
        if (tag == "FireCharge")
        {
            damage = Charge.Damage;

        }
        else if (tag == "BubbleCharge")
        {
            damage = Charge.Damage;

        }
        else if (tag == "ThunderCharge")
        {
            damage = Charge.Damage;

        }
        return damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _hp -= DamageCalculation(collision.gameObject.tag);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
