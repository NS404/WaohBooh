using Godot;
using System;

public partial class ObjectiveLabel : Label {

	[ExportGroup("Components")]
	private ObjectiveHandler objectiveHandler;

    public override void _Ready () {

		objectiveHandler = ObjectiveHandler.Instance;
		objectiveHandler.OnObjectiveChanged += OnObjectiveChanged;

    }

    public override void _ExitTree () {

        objectiveHandler.OnObjectiveChanged += OnObjectiveChanged;

    }

    private void OnObjectiveChanged ( Objective objective ) {

		Text = objective.description;

	}

}