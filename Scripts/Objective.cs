using Godot;
using System;

[GlobalClass]
public partial class Objective : Resource {

    [Export]
    public NodePath marker;

    [Export]
    public string description;

}