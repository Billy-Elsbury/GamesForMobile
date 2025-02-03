using UnityEngine;

public class CylinderScript : BaseObjectScript
{
    protected override void Start()
    {
        base.Start();
    }

    public override void SelectToggle(bool selected)
    {
        objectRenderer.material.color = selected ? Color.yellow : Color.white;
    }

    public override void MoveObject(Transform transform, Touch touch)
    {
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        int groundLayerMask = LayerMask.GetMask("Ground");
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
        {
            Vector3 touchPosition = hit.point;

            Collider collider = transform.GetComponent<Collider>();
            if (collider != null)
            {
                float heightOffset = collider.bounds.extents.y;
                transform.position = new Vector3(touchPosition.x, touchPosition.y + heightOffset, touchPosition.z);
            }
        }
    }
}
