using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    int collided;

    // Start is called before the first frame update
    void Start()
    {
        collided = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.transform.name == "Wall" && Simulate.impact_choice == 'f') || (collision.transform.name == "jeep" && Simulate.impact_choice != 'f'))
        {
            collided = 1;
            Simulate.collided = 1;
        }
    }
}
