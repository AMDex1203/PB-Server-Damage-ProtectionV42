
// Type: Game.global.clientpacket.BATTLE_3329_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.room;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class BATTLE_3329_REC : ReceiveGamePacket
  {
    public BATTLE_3329_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        Room room = player._room;
        Logger.warning("BATTLE_3329_REC. (PlayerID: " + player.player_id.ToString() + "; Name: " + player.player_name + "; Room: " + (player._room != null ? player._room._roomId : -1).ToString() + "; Channel: " + player.channelId.ToString() + ")");
        if (room != null)
        {
          Logger.warning("Room3329; BOT: " + room.isBotMode().ToString());
          Slot slot = room.getSlot(player._slotId);
          if (slot != null)
            Logger.warning("SLOT Id: " + slot._id.ToString() + "; State: " + slot.state.ToString());
        }
        this._client.SendPacket((SendPacket) new A_3329_PAK());
      }
      catch (Exception ex)
      {
        Logger.info(ex.ToString());
      }
    }
  }
}
