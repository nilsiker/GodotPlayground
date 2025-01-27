namespace GodotPlayground;

using Chickensoft.LogicBlocks;
using Godot;

public partial class PlayerLogic {

  public abstract partial record State {
    public abstract partial record Alive {
      public record Attacking : Alive, IGet<Input.AnimationFinished>, IGet<Input.PhysicsTick>, IGet<Input.Lunge> {
        public Attacking() {
          this.OnEnter(() => {
            Output(new Output.AnimationChanged("attack"));
            OutputSpriteFlipped();
          });
        }

        public Transition On(in Input.AnimationFinished input) {
          Get<Data>().LastVelocity = Vector2.Zero;

          return Inputs.IsActionBuffered(Inputs.Attack)
            ? ToSelf().With(state => state.Enter())
            : To<Idle>();
        }

        public Transition On(in Input.Lunge input) {
          Get<ICameraRepo>().RequestShake(2, 0.1f, 20);

          var data = Get<Data>();
          data.LastVelocity = Inputs.GetMoveInput() * data.Speed * 1f;
          Output(new Output.VelocityUpdated(data.LastVelocity));
          OutputSpriteFlipped();
          return ToSelf();
        }

        public Transition On(in Input.PhysicsTick input) {
          var data = Get<Data>();
          data.LastVelocity = data.LastVelocity.MoveToward(Vector2.Zero, input.Delta * data.Speed * 3f);
          Output(new Output.VelocityUpdated(data.LastVelocity));
          Output(new Output.Moved());
          return ToSelf();
        }

      }
    }
  }
}
