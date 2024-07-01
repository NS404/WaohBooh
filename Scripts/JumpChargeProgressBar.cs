using Godot;
using System;

public partial class JumpChargeProgressBar : ProgressBar {

	[ExportGroup("Components")]
	private BottleHandler player;

	public override void _Ready () {

		player = BottleHandler.Instance;

	}

    public override void _Process ( double delta ) {

        Value = (player.ChargedForce.Y / player.maxForce.Y) * 100f;

    }
	
}