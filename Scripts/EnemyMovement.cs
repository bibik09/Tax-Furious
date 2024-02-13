using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] float speed;

    public float horizontal;
    public float vertical;

    private void FixedUpdate()
    {
        enemy.transform.Translate(speed * Time.deltaTime*horizontal, speed * Time.deltaTime*vertical, 0);
    }
}
