using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform target = null;

    public float smoothing = 0.04f;

    public float zoomDistance;
    public float minZoomDistance = 6;
    public float maxZoomDistance = 10;

    public Vector2 topLeft = new Vector2(-30,30);
    public Vector2 bottomRight = new Vector2(30,-30);

    public static bool freeLook;

    private Vector3 change;

	public bool movementDisabled = false;

    void Start()
    {
     
        Move();
        Zoom();   
    }

    void Update()
    {	
		

		if (movementDisabled) {
			return;
		}

        Move();
        Zoom();

        Vector3 borderCheck = CheckBorders();
        if (borderCheck != transform.position)
        {
            transform.position = borderCheck;
        }

        // Press Q to focus on the selected character
        if (Input.GetKeyDown(KeyCode.Q))
        {
            target = Character.selectedCharacter.transform;
        }

    }
    void LateUpdate()
    {
        if (target != null)
        {
            if (transform.position != target.position)
            {
                Vector3 targetVector = new Vector3(target.position.x, target.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetVector, smoothing);
            }
        }

		GetComponent<Camera>().orthographicSize = zoomDistance;
    }

	bool PlayerDidUseCameraControls = false;
    void Move()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        if (change != Vector3.zero)
        {
			if (!PlayerDidUseCameraControls) {
				LevelManager.emit("PlayerDidUseCameraControls");
				PlayerDidUseCameraControls = true;
			}
			
            if (!freeLook)
            {
                freeLook = true;
            }
            if (target != null)
            {
                target = null;
            }
            transform.position += change * Time.deltaTime * 15;
        }
    }

    void Zoom()
    {
        zoomDistance -= Input.mouseScrollDelta.y * Time.deltaTime * 30;
        zoomDistance = Mathf.Clamp(zoomDistance, minZoomDistance, maxZoomDistance);
    }

    Vector3 CheckBorders()
    {
        Vector3 border = Vector3.zero;
        border.x = Mathf.Clamp(transform.position.x, topLeft.x, bottomRight.x);
        border.y = Mathf.Clamp(transform.position.y, bottomRight.y, topLeft.y);
        border.z = transform.position.z;

        return border;
    }
}
