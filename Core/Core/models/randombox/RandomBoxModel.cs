
// Type: Core.models.randombox.RandomBoxModel
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System.Collections.Generic;

namespace Core.models.randombox
{
  public class RandomBoxModel
  {
    public int itemsCount;
    public int minPercent;
    public int maxPercent;
    public List<RandomBoxItem> items = new List<RandomBoxItem>();

    public List<RandomBoxItem> getRewardList(
      List<RandomBoxItem> sortedList,
      int rnd)
    {
      List<RandomBoxItem> randomBoxItemList = new List<RandomBoxItem>();
      if (sortedList.Count > 0)
      {
        int index1 = sortedList[rnd].index;
        for (int index2 = 0; index2 < sortedList.Count; ++index2)
        {
          RandomBoxItem sorted = sortedList[index2];
          if (sorted.index == index1)
            randomBoxItemList.Add(sorted);
        }
      }
      return randomBoxItemList;
    }

    public List<RandomBoxItem> getSortedList(int percent)
    {
      if (percent < this.minPercent)
        percent = this.minPercent;
      List<RandomBoxItem> randomBoxItemList = new List<RandomBoxItem>();
      for (int index = 0; index < this.items.Count; ++index)
      {
        RandomBoxItem randomBoxItem = this.items[index];
        if (percent <= randomBoxItem.percent)
          randomBoxItemList.Add(randomBoxItem);
      }
      return randomBoxItemList;
    }

    public void SetTopPercent()
    {
      int num1 = 100;
      int num2 = 0;
      for (int index = 0; index < this.items.Count; ++index)
      {
        RandomBoxItem randomBoxItem = this.items[index];
        if (randomBoxItem.percent < num1)
          num1 = randomBoxItem.percent;
        if (randomBoxItem.percent > num2)
          num2 = randomBoxItem.percent;
      }
      this.minPercent = num1;
      this.maxPercent = num2;
    }
  }
}
