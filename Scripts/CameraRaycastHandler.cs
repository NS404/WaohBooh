using Godot;
using System;

public partial class CameraRaycastHandler : RayCast3D {

    [ExportGroup("Parameters")]
    [Export] public float maxDistance = 1f;

    public float GetDistance () {

        TargetPosition = new Vector3(0f, 0f, maxDistance);

        ForceRaycastUpdate();

        if ( IsColliding() ) {

            Vector3 origin = GlobalTransform.Origin;
            Vector3 collisionPoint = GetCollisionPoint();
            float distance = origin.DistanceTo(collisionPoint);

            return distance;

        }

        return maxDistance;

    }

}