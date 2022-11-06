
// Type: Game.global.clientpacket.BASE_TITLE_DETACH_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account.title;
using Core.server;
using Core.xml;
using Game.data.model;
using Game.data.utils;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class BASE_TITLE_DETACH_REC : ReceiveGamePacket
  {
    private int slotIdx;
    private uint erro;

    public BASE_TITLE_DETACH_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read() => this.slotIdx = (int) this.readH();

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null || this.slotIdx >= 3 || player._titles == null)
          return;
        PlayerTitles titles = player._titles;
        int equip = titles.GetEquip(this.slotIdx);
        if (equip > 0 && TitleManager.getInstance().updateEquipedTitle(titles.ownerId, this.slotIdx, 0))
        {
          titles.SetEquip(this.slotIdx, 0);
          if (TitleAwardsXML.Contains(equip, player._equip._beret) && ComDiv.updateDB("accounts", "char_beret", (object) 0, "player_id", (object) player.player_id))
          {
            player._equip._beret = 0;
            Room room = player._room;
            if (room != null)
              AllUtils.updateSlotEquips(player, room);
          }
        }
        else
          this.erro = 2147483648U;
        this._client.SendPacket((SendPacket) new BASE_TITLE_DETACH_PAK(this.erro));
      }
      catch (Exception ex)
      {
        Logger.info("BASE_TITLE_DETACH_REC: " + ex.ToString());
      }
    }
  }
}
