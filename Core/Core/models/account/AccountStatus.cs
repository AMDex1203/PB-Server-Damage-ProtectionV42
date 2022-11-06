
// Type: Core.models.account.AccountStatus
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.server;
using System;

namespace Core.models.account
{
  public class AccountStatus
  {
    public long player_id;
    public byte channelId;
    public byte roomId;
    public byte clanFId;
    public byte serverId;
    public byte[] buffer = new byte[4];

    public void ResetData(long player_id)
    {
      if (player_id == 0L)
        return;
      int channelId = (int) this.channelId;
      int roomId = (int) this.roomId;
      int clanFid = (int) this.clanFId;
      int serverId = (int) this.serverId;
      this.SetData(uint.MaxValue, player_id);
      if (channelId == (int) this.channelId && roomId == (int) this.roomId && clanFid == (int) this.clanFId && serverId == (int) this.serverId)
        return;
      ComDiv.updateDB("accounts", "status", (object) (long) uint.MaxValue, nameof (player_id), (object) player_id);
    }

    public void SetData(uint uintData, long pId) => this.SetData(BitConverter.GetBytes(uintData), pId);

    public void SetData(byte[] buffer, long pId)
    {
      this.player_id = pId;
      this.buffer = buffer;
      this.channelId = buffer[0];
      this.roomId = buffer[1];
      this.serverId = buffer[2];
      this.clanFId = buffer[3];
    }

    public void updateChannel(byte channelId)
    {
      this.channelId = channelId;
      this.buffer[0] = channelId;
      this.UpdateDB();
    }

    public void updateRoom(byte roomId)
    {
      this.roomId = roomId;
      this.buffer[1] = roomId;
      this.UpdateDB();
    }

    public void updateServer(byte serverId)
    {
      this.serverId = serverId;
      this.buffer[2] = serverId;
      this.UpdateDB();
    }

    public void updateClanMatch(byte clanFId)
    {
      this.clanFId = clanFId;
      this.buffer[3] = clanFId;
      this.UpdateDB();
    }

    private void UpdateDB() => ComDiv.updateDB("accounts", "status", (object) (long) BitConverter.ToUInt32(this.buffer, 0), "player_id", (object) this.player_id);
  }
}
