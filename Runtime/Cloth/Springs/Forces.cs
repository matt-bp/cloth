using UnityEngine;

namespace Cloth.Springs
{
    public static class Forces
    {
        public static Vector3 GetSpringForce(Vector3 position1, Vector3 position2, float k, float restLength)
        {
            var distance = Vector3.Distance(position1, position2);
            var force = k * (distance - restLength) * ((position1 - position2) / distance);
            return force;
        }

        public static Vector3 GetDampingForce(Vector3 velocity1, Vector3 velocity2, float kd)
        {
            return -kd * (velocity1 - velocity2);
        }
    }
}