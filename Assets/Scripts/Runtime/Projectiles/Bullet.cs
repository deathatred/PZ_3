using TMPro;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _flySpeed = 5f;

    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * _flySpeed;
    }
}
