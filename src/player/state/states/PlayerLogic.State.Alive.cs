namespace GodotPlayground;

public partial class PlayerLogic {

  public abstract partial record State {
    public abstract partial record Alive : State, IGet<Input.Damage>, IGet<Input.Attack>, IGet<Input.GrabCamera> {
      public Alive() {
      }


      public Transition On(in Input.Damage input) {
        var data = Get<Data>();
        data.Health -= input.Amount;
        return data.Health > 0
          ? ToSelf()
          : To<Dead>();
      }

      public Transition On(in Input.Attack input) => To<Attacking>();
      public Transition On(in Input.GrabCamera input) {
        Get<ICameraRepo>().FollowTarget(input.Self);
        return ToSelf();
      }
    }
  }
}
