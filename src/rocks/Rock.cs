namespace GodotPlayground.Rocks;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;
using Godot;

public interface IRock : INode2D { }

[Meta(typeof(IAutoNode)), Tool, SceneTree]
public partial class Rock : Node2D, IRock {
  private RockSettings? _settings;

  #region Exports
  [Export]
  private RockSettings Settings {
    get => _settings!;
    set {
      _settings = value;

      // NOTE observe so that this runs as expected,
      // avoids errors on project startup when nodes aren't initialized or visible.
      void update(RockSettings settings) {
        GetNode<Sprite2D>("%Model").Texture = _settings?.Model;
        GetNode<Sprite2D>("%Model").Offset = _settings?.ModelOffset ?? Vector2.Zero;
        GetNode<CollisionShape2D>("%Collider").Shape = _settings?.ColliderShape;
        GetNode<CollisionShape2D>("%Collider").RotationDegrees = _settings?.ColliderRotation ?? 0;
      }

      if (!IsNodeReady()) {
        ToSignal(this, Node.SignalName.Ready).OnCompleted(() => update(_settings));
      }
      else {
        update(_settings);
      }
    }
  }
  #endregion



  #region State
  private RockLogic Logic { get; set; } = default!;
  private RockLogic.IBinding Binding { get; set; } = default!;
  #endregion

  #region Dependency Lifecycle
  public void Setup() {
    if (Engine.IsEditorHint()) {
      return;
    }

    Logic = new();
  }

  public void OnResolved() {
    if (Engine.IsEditorHint()) {
      return;
    }

    Binding = Logic.Bind();

    // Bind functions to state outputs here

    Logic.Start();
  }

  #region Input Callbacks
  #endregion

  #region Output Callbacks
  #endregion
  #endregion

  #region Godot Lifecycle
  public override void _Notification(int what) => this.Notify(what);

  public void OnReady() { }

  public void OnExitTree() {
    if (Engine.IsEditorHint()) {
      return;
    }

    Logic.Stop();
    Binding.Dispose();
  }
  #endregion
}

public interface IRockLogic : ILogicBlock<RockLogic.State>;

[Meta]
[LogicBlock(typeof(State), Diagram = true)]
public partial class RockLogic
  : LogicBlock<RockLogic.State>,
    IRockLogic {
  public override Transition GetInitialState() => To<State>();

  public static class Input { }

  public static class Output { }

  public partial record State : StateLogic<State> {
    public State() {
      OnAttach(() => { });
      OnDetach(() => { });
    }
  }
}
