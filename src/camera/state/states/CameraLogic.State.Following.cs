
namespace GodotPlayground;

using Chickensoft.GodotNodeInterfaces;

public partial class CameraLogic {

  public partial record State {
    public record Following : State, IGet<Input.Tick>, IGet<Input.PhysicsTick>, IGet<Input.Unfollow> {
      public Transition On(in Input.PhysicsTick input) {
        if (Get<Data>().Target is INode2D target) {
          Output(new Output.TargetGlobalPositionUpdated(target.GlobalPosition));
        }
        return ToSelf();
      }

      public Transition On(in Input.Tick input) {
        if (Get<Data>().Target is INode2D target) {
          Output(new Output.TargetGlobalPositionUpdated(target.GlobalPosition));
        }
        return ToSelf();
      }

      public Transition On(in Input.Unfollow input) {
        Get<Data>().Target = null;
        return To<State>();
      }
    }
  }
}
