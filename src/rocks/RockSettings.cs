namespace GodotPlayground.Rocks;

using Godot;

[GlobalClass, Tool]
public partial class RockSettings : Resource {
  [Export] public bool Destructible = default!;
  [Export] public Texture2D Model = default!;
  [Export] public Vector2 ModelOffset = default!;
  [Export] public Shape2D ColliderShape = default!;
  [Export] public float ColliderRotation = default!;
}
