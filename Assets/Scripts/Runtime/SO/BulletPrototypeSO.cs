using UnityEngine;

[CreateAssetMenu(fileName = ("Bullet prototype"))]
public class BulletPrototypeSO : ScriptableObject
{
    public float Speed = 10f;
    public int Damage = 1;
    public Color Color = Color.white;
}
