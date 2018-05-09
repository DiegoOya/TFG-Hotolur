using UnityEngine;

/// <summary>
/// This script is used as a helper to not call the function StringToHash() 
/// everytime it is needed some variable of the Animator 
/// </summary>
public class HashIDs : MonoBehaviour {

    // Singleton of HashIDs
    public static HashIDs instance;

    public int dyingState;
    public int deadBool;
    public int jumpBool;
    public int jumpState;
    public int isGroundedBool;
    public int locomotionState;
    public int speedFloat;
    public int playerInSightBool;
    public int shotFloat;
    public int aimWeightFloat;
    public int angularSpeedFloat;
    public int openBool;
    public int throwBool;
    public int meleeBool;
    public int shootBool;
    public int hitBool;
    public int pickUpBool;
    public int pickUpState;
    public int hitState;

    // Initialize all variables
    private void Awake()
    {
        instance = this;

        dyingState = Animator.StringToHash("Base Layer.Dying");
        deadBool = Animator.StringToHash("Dead");
        jumpBool = Animator.StringToHash("Jump");
        jumpState = Animator.StringToHash("Base Layer.Dying");
        isGroundedBool = Animator.StringToHash("IsGrounded");
        locomotionState = Animator.StringToHash("Base Layer.Locomotion");
        speedFloat = Animator.StringToHash("Speed");
        playerInSightBool = Animator.StringToHash("PlayerInSight");
        shotFloat = Animator.StringToHash("Shot");
        aimWeightFloat = Animator.StringToHash("AimWeight");
        angularSpeedFloat = Animator.StringToHash("AngularSpeed");
        openBool = Animator.StringToHash("Open");
        throwBool = Animator.StringToHash("Throw Attack");
        meleeBool = Animator.StringToHash("Melee Attack");
        shootBool = Animator.StringToHash("Shoot");
        hitBool = Animator.StringToHash("Hit");
        pickUpBool = Animator.StringToHash("PickUp");
        pickUpState = Animator.StringToHash("Actions Layer.Picking Up");
        hitState = Animator.StringToHash("Actions Layer.Hit Reaction");
    }
}
