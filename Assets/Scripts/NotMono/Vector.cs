using UnityEngine;

public class Vector
{
    public Vector3 Multiple(Vector3 a, Vector3 b)//定義されてないVector3同士の掛け算
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    public Vector3 CoordinatetoOne(Vector3 a)//座標の絶対値をすべて１にする
    {
        return new Vector3(a.x / Mathf.Abs(a.x), a.y / Mathf.Abs(a.y), a.z / Mathf.Abs(a.z));
    }
}
