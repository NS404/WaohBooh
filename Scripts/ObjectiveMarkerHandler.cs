using Godot;
using Godot.Collections;
using System;

public partial class ObjectiveMarkerHandler : Area3D {

    [ExportGroup("Components")]
    private ObjectiveHandler objectiveHandler;

    public override void _Ready () {

        objectiveHandler = ObjectiveHandler.Instance;

    }

    public override void _PhysicsProcess ( double delta ) {

        if ( !HasOverlappingBodies() )
            return;

        Array<Node3D> bodies = GetOverlappingBodies();

        foreach ( Node3D body in bodies ) {

            if ( body is not BottleHandler )
                continue;
            
            BottleHandler player = (BottleHandler)body;

            // TODO: Fix comparing if velocities are zero.
            if ( Math.Abs(player.RelativeUp.Y) > 0.98f /*&& player.LinearVelocity.IsZeroApprox() && player.AngularVelocity.IsZeroApprox()*/ )
                objectiveHandler.NextObjective();

        }  

    }

}