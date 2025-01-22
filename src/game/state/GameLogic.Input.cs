namespace GodotPlayground;

using Godot;

public partial class GameLogic {
  public static class Input {
    public record struct RequestedSceneLoaded(string ScenePath);
    public record struct SceneReady;
  }
}
