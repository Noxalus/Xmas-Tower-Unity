using UnityEngine;
using System.Collections;

public class DragObject2D : MonoBehaviour
{
    public float dampingRatio = 0.5f;
    public float frequency = 5f;

    private TargetJoint2D targetJoint;

    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Vector2.zero,
            0f,
            LayerMask.GetMask("Gift")
        );

        if (hit.transform == null || hit.transform.gameObject != gameObject)
            return;

        if (hit.collider == null || !hit.rigidbody || hit.rigidbody.isKinematic)
            return;

        var selectedGift = hit.transform.gameObject.GetComponent<Gift>();

        if (!selectedGift || selectedGift.GetCurrentState() == Gift.GiftState.SLEEPING)
            return;

        selectedGift.Select();

        if (!targetJoint)
        {
            targetJoint = gameObject.AddComponent<TargetJoint2D>();
            targetJoint.dampingRatio = dampingRatio;
            targetJoint.frequency = frequency;
        }

        targetJoint.anchor = transform.InverseTransformPoint(hit.point);
        StartCoroutine(DragObject());
    }

    IEnumerator DragObject()
    {
        while (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetJoint.target = mousePos;
            yield return null;
        }

        // Release the input
        Destroy(targetJoint);
    }
}