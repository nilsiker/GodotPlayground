namespace GodotPlayground;

public partial class PlayerLogic {

  public abstract partial record State {
    public partial record Dead : State { }
  }
}
