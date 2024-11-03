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

    Direction directionGuess = Direction.Null; // the direction the plane is facing
    Direction locationGuess = Direction.Null; // which square around the beacon the plane is in

    TMPro.TextMeshProUGUI guessText;
    TMPro.TextMeshProUGUI answerText;

    void Awake()
    {
        gyrocompass = GameObject.Find("GYRO Compass");
        arrow = GameObject.Find("Arrow");
        guessText = GameObject.Find("Guess Text").GetComponent<TMPro.TextMeshProUGUI>();

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

    private Direction GetDirection(int angle) 
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

    private Direction GetRandomDirection() {
        return (Direction) (Random.Range(0, 8) * 45);
    }

    public void GuessDirection(Direction direction) {
        locationGuess = Direction.Null; // location can only be set after a guess, so reset when a new guess is made
        directionGuess = direction;

        UpdateGuessText();
    }

    public void GuessLocation(Direction direction) {
        if(directionGuess == Direction.Null) { //need a direction guess first (for placing plane sprite)
            Debug.Log("Guess the direction first");
            return;
        }
        locationGuess = direction;

        UpdateGuessText();
    }
    
    private void UpdateGuessText() {
        guessText.text = "Guessed Direction: " + directionGuess + "\n Guessed Location: " + locationGuess;
    }

    private void UpdateAnswerText() {
        answerText.text = "True Direction: " + dirPlaneTravel + "\nTrue Location: " + dirBeaconToPlane;
    }

    private void SubmitGuess() {
        if(directionGuess == Direction.Null || locationGuess == Direction.Null) {
            Debug.Log("Need to guess both direction and location");
            return;
        }
        if(directionGuess == dirPlaneTravel && locationGuess == dirBeaconToPlane) {
            Debug.Log("Correct!");
        }
        else {
            Debug.Log("Incorrect");
        }
        UpdateAnswerText();
    }
}
