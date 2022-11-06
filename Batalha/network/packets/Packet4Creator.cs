
// Type: Battle.network.packets.Packet4Creator
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data;
using Battle.data.enums;
using Battle.data.models;
using System;
using System.Collections.Generic;

namespace Battle.network.packets
{
  public class Packet4Creator
  {
    public static byte[] getCode4(byte[] actions, DateTime date, int round, int slot) => Packet4Creator.BaseGetCode4(AllUtils.encrypt(actions, (13 + actions.Length) % 6 + 1), date, round, slot);

    private static byte[] BaseGetCode4(byte[] actionsBuffer, DateTime date, int round, int slot)
    {
      using (SendPacket sendPacket = new SendPacket())
      {
        sendPacket.writeC((byte) 4);
        sendPacket.writeC((byte) slot);
        sendPacket.writeT(AllUtils.GetDuration(date));
        sendPacket.writeC((byte) round);
        sendPacket.writeH((ushort) (13 + actionsBuffer.Length));
        sendPacket.writeD(0);
        sendPacket.writeB(actionsBuffer);
        return sendPacket.mstream.ToArray();
      }
    }

    public static byte[] getCode4SyncData(List<ObjectHitInfo> objs)
    {
      using (SendPacket sendPacket = new SendPacket())
      {
        for (int index = 0; index < objs.Count; ++index)
        {
          ObjectHitInfo objectHitInfo = objs[index];
          if (objectHitInfo.syncType == 1)
          {
            if (objectHitInfo.objSyncId == 0)
            {
              sendPacket.writeC((byte) 3);
              sendPacket.writeH((ushort) objectHitInfo.objId);
              sendPacket.writeH((short) 8);
              sendPacket.writeH((ushort) objectHitInfo.objLife);
              sendPacket.writeC((byte) objectHitInfo.killerId);
            }
            else
            {
              sendPacket.writeC((byte) 6);
              sendPacket.writeH((ushort) objectHitInfo.objId);
              sendPacket.writeH((short) 13);
              sendPacket.writeH((ushort) objectHitInfo.objLife);
              sendPacket.writeC((byte) objectHitInfo._animId1);
              sendPacket.writeC((byte) objectHitInfo._animId2);
              sendPacket.writeT(objectHitInfo._specialUse);
            }
          }
          else if (objectHitInfo.syncType == 2)
          {
            Events events = Events.LifeSync;
            int num = 11;
            if (objectHitInfo.objLife == 0)
            {
              events |= Events.Death;
              num += 12;
            }
            sendPacket.writeC((byte) 0);
            sendPacket.writeH((ushort) objectHitInfo.objId);
            sendPacket.writeH((ushort) num);
            sendPacket.writeD((uint) events);
            sendPacket.writeH((ushort) objectHitInfo.objLife);
            if (events.HasFlag((Enum) Events.Death))
            {
              sendPacket.writeC((byte) (objectHitInfo.deathType + objectHitInfo.objId * 16));
              sendPacket.writeC((byte) objectHitInfo.hitPart);
              sendPacket.writeH((short) 0);
              sendPacket.writeH((short) 0);
              sendPacket.writeH((short) 0);
              sendPacket.writeD(objectHitInfo.weaponId);
            }
          }
          else if (objectHitInfo.syncType == 3)
          {
            if (objectHitInfo.objSyncId == 0)
            {
              sendPacket.writeC((byte) 9);
              sendPacket.writeH((ushort) objectHitInfo.objId);
              sendPacket.writeH((short) 6);
              sendPacket.writeC(objectHitInfo.objLife == 0);
            }
            else
            {
              sendPacket.writeC((byte) 12);
              sendPacket.writeH((ushort) objectHitInfo.objId);
              sendPacket.writeH((short) 14);
              sendPacket.writeC((byte) objectHitInfo._destroyState);
              sendPacket.writeH((ushort) objectHitInfo.objLife);
              sendPacket.writeT(objectHitInfo._specialUse);
              sendPacket.writeC((byte) objectHitInfo._animId1);
              sendPacket.writeC((byte) objectHitInfo._animId2);
            }
          }
          else if (objectHitInfo.syncType == 4)
          {
            sendPacket.writeC((byte) 8);
            sendPacket.writeH((ushort) objectHitInfo.objId);
            sendPacket.writeH((short) 11);
            sendPacket.writeD(256);
            sendPacket.writeH((ushort) objectHitInfo.objLife);
          }
          else if (objectHitInfo.syncType == 5)
          {
            sendPacket.writeC((byte) 0);
            sendPacket.writeH((short) objectHitInfo.objId);
            sendPacket.writeH((short) 11);
            sendPacket.writeD(524288);
            sendPacket.writeC((byte) (objectHitInfo.killerId + (int) objectHitInfo.deathType * 16));
            sendPacket.writeC((byte) objectHitInfo.objLife);
          }
        }
        return sendPacket.mstream.ToArray();
      }
    }
  }
}
