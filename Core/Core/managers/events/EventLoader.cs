
// Type: Core.managers.events.EventLoader
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

namespace Core.managers.events
{
  public static class EventLoader
  {
    public static void LoadAll()
    {
      EventVisitSyncer.Load();
      EventLoginSyncer.Load();
      EventMapSyncer.Load();
      EventPlayTimeSyncer.Load();
      EventQuestSyncer.Load();
      EventRankUpSyncer.Load();
      EventXmasSyncer.Load();
    }

    public static void ReloadEvent(int index)
    {
      switch (index)
      {
        case 0:
          EventVisitSyncer.ReGenerateList();
          break;
        case 1:
          EventLoginSyncer.ReGenerateList();
          break;
        case 2:
          EventMapSyncer.ReGenerateList();
          break;
        case 3:
          EventPlayTimeSyncer.ReGenerateList();
          break;
        case 4:
          EventQuestSyncer.ReGenerateList();
          break;
        case 5:
          EventRankUpSyncer.ReGenerateList();
          break;
        case 6:
          EventXmasSyncer.ReGenerateList();
          break;
      }
    }
  }
}
