using Godot;
using System;

public partial class CanvasHandler : Control {

	public static CanvasHandler Instance { get; private set; }

	public override void _EnterTree () {

		Instance = this;

	}
	
}