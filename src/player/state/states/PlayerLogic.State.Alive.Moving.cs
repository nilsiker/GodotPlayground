namespace GodotPlayground;

using Chickensoft.LogicBlocks;

public partial class PlayerLogic {

  public abstract partial record State {
    public abstract partial record Alive {
      public partial record Moving : Alive, IGet<Input.UpdateMovementDirection>, IGet<Input.PhysicsTick> {

        public Moving() {
          this.OnEnter(() => Output(new Output.AnimationChanged("run")));
        }

        public Transition On(in Input.UpdateMovementDirection input) {
          var data = Get<Data>();
          data.DesiredVelocity = input.Direction * data.Speed;
          return ToSelf();
        }

        public Transition On(in Input.PhysicsTick input) {
          var data = Get<Data>();
          data.LastVelocity = data.LastVelocity.MoveToward(data.DesiredVelocity, input.Delta * data.Speed * 6);

          OutputSpriteFlipped();

          Output(new Output.VelocityUpdated(data.LastVelocity));
          Output(new Output.Moved());


          return data.LastVelocity.IsZeroApprox()
            ? To<Idle>()
            : ToSelf();
        }
      }
    }
  }
}
