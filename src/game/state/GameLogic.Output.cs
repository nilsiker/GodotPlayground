namespace GodotPlayground;

using Godot;

public partial class GameLogic {
  public static class Output {
    public record struct RequestScene(string ScenePath);
    public record struct CheckSceneProgress(string ScenePath);
    public record struct ChangeScene(string ScenePath);
  }
}
