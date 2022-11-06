
// Type: Core.managers.CupomEffectManager
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using Core.models.enums.flags;
using Core.sql;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Core.managers
{
  public static class CupomEffectManager
  {
    private static List<CupomFlag> Effects = new List<CupomFlag>();

    public static void LoadCupomFlags()
    {
      try
      {
        using (NpgsqlConnection npgsqlConnection = SQLjec.getInstance().conn())
        {
          NpgsqlCommand command = npgsqlConnection.CreateCommand();
          npgsqlConnection.Open();
          command.CommandText = "SELECT * FROM info_cupons_flags";
          command.CommandType = CommandType.Text;
          NpgsqlDataReader npgsqlDataReader = command.ExecuteReader();
          while (npgsqlDataReader.Read())
          {
            CupomFlag cupomFlag = new CupomFlag()
            {
              ItemId = npgsqlDataReader.GetInt32(0),
              EffectFlag = (CupomEffects) npgsqlDataReader.GetInt64(1)
            };
            CupomEffectManager.Effects.Add(cupomFlag);
          }
          command.Dispose();
          npgsqlDataReader.Close();
          npgsqlConnection.Dispose();
          npgsqlConnection.Close();
        }
      }
      catch (Exception ex)
      {
        Logger.error(ex.ToString());
      }
    }

    public static CupomFlag getCupomEffect(int id)
    {
      for (int index = 0; index < CupomEffectManager.Effects.Count; ++index)
      {
        CupomFlag effect = CupomEffectManager.Effects[index];
        if (effect.ItemId == id)
          return effect;
      }
      return (CupomFlag) null;
    }
  }
}
