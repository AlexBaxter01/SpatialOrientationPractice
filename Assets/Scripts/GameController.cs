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

    bool isCorrect = false;
    bool hasGuessed = true;

    void Awake()
    {
        gyrocompass = GameObject.Find("GYRO Compass");
        arrow = GameObject.Find("Arrow");
        guessText = GameObject.Find("Guess Text").GetComponent<TMPro.TextMeshProUGUI>();
        answerText = GameObject.Find("Answer Text").GetComponent<TMPro.TextMeshProUGUI>();

        StartNewQuestion();
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
        return (Direction) (UnityEngine.Random.Range(0, 8) * 45);
    }

    public void GuessDirection(string dir) {
        Direction direction = GetDirectionFromString(dir);
        locationGuess = Direction.Null; // location can only be set after a guess, so reset when a new guess is made
        directionGuess = direction;

        UpdateGuessText();
    }

    public void GuessLocation(string dir) {
        Direction direction = GetDirectionFromString(dir);
        if(directionGuess == Direction.Null) { //need a direction guess first (for placing plane sprite)
            Debug.Log("Guess the direction first");
            return;
        }
        locationGuess = direction;

        UpdateGuessText();
    }
    
    private Direction GetDirectionFromString(string dir) {
        switch(dir) {
            case "North":
                return Direction.North;
            case "NorthEast":
                return Direction.NorthEast;
            case "East":
                return Direction.East;
            case "SouthEast":
                return Direction.SouthEast;
            case "South":
                return Direction.South;
            case "SouthWest":
                return Direction.SouthWest;
            case "West":
                return Direction.West;
            case "NorthWest":
                return Direction.NorthWest;
            default:
                Debug.Log("Invalid direction");
                return Direction.Null;
        }
    }
    
    private void UpdateGuessText() {
        if(locationGuess == Direction.Null) {
            guessText.text = "Guessed Direction: " + directionGuess;
            return;
        }
        guessText.text = "Guessed Direction: " + directionGuess + "\nGuessed Location: " + locationGuess;
    }

    private void UpdateAnswerText() {
        if(isCorrect) {
            answerText.text = "Correct!";
            return;
        }
        else{
            answerText.text = "Incorrect!" + "\nCorrect Direction: " + dirPlaneTravel + "\nCorrect Location: " + dirBeaconToPlane;
            return;
        }
        //answerText.text = "True Direction: " + dirPlaneTravel + "\nTrue Location: " + dirBeaconToPlane;
    }

    public void SubmitGuess() {
        if(directionGuess == Direction.Null || locationGuess == Direction.Null) {
            Debug.Log("Need to guess both direction and location");
            return;
        }
        if(directionGuess == dirPlaneTravel && locationGuess == dirBeaconToPlane) {
            Debug.Log("Correct!");
            isCorrect = true;
        }
        else {
            Debug.Log("Incorrect");
            isCorrect = false;
        }
        UpdateAnswerText();
    }

    public void StartNewQuestion() {
        if(hasGuessed) {
        dirPlaneTravel = GetRandomDirection();
        dirBeaconToPlane = GetRandomDirection();
        
        Debug.Log("Plane Travel: " + dirPlaneTravel);
        Debug.Log("Beacon to Plane: " + dirBeaconToPlane);
        Debug.Log("Plane to Beacon: " + dirPlaneToBeacon);
        Debug.Log("Relative Plane to Beacon: " + dirRelativePlaneToBeacon);

        RotateGyrocompass();
        RotateArrow();
        
        directionGuess = Direction.Null;
        locationGuess = Direction.Null;
        
        answerText.text = "";
        guessText.text = "";
        
        isCorrect = false;
        hasGuessed = false;

        }

        else {
            Debug.Log("Need to submit a guess first");
        }
    }
}
