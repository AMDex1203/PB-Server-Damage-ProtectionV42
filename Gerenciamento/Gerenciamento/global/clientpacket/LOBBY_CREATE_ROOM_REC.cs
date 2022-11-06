
// Type: Game.global.clientpacket.LOBBY_CREATE_ROOM_REC
// Assembly: pbserver_game, Version=1.0.7804.36737, Culture=neutral, PublicKeyToken=null
// MVID: 2C33C976-0912-46B3-A685-4C330D0AD5C2
// Interprise: C:\Users\Cuzin\3,50pbserver_game.exe

using Core;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;
using System;

namespace Game.global.clientpacket
{
  public class LOBBY_CREATE_ROOM_REC : ReceiveGamePacket
  {
    private uint erro;
    private Room room;
    private Account p;

    public LOBBY_CREATE_ROOM_REC(GameClient client, byte[] data) => this.makeme(client, data);

    public override void read()
    {
      this.p = this._client._player;
      Channel ch = this.p == null ? (Channel) null : this.p.getChannel();
      try
      {
        if (this.p == null || ch == null || (this.p.player_name.Length == 0 || this.p._room != null) || this.p._match != null)
        {
          this.erro = 2147483648U;
        }
        else
        {
          lock (ch._rooms)
          {
            for (int index = 0; index < 300; ++index)
            {
              if (ch.getRoom(index) == null)
              {
                this.room = new Room(index, ch);
                this.readD();
                this.room.name = this.readS(23);
                this.room.mapId = (int) this.readH();
                this.room.stage4v4 = this.readC();
                this.room.room_type = this.readC();
                if (this.room.room_type != (byte) 0)
                {
                  int num1 = (int) this.readC();
                  int num2 = (int) this.readC();
                  this.room.initSlotCount((int) this.readC());
                  int num3 = (int) this.readC();
                  this.room.weaponsFlag = this.readC();
                  this.room.random_map = this.readC();
                  this.room.special = this.readC();
                  bool flag = this.room.isBotMode();
                  if (flag && this.room._channelType == 4)
                  {
                    this.erro = 2147487869U;
                    return;
                  }
                  this.readS(33);
                  this.room.killtime = (int) this.readC();
                  int num4 = (int) this.readC();
                  int num5 = (int) this.readC();
                  int num6 = (int) this.readC();
                  this.room.limit = this.readC();
                  this.room.seeConf = this.readC();
                  this.room.autobalans = (int) this.readH();
                  if (ch._type == 4)
                  {
                    this.room.limit = (byte) 1;
                    this.room.autobalans = 0;
                  }
                  this.room.password = this.readS(4);
                  if (flag)
                  {
                    this.room.aiCount = this.readC();
                    this.room.aiLevel = this.readC();
                  }
                  this.room.addPlayer(this.p);
                  this.p.ResetPages();
                  ch.AddRoom(this.room);
                  return;
                }
                break;
              }
            }
          }
          this.erro = 2147483648U;
        }
      }
      catch (Exception ex)
      {
        Logger.error("[ROOM_CREATE_REC] " + ex.ToString());
        this.erro = 2147483648U;
      }
    }

    public override void run() => this._client.SendPacket((SendPacket) new LOBBY_CREATE_ROOM_PAK(this.erro, this.room, this.p));
  }
}
