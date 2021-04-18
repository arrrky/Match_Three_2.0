using UnityEngine;

public class TileMover : MonoBehaviour
{
    private float step;
    private Vector3 destinationPoint;
    private const float DefaultMovingSpeed = 5;
    private float speedMultiplier;
    private bool isMoving;

    public float SpeedMultiplier
    {
        get => speedMultiplier;
        set
        {
            if (value <= 0)
            {
                speedMultiplier = 1;
            }
            else
            {
                speedMultiplier = value;
            }
        }
    }

    public void Init(Vector3 destinationPoint, float speedMultiplier)
    {
        this.destinationPoint = destinationPoint;
        SpeedMultiplier = speedMultiplier;
        isMoving = true;
        enabled = true;
    }

    void Update()
    {  
        if (isMoving)
        {
            step = DefaultMovingSpeed * SpeedMultiplier * Time.deltaTime;
            Move();
        }
    }

    private void Move()
    {
        gameObject.transform.position =
            Vector3.MoveTowards(gameObject.transform.position, destinationPoint, step);

        if (gameObject.transform.position == destinationPoint)
        {
            isMoving = false;
            this.enabled = false;
        }
    }
}