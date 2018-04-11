using Assets.Scripts;
using UnityEngine;

public class CameraDrag : MonoBehaviour {

    public float dragSpeed = Config.CAMERA_DRAG_SPEED;

    private Vector3 dragOrigin;
    private float dragHeightLimit = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0))
            return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(0, pos.y * -dragSpeed, 0);

        transform.Translate(move, Space.World);

        // Make sure the camera is bound
        transform.position = new Vector3(
           transform.position.x,
           Mathf.Clamp(transform.position.y, 0, dragHeightLimit),
           transform.position.z
        );
    }

    public void SetDragHeightLimit(float value)
    {
        dragHeightLimit = value;
    }
}
