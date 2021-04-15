using UnityEngine;

public class TileMover : MonoBehaviour, IMove
{
    private Vector3 destinationPoint;
    private bool isMoving;
    private float movingSpeed = 5;
    private float step;

    public bool IsMoving
    {
        get => isMoving;
        set => isMoving = value;
    }

    public Vector3 DestinationPoint
    {
        get => destinationPoint;
        set => destinationPoint = value;
    }

    public float MovingSpeed
    {
        get => movingSpeed;
        set => movingSpeed = value;
    }

    void Update()
    {  
        if (IsMoving)
        {
            step = MovingSpeed * Time.deltaTime;
            Move();
        }
    }

    public void Move()
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
