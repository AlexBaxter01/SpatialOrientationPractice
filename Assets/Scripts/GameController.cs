using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Directions;
public class GameController : MonoBehaviour
{

    GameObject gyrocompass;
    GameObject arrow; 

    Direction dirPlaneTravel;
    Direction dirBeaconToPlane; 
    Direction dirPlaneToBeacon;
    Direction dirRelativePlaneToBeacon; // dirPlaneToBeacon from the axis of the plane


    void Awake()
    {
        gyrocompass = GameObject.Find("GYRO Compass");
        arrow = GameObject.Find("Arrow");

        dirPlaneTravel = Direction.East;
        dirBeaconToPlane = Direction.SouthWest;
        //RotateGyrocompass();
        //RotateArrow();
        
        RotateGyrocompass();
        RotateArrow();
        Debug.Log("Plane Travel: " + dirPlaneTravel);
        Debug.Log("Beacon to Plane: " + dirBeaconToPlane);
        Debug.Log("Plane to Beacon: " + dirPlaneToBeacon);
        Debug.Log("Relative Plane to Beacon: " + dirRelativePlaneToBeacon);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void RotateGyrocompass() {
        int angle = dirPlaneTravel.GetAngle();
        gyrocompass.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void RotateArrow() {
        
        dirPlaneToBeacon = dirBeaconToPlane.GetReverse();
        
        dirRelativePlaneToBeacon = dirPlaneToBeacon.RotateDirection(-dirPlaneTravel.GetAngle());
        int angle = dirRelativePlaneToBeacon.GetAngle();
        arrow.transform.rotation = Quaternion.Euler(0, 0, -angle);
    }

    public static Direction GetDirection(int angle) 
    {
        try 
        {
            return (Direction) angle;
        }
        catch 
        {
            Debug.Log("Invalid angle, use angles in 45 degree increments");
            return Direction.North;
        }
    }

    public static Direction GetRandomDirection() {
        return (Direction) (Random.Range(0, 8) * 45);
    }

    //private void RotateObject(GameObject obj, Direction direction) 
    //{
      //  obj.transform.rotation = Quaternion.Euler(0, 0, -GetAngle(direction));
    //}

    //private void RotateGyrocompass() 
    //{
    //    RotateObject(gyrocompass, dirPlaneTravel);
    //}

    //private void RotateArrow() 
    //{
    //    return;
    //}

    
}
