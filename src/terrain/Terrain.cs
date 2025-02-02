namespace GodotPlayground;

using System;
using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;
using Godot;

public interface ITerrain : INode2D { }

[Meta(typeof(IAutoNode))]
public partial class Terrain : Node2D, ITerrain {
  #region Exports
  #endregion

  #region Nodes
  #endregion

  #region Provisions
  #endregion

  #region Dependencies
  #endregion

  #region State
  private TerrainLogic Logic { get; set; } = default!;
  private TerrainLogic.IBinding Binding { get; set; } = default!;
  #endregion

  #region Dependency Lifecycle
  public void Setup() => Logic = new();

  public void OnResolved() {
    Binding = Logic.Bind();

    // Bind functions to state outputs here

    Logic.Start();
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

  #region Input Callbacks
  #endregion

  #region Output Callbacks
  #endregion
}

public interface ITerrainLogic : ILogicBlock<TerrainLogic.State>;

[Meta]
[LogicBlock(typeof(State), Diagram = true)]
public partial class TerrainLogic
  : LogicBlock<TerrainLogic.State>,
    ITerrainLogic {
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
