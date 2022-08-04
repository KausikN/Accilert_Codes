#include<iostream>
using namespace std;

    float Vel_to_Prob_Function(float impact_vel, char impact_choice)
    {
        float v = impact_vel;
        float accident_prob = 0;
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
        return accident_prob;
    }

    
	
    void AccidentDetect(float impact_vel, char impact_choice)
    {
    	//Get impact velocity in impact vel
        float accident_prob = Vel_to_Prob_Function(impact_vel, impact_choice);
        if (accident_prob >= 0.1)
        {
            cout<<"Send alert to ambulance (Prob > 0.1)";
        }
        else
        {
            cout<<"Dont send alert (Prob < 0.1)";
        }

    }

    int  main()
    {
    	cout<<"---------------------Accident Detector-------------------\n";
    	float v = 0;
    	cout<<"Enter the impact velocity: ";
    	cin>>v;
    	cout<<"Enter the direction(front - f, back - b, left - l, right - r): ";
    	char dir;
    	cin>>dir;
    	AccidentDetect(v, dir);

    	return 0;
    }
