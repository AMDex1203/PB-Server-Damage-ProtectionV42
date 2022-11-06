
// Type: Game.data.model.SLOT_MATCH
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core.models.enums.match;

namespace Game.data.model
{
  public class SLOT_MATCH
  {
    public SlotMatchState state;
    public long _playerId;
    public long _id;

    public SLOT_MATCH(int slot) => this._id = (long) slot;
  }
}
