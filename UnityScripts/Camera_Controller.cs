using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{

    GameObject jeep;
    GameObject wall;

    char impact_loc;

    // Start is called before the first frame update
    void Start()
    {
        init_Objects();
    }

    // Update is called once per frame
    void Update()
    {
        Update_View();
       // Debug.Log(impact_loc);
    }

    ///////////////////////////////////////////////////////////////////////
    void init_Objects()
    {
        jeep = GameObject.Find("jeep");
        wall = GameObject.Find("Wall");

        //impact_loc = 'f';
    }
    ///////////////////////////////////////////////////////////////////////
    void Update_View()
    {
        impact_loc = Simulate.impact_loc;
        if (impact_loc == 'f')
        {
            Vector3 pos = new Vector3(jeep.transform.position.x, jeep.transform.position.y, (float)(jeep.transform.position.z - 1.5));
            transform.position = pos;

            transform.LookAt(jeep.transform);

            pos = new Vector3(jeep.transform.position.x, (float)(jeep.transform.position.y + 0.5), (float)(jeep.transform.position.z - 1.5));
            transform.position = pos;

            wall.SetActive(true);
            //Debug.Log("Front");
        }

        else if (impact_loc == 'l')
        {
            Vector3 pos = new Vector3((float)(jeep.transform.position.x + 3.0), jeep.transform.position.y, (float)(jeep.transform.position.z + 0.0));
            transform.position = pos;
            transform.LookAt(jeep.transform);

            pos.y = (float)(pos.y + 1.0);
            transform.position = pos;

            wall.SetActive(false);
            //Debug.Log("Left");
        }

        else if (impact_loc == 'b')
        {
            Vector3 pos = new Vector3(jeep.transform.position.x, jeep.transform.position.y, (float)(jeep.transform.position.z + 1.5));
            transform.position = pos;

            transform.LookAt(jeep.transform);

            pos = new Vector3(jeep.transform.position.x, (float)(jeep.transform.position.y + 0.5), (float)(jeep.transform.position.z + 1.5));
            transform.position = pos;

            wall.SetActive(false);
            //Debug.Log("Back");
        }

        else if (impact_loc == 'r')
        {
            Vector3 pos = new Vector3((float)(jeep.transform.position.x - 3.0), jeep.transform.position.y, (float)(jeep.transform.position.z + 0.0));
            transform.position = pos;
            transform.LookAt(jeep.transform);

            pos.y = (float)(pos.y + 1.0);
            transform.position = pos;

            wall.SetActive(false);
            //Debug.Log("Right");
        }

    }
}
