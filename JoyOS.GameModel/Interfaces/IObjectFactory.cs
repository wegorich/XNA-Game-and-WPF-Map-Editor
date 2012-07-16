using System.Collections.Generic;
using JoyOS.GameModel.Setting;

namespace JoyOS.GameModel
{
    public interface IObjectFactory
    {
        Dictionary<object, ObjectSetting> Setting { get; }
        IBird CreateBird(BirdSetting s);
        IGameObject CreatePig(PigSetting s);
        IGameObject CreateBlock(BlockSetting s);
    }
}