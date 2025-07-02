using UnityEngine;

public class Charge : MonoBehaviour
{
    Rigidbody2D _rb2d;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _rb2d.AddForce(Vector3.right * GameObject.Find("Chameleon").transform.localScale.x * 5f, ForceMode2D.Impulse);
    }
}
