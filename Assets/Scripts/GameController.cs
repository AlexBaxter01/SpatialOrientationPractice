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

    TMPro.TextMeshProUGUI timerText;

    TMPro.TextMeshProUGUI statsText;

    bool isCorrect = false;
    bool hasGuessed = true;

    int questionsAnswered = 0;
    int questionsCorrect = 0;

    private float timeLeft = 10.0f;

    void Awake()
    {
        gyrocompass = GameObject.Find("GYRO Compass");
        arrow = GameObject.Find("Arrow");
        guessText = GameObject.Find("Guess Text").GetComponent<TMPro.TextMeshProUGUI>();
        answerText = GameObject.Find("Answer Text").GetComponent<TMPro.TextMeshProUGUI>();
        timerText = GameObject.Find("Timer Text").GetComponent<TMPro.TextMeshProUGUI>();
        statsText = GameObject.Find("Stats Text").GetComponent<TMPro.TextMeshProUGUI>();
        StartNewQuestion();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        UpdateText();
        if(timeLeft <= 0) {
            Debug.Log("Time's up!");
            return;
        }
        timeLeft -= Time.deltaTime;
        
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
    }

    public void GuessLocation(string dir) {
        Direction direction = GetDirectionFromString(dir);
        if(directionGuess == Direction.Null) { //need a direction guess first (for placing plane sprite)
            Debug.Log("Guess the direction first");
            return;
        }
        locationGuess = direction;
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
    
    private void UpdateText() {      
        UpdateGuessText();
        UpdateAnswerText();
        UpdateTimerText();
        UpdateStatsText();
    }

    private void UpdateGuessText() {
        if(timeLeft <= 0) {
            guessText.text = "";
            return;
        }
        if(directionGuess == Direction.Null) {
            guessText.text = "";
            return;
        }
        if(locationGuess == Direction.Null) {
            guessText.text = "Guessed Direction: " + directionGuess;
            return;
        }
        guessText.text = "Guessed Direction: " + directionGuess + "\nGuessed Location: " + locationGuess;
    }

    private void UpdateAnswerText() {
        if (timeLeft <= 0) {
            answerText.text = "";
            return;
        }
        if(!hasGuessed) {
            answerText.text = "";
            return;
        }
        if(isCorrect) {
            answerText.text = "Correct!";
            return;
        }
        else{
            answerText.text = "Incorrect!" + "\nCorrect Direction: " + dirPlaneTravel + "\nCorrect Location: " + dirBeaconToPlane;
            return;
        }
    }

    private void UpdateTimerText() {
        if(timeLeft <= 0) {
            timerText.text = "Time's up!";
            return;
        }
        timerText.text = "Time Left: " + (int) (timeLeft + 0.99f);
    }

    private void UpdateStatsText() {
        if(timeLeft > 0) {
            statsText.text = "";
            return;
        }

        float score = 100 * (float) questionsCorrect / questionsAnswered;
        int intScore = score > 100 || score < 0 ? 0 : (int) score;
        statsText.text = "Questions Answered: " + questionsAnswered + "\nQuestions Correct: " + questionsCorrect + "\nScore: " + intScore + "%";
    }

    public void SubmitGuess() {
        if(timeLeft <= 0) {
            return;
        }

        if(directionGuess == Direction.Null || locationGuess == Direction.Null) {
            Debug.Log("Need to guess both direction and location");
            return;
        }
        if(directionGuess == dirPlaneTravel && locationGuess == dirBeaconToPlane) {
            questionsCorrect++;
            Debug.Log("Correct!");
            isCorrect = true;
        }
        else {
            Debug.Log("Incorrect");
            isCorrect = false;
        }
        questionsAnswered++;
        hasGuessed = true;
    }

    public void StartNewQuestion() 
    {
        if(!hasGuessed) {
            Debug.Log("Need to submit a guess first");
            return;
        }
        if(timeLeft <= 0) {
            Debug.Log("Time's up!");
            return;
        }

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
        statsText.text = "";
        
        isCorrect = false;
        hasGuessed = false;
    }
}
