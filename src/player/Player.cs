namespace GodotPlayground;

using System;
using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;

public interface IPlayer : ICharacterBody2D { }

[Meta(typeof(IAutoNode)), SceneTree]
public partial class Player : CharacterBody2D, IPlayer {
  #region Exports
  #endregion

  #region Nodes
  #endregion

  #region Provisions
  #endregion

  #region Dependencies
  #endregion

  #region State
  private PlayerLogic Logic { get; set; } = default!;
  private PlayerLogic.IBinding Binding { get; set; } = default!;
  #endregion

  #region Dependency Lifecycle
  public void Setup() => Logic = new();

  public void OnResolved() {
    Binding = Logic.Bind();

    // Bind functions to state outputs here
    Binding
      .Handle((in PlayerLogic.Output.VelocityUpdated output) => OnOutputVelocityUpdated(output.Velocity))
      .Handle((in PlayerLogic.Output.Moved output) => OnOutputMove())
      .Handle((in PlayerLogic.Output.SpriteFlipped output) => OnOutputSpriteFlipped(output.IsFlipped))
      .Handle((in PlayerLogic.Output.AnimationChanged output) => OnOutputAnimationChanged(output.AnimationName));


    Logic.Set(new PlayerLogic.Data() {
      Health = 3,
      Speed = 100f,
      LastVelocity = Vector2.Zero
    });

    Logic.Start();
  }


  #region Input Callbacks
  private void OnAnimationFinished(StringName animName) => Logic.Input(new PlayerLogic.Input.AnimationFinished(animName));
  private void OnAnimEventLunge() {
    Logic.Input(new PlayerLogic.Input.Lunge());
    FootstepParticles.Restart();
  }
  #endregion


  #region Output Callbacks
  private void OnOutputVelocityUpdated(Vector2 velocity) {
    Velocity = velocity;
    FootstepParticles.Direction = -velocity;
  }


  private void OnOutputMove() => MoveAndSlide();
  private void OnOutputSpriteFlipped(bool isFlipped) => Model.FlipH = isFlipped;
  private void OnOutputAnimationChanged(StringName animationName) => AnimationPlayer.Play(animationName);

  #endregion
  #endregion


  #region Godot Lifecycle
  public override void _Notification(int what) => this.Notify(what);

  public void OnReady() {
    SetProcess(true);
    SetPhysicsProcess(true);

    AnimationPlayer.AnimationFinished += OnAnimationFinished;
  }



  public void OnProcess(double delta) { }
  public void OnPhysicsProcess(double delta) {
    Logic.Input(new PlayerLogic.Input.UpdateMovementDirection(Inputs.GetMoveInput()));

    Logic.Input(new PlayerLogic.Input.PhysicsTick((float)delta));
  }

  public override void _UnhandledInput(InputEvent @event) {
    if (@event.IsActionPressed(Inputs.Attack)) {
      Logic.Input(new PlayerLogic.Input.Attack());
      Inputs.Buffer(Inputs.Attack);
    }
  }


  public void OnExitTree() {
    Logic.Stop();
    Binding.Dispose();
  }
  #endregion
}
