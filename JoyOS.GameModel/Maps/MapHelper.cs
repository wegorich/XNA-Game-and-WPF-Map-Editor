using System.Collections.Generic;
using System.Linq;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace JoyOS.GameModel.Maps
{
    public static class MapHelper
    {
        public static IBird LoadMapToGame(Map m, World world, IObjectFactory f, List<IGameObject> obj)
        {
            world.Gravity = m.Gravity;

            List<IBird> birds = m.Birds.Select(f.CreateBird).ToList();
            obj.AddRange(m.Pigs.Select(f.CreatePig));
            obj.AddRange(m.Blocks.Select(f.CreateBlock));
            obj.AddRange(birds);

            return birds[0];
        }

        public static Body SetBorder(Map m, World world, int width, int height)
        {
            //createBorder
            var borders = new Vertices(4)
                              {
                                  new Vector2(0, height),
                                  new Vector2(width, height),
                                  new Vector2(width, 0),
                                  new Vector2(0, 0)
                              };
            Body borderBody = BodyFactory.CreateLoopShape(world, borders);
            borderBody.CollisionCategories = Category.All;
            borderBody.CollidesWith = Category.All;
            return borderBody;
        }
    }
}