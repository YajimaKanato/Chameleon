using UnityEditor.Rendering;
using UnityEngine;

public class Colors : MonoBehaviour
{
    SpriteRenderer sp;
    public float r, g, b, a;//ŠO‚©‚çF‚ğ•Ï‚¦‚ç‚ê‚é‚æ‚¤‚É
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sp= GetComponent<SpriteRenderer>();
        sp.color = new Color(r,g,b,a);//F‚ğ•Ï‚¦‚é
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
