using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntMovement : MonoBehaviour
{

    #region Serialized Variables
    [SerializeField]
    private float turnSpeed;
    [SerializeField]
    private float moveSpeed;


    #endregion

    #region Local Variables

    private Transform headPivot;
    private float rotationRemaining;
    private float rotationDirection;
    private float turnSpeedModifier;
    private Rigidbody2D rb;
    #endregion

    #region Head Rotation
    private void UpdateRotationRemaining(float rotateAmount) 
    {
        rotationRemaining = rotateAmount >= rotationRemaining ? 0 : rotationRemaining - rotateAmount;
    }

    //Fixed update, don't need Time.DeltaTime
    private void RotateHead() 
    {

        if (rotationRemaining > 0) 
        {
            float rotateAmount = turnSpeed * turnSpeedModifier;
            UpdateRotationRemaining(rotateAmount);
            float rotateIncrement =  rotateAmount * rotationDirection;
            headPivot.Rotate(0, 0, rotateIncrement);
        }
    }

    public void RotateAnt(Vector3 to)
    {
        Vector3 from = headPivot.transform.right;
        rotationRemaining = CalculateRotation(from, to);
        SetDirection();


    }
    private void SetDirection() 
    {
        rotationDirection = 1f; // Default is anit clockwise
        if (rotationRemaining < 0)
        {
            rotationDirection = -1f;
            rotationRemaining = Mathf.Abs(rotationRemaining);
        }
    }

    private float CalculateRotation(Vector3 a, Vector3 b)
    {
        float radiansAngle = Mathf.Atan2(a.x * b.y - a.y * b.x, a.x * b.x + a.y * b.y); // Radians
        float degreesAngle = Mathf.Rad2Deg * radiansAngle;
        return degreesAngle;
    }


    #endregion

    #region Move Ant

    private void MoveAnt()
    {
        Vector3 moveDirection = headPivot.transform.right;
        Vector3 moveVector = moveDirection * moveSpeed;
        rb.MovePosition(transform.position + moveVector);
    }

    private void HandleEdgeDetection(Collider2D collision) 
    {
        if (collision.CompareTag("Wall"))
        {
            headPivot.Rotate(new Vector3(0, 0, 180f));
            Vector3 moveDirection = headPivot.transform.right;
            Vector3 moveVector = moveDirection * transform.localScale.x;
            transform.position += moveVector;
        }
    }

    #endregion

    #region Initialising Variables

    private void SetDefaultValues()
    {
        headPivot = transform.GetChild(0);
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
        rotationRemaining = 0;
        rotationDirection = 1;
        turnSpeedModifier = 1f;
        rb = GetComponent<Rigidbody2D>();
    }

    #endregion

    #region Public Setters/Getters

    public float GetMoveSpeed() { return moveSpeed; }

    public void SetMoveSpeed(float moveSpeed) { this.moveSpeed = moveSpeed; }


    public void SetDirection(float rotationRemaining, float rotationDirection)
    {
        this.rotationRemaining = rotationRemaining;
        this.rotationDirection = rotationDirection;
    }

    public void SetTurnSpeedModifier(float turnSpeedModifier) { this.turnSpeedModifier = turnSpeedModifier; }

    public void ResetTurnSpeedModifier() { this.turnSpeedModifier = 1f; }


    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleEdgeDetection(collision);
    }

    // Fixed Update is called for Physics and should be used for any changes to RigidBody
    private void FixedUpdate()
    {
        RotateHead();
        MoveAnt();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        SetDefaultValues();
    }


 


}
