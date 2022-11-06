
// Type: Core.xml.Card
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.enums.item;
using Core.models.enums.missions;

namespace Core.xml
{
  public class Card
  {
    public ClassType _weaponReq;
    public MISSION_TYPE _missionType;
    public int _missionId;
    public int _mapId;
    public int _weaponReqId;
    public int _missionLimit;
    public int _missionBasicId;
    public int _cardBasicId;
    public int _arrayIdx;
    public int _flag;

    public Card(int cardBasicId, int missionBasicId)
    {
      this._cardBasicId = cardBasicId;
      this._missionBasicId = missionBasicId;
      this._arrayIdx = this._cardBasicId * 4 + this._missionBasicId;
      this._flag = 15 << 4 * this._missionBasicId;
    }
  }
}
