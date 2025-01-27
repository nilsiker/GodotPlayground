namespace GodotPlayground;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;

public interface IGame : INode2D,
  IProvide<IBlackoutRepo>,
  IProvide<IGameRepo>,
  IProvide<ICameraRepo>;

[Meta(typeof(IAutoNode))]
public partial class Game : Node2D, IGame {
  #region Exports
  #endregion

  #region Nodes
  [Node] private Blackout Blackout { get; set; } = default!;
  [Node] private ICamera GameCamera { get; set; } = default!;
  [Node] private IPlayer Player { get; set; } = default!;
  #endregion

  #region Provisions
  IBlackoutRepo IProvide<IBlackoutRepo>.Value() => Blackout.BlackoutRepo;
  IGameRepo IProvide<IGameRepo>.Value() => GameRepo;
  ICameraRepo IProvide<ICameraRepo>.Value() => GameCamera.CameraRepo;
  #endregion

  #region Dependencies
  [Dependency] private IAppRepo AppRepo => this.DependOn<IAppRepo>();
  #endregion

  #region State
  private IGameRepo GameRepo { get; set; } = default!;
  private GameLogic Logic { get; set; } = default!;
  private GameLogic.IBinding Binding { get; set; } = default!;
  #endregion

  #region Dependency Lifecycle
  public void Setup() => Logic = new();

  public void OnResolved() {
    Binding = Logic.Bind();

    GameRepo = new GameRepo();

    Logic.Set(Blackout.BlackoutRepo);
    Logic.Set(GameRepo);

    // Bind functions to state outputs here
    Binding
      .Handle((in GameLogic.Output.RequestScene output) =>
         OnOutputRequestScene(output.ScenePath))
      .Handle((in GameLogic.Output.CheckSceneProgress output) =>
        OnOutputCheckSceneProgress(output.ScenePath))
      .Handle((in GameLogic.Output.ChangeScene output) =>
        OnOutputChangeScene(output.ScenePath));

    this.Provide();
    Logic.Start();
  }
  #endregion

  #region Input Callbacks
  #endregion

  #region Output Callbacks
  private static void OnOutputRequestScene(string scenePath) =>
    ResourceLoader.LoadThreadedRequest(scenePath);
  private static void OnOutputCheckSceneProgress(string scenePath) {
    Godot.Collections.Array progress = [];
    var res = ResourceLoader.LoadThreadedGetStatus(scenePath, progress);
    GD.Print($"{res}: {progress}");
  }
  private static void OnOutputChangeScene(string scenePath) {
    var scene = ResourceLoader.LoadThreadedGet(scenePath) as PackedScene;
    GD.Print(scene);
  }
  #endregion

  #region Godot Lifecycle
  public override void _Notification(int what) => this.Notify(what);

  public void OnReady() {
    SetProcess(true);
    SetPhysicsProcess(true);
  }

  public void OnProcess(double delta) { }

  public void OnPhysicsProcess(double delta) { }

  public void OnExitTree() {
    Logic.Stop();
    Binding.Dispose();
  }

  #endregion
}
