using UnityEngine;

public class HashIDs : MonoBehaviour {

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
    }
}
