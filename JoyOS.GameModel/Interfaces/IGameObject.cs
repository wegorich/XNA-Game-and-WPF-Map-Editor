using FarseerPhysics.Dynamics;

namespace JoyOS.GameModel
{
    public interface IGameObject
    {
        Category CollisionCategories { get; set; }
        Category CollidesWith { get; set; }
        Body Body { get; }
        float Health { get; set; }
        void Draw();
        void Update();
    }
}