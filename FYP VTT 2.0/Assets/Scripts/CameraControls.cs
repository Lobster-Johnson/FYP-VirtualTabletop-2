using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour
{
    public KeyCode forward;
    public KeyCode left;
    public KeyCode right;
    public KeyCode backward;

    public float speed = 10;




    public float movesensitivityX = 1.0f;
    public float movesensitivityY = 1.0f;

    ////selection ray
    //Ray ray;
    //RaycastHit hit;

    // Use this for initialization
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    // Update is called once per frame
    void Update()
    {
        //wasd controls for debug
        if (Input.GetKey(forward))
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
        }
        if (Input.GetKey(left))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(right))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(backward))
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
        }

        //touchscreen camera controls



        //if the user is touching the screen
        if (Input.touchCount > 0)
        {
            
            //if the user's touch moves
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                //Debug.Log("Touch Moved");
                Vector2 delta = Input.GetTouch(0).deltaPosition;

                float positionX = (delta.x * movesensitivityX * Time.deltaTime) * -1;
                float positionY = (delta.y * movesensitivityY * Time.deltaTime) * -1;

                transform.position += new Vector3(-positionX, positionY, 0);
            }
        }

    }
}
