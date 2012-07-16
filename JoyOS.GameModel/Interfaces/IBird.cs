using JoyOS.System.Screen;

namespace JoyOS.GameModel
{
    public interface IBird : IGameObject
    {
        void Input(InputHelper input);
    }
}