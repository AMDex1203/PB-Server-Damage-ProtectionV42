
// Type: Game.data.chat.TakeTitles
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.managers;
using Core.models.account.players;
using Core.models.account.title;
using Core.server;
using Core.xml;
using Game.data.model;
using Game.global.serverpacket;
using System.Collections.Generic;

namespace Game.data.chat
{
  public static class TakeTitles
  {
    public static string GetAllTitles(Account p)
    {
      if (p._titles.ownerId == 0L)
      {
        TitleManager.getInstance().CreateTitleDB(p.player_id);
        p._titles = new PlayerTitles()
        {
          ownerId = p.player_id
        };
      }
      PlayerTitles titles = p._titles;
      int num = 0;
      for (int titleId = 1; titleId <= 44; ++titleId)
      {
        TitleQ title = TitlesXML.getTitle(titleId);
        if (title != null && !titles.Contains(title._flag))
        {
          ++num;
          titles.Add(title._flag);
          if (titles.Slots < title._slot)
            titles.Slots = title._slot;
        }
      }
      if (num > 0)
      {
        ComDiv.updateDB("player_titles", "titleslots", (object) titles.Slots, "owner_id", (object) p.player_id);
        TitleManager.getInstance().updateTitlesFlags(p.player_id, titles.Flags);
        p.SendPacket((SendPacket) new BASE_2626_PAK(p));
      }
      for (int titleId = 1; titleId <= 44; ++titleId)
      {
        TitleQ title = TitlesXML.getTitle(titleId);
        if (title != null)
        {
          List<ItemsModel> awards = TitleAwardsXML.getAwards(title._id);
          if (awards.Count > 0)
            p.SendPacket((SendPacket) new INVENTORY_ITEM_CREATE_PAK(1, p, awards));
        }
      }
      return Translation.GetLabel("TitleAcquisiton", (object) num);
    }
  }
}
