using UnityEngine;

public interface ITouchable
{
    void SelectToggle(bool selected);
    void MoveObject(Vector3 newPanPosition);
    void ScaleObject(Touch t1, Touch t2);
    void RotateObject(Touch t1, Touch t2);
}
