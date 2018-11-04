
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace hacbuild
{
    class Program
    {

        // Static stuff used in the program
        public static Random Rand = new Random();
        public static SHA256 SHA256 = SHA256CryptoServiceProvider.Create();
        public static Aes AES128CBC = Aes.Create();
        

        // Entrypoint
        static void Main(string[] args)
        {
            Console.WriteLine("");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Hacbuild by LucaFraga. Modded by JulesOnTheRoad. v1.1");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("github.com/julesontheroad/hacbuild");
            // Configure AES
            AES128CBC.BlockSize = 128;
            AES128CBC.Mode = CipherMode.CBC;
            AES128CBC.Padding = PaddingMode.Zeros;
            if (args.Length < 2)
            {
                PrintUsage();
                return;
            }


            switch (args[0])
            {
                case "read":
                    switch(args[1])
                    {
                        case "xci":
                            if (args.Length < 3)
                            {
                                PrintUsage();
                                return;
                            }
                            Console.WriteLine("Reading {0}", args[2]);
                            XCIManager.xci_header header =  XCIManager.GetXCIHeader(args[2]);
                            XCIManager.gamecard_info gcInfo = XCIManager.DecryptGamecardInfo(header);


                            Console.WriteLine(header.ToString());
                            Console.WriteLine(gcInfo.ToString());

                            Directory.CreateDirectory(args[3]);
                            string folder = args[3];
                            string iniPath = Path.Combine(folder, Path.GetFileNameWithoutExtension(args[2]) ) + ".ini";

                            IniFile ini = new IniFile(iniPath);

                            ini.Write("PackageID", header.PackageID.ToString(), "XCI_Header");
                            ini.WriteBytes("GamecardIV", header.GamecardIV, "XCI_Header");
                            ini.Write("KEKIndex", header.KEK.ToString(), "XCI_Header");
                            ini.WriteBytes("InitialDataHash", header.InitialDataHash, "XCI_Header");

                            ini.Write("Version", gcInfo.Version.ToString(), "GameCard_Info");
                            ini.Write("AccessControlFlags", gcInfo.AccessControlFlags.ToString(), "GameCard_Info");
                            ini.Write("ReadWaitTime", gcInfo.ReadWaitTime.ToString(), "GameCard_Info");
                            ini.Write("ReadWaitTime2", gcInfo.ReadWaitTime2.ToString(), "GameCard_Info");
                            ini.Write("WriteWriteTime", gcInfo.WriteWriteTime.ToString(), "GameCard_Info");
                            ini.Write("WriteWriteTime2", gcInfo.WriteWriteTime2.ToString(), "GameCard_Info");
                            ini.Write("FirmwareMode", gcInfo.FirmwareMode.ToString(), "GameCard_Info");
                            ini.Write("CUPVersion", gcInfo.CUPVersion.ToString(), "GameCard_Info");
                            ini.Write("CUPID", gcInfo.CUPID.ToString(), "GameCard_Info");
                            // end dump to ini
                            break;
                        default:
                            Console.WriteLine("Usage: hacbuild.exe read xci <IN>");
                            break;
                    }
                    break;

                case "gameinfo_ini":
                    switch (args[1])
                    {
                        case "xci":
                            if (args.Length < 3)
                            {
                                PrintUsage();
                                return;
                            }
                            Console.WriteLine("Reading {0}", args[2]);
                            XCIManager.xci_header header = XCIManager.GetXCIHeader(args[2]);
                            XCIManager.gamecard_info gcInfo = XCIManager.DecryptGamecardInfo(header);
                            
                            Console.WriteLine(header.ToString());
                            Console.WriteLine(gcInfo.ToString());

                            Directory.CreateDirectory(args[3]);
                            string folder = args[3];
                            string iniPath = folder + "/game_info.ini";

                            IniFile ini = new IniFile(iniPath);

                            ini.Write("PackageID", header.PackageID.ToString(), "XCI_Header");
                            ini.WriteBytes("GamecardIV", header.GamecardIV, "XCI_Header");
                            ini.Write("KEKIndex", header.KEK.ToString(), "XCI_Header");
                            ini.WriteBytes("InitialDataHash", header.InitialDataHash, "XCI_Header");

                            ini.Write("Version", gcInfo.Version.ToString(), "GameCard_Info");
                            ini.Write("AccessControlFlags", gcInfo.AccessControlFlags.ToString(), "GameCard_Info");
                            ini.Write("ReadWaitTime", gcInfo.ReadWaitTime.ToString(), "GameCard_Info");
                            ini.Write("ReadWaitTime2", gcInfo.ReadWaitTime2.ToString(), "GameCard_Info");
                            ini.Write("WriteWriteTime", gcInfo.WriteWriteTime.ToString(), "GameCard_Info");
                            ini.Write("WriteWriteTime2", gcInfo.WriteWriteTime2.ToString(), "GameCard_Info");
                            ini.Write("FirmwareMode", gcInfo.FirmwareMode.ToString(), "GameCard_Info");
                            ini.Write("CUPVersion", gcInfo.CUPVersion.ToString(), "GameCard_Info");
                            ini.Write("CUPID", gcInfo.CUPID.ToString(), "GameCard_Info");
                            // end dump to ini
                            break;
                        default:
                            Console.WriteLine("Usage: hacbuild.exe read xci <IN>");
                            break;
                    }
                    break;

                case "hfs0":
                    if (args.Length < 3)
                    {
                        PrintUsage();
                        return;
                    }
                    string multiplier = HFS0Manager.xci_multiplier(args[1]);
                    HFS0Manager.BuildHFS0(args[1], args[2], multiplier);
                    Console.WriteLine("Done");
                    break;
                case "root_hfs0":
                    if (args.Length < 3)
                    {
                        PrintUsage();
                        return;
                    }
                    HFS0Manager.BuildHFS0(args[1], args[2], args[3]);
                    break;
                case "xci":
                    if (args.Length < 3)
                    {
                        PrintUsage();
                        return;
                    }
                    XCIManager.BuildXCI(args[1], args[2]);
                    Console.WriteLine("Done");
                    break;
                case "xci_auto":
                    if (args.Length < 3)
                    {
                        PrintUsage();
                        return;
                    }
                    string inPath = Path.Combine(Environment.CurrentDirectory,  args[1]);
                    string outPath = Path.Combine(Environment.CurrentDirectory, args[2]);
                    string tmpPath = Path.Combine(inPath, "root_tmp");
                    Directory.CreateDirectory(tmpPath);
                    Console.WriteLine("....................................");
                    Console.WriteLine("Preparing to build secure partition");
                    Console.WriteLine("....................................");
                    string multiplier1 = HFS0Manager.xci_multiplier(Path.Combine(inPath, "secure"));
                    HFS0Manager.BuildHFS0(Path.Combine(inPath, "secure"), Path.Combine(tmpPath, "secure"), multiplier1);
                    Console.WriteLine("....................................");
                    Console.WriteLine("Preparing to build normal partition");
                    Console.WriteLine("....................................");
                    string multiplier2 = HFS0Manager.xci_multiplier(Path.Combine(inPath, "normal"));
                    HFS0Manager.BuildHFS0(Path.Combine(inPath, "normal"), Path.Combine(tmpPath, "normal"), multiplier2);
                    Console.WriteLine("....................................");
                    Console.WriteLine("Preparing to build update partition");
                    Console.WriteLine("....................................");
                    string multiplier3 = HFS0Manager.xci_multiplier(Path.Combine(inPath, "update"));
                    HFS0Manager.BuildHFS0(Path.Combine(inPath, "update"), Path.Combine(tmpPath, "update"), multiplier3);
                    if (Directory.Exists(Path.Combine(inPath, "logo"))) {
                        Console.WriteLine("....................................");
                        Console.WriteLine("Preparing to build logo partition");
                        Console.WriteLine("....................................");
                        string multiplier4 = HFS0Manager.xci_multiplier(Path.Combine(inPath, "logo"));
                        HFS0Manager.BuildHFS0(Path.Combine(inPath, "logo"), Path.Combine(tmpPath, "logo"), multiplier4);
                    }
                    Console.WriteLine("....................................");
                    Console.WriteLine("Preparing to build root.hfs0");
                    Console.WriteLine("....................................");
                    HFS0Manager.BuildHFS0(tmpPath, Path.Combine(inPath, "root.hfs0"), multiplier1);
                    Directory.Delete(tmpPath, true);
                    Console.WriteLine("....................................");
                    Console.WriteLine("Building xci");
                    Console.WriteLine("....................................");
                    XCIManager.BuildXCI(inPath, outPath);
                    Console.WriteLine("DONE");
                    Console.WriteLine("....................................");
                    Console.WriteLine("Erasing root.hfs0");
                    Console.WriteLine("....................................");
                    File.Delete(Path.Combine(inPath, "root.hfs0"));
                    Console.WriteLine("DONE");
                    break;
                case "xci_auto_del":
                    if (args.Length < 3)
                    {
                        PrintUsage();
                        return;
                    }
                    string inPath_d = Path.Combine(Environment.CurrentDirectory, args[1]);
                    string outPath_d = Path.Combine(Environment.CurrentDirectory, args[2]);
                    string tmpPath_d = Path.Combine(inPath_d, "root_tmp");
                    Directory.CreateDirectory(tmpPath_d);
                    Console.WriteLine("....................................");
                    Console.WriteLine("Preparing to build secure partition");
                    Console.WriteLine("....................................");
                    string multiplier1_d = HFS0Manager.xci_multiplier(Path.Combine(inPath_d, "secure"));
                    HFS0Manager.BuildHFS0(Path.Combine(inPath_d, "secure"), Path.Combine(tmpPath_d, "secure"), multiplier1_d);
                    Directory.Delete(Path.Combine(inPath_d, "secure"), true);
                    Console.WriteLine("....................................");
                    Console.WriteLine("Preparing to build normal partition");
                    Console.WriteLine("....................................");
                    string multiplier2_d = HFS0Manager.xci_multiplier(Path.Combine(inPath_d, "normal"));
                    HFS0Manager.BuildHFS0(Path.Combine(inPath_d, "normal"), Path.Combine(tmpPath_d, "normal"), multiplier2_d);
                    Console.WriteLine("....................................");
                    Directory.Delete(Path.Combine(inPath_d, "normal"), true);
                    Console.WriteLine("Preparing to build update partition");
                    Console.WriteLine("....................................");
                    string multiplier3_d = HFS0Manager.xci_multiplier(Path.Combine(inPath_d, "update"));
                    HFS0Manager.BuildHFS0(Path.Combine(inPath_d, "update"), Path.Combine(tmpPath_d, "update"), multiplier3_d);
                    Directory.Delete(Path.Combine(inPath_d, "update"), true);
                    if (Directory.Exists(Path.Combine(inPath_d, "logo")))
                    {
                        Console.WriteLine("....................................");
                        Console.WriteLine("Preparing to build logo partition");
                        Console.WriteLine("....................................");
                        string multiplier4 = HFS0Manager.xci_multiplier(Path.Combine(inPath_d, "logo"));
                        HFS0Manager.BuildHFS0(Path.Combine(inPath_d, "logo"), Path.Combine(tmpPath_d, "logo"), multiplier4);
                        Directory.Delete(Path.Combine(inPath_d, "logo"), true);
                    }
                    Console.WriteLine("....................................");
                    Console.WriteLine("Preparing to build root.hfs0");
                    Console.WriteLine("....................................");
                    HFS0Manager.BuildHFS0(tmpPath_d, Path.Combine(inPath_d, "root.hfs0"), multiplier1_d);
                    Directory.Delete(tmpPath_d, true);
                    Console.WriteLine("....................................");
                    Console.WriteLine("Building xci");
                    Console.WriteLine("....................................");
                    XCIManager.BuildXCI(inPath_d, outPath_d);
                    Console.WriteLine("DONE");
                    Console.WriteLine("....................................");
                    Console.WriteLine("Erasing root.hfs0");
                    Console.WriteLine("....................................");
                    File.Delete(Path.Combine(inPath_d, "root.hfs0"));
                    Console.WriteLine("DONE");
                    break;
                case "rhfs0_auto":
                    if (args.Length < 2)
                    {
                        PrintUsage();
                        return;
                    }
                    string inPath_rh = Path.Combine(Environment.CurrentDirectory, args[1]);
                    string tmpPath_rh = Path.Combine(inPath_rh, "root_tmp");
                    Directory.CreateDirectory(tmpPath_rh);
                    Console.WriteLine("....................................");
                    Console.WriteLine("Preparing to build secure partition");
                    Console.WriteLine("....................................");
                    string multiplier1_rh = HFS0Manager.xci_multiplier(Path.Combine(inPath_rh, "secure"));
                    HFS0Manager.BuildHFS0(Path.Combine(inPath_rh, "secure"), Path.Combine(tmpPath_rh, "secure"), multiplier1_rh);
                    Console.WriteLine("....................................");
                    Console.WriteLine("Preparing to build normal partition");
                    Console.WriteLine("....................................");
                    string multiplier2_rh = HFS0Manager.xci_multiplier(Path.Combine(inPath_rh, "normal"));
                    HFS0Manager.BuildHFS0(Path.Combine(inPath_rh, "normal"), Path.Combine(tmpPath_rh, "normal"), multiplier2_rh);
                    Console.WriteLine("....................................");
                    Console.WriteLine("Preparing to build update partition");
                    Console.WriteLine("....................................");
                    string multiplier3_rh = HFS0Manager.xci_multiplier(Path.Combine(inPath_rh, "update"));
                    HFS0Manager.BuildHFS0(Path.Combine(inPath_rh, "update"), Path.Combine(tmpPath_rh, "update"), multiplier3_rh);
                    if (Directory.Exists(Path.Combine(inPath_rh, "logo")))
                    {
                        Console.WriteLine("....................................");
                        Console.WriteLine("Preparing to build logo partition");
                        Console.WriteLine("....................................");
                        string multiplier4 = HFS0Manager.xci_multiplier(Path.Combine(inPath_rh, "logo"));
                        HFS0Manager.BuildHFS0(Path.Combine(inPath_rh, "logo"), Path.Combine(tmpPath_rh, "logo"), multiplier4);
                    }
                    Console.WriteLine("....................................");
                    Console.WriteLine("Preparing to build root.hfs0");
                    Console.WriteLine("....................................");
                    HFS0Manager.BuildHFS0(tmpPath_rh, Path.Combine(inPath_rh, "root.hfs0"), multiplier1_rh);
                    Directory.Delete(tmpPath_rh, true);
                    break;
                case "rhfs0_auto_del":
                    if (args.Length < 2)
                    {
                        PrintUsage();
                        return;
                    }
                    string inPath_rd = Path.Combine(Environment.CurrentDirectory, args[1]);
                    string tmpPath_rd = Path.Combine(inPath_rd, "root_tmp");
                    Directory.CreateDirectory(tmpPath_rd);
                    Console.WriteLine("....................................");
                    Console.WriteLine("Preparing to build secure partition");
                    Console.WriteLine("....................................");
                    string multiplier1_rd = HFS0Manager.xci_multiplier(Path.Combine(inPath_rd, "secure"));
                    HFS0Manager.BuildHFS0(Path.Combine(inPath_rd, "secure"), Path.Combine(tmpPath_rd, "secure"), multiplier1_rd);
                    Directory.Delete(Path.Combine(inPath_rd, "secure"), true);
                    Console.WriteLine("....................................");
                    Console.WriteLine("Preparing to build normal partition");
                    Console.WriteLine("....................................");
                    string multiplier2_rd = HFS0Manager.xci_multiplier(Path.Combine(inPath_rd, "normal"));
                    HFS0Manager.BuildHFS0(Path.Combine(inPath_rd, "normal"), Path.Combine(tmpPath_rd, "normal"), multiplier2_rd);
                    Console.WriteLine("....................................");
                    Directory.Delete(Path.Combine(inPath_rd, "normal"), true);
                    Console.WriteLine("Preparing to build update partition");
                    Console.WriteLine("....................................");
                    string multiplier3_rd = HFS0Manager.xci_multiplier(Path.Combine(inPath_rd, "update"));
                    HFS0Manager.BuildHFS0(Path.Combine(inPath_rd, "update"), Path.Combine(tmpPath_rd, "update"), multiplier3_rd);
                    Directory.Delete(Path.Combine(inPath_rd, "update"), true);
                    Console.WriteLine("....................................");
                    Console.WriteLine("Preparing to build logo partition");
                    Console.WriteLine("....................................");
                    if (Directory.Exists(Path.Combine(inPath_rd, "logo")))
                    {
                        string multiplier4 = HFS0Manager.xci_multiplier(Path.Combine(inPath_rd, "logo"));
                        HFS0Manager.BuildHFS0(Path.Combine(inPath_rd, "logo"), Path.Combine(tmpPath_rd, "logo"), multiplier4);
                        Directory.Delete(Path.Combine(inPath_rd, "logo"), true);
                    }
                    Console.WriteLine("....................................");
                    Console.WriteLine("Preparing to build root.hfs0");
                    Console.WriteLine("....................................");
                    HFS0Manager.BuildHFS0(tmpPath_rd, Path.Combine(inPath_rd, "root.hfs0"), multiplier1_rd);
                    Directory.Delete(tmpPath_rd, true);
                    break;
                default:
                    PrintUsage();
                    break;
            }      
        }
        static bool LoadKeys()
        {
            bool ret = false;
            try
            {
                StreamReader file = new StreamReader("keys.txt");

                string line;
                while((line = file.ReadLine()) != null) 
                {
                    string[] parts = line.Split('=');
                    if (parts.Length < 2) continue;

                    string name = parts[0].Trim(" \0\n\r\t".ToCharArray());
                    string key = parts[1].Trim(" \0\n\r\t".ToCharArray());

                    //Console.WriteLine("{0} = {1}", name, key);

                    if (name == "xci_header_key")
                    {
                        XCIManager.XCI_GAMECARDINFO_KEY = Utils.StringToByteArray(key);
                        ret = true;
                    }
                }

            } catch(Exception ex)
            {
                Console.WriteLine("[ERR] keys.txt is either missing or unaccessible.");
                ret = false;
            }
            return ret;
           
        }
        static void PrintUsage()
        {
            Console.WriteLine("..............");
            Console.WriteLine("INSTRUCTIONS");
            Console.WriteLine("..............");
            Console.WriteLine(" ");
            Console.WriteLine("- xci auto builder:");
            Console.WriteLine("\t hacbuild.exe xci_auto input_folder output_file");
            Console.WriteLine(" ");
            Console.WriteLine("- xci auto builder with file deletion while building:");
            Console.WriteLine("\t hacbuild.exe xci_auto_del input_folder output_file");
            Console.WriteLine(" ");
            Console.WriteLine("- Manual builder for xci from root.hfs0:");
            Console.WriteLine("\t hacbuild.exe xci input_folder output_file");
            Console.WriteLine(" ");
            Console.WriteLine("- root.hfs0 auto builder:");
            Console.WriteLine("\t hacbuild.exe rhfs0_auto input_folder");
            Console.WriteLine(" ");
            Console.WriteLine("- root.hfs0 auto builder with file deletion while building:");
            Console.WriteLine("\t hacbuild.exe rhfs0_auto_del input_folder");
            Console.WriteLine(" ");
            Console.WriteLine("- Manual builder for hfs0 (normal/secure/update):");
            Console.WriteLine("\t hacbuild.exe hfs0 input_folder output_file");
            Console.WriteLine(" ");
            Console.WriteLine("- Manual builder for root.hfs0:");
            Console.WriteLine("\t hacbuild.exe root_hfs0 input_folder output_file multiplier");
            Console.WriteLine(" ");
            Console.WriteLine("- Get 'game_info' file named as file input's name:");
            Console.WriteLine("\t hacbuild.exe read xci input_file output_folder");
            Console.WriteLine(" ");
            Console.WriteLine("- Get 'game_info' file named as gameinfo.ini:");
            Console.WriteLine("\t hacbuild.exe read xci input_file output_folder");
            Console.WriteLine(" ");
        }
    }
}
