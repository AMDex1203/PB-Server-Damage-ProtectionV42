
// Type: Game.global.clientpacket.BATTLE_DEATH_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.models.enums;
using Core.models.enums.missions;
using Core.models.enums.room;
using Core.models.room;
using Core.server;
using Game.data.model;
using Game.data.sync.client_side;
using Game.data.utils;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class BATTLE_DEATH_REC : ReceiveGamePacket
  {
    private FragInfos kills = new FragInfos();
    private bool isSuicide;

    public BATTLE_DEATH_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.kills.killingType = (CharaKillType) this.readC();
      this.kills.killsCount = this.readC();
      this.kills.killerIdx = this.readC();
      this.kills.weapon = this.readD();
      this.kills.x = this.readT();
      this.kills.y = this.readT();
      this.kills.z = this.readT();
      this.kills.flag = this.readC();
      for (int index = 0; index < (int) this.kills.killsCount; ++index)
      {
        Frag frag = new Frag();
        frag.victimWeaponClass = this.readC();
        frag.SetHitspotInfo(this.readC());
        int num = (int) this.readH();
        frag.flag = this.readC();
        frag.x = this.readT();
        frag.y = this.readT();
        frag.z = this.readT();
        this.kills.frags.Add(frag);
        if (frag.VictimSlot == (int) this.kills.killerIdx)
          this.isSuicide = true;
      }
    }

    public override void run()
    {
      try
      {
        Account player = this._client._player;
        if (player == null)
          return;
        Room room = player._room;
        if (room == null || room.round.Timer != null || room._state < RoomState.Battle)
          return;
        bool isBotMode = room.isBotMode();
        Slot slot = room.getSlot((int) this.kills.killerIdx);
        if (slot == null || !isBotMode && (slot.state < SLOT_STATE.BATTLE || slot._id != player._slotId))
          return;
        int score;
        Net_Room_Death.RegistryFragInfos(room, slot, out score, isBotMode, this.isSuicide, this.kills);
        if (isBotMode)
        {
          slot.Score += slot.killsOnLife + (int) room.IngameAiLevel + score;
          if (slot.Score > (int) ushort.MaxValue)
          {
            slot.Score = (int) ushort.MaxValue;
            Logger.LogHack("[Player: " + player.player_name + "; Id: " + this._client.player_id.ToString() + "] chegou a pontuação máxima do modo BOT.");
          }
          this.kills.Score = slot.Score;
        }
        else
        {
          slot.Score += score;
          AllUtils.CompleteMission(room, player, slot, this.kills, MISSION_TYPE.NA, 0);
          this.kills.Score = score;
        }
        using (BATTLE_DEATH_PAK battleDeathPak = new BATTLE_DEATH_PAK(room, this.kills, slot, isBotMode))
          room.SendPacketToPlayers((SendPacket) battleDeathPak, SLOT_STATE.BATTLE, 0);
        Net_Room_Death.EndBattleByDeath(room, slot, isBotMode, this.isSuicide);
      }
      catch (Exception ex)
      {
        Logger.info("BATTLE_DEATH_REC: " + ex.ToString());
        this._client.Close(0);
      }
    }
  }
}
