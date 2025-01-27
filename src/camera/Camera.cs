
namespace GodotPlayground;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;


public interface ICamera : ICamera2D, IProvide<ICameraRepo> {
  public ICameraRepo CameraRepo { get; }
}

[Meta(typeof(IAutoNode))]
public partial class Camera : Camera2D, ICamera {
  #region Exports
  #endregion

  #region Nodes
  #endregion

  #region Provisions
  ICameraRepo IProvide<ICameraRepo>.Value() => CameraRepo;
  #endregion

  #region Dependencies
  [Dependency] private IGameRepo GameRepo => this.DependOn<IGameRepo>();
  #endregion

  #region State
  public ICameraRepo CameraRepo { get; private set; } = default!;
  private CameraLogic Logic { get; set; } = default!;
  private CameraLogic.IBinding Binding { get; set; } = default!;
  #endregion

  #region Dependency Lifecycle
  public void Setup() => Logic = new();

  public void OnResolved() {
    Binding = Logic.Bind();
    CameraRepo = new CameraRepo();

    // Bind functions to state outputs here
    Binding
      .Handle((in CameraLogic.Output.TargetGlobalPositionUpdated output) => OnOutputTargetGlobalPositionUpdated(output.GlobalPosition))
      .Handle((in CameraLogic.Output.Shaken output) => OnOutputShaken(output.Intensity, output.Duration, output.Rate));

    Logic.Set(new CameraLogic.Data());
    Logic.Set(CameraRepo);
    Logic.Set(GameRepo);

    Logic.Start();
  }
  #endregion

  #region Input Callbacks
  #endregion

  #region Output Callbacks
  private void OnOutputTargetGlobalPositionUpdated(Vector2 globalPosition) => GlobalPosition = globalPosition;
  private Tween _shakeTween = default!;
  private void OnOutputShaken(float intensity, float duration, float rate) {
    if (_shakeTween is not null && _shakeTween.IsRunning()) {
      _shakeTween.Kill();
    }
    _shakeTween = CreateTween();

    for (float time = 0; time < duration; time += 1.0f / rate) {
      // Generate random values for the offset within the intensity range
      var randomX = (GD.RandRange(0, 2) - 1) * intensity;
      var randomY = (GD.RandRange(0, 2) - 1) * intensity;
      var shakeOffset = new Vector2(randomX, randomY);

      // Add a tween step for each point in time
      _shakeTween.TweenProperty(this, "offset", shakeOffset, 1.0f / rate);
    }

    // Return to the original offset at the end of the shake
    _shakeTween.TweenProperty(this, "offset", Vector2.Zero, 1.0f / rate);
  }
  #endregion


  #region Godot Lifecycle
  public override void _Notification(int what) => this.Notify(what);

  public void OnReady() {
    SetProcess(ProcessCallback == Camera2DProcessCallback.Idle);
    SetPhysicsProcess(ProcessCallback == Camera2DProcessCallback.Physics);
  }

  public void OnProcess(double delta) => Logic.Input(new CameraLogic.Input.Tick((float)delta));

  public void OnPhysicsProcess(double delta) => Logic.Input(new CameraLogic.Input.PhysicsTick((float)delta));

  public void OnExitTree() {
    Logic.Stop();
    Binding.Dispose();
  }

  #endregion
}
