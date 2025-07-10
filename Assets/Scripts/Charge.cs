using UnityEngine;

public class Charge : MonoBehaviour
{
    public enum Element
    {
        Red,
        Blue,
        Yellow
    }

    [Header("Element")]
    public Element _element;

    Rigidbody2D _rb2d;

    [Header("ShotPower"), Tooltip("éÀèoë¨ìx")]
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
