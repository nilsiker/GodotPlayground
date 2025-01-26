namespace GodotPlayground;

using Chickensoft.LogicBlocks;

public partial class PlayerLogic {

  public abstract partial record State {
    public partial record Alive {
      public partial record Idle : Alive, IGet<Input.UpdateMovementDirection> {
        public Idle() {
          this.OnEnter(() => Output(new Output.AnimationChanged("idle")));
        }

        public Transition On(in Input.UpdateMovementDirection input) {
          var data = Get<Data>();
          data.DesiredVelocity = data.Speed * input.Direction;

          return data.DesiredVelocity.IsZeroApprox()
            ? ToSelf()
            : To<Moving>();
        }
      }
    }
  }
}
