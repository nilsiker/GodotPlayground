namespace GodotPlayground;

using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;


[InputMap]
public static partial class Inputs {
  private static readonly HashSet<StringName> _actionBuffer = [];
  public static Vector2 GetMoveInput() => Input.GetVector(Left, Right, Up, Down);
  public static bool IsActionBuffered(StringName action) => _actionBuffer.Contains(action);
  public static void Buffer(StringName action) {
    _actionBuffer.Add(action);

    Task.Delay(200).ContinueWith((_) => {
      _actionBuffer.Remove(action);
    });
  }
}
