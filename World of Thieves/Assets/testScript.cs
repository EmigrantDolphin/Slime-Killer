using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class testScript : MonoBehaviour
{

    float rotationSpeed = 0.01f * Mathf.PI;

    Vector2 currentDir = new Vector2(0, 1);

    Vector2 Up = new Vector2(0, 1);
    Vector2 UpRight = new Vector2(0.5f, 0.5f);
    Vector2 Right = new Vector2(1, 0);
    Vector2 DownRight = new Vector2(0.5f, -0.5f);
    Vector2 Down = new Vector2(0, -1);
    Vector2 DownLeft = new Vector2(-0.5f, -0.5f);
    Vector2 Left = new Vector2(-1, 0);
    Vector2 UpLeft = new Vector2(-0.5f, 0.5f);


    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        
        var targetVector = DownLeft;


        if (Input.anyKeyDown) 
            currentDir = ApplyRotationToVector(currentDir, targetVector, rotationSpeed);


        Debug.DrawLine(new Vector2(0, 0), currentDir);
    }

    //rotationStep == rotationSpeed * deltaTime
    Vector2 ApplyRotationToVector(Vector2 currDir, Vector2 targetDir, float rotationStep) {
        float lockOnAngleDistance = 3f; //degrees

        //getting angles [0-360)
        float targetAngle = (float)(Math.Atan2(targetDir.y, targetDir.x) * (180 / Math.PI));
        if (targetDir.y < 0) //if it's negative, angle gonna be from 0 to -180, so making it 0 to 360
            targetAngle = 360 + targetAngle;

        float currAngle = (float)(Math.Atan2(currDir.y, currDir.x) * (180 / Math.PI));
        if (currDir.y < 0)
            currAngle = 360 + currAngle;
        print(Anglee(currDir, targetDir));

        //locking on if close to target
        float difference = currAngle > targetAngle ? currAngle - targetAngle : targetAngle - currAngle;
        difference = difference > 180 ? 360 - difference : difference;
        if (difference < lockOnAngleDistance)
            return targetDir.normalized;
        //return Vector2.Normalize(targetDir);

        if (Anglee(currDir, Up) >= 90)
            return currDir;

        //deciding which direction to go (probably a better way exists, too tired to think clearer)
        float rotationDirection = -1;
        if (targetAngle > currAngle) {
            if (targetAngle - currAngle < currAngle - targetAngle + 360)
                rotationDirection = 1;
            else
                rotationDirection = -1;
        } else {
            if (currAngle - targetAngle < targetAngle - currAngle + 360)
                rotationDirection = -1;
            else
                rotationDirection = 1;
        }

        //calculating 
        float currRadian = (float)(currAngle * (Math.PI / 180));
        float y = (float)Math.Sin(currRadian + rotationStep * rotationDirection);
        float x = (float)Math.Cos(currRadian + rotationStep * rotationDirection);

        //currDir = Vector2.Normalize(new Vector2(x, y));
        currDir = new Vector2(x, y).normalized;

        return currDir;
    }

    public static float Anglee(Vector2 from, Vector2 to) {
        return (float)Math.Acos(Mathf.Clamp(Vector2.Dot(from.normalized, to.normalized), -1f, 1f)) * 57.29578f;
    }

}


