
// Type: Game.data.chat.CreateItem
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.account.players;
using Core.server;
using Game.data.managers;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.data.chat
{
    public static class CreateItem
    {
        public static string CreateItemYourself(string str, Account player)
        {
            int id = int.Parse(str.Substring(3));
            if (id < 100000000 || id == 1103003010 || id == 1003003010)
                return Translation.GetLabel("CreateItemWrongID");
            if (player == null)
                return Translation.GetLabel("CreateItemFail");
            int itemCategory = ComDiv.GetItemCategory(id);
            player.SendPacket((SendPacket)new INVENTORY_ITEM_CREATE_PAK(1, player, new ItemsModel(id, itemCategory, "Command item", itemCategory == 1 ? 1 : 3, 2592000U)));
            player.SendPacket((SendPacket)new SERVER_MESSAGE_ITEM_RECEIVE_PAK(0U));
            return Translation.GetLabel("CreateItemSuccess");
        }

        public static string CreateItemByNick(string str, Account player)
        {
            string[] strArray = str.Substring(str.IndexOf(" ") + 1).Split(' ');
            string text = strArray[0];
            int int32 = Convert.ToInt32(strArray[1]);
            if (int32 < 100000000 || int32 == 1103003010 || int32 == 1003003010)
                return Translation.GetLabel("CreateItemWrongID");
            Account account = AccountManager.getAccount(text, 1, 0);
            if (account == null)
                return Translation.GetLabel("CreateItemFail");
            if (account.player_id == player.player_id)
                return Translation.GetLabel("CreateItemUseOtherCMD");
            int itemCategory = ComDiv.GetItemCategory(int32);
            account.SendPacket((SendPacket)new INVENTORY_ITEM_CREATE_PAK(1, account, new ItemsModel(int32, itemCategory, "Command item", itemCategory == 3 ? 1 : 3, 1U)), false);
            account.SendPacket((SendPacket)new SERVER_MESSAGE_ITEM_RECEIVE_PAK(0U), false);
            return Translation.GetLabel("CreateItemSuccess");
        }

        public static string CreateItemById(string str, Account player)
        {
            string[] strArray = str.Substring(str.IndexOf(" ") + 1).Split(' ');
            int int32 = Convert.ToInt32(strArray[1]);
            long int64 = Convert.ToInt64(strArray[0]);
            if (int32 < 100000000 || int32 == 1103003010 || int32 == 1003003010)
                return Translation.GetLabel("CreateItemWrongID");
            Account account = AccountManager.getAccount(int64, 0);
            if (account == null)
                return Translation.GetLabel("CreateItemFail");
            if (account.player_id == player.player_id)
                return Translation.GetLabel("CreateItemUseOtherCMD");
            int itemCategory = ComDiv.GetItemCategory(int32);
            account.SendPacket((SendPacket)new INVENTORY_ITEM_CREATE_PAK(1, account, new ItemsModel(int32, itemCategory, "Command item", itemCategory == 3 ? 1 : 3, 1U)), false);
            account.SendPacket((SendPacket)new SERVER_MESSAGE_ITEM_RECEIVE_PAK(0U), false);
            return Translation.GetLabel("CreateItemSuccess");
        }

        public static string CreateGoldCupom(string str)
        {
            string[] strArray = str.Substring(str.IndexOf(" ") + 1).Split(' ');
            int int32 = Convert.ToInt32(strArray[1]);
            long int64 = Convert.ToInt64(strArray[0]);
            if (!int32.ToString().EndsWith("00"))
                return Translation.GetLabel("CreateSItemFail");
            if (int32 < 100 || int32 > 99999999)
                return Translation.GetLabel("CreateSItemWrongID");
            Account account = AccountManager.getAccount(int64, 0);
            if (account == null)
                return Translation.GetLabel("CreateItemFail");
            int itemId = ComDiv.createItemId(15, int32 / 1000000, int32 % 1000 / 100, int32 % 1000000 / 1000);
            account.SendPacket((SendPacket)new INVENTORY_ITEM_CREATE_PAK(1, account, new ItemsModel(itemId, 3, "Gold CMD item", 1, 1U)), false);
            account.SendPacket((SendPacket)new SERVER_MESSAGE_ITEM_RECEIVE_PAK(0U), false);
            return Translation.GetLabel("CreateSItemSuccess", (object)int32);
        }
    }
}
