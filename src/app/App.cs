namespace GodotPlayground;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;

public interface IApp : INode, IProvide<IAppRepo> { }

[Meta(typeof(IAutoNode))]
public partial class App : Node, IApp {
  #region Exports
  #endregion

  #region Nodes
  #endregion

  #region Provisions
  IAppRepo IProvide<IAppRepo>.Value() => AppRepo;
  #endregion

  #region Dependencies
  #endregion

  #region State
  private IAppRepo AppRepo { get; set; } = default!;
  private AppLogic Logic { get; set; } = default!;
  private AppLogic.IBinding Binding { get; set; } = default!;
  #endregion

  #region Dependency Lifecycle
  public void Setup() => Logic = new();

  public void OnResolved() {
    Binding = Logic.Bind();

    // Bind functions to state outputs here
    Binding
    .Handle((in AppLogic.Output.ToggleFullscreen _) => OnOutputToggleFullscreen())
    .Handle((in AppLogic.Output.QuitApplication _) => OnOutputQuitApplication());

    AppRepo = new AppRepo();
    Logic.Set(AppRepo);


    this.Provide();
    Logic.Start();
  }

  #endregion

  #region Input Callbacks
  #endregion


  #region Output Callbacks
  private static void OnOutputToggleFullscreen() {
    if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen) {
      DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
    }
    else {
      DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
    }
  }
  private void OnOutputQuitApplication() => GetTree().Quit();
  #endregion


  #region Godot Lifecycle
  public override void _Notification(int what) => this.Notify(what);

  public void OnReady() {
    SetProcess(true);
    SetPhysicsProcess(true);
  }

  public void OnProcess(double delta) { }

  public void OnPhysicsProcess(double delta) { }

  public override void _UnhandledInput(InputEvent @event) {
    if (@event.IsActionPressed("toggle_fullscreen")) {
      Logic.Input(new AppLogic.Input.RequestFullscreenToggle());
    }
  }

  public void OnExitTree() {
    Logic.Stop();
    Binding.Dispose();
  }
  #endregion
}
