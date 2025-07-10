using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public enum Element
    {
        Red,
        Blue,
        Yellow,
        Purple,
        Green,
        Orange,
        White
    }

    [Header("Element")]
    [SerializeField]
    Element _element;

    [Header("PlayerElement")]
    [SerializeField]
    PlayerElement _playerElement;

    [Header("Player")]
    [SerializeField]
    GameObject _player;

    DamageCalculation _damageCalc;

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
        _damageCalc = new DamageCalculation();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Charge")
        {
            Charge charge = collision.gameObject.GetComponent<Charge>();
            _hp -= _damageCalc.DamageCalc(charge._element.ToString(), _element.ToString(), _playerElement._elementData[_player.GetComponent<Chameleon>()._element]._chargePower);
        }
        else if (collision.gameObject.tag == "Player")
        {
            _hp -= _damageCalc.DamageCalc(_playerElement._elementData[_player.GetComponent<Chameleon>()._element]._element.ToString(), _element.ToString(), _playerElement._elementData[_player.GetComponent<Chameleon>()._element]._tonguePower);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
