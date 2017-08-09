 using UnityEngine;
 
 public static class Vector2Extension {
     
     public static Vector2 Rotate(this Vector2 v, float degrees)
     {
         return Quaternion.Euler(0, 0, degrees) * v;
     }

    public static Vector2 RotateTowards(Vector3 v, Vector2 target, float angleStep)
    {
        if (v == Vector3.zero) return Vector3.zero;
        if (target == Vector2.zero) return v;

        v.z = 0;
        float angleTarget = Vector2.Angle(Vector2.right, target);
        float angleV = Vector2.Angle(Vector2.right, v);
        if (target.y < 0) angleTarget = -angleTarget;
        if (v.y < 0) angleV = -angleV;
        float dA = angleTarget - angleV;

        if (Mathf.Abs(dA) < 0.01f) return target;
        while (angleStep > Mathf.Abs(dA))
        {
            if (Mathf.Abs(angleStep) < 0.001f) return target;
            angleStep /= 2;
        } 

        return Quaternion.Euler(0,0,angleStep * Mathf.Sign(dA) * angleStep) * v;
    }

 }