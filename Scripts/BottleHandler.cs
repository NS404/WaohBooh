using Godot;
using System;

public partial class BottleHandler : RigidBody3D {

	public static BottleHandler Instance { get; private set; }

	public enum FlipType {
		Cap,
		Base
	}

	[ExportGroup("Parameters")]
	[Export] public float fillAmount = 0.33f;
	[Export] public float maxMass = 0.5f;
	[Export] public float chargeTime = 3f;
	[Export] public Vector2 maxForce = new Vector2(0.5f, 1f);
	[Export] public FlipType flipType = FlipType.Cap;

	private Vector3 centerOfMass;

	private Vector2 chargedForce;
	private float chargeSign;

	public Vector2 ChargedForce =>
		chargedForce;

	[ExportGroup("Components")]
	private CollisionShape3D collisionShape;
	private Camera3D camera;

	public Vector3 RelativeUp =>
		Transform.Basis.Column1;

	public Vector3 CameraRelativeForward =>
		camera.GlobalTransform.Basis.Column2;

    public override void _EnterTree () {
    
		Instance = this;

    }

    public override void _Ready () {

		collisionShape = GetNode<CollisionShape3D>("CollisionShape3D");
		camera = GetViewport().GetCamera3D();

		CalculateStartingProperties();

	}

	private void CalculateStartingProperties () {

		Vector3 bounds = GetCollisionShapeBounds();

		Mass = Math.Clamp(maxMass * fillAmount, 0.01f, maxMass);
		GD.Print(Mass);

		centerOfMass = new Vector3(
			0f,
			bounds.Y * fillAmount / -2f,
			0f
		);

		CenterOfMass = centerOfMass;

	}

    public override void _PhysicsProcess ( double delta ) {

		if ( Input.IsActionPressed("flip_bottle") )
			ChargeForce(delta);

		if ( Input.IsActionJustReleased("flip_bottle") )
			ReleaseForce();

		CenterOfMass = centerOfMass * RelativeUp;

		// GD.Print($"Charged Force: {chargedForce}");

    }

	private void ChargeForce ( double delta ) {

		if ( chargedForce == Vector2.Zero )
			chargeSign = 1f;
		
		if ( chargedForce == maxForce )
			chargeSign = -1f;

		chargedForce.Y += maxForce.Y * ((float)delta / chargeTime) * chargeSign;
		chargedForce.X += maxForce.X * ((float)delta / chargeTime) * chargeSign;

		chargedForce.Y = Math.Clamp(chargedForce.Y, 0f, maxForce.Y);
		chargedForce.X = Math.Clamp(chargedForce.X, 0f, maxForce.X);

	}

	private void ReleaseForce () {

		Vector3 force = new Vector3(
			-CameraRelativeForward.X * chargedForce.X,
			chargedForce.Y,
			-CameraRelativeForward.Z * chargedForce.X
		);

		//Vector3 position = forcePosition * RelativeUp;

		Vector3 position = CalculateForcePosition();

		LinearVelocity = Vector3.Zero;
		AngularVelocity = Vector3.Zero;

		ApplyImpulse(force, position);

		chargedForce = Vector2.Zero;

	}

	private Vector3 CalculateForcePosition () {

		Vector3 bounds = GetCollisionShapeBounds();
		Vector3 position = Vector3.Zero;

		switch (flipType) {

			case FlipType.Cap:
				position = bounds / 2f * RelativeUp;
				break;

			case FlipType.Base:
				position = bounds / 2f * -RelativeUp;
				break;

		}

		return position;

	}

	public Vector3 GetCollisionShapeBounds () {

		if ( collisionShape.Shape is CylinderShape3D ) {

			CylinderShape3D cylinder = (CylinderShape3D)collisionShape.Shape;
			
			return new Vector3(
				cylinder.Radius,
				cylinder.Height,
				cylinder.Radius
			);

		} else if ( collisionShape.Shape is BoxShape3D ) {

			BoxShape3D box = (BoxShape3D)collisionShape.Shape;
			return box.Size;

		}

		return Vector3.Zero;

	}

}