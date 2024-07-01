using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class ObjectiveHandler : Node3D {

	public static ObjectiveHandler Instance { get; private set; }

	[ExportGroup("Objectives")]
	[Export] public Array<Objective> objectives = new Array<Objective>();
	[Export] public Area3D objectiveMarker;

	public delegate void OnObjectiveChangedDelegate ( Objective objective );
	public event OnObjectiveChangedDelegate OnObjectiveChanged;

	[ExportGroup("Parameters")]
	private Objective currentObjective;
	private int currentObjectiveIndex = 0;

	public Objective CurrentObjective =>
		currentObjective;

    public override void _EnterTree () {

        Instance = this;

    }

    public override void _Ready () {

		currentObjective = objectives[currentObjectiveIndex];
		SetObjectiveMarkerPosition();

		OnObjectiveChanged?.Invoke(currentObjective);

	}

	public void NextObjective () {

		if ( currentObjectiveIndex < objectives.Count - 1 )
			currentObjectiveIndex++;

		currentObjective = objectives[currentObjectiveIndex];
		SetObjectiveMarkerPosition();

		OnObjectiveChanged?.Invoke(currentObjective);

	}

	private void SetObjectiveMarkerPosition () {

		Node3D marker = (Node3D)GetNode(currentObjective.marker);
		objectiveMarker.Position = marker.Position;

	}

}