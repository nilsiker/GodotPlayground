namespace GodotPlayground;
public partial class GameLogic
{
    public partial record State
    {
        public partial record LeavingMainMenu : State { }
    }
}
