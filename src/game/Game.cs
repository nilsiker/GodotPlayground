namespace GodotPlayground;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;

public interface IGame : INode2D { }

[Meta(typeof(IAutoNode))]
public partial class Game : Node2D, IGame, IProvide<IBlackoutRepo> {
  #region Exports
  #endregion

  #region Nodes
  [Node]
  private Blackout Blackout { get; set; } = default!;
  #endregion

  #region Provisions
  IBlackoutRepo IProvide<IBlackoutRepo>.Value() => Blackout.BlackoutRepo;
  #endregion

  #region Dependencies
  [Dependency] private IAppRepo AppRepo => this.DependOn<IAppRepo>();
  #endregion

  #region State
  private GameLogic Logic { get; set; } = default!;
  private GameLogic.IBinding Binding { get; set; } = default!;
  #endregion

  #region Dependency Lifecycle
  public void Setup() => Logic = new();

  public void OnResolved() {
    Binding = Logic.Bind();

    Logic.Set(Blackout.BlackoutRepo);

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
