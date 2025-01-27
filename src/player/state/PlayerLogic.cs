namespace GodotPlayground;

using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;
using Godot;

public interface IPlayerLogic : ILogicBlock<PlayerLogic.State>;

[Meta]
[LogicBlock(typeof(State), Diagram = true)]
public partial class PlayerLogic
  : LogicBlock<PlayerLogic.State>,
    IPlayerLogic {
  public override Transition GetInitialState() => To<State.Alive.Idle>();

  public static class Input {
    public record struct PhysicsTick(float Delta);
    public record struct UpdateMovementDirection(Vector2 Direction);
    public record struct Damage(int Amount);
    public record struct Attack;
    public record struct Lunge;
    public record struct AnimationFinished(StringName AnimationName);
    public record struct GrabCamera(INode2D Self);
  }

  public static class Output {
    public record struct VelocityUpdated(Vector2 Velocity);
    public record struct Moved;
    public record struct SpriteFlipped(bool IsFlipped);
    public record struct AnimationChanged(StringName AnimationName);

  }

  public abstract partial record State : StateLogic<State> {
    public State() {
      OnAttach(() => { });
      OnDetach(() => { });
    }

    protected void OutputSpriteFlipped() {
      var vel = Inputs.GetMoveInput();
      if (vel.X < 0) {
        Output(new Output.SpriteFlipped(true));
      }
      else if (vel.X > 0) {
        Output(new Output.SpriteFlipped(false));
      }
    }
  }
}
