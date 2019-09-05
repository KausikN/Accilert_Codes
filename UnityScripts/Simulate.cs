using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Simulate : MonoBehaviour
{
    GameObject[] panel_objs;
    GameObject prob_panel;
    GameObject reset;
    GameObject acc;

    GameObject jeep;
    GameObject jeep_opp;
    GameObject wall;

    GameObject sim;
    GameObject imp_loc_button;

    GameObject s_dist;
    GameObject s_vel;

    public float max_dist;
    public float max_vel;

    float dist;
    float vel;

    public float virtual_by_real;

    int t_temp;
    int frame;

    int started;
    int jeep_opp_started;

    public static int collided;

    float accident_prob;

    public static char impact_loc;
    public static char impact_choice;

    float impact_vel;

    // Start is called before the first frame update
    void Start()
    {
        init_Objects();
    }

    // Update is called once per frame
    void Update()
    {
        if (collided == 0)
        {
            if(impact_choice == 'f') impact_vel = jeep.GetComponent<Rigidbody>().velocity.z;
            else if (impact_choice == 'b') impact_vel = jeep_opp.GetComponent<Rigidbody>().velocity.z;
            else if (impact_choice == 'l') impact_vel = jeep_opp.GetComponent<Rigidbody>().velocity.x;
            else if (impact_choice == 'r') impact_vel = -1 * jeep_opp.GetComponent<Rigidbody>().velocity.x;
        } 
        Reset_Check();
        UI_Interactions();
    }

    ///////////////////////////////////////////////////////////////////////
    void init_Objects()
    {
        panel_objs = GameObject.FindGameObjectsWithTag("Panel");
        prob_panel = GameObject.Find("Prob_Panel");
        reset = GameObject.FindGameObjectWithTag("Reset");
        prob_panel.SetActive(false);

        jeep = GameObject.Find("jeep");
        jeep_opp = GameObject.Find("jeep_opponent");
        wall = GameObject.Find("Wall");

        sim = GameObject.Find("Sim");

        imp_loc_button = GameObject.Find("Imp_Loc");

        s_dist = GameObject.Find("Slider_Dist");

        s_vel = GameObject.Find("Slider_Vel");

        virtual_by_real = 2;    //Based on Jeep length - 5m which is 10 units here

        max_dist = 100;

        max_vel = 100;

        collided = 0;

        started = 0;

        jeep_opp_started = 0;

        impact_loc = 'f'; // Can be f | b | l | r

        impact_choice = 'f';

        impact_vel = 0;

        t_temp = 1;
        frame = 0;
    }
    ///////////////////////////////////////////////////////////////////////



    ///////////////////////////////////////////////////////////////////////
    void UI_Interactions()
    {
        t_temp = 1;
        sim.GetComponent<Button>().onClick.AddListener(Simulator);

        t_temp = 1;
        imp_loc_button.GetComponent<Button>().onClick.AddListener(Impact_Location);

        t_temp = 1;
        reset.GetComponent<Button>().onClick.AddListener(Reset_Scene);

        Dist_Update(jeep_opp_started);
    }

    void Simulator()
    {
        if (t_temp == 1)
        {
            impact_choice = impact_loc;
            GetSliderValues();

            Canvas_Hide();
            Dist_Update(jeep_opp_started);
            Vector3 v;
            if (impact_choice == 'f')
            {
                v = new Vector3(0, 0, vel);
                Vel(jeep, v);
                started = 1;
            }
            else if (impact_choice == 'b')
            {
                v = new Vector3(0, 0, vel);
                Vel(jeep_opp, v);
                jeep_opp_started = 1;
            }
            else if (impact_choice == 'l')
            {
                v = new Vector3(vel, 0, 0);
                Vel(jeep_opp, v);
                jeep_opp_started = 1;
            }
            else if (impact_choice == 'r')
            {
                v = new Vector3(-vel, 0, 0);
                Vel(jeep_opp, v);
                jeep_opp_started = 1;
            }

            t_temp = 0;
        }

    }
    ///////////////////////////////////////////////////////////////////////


    ///////////////////////////////////////////////////////////////////////
    float Real_to_Virtual_ValueConverter(float val)
    {
        val = val * virtual_by_real;
        return val;
    }

    void GetSliderValues()
    {
        dist = Real_to_Virtual_ValueConverter(s_dist.GetComponent<Slider>().value * max_dist);
        vel = Real_to_Virtual_ValueConverter(s_vel.GetComponent<Slider>().value * max_vel);
    }
    ///////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////
    void Canvas_Hide()
    {
        for (int i = 0; i < panel_objs.Length; i++) panel_objs[i].SetActive(false);
    }

    ///////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////
    void Vel(GameObject j, Vector3 v)
    {
        j.GetComponent<Rigidbody>().velocity = v;
    }
    ///////////////////////////////////////////////////////////////////////

    void Dist_Update(int start_check)
    {
        dist = Real_to_Virtual_ValueConverter(s_dist.GetComponent<Slider>().value * max_dist) + 5;
        vel = Real_to_Virtual_ValueConverter(s_vel.GetComponent<Slider>().value * max_vel);

        float dist2 = ((int)(dist * 100)) / 100;
        float vel2 = ((int)(vel * 100)) / 100;
        GameObject dist_val = GameObject.Find("Dist_Val");
        if (dist_val != null) dist_val.GetComponent<Text>().text = (dist2 + " m");
        GameObject vel_val = GameObject.Find("Vel_Val");
        if (vel_val != null) vel_val.GetComponent<Text>().text = (vel2 + " km/hr");

        Vector3 p = new Vector3(wall.transform.position.x, wall.transform.position.y, dist);
        wall.transform.position = p;
        if (impact_loc == 'b' && start_check == 0)
        {
            Vector3 j = new Vector3(jeep.transform.position.x, jeep_opp.transform.position.y, -dist);
            jeep_opp.transform.position = j;
            jeep_opp.transform.LookAt(jeep.transform);
        }
        else if(start_check == 0)
        {
            int temp_choice = 1;
            if (impact_loc == 'l') temp_choice = -1;
            Vector3 j = new Vector3((dist * temp_choice), jeep_opp.transform.position.y, jeep.transform.position.z);
            jeep_opp.transform.position = j;
            jeep_opp.transform.LookAt(jeep.transform);
        }

    }
    ///////////////////////////////////////////////////////////////////////
    void Reset_Check()
    {
        if (collided == 1)
        {
            acc = GameObject.FindGameObjectWithTag("Acc");
            for (int i = 0; i < panel_objs.Length; i++) panel_objs[i].SetActive(false);
            prob_panel.SetActive(true);
            AccidentDetect();
            GameObject prob_text = GameObject.FindGameObjectWithTag("Prob");
            prob_text.GetComponent<Text>().text = (accident_prob + "");
            Debug.Log(accident_prob);
            GameObject vel_text = GameObject.FindGameObjectWithTag("Vel");
            impact_vel = ((int)(impact_vel * 100))/100;
            vel_text.GetComponent<Text>().text = (impact_vel + "");
            reset = GameObject.FindGameObjectWithTag("Reset");
            collided = 0;
            started = 0;
            jeep_opp_started = 0;
        }
        else if (started == 1)
        {
            
            Vector3 v = new Vector3(0, 0, 0);
            if (jeep.GetComponent<Rigidbody>().velocity == v)
            {
                accident_prob = (float)0.0;
                for (int i = 0; i < panel_objs.Length; i++) panel_objs[i].SetActive(false);
                prob_panel.SetActive(true);
                GameObject prob_text = GameObject.FindGameObjectWithTag("Prob");
                prob_text.GetComponent<Text>().text = ("0");
                GameObject vel_text = GameObject.FindGameObjectWithTag("Vel");
                vel_text.GetComponent<Text>().text = ("No Impact");
                acc = GameObject.FindGameObjectWithTag("Acc");
                if(acc != null) acc.GetComponent<Text>().text = ("Not an Accident");

                reset = GameObject.FindGameObjectWithTag("Reset");

                started = 0;
            }
        }
        else if (jeep_opp_started == 1)
        {
            Vector3 v = new Vector3(0, 0, 0);
            if (jeep_opp.GetComponent<Rigidbody>().velocity == v)
            {
                acc = GameObject.FindGameObjectWithTag("Acc");
                accident_prob = (float)0.0;
                for (int i = 0; i < panel_objs.Length; i++) panel_objs[i].SetActive(false);
                prob_panel.SetActive(true);
                GameObject prob_text = GameObject.FindGameObjectWithTag("Prob");
                prob_text.GetComponent<Text>().text = ("0");
                GameObject vel_text = GameObject.FindGameObjectWithTag("Vel");
                vel_text.GetComponent<Text>().text = ("No Impact");
                acc = GameObject.FindGameObjectWithTag("Acc");
                if (acc != null) acc.GetComponent<Text>().text = ("Not an Accident");

                reset = GameObject.FindGameObjectWithTag("Reset");

                jeep_opp_started = 0;
            }
        }

    }

    void Reset_Scene()
    {
        if (t_temp == 1)
        {
            prob_panel.SetActive(false);

            s_dist.GetComponent<Slider>().value = 0;
            s_vel.GetComponent<Slider>().value = 0;

            for (int i = 0; i < panel_objs.Length; i++) panel_objs[i].SetActive(true);

            dist = 0;
            vel = 0;

            started = 0;
            collided = 0;

            Reset_Objects();

            t_temp = 0;
        }
    }

    void Reset_Objects()
    {
        Vector3 p = new Vector3(0, (float)0.5, 0);
        jeep.transform.position = p;

        Vector3 v = new Vector3(0, 0, 0);
        jeep.GetComponent<Rigidbody>().velocity = v;

        Quaternion q = new Quaternion(0, 0, 0, 0);
        jeep.transform.rotation = q;

        Vector3 p2;
        if(impact_loc == 'r') p2 = new Vector3(5, (float)0.5, 0);
        else if (impact_loc == 'b') p2 = new Vector3(0, (float)0.5, -5);
        else if (impact_loc == 'l') p2 = new Vector3(-5, (float)0.5, 0);
        else p2 = new Vector3(0, (float)0.5, -5);
        jeep_opp.transform.position = p2;
        jeep_opp.transform.LookAt(jeep.transform);

        jeep_opp.GetComponent<Rigidbody>().velocity = v;

        jeep_opp.transform.rotation = q;

        //Camera_Controller.Update_View();
    }
    ///////////////////////////////////////////////////////////////////////
    void AccidentDetect()
    {
        Vel_to_Prob_Function();
        acc = GameObject.FindGameObjectWithTag("Acc");
        if (accident_prob >= 0.1)
        {
            Debug.Log("Accident");
            if (acc != null) acc.GetComponent<Text>().text = ("Serious Accident (Prob > 0.1)");
        }
        else
        {
            Debug.Log("Not Accident");
            if (acc != null) acc.GetComponent<Text>().text = ("Not a Serious Accident (Prob < 0.1)");
        }
       
    }

    void Vel_to_Prob_Function()
    {
        float v = impact_vel;
        if (impact_choice == 'f')
        {
            if (v < 20) accident_prob = 0;
            else if (v >= 20 && v < 30) accident_prob = (float)0.15;
            else if (v >= 30 && v < 40) accident_prob = (float)0.5;
            else if (v >= 40 && v < 50) accident_prob = (float)0.8;
            else if (v >= 50 && v < 60) accident_prob = (float)0.95;
            else if (v >= 60) accident_prob = (float)1.0;
        }
        else if (impact_choice == 'b')
        {
            if (v < 20) accident_prob = 0;
            else if (v >= 20 && v < 30) accident_prob = (float)0.05;
            else if (v >= 30 && v < 40) accident_prob = (float)0.15;
            else if (v >= 40 && v < 50) accident_prob = (float)0.45;
            else if (v >= 50 && v < 60) accident_prob = (float)0.75;
            else if (v >= 60 && v < 70) accident_prob = (float)0.95;
            else if (v >= 70) accident_prob = (float)1.0;
        }
        else if (impact_choice == 'l') // Far Side
        {
            if (v < 20) accident_prob = (float)0.04;
            else if (v >= 20 && v < 30) accident_prob = (float)0.25;
            else if (v >= 30 && v < 40) accident_prob = (float)0.8;
            else if (v >= 40 && v < 50) accident_prob = (float)0.95;
            else if (v >= 50) accident_prob = (float)1.0;
        }
        else if (impact_choice == 'r') // Near Side
        {
            if (v < 20) accident_prob = (float)0.1;
            else if (v >= 20 && v < 30) accident_prob = (float)0.5;
            else if (v >= 30 && v < 40) accident_prob = (float)0.9;
            else if (v >= 40) accident_prob = (float)1.0;
        }

    }
    ///////////////////////////////////////////////////////////////////////
    void Impact_Location()
    {
        if (t_temp == 1)
        {
            if (impact_loc == 'f')
            {
                GameObject disText = GameObject.Find("Text_Dist");
                disText.GetComponent<Text>().text = ("Distance of other Car");
                impact_loc = 'b';
                Material temp = (Material)Resources.Load("Back");
                imp_loc_button.GetComponent<Image>().material = temp;
            }
            else if (impact_loc == 'b')
            {
                impact_loc = 'l';
                Material temp = (Material)Resources.Load("Lefty");
                imp_loc_button.GetComponent<Image>().material = temp;
            }
            else if (impact_loc == 'l')
            {
                impact_loc = 'r';
                Material temp = (Material)Resources.Load("Right");
                imp_loc_button.GetComponent<Image>().material = temp;
            }
            else if (impact_loc == 'r')
            {
                GameObject disText = GameObject.Find("Text_Dist");
                disText.GetComponent<Text>().text = ("Distance of Wall");
                impact_loc = 'f';
                Material temp = (Material)Resources.Load("Front");
                imp_loc_button.GetComponent<Image>().material = temp;
            }

            t_temp = 0;
        }
    }
}
///////////////////////////////////////////////////////////////////////
