
namespace GodotPlayground;

using System;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;
using Godot;

public interface ICameraLogic : ILogicBlock<CameraLogic.State>;

[Meta]
[LogicBlock(typeof(State), Diagram = true)]
public partial class CameraLogic
  : LogicBlock<CameraLogic.State>,
    ICameraLogic {
  public override Transition GetInitialState() => To<State>();

  public class Data {
    public INode2D? Target;
  }

  public static class Input {
    public record struct Tick(float Delta);
    public record struct PhysicsTick(float Delta);
    public record struct Follow(INode2D Target);
    public record struct Unfollow;
  }

  public static class Output {
    public record struct TargetGlobalPositionUpdated(Vector2 GlobalPosition);
    public record struct Shaken(float Intensity, float Duration, float Rate);
  }

  public partial record State : StateLogic<State>, IGet<Input.Follow> {
    public State() {
      OnAttach(() => {
        var repo = Get<ICameraRepo>();
        repo.ShakeRequested += OnCameraShakeRequested;
        repo.TargetFollowed += OnCameraTargetFollowed;
      });
      OnDetach(() => {
        var repo = Get<ICameraRepo>();
        repo.ShakeRequested -= OnCameraShakeRequested;
        repo.TargetFollowed -= OnCameraTargetFollowed;
      });
    }

    private void OnCameraTargetFollowed(INode2D target) => Input(new Input.Follow(target));
    private void OnCameraShakeRequested(float intensity, float duration, float rate) =>
      Output(new Output.Shaken(intensity, duration, rate));

    public Transition On(in Input.Follow input) {
      Get<Data>().Target = input.Target;
      return To<Following>();
    }
  }
}
