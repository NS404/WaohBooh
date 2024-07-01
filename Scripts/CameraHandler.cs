using Godot;
using System;

public partial class CameraHandler : Node3D {

    [ExportGroup("Parameters")]
    [Export] public Vector3 playerOffset;
    [Export] public float playerDistance;
    [Export] public float lerpWeight = 5f;
    [Export] public float mouseSensitivity = 10f;

    private Vector2 mouse;
    private Vector2 currentRotation;

    public Vector3 RelativeRight =>
        Transform.Basis.Column0;

    public Vector3 RelativeUp =>
        Transform.Basis.Column1;

    public Vector3 RelativeForward =>
        Transform.Basis.Column2;

    [ExportGroup("Components")]
    private BottleHandler player;
    private Camera3D camera;
    private CameraRaycastHandler cameraRaycast;

    public override void _Ready () {

        player = BottleHandler.Instance;

        Input.MouseMode = Input.MouseModeEnum.Captured;

        camera = GetNode<Camera3D>("Camera3D");
        cameraRaycast = GetNode<CameraRaycastHandler>("RayCast3D");

    }

    public override void _Input ( InputEvent @event ) {

        if ( @event is InputEventMouseMotion ) {

            InputEventMouseMotion mouseMotion = (InputEventMouseMotion)@event;

            RotationDegrees -= new Vector3(
                mouseMotion.Relative.Y * mouseSensitivity * (float)GetProcessDeltaTime(),
                mouseMotion.Relative.X * mouseSensitivity * (float)GetProcessDeltaTime(),
                0f
            );

        }

    }

    public override void _Process ( double delta ) {

        Position = player.Position + playerOffset;

        float distance = cameraRaycast.GetDistance();
        camera.Position = new Vector3(0f, 0f, distance);

    }

}