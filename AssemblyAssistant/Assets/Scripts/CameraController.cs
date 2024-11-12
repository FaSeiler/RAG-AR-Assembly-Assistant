using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mainSpeed = 2.0f; // Regular speed
    [SerializeField] private float shiftAdd = 5.0f; // Multiplied speed by how long shift is held
    [SerializeField] private float maxShift = 10.0f; // Maximum speed when holding shift
    [SerializeField] private float camSens = 0.1f; // How sensitive is the mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); // Kind of in the middle of the screen, rather than at the top
    private float totalRun = 1.0f;

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            lastMouse = Input.mousePosition - lastMouse;
            lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
            lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
            transform.eulerAngles = lastMouse;
        }

        lastMouse = Input.mousePosition;
        //Mouse  camera angle done

        //Keyboard commands
        Vector3 p = GetBaseInput();

        if (p.sqrMagnitude > 0)
        { 
            // Only move while a direction key is pressed
            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalRun += Time.deltaTime;
                p = p * totalRun * (shiftAdd / Time.timeScale);
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            }
            else
            {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * (mainSpeed / Time.timeScale);
            }

            p = p * Time.deltaTime;
            Vector3 newPosition = transform.position;
            if (Input.GetKey(KeyCode.Space))
            { 
                //If player wants to move on X and Z axis only
                transform.Translate(p);
                newPosition.x = transform.position.x;
                newPosition.z = transform.position.z;
                transform.position = newPosition;
            }
            else
            {
                transform.Translate(p);
            }
        }

        // Up and down camera movements
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(new Vector3(0, -1, 0) * (mainSpeed / Time.timeScale) * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(new Vector3(0, 1, 0) * (mainSpeed / Time.timeScale) * Time.deltaTime, Space.World);
        }
    }

    private Vector3 GetBaseInput()
    { 
        // Returns the basic values, if it's 0 then it's not active
        Vector3 p_Velocity = new Vector3();

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }

        return p_Velocity;
    }
}
