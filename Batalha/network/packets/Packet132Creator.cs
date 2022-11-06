
// Type: Battle.network.packets.Packet132Creator
// Assembly: pbserver_battle, Version=1.0.0.2646, Culture=neutral, PublicKeyToken=null
// MVID: AA2D7911-B687-4238-AB73-D37E54123039
// Interprise: C:\Users\Cuzin\3,50pbserver_battle.exe

using Battle.data;
using Battle.data.enums;
using Battle.data.models;
using Battle.network.actions.others;
using System;
using System.IO;

namespace Battle.network.packets
{
  public class Packet132Creator
  {
    public static byte[] getBaseData132(byte[] data)
    {
      ReceivePacket p = new ReceivePacket(data);
      using (SendPacket s = new SendPacket())
      {
        s.writeT(p.readT());
        for (int index = 0; index < 16; ++index)
        {
          ActionModel actionModel = new ActionModel();
          try
          {
            bool exception;
            actionModel._type = (P2P_SUB_HEAD) p.readC(out exception);
            if (!exception)
            {
              actionModel._slot = p.readUH();
              actionModel._lengthData = p.readUH();
              if (actionModel._lengthData != ushort.MaxValue)
              {
                s.writeC((byte) actionModel._type);
                s.writeH(actionModel._slot);
                s.writeH(actionModel._lengthData);
                if (actionModel._type == P2P_SUB_HEAD.GRENADE)
                  code1_GrenadeSync.writeInfo(s, p);
                else if (actionModel._type == P2P_SUB_HEAD.DROPEDWEAPON)
                  code2_WeaponSync.writeInfo(s, p);
                else if (actionModel._type == P2P_SUB_HEAD.OBJECT_STATIC)
                  code3_ObjectStatic.writeInfo(s, p);
                else if (actionModel._type == P2P_SUB_HEAD.OBJECT_ANIM)
                  code6_ObjectAnim.writeInfo(s, p);
                else if (actionModel._type == P2P_SUB_HEAD.STAGEINFO_OBJ_STATIC)
                  code9_StageInfoObjStatic.writeInfo(s, p, false);
                else if (actionModel._type == P2P_SUB_HEAD.STAGEINFO_OBJ_ANIM)
                  code12_StageObjAnim.writeInfo(s, p);
                else if (actionModel._type == P2P_SUB_HEAD.CONTROLED_OBJECT)
                  code13_ControledObj.writeInfo(s, p, false);
                else if (actionModel._type == P2P_SUB_HEAD.USER || actionModel._type == P2P_SUB_HEAD.STAGEINFO_CHARA)
                {
                  actionModel._flags = (Events) p.readUD();
                  actionModel._data = p.readB((int) actionModel._lengthData - 9);
                  s.writeD((uint) actionModel._flags);
                  s.writeB(actionModel._data);
                  if (actionModel._data.Length == 0 && actionModel._flags > (Events) 0)
                    break;
                }
                else
                {
                  Logger.warning("[New user packet type2 '" + actionModel._type.ToString() + "' or '" + ((int) actionModel._type).ToString() + "']: " + BitConverter.ToString(data));
                  throw new Exception("Unknown action type2");
                }
              }
              else
                break;
            }
            else
              break;
          }
          catch (Exception ex)
          {
            Logger.warning("B: " + BitConverter.ToString(data));
            Logger.warning(ex.ToString());
            s.mstream = new MemoryStream();
            break;
          }
        }
        return s.mstream.ToArray();
      }
    }

    public static byte[] getCode132(byte[] data, DateTime time, int round, int slot)
    {
      using (SendPacket sendPacket = new SendPacket())
      {
        byte[] baseData132 = Packet132Creator.getBaseData132(data);
        sendPacket.writeC((byte) 132);
        sendPacket.writeC((byte) slot);
        sendPacket.writeT(AllUtils.GetDuration(time));
        sendPacket.writeC((byte) round);
        sendPacket.writeH((ushort) (13 + baseData132.Length));
        sendPacket.writeD(0);
        sendPacket.writeB(AllUtils.encrypt(baseData132, (13 + baseData132.Length) % 6 + 1));
        return sendPacket.mstream.ToArray();
      }
    }
  }
}
