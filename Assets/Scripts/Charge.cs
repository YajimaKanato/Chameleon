using UnityEngine;

public class Charge : MonoBehaviour
{
    Rigidbody2D _rb2d;

    //与えるダメージ量
    static float _damage;
    public static float Damage { get { return _damage; } set { _damage = value; } }

    [Header("ShotPower"), Tooltip("射出速度")]
    [SerializeField]
    float _shotPower;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _rb2d.AddForce(Vector3.right * GameObject.Find("Chameleon").transform.localScale.x * _shotPower, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Destroy(gameObject);
    }
}
