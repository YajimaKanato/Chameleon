using UnityEditor.Rendering;
using UnityEngine;

public class Colors : MonoBehaviour
{
    SpriteRenderer sp;
    public float r, g, b, a;//�O����F��ς�����悤��
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sp= GetComponent<SpriteRenderer>();
        sp.color = new Color(r,g,b,a);//�F��ς���
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
