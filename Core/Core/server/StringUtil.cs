
// Type: Core.server.StringUtil
// Assembly: Core, Version=0.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 263E2FBF-1098-4552-8FB3-F6A660503737
// Interprise: C:\Users\Cuzin\3,50Core.dll

using System.Text;

namespace Core.server
{
  public class StringUtil
  {
    private static StringBuilder builder;

    public StringUtil() => StringUtil.builder = new StringBuilder();

    public void AppendLine(string text) => StringUtil.builder.AppendLine(text);

    public string getString() => StringUtil.builder.Length == 0 ? StringUtil.builder.ToString() : StringUtil.builder.Remove(StringUtil.builder.Length - 1, 1).ToString();
  }
}
