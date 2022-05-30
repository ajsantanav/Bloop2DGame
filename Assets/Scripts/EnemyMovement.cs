using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private Vector3[] positions;
    [SerializeField] private int index;


    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, positions[index], Time.deltaTime * speed);

        if(transform.position == positions[index])
        {
            if(index == positions.Length -1)
            {
                index = 0;

            }
            else
            {
                index++;
            }
        }
    }
}
