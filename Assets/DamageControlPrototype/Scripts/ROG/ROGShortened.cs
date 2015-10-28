using UnityEngine;
using System.Collections;

public class ROGShortened : MonoBehaviour {

    public static Vector3 GetMouseWorldPosition(GameObject fromObject)
    {
        // Assumes use of default "Main Camera"
        float camDist = Vector3.Distance(fromObject.transform.position, Camera.main.transform.position);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camDist));
        return mousePos;
    }

    public static Vector3 GetDirectionToMouse(GameObject fromObject, bool ignoreUp)
    {
        // Assumes use of default "Main Camera"
        Vector3 mPos = GetMouseWorldPosition(fromObject);

        if (ignoreUp)
            mPos.y = fromObject.transform.position.y;

        Vector3 mouseDirection = (mPos - fromObject.transform.position).normalized;
        return mouseDirection;
    }

    public static Quaternion LookAtMouse(GameObject obj, bool ignoreUp)
    {
        Vector3 lookVector = GetDirectionToMouse(obj, ignoreUp);

        if (lookVector.magnitude != 0)
            return Quaternion.LookRotation(lookVector);
        else
            return Quaternion.identity;
    }
}
