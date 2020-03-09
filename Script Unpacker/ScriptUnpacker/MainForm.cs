using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace ScriptUnpacker
{
    public partial class MainForm : Form
    {

        public int RSDKver = 0;
        public string DataFolderPath;

        public List<RSDKv2.Bytecode> bytecodev2 = new List<RSDKv2.Bytecode>();
        public List<RSDKvB.Bytecode> bytecodevB = new List<RSDKvB.Bytecode>();
        public List<RSDKvRS.Script> bytecodevRS = new List<RSDKvRS.Script>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void RSDKverBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RSDKverBox.SelectedIndex >= 0)
            {
                RSDKver = RSDKverBox.SelectedIndex;
            }
        }

        private void SelectDataFolderButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                DataFolderPath = dlg.SelectedPath;

                switch (RSDKver)
                {
                    case 0:
                        UnpackDataFolderV2(DataFolderPath);
                        break;
                    case 1:
                        UnpackDataFolderVB(DataFolderPath + "..//");
                        break;
                }

            }
        }

        private void UnpackButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                switch (RSDKver)
                {
                    case 0:
                        ExtractV2(dlg.SelectedPath);
                        break;
                    case 1:
                        ExtractVB(dlg.SelectedPath);
                        break;
                }
            }
        }

        public void UnpackDataFolderV2(string datafolderpath)
        {
            RSDKv2.Gameconfig gc = new RSDKv2.Gameconfig(datafolderpath + "/Game/Gameconfig.bin");
            DirectoryInfo dir = new DirectoryInfo("Scripts");
            dir.Create();
            bytecodev2.Clear();
            for (int i = 0; i < gc.ScriptPaths.Count; i++)
            {
                if (!Directory.Exists("Scripts/" + Path.GetDirectoryName(gc.ScriptPaths[i])))
                {
                    Directory.CreateDirectory("Scripts/" + Path.GetDirectoryName(gc.ScriptPaths[i]));
                }
                if (!File.Exists("Scripts/" + gc.ScriptPaths[i]))
                {
                    File.CreateText("Scripts/" + gc.ScriptPaths[i]);
                }
            }

            RSDKv2.Gameconfig gcv2 = new RSDKv2.Gameconfig(datafolderpath + "/Game/Gameconfig.bin");

            string GlobalPath = datafolderpath + "/Scripts/Bytecode/GS000.bin";

            bool MobileVer = false;

            if (!File.Exists(GlobalPath))
            {
                GlobalPath = datafolderpath + "/Scripts/Bytecode/GlobalCode.bin";
                MobileVer = true;
            }

            RSDKv2.Bytecode GlobalCode = new RSDKv2.Bytecode(new RSDKv2.Reader(GlobalPath), 1,MobileVer);

            GlobalCode.sourceNames = new string[gcv2.ScriptPaths.Count + 1];
            GlobalCode.typeNames = new string[gcv2.ObjectsNames.Count + 1];
            GlobalCode.sfxNames = new string[gcv2.SoundFX.Count];

            GlobalCode.sourceNames[0] = "BlankObject";
            GlobalCode.typeNames[0] = "BlankObject";

            for (int i = 0; i < gcv2.GlobalVariables.Count; i++)
            {
                GlobalCode.globalVariableNames[i] = gcv2.GlobalVariables[i].Name;
            }

            for (int i = 1; i < gcv2.ObjectsNames.Count + 1; i++)
            {
                GlobalCode.sourceNames[i] = gcv2.ScriptPaths[i - 1];
                GlobalCode.typeNames[i] = Path.GetFileNameWithoutExtension(gcv2.ScriptPaths[i - 1]);
            }

            for (int i = 0; i < gcv2.SoundFX.Count; i++)
            {
                GlobalCode.sfxNames[i] = Path.GetFileNameWithoutExtension(gcv2.SoundFX[i]).Replace(" ","");
            }

            bytecodev2.Add(GlobalCode);

            string[] categoryChars = new string[] { "PS", "RS", "SS", "BS" };
            for (int c = 0; c < gcv2.Categories.Count; c++)
            {
                for (int s = 0; s < gcv2.Categories[c].Scenes.Count; s++)
                {
                    string BytecodeName = "";
                    if (!MobileVer)
                    {
                        char StageNo1 = (char)((s / 100) + '0');
                        char StageNo2 = (char)(((s % 100) / 10) + '0');
                        char StageNo3 = (char)((s % 10) + '0');

                        BytecodeName = datafolderpath + "/Scripts/Bytecode/" + categoryChars[c] + StageNo1 + StageNo2 + StageNo3 + ".bin";
                    }
                    else
                    {
                        BytecodeName = datafolderpath + "/Scripts/Bytecode/" + gcv2.Categories[c].Scenes[s].SceneFolder + ".bin";
                    }

                    if (File.Exists(DataFolderPath + "/Stages/" + gc.Categories[c].Scenes[s].SceneFolder + "/Stageconfig.bin") && File.Exists(BytecodeName))
                    {
                        RSDKv2.Stageconfig scv2 = new RSDKv2.Stageconfig(DataFolderPath + "/Stages/" + gc.Categories[c].Scenes[s].SceneFolder + "/Stageconfig.bin");

                        RSDKv2.Bytecode bytecode = new RSDKv2.Bytecode(new RSDKv2.Reader(BytecodeName), gcv2.ObjectsNames.Count + 1, MobileVer);

                        if (scv2.LoadGlobalScripts)
                        {
                            bytecode = new RSDKv2.Bytecode(new RSDKv2.Reader(GlobalPath), 1, MobileVer);
                            bytecode.LoadStageBytecodeData(new RSDKv2.Reader(BytecodeName), gcv2.ObjectsNames.Count + 1, MobileVer);
                            bytecode.GlobalfunctionCount += GlobalCode.functionCount;
                            bytecode.sourceNames = new string[gcv2.ObjectsNames.Count + scv2.ScriptPaths.Count + 1];
                            bytecode.typeNames = new string[gcv2.ObjectsNames.Count + scv2.ObjectsNames.Count + 1];
                            bytecode.sfxNames = new string[gcv2.SoundFX.Count + scv2.SoundFX.Count];

                            bytecode.sourceNames[0] = "BlankObject";
                            bytecode.typeNames[0] = "BlankObject";

                            for (int i = 1; i < gcv2.GlobalVariables.Count; i++)
                            {
                                bytecode.globalVariableNames[i] = gcv2.GlobalVariables[i].Name;
                            }

                            int ID = 1;

                            for (int i = 0; i < gcv2.ObjectsNames.Count; i++)
                            {
                                bytecode.sourceNames[ID] = gcv2.ScriptPaths[i];
                                bytecode.typeNames[ID] = gcv2.ObjectsNames[i];
                                ID++;
                            }

                            for (int i = 0; i < scv2.ObjectsNames.Count; i++)
                            {
                                bytecode.sourceNames[ID] = scv2.ScriptPaths[i];
                                bytecode.typeNames[ID] = scv2.ObjectsNames[i];
                                ID++;
                            }

                            ID = 0;

                            for (int i = 0; i < gcv2.SoundFX.Count; i++)
                            {
                                bytecode.sfxNames[ID] = Path.GetFileNameWithoutExtension(gcv2.SoundFX[i]).Replace(" ", "");
                                ID++;
                            }

                            for (int i = 0; i < scv2.SoundFX.Count; i++)
                            {
                                bytecode.sfxNames[ID] = Path.GetFileNameWithoutExtension(scv2.ScriptPaths[i]).Replace(" ", "");
                                ID++;
                            }
                        }
                        else
                        {
                            bytecode = new RSDKv2.Bytecode(new RSDKv2.Reader(BytecodeName), 1, MobileVer);

                            bytecode.sourceNames = new string[scv2.ScriptPaths.Count + 1];
                            bytecode.typeNames = new string[scv2.ObjectsNames.Count + 1];

                            bytecode.sourceNames[0] = "BlankObject";
                            bytecode.typeNames[0] = "BlankObject";

                            for (int i = 1; i < gcv2.GlobalVariables.Count; i++)
                            {
                                bytecode.globalVariableNames[i] = gcv2.GlobalVariables[i].Name;
                            }

                            int ID = 1;

                            for (int i = 0; i < scv2.ObjectsNames.Count; i++)
                            {
                                bytecode.sourceNames[ID] = scv2.ScriptPaths[i];
                                bytecode.typeNames[ID] = scv2.ObjectsNames[i];
                                ID++;
                            }
                        }

                        bytecodev2.Add(bytecode);
                    }

                    
                }
            }
        }

        public void UnpackDataFolderVB(string datafolderpath)
        {
            RSDKvB.Gameconfig gc = new RSDKvB.Gameconfig(datafolderpath + "/Game/Gameconfig.bin");
            DirectoryInfo dir = new DirectoryInfo("Scripts");
            dir.Create();
            bytecodevB.Clear();
            for (int i = 0; i < gc.ScriptPaths.Count; i++)
            {
                if (!Directory.Exists("Scripts/" + Path.GetDirectoryName(gc.ScriptPaths[i])))
                {
                    Directory.CreateDirectory("Scripts/" + Path.GetDirectoryName(gc.ScriptPaths[i]));
                }
                if (!File.Exists("Scripts/" + gc.ScriptPaths[i]))
                {
                    File.CreateText("Scripts/" + gc.ScriptPaths[i]);
                }
            }

            foreach (RSDKvB.Gameconfig.Category sg in gc.Categories)
            {
                foreach (RSDKvB.Gameconfig.Category.SceneInfo si in sg.Scenes)
                {
                    RSDKvB.Stageconfig sc = new RSDKvB.Stageconfig(datafolderpath + "/Stages/" + si.SceneFolder + "/Stageconfig.bin");

                    for (int i = 0; i < sc.ScriptPaths.Count; i++)
                    {
                        if (!Directory.Exists("Scripts/" + Path.GetDirectoryName(sc.ScriptPaths[i])))
                        {
                            Directory.CreateDirectory("Scripts/" + Path.GetDirectoryName(sc.ScriptPaths[i]));
                        }
                        if (!File.Exists("Scripts/" + sc.ScriptPaths[i]))
                        {
                            File.CreateText("Scripts/" + sc.ScriptPaths[i]);
                        }
                    }
                }
            }

            RSDKvB.Gameconfig gcvB = new RSDKvB.Gameconfig(datafolderpath + "/Game/Gameconfig.bin");

            RSDKvB.Bytecode GlobalCode = new RSDKvB.Bytecode(new RSDKvB.Reader(datafolderpath + "../Bytecode/GlobalCode.bin"), 1);

            GlobalCode.sourceNames = new string[gcvB.ScriptPaths.Count + 1];
            GlobalCode.typeNames = new string[gcvB.ObjectsNames.Count + 1];
            GlobalCode.sfxNames = new string[gcvB.SfxNames.Count];

            GlobalCode.sourceNames[0] = "BlankObject";
            GlobalCode.typeNames[0] = "BlankObject";

            for (int i = 0; i < gcvB.GlobalVariables.Count; i++)
            {
                GlobalCode.globalVariableNames[i] = gcvB.GlobalVariables[i].Name;
            }

            for (int i = 1; i < gcvB.ObjectsNames.Count + 1; i++)
            {
                GlobalCode.sourceNames[i] = gcvB.ScriptPaths[i - 1];
                GlobalCode.typeNames[i] = gcvB.ObjectsNames[i - 1];
            }

            for (int i = 0; i < gcvB.SfxNames.Count; i++)
            {
                GlobalCode.sfxNames[i] = gcvB.SfxNames[i];
            }

            bytecodevB.Add(GlobalCode);

            List<string> names = new List<string>();

            for (int c = 0; c < gcvB.Categories.Count; c++)
            {
                for (int s = 0; s < gcvB.Categories[c].Scenes.Count; s++)
                {
                    string BytecodeName = "";
                    BytecodeName = datafolderpath + "../Bytecode/" + gcvB.Categories[c].Scenes[s].SceneFolder + ".bin";

                    if (!names.Contains(BytecodeName))
                    {
                        names.Add(BytecodeName);

                        RSDKvB.Stageconfig scvB = new RSDKvB.Stageconfig(DataFolderPath + "/Stages/" + gc.Categories[c].Scenes[s].SceneFolder + "/Stageconfig.bin");

                        RSDKvB.Bytecode bytecode = new RSDKvB.Bytecode(new RSDKvB.Reader(BytecodeName), gcvB.ObjectsNames.Count + 1);

                        if (scvB.LoadGlobalScripts)
                        {
                            bytecode = new RSDKvB.Bytecode(new RSDKvB.Reader(datafolderpath + "../Bytecode/GlobalCode.bin"), 1);
                            bytecode.LoadStageBytecodeData(new RSDKvB.Reader(BytecodeName), gcvB.ObjectsNames.Count + 1);

                            bytecode.sourceNames = new string[gcvB.ScriptPaths.Count + scvB.ScriptPaths.Count + 1];
                            bytecode.typeNames = new string[gcvB.ObjectsNames.Count + scvB.ObjectsNames.Count + 1];
                            bytecode.sfxNames = new string[gcvB.SfxNames.Count + scvB.SfxNames.Count];
                            bytecode.GlobalfunctionCount += GlobalCode.functionCount;
                            bytecode.sourceNames[0] = "BlankObject";
                            bytecode.typeNames[0] = "BlankObject";

                            for (int i = 1; i < gcvB.GlobalVariables.Count; i++)
                            {
                                bytecode.globalVariableNames[i] = gcvB.GlobalVariables[i].Name;
                            }

                            int ID = 1;

                            for (int i = 0; i < gcvB.ObjectsNames.Count; i++)
                            {
                                bytecode.sourceNames[ID] = gcvB.ScriptPaths[i];
                                bytecode.typeNames[ID] = gcvB.ObjectsNames[i];
                                ID++;
                            }

                            for (int i = 0; i < scvB.ObjectsNames.Count; i++)
                            {
                                bytecode.sourceNames[ID] = scvB.ScriptPaths[i];
                                bytecode.typeNames[ID] = scvB.ObjectsNames[i];
                                ID++;
                            }

                            //Add SFX
                            ID = 0;

                            for (int i = 0; i < gcvB.SfxNames.Count; i++)
                            {
                                bytecode.sfxNames[ID] = gcvB.SfxNames[i];
                                ID++;
                            }

                            for (int i = 0; i < scvB.SfxNames.Count; i++)
                            {
                                bytecode.sfxNames[ID] = scvB.SfxNames[i];
                                ID++;
                            }

                            bytecodevB.Add(bytecode);
                        }
                        else
                        {
                            bytecode = new RSDKvB.Bytecode(new RSDKvB.Reader(BytecodeName), 1);

                            bytecode.sourceNames = new string[scvB.ScriptPaths.Count + 1];
                            bytecode.typeNames = new string[scvB.ObjectsNames.Count + 1];

                            bytecode.sourceNames[0] = "BlankObject";
                            bytecode.typeNames[0] = "BlankObject";

                            for (int i = 1; i < gcvB.GlobalVariables.Count; i++)
                            {
                                bytecode.globalVariableNames[i] = gcvB.GlobalVariables[i].Name;
                            }

                            int ID = 1;

                            for (int i = 0; i < scvB.ObjectsNames.Count; i++)
                            {
                                bytecode.sourceNames[ID] = scvB.ScriptPaths[i];
                                bytecode.typeNames[ID] = scvB.ObjectsNames[i];
                                ID++;
                            }

                            bytecodevB.Add(bytecode);
                        }
                    }
                }
            }
        }

        public void ExtractV2(string folderpath = "")
        {
            for (int i = 0; i < bytecodev2.Count; i++)
            {
                //try
                //{
                bytecodev2[i].UseHex = UseHexCB.Checked;
                bytecodev2[i].Decompile(folderpath);
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //}
            }
        }

        public void ExtractVB(string folderpath = "")
        {
            for (int i = 0; i < bytecodevB.Count; i++)
            {
               // try
                //{
                    bytecodevB[i].UseHex = UseHexCB.Checked;
                    bytecodevB[i].Decompile(folderpath);
                //}
                //catch(Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //}
            }
        }

        private void BytecodeButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select Data Folder";
            dlg.Filter = "RSDK Bytecode Files|*.bin|Retro-Sonic Scripts|*.rsf";


            OpenFileDialog dlg2 = new OpenFileDialog();
            dlg2.Title = "Select Gameconfig/Stageconfig Files";

            RSDKvB.Gameconfig gcvB = new RSDKvB.Gameconfig();
            RSDKv2.Gameconfig gcv2 = new RSDKv2.Gameconfig();

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                string tmp = Path.GetFileNameWithoutExtension(dlg.FileName);
                bool IsGlobal = tmp == "GS000" || tmp == "GlobalCode";

                bool mobile = tmp == "GlobalCode";

                if (IsGlobal)
                {
                    dlg2.Filter = "RSDK Gameconfig Files|Gameconfig*.bin";
                }
                else
                {
                    dlg2.Filter = "RSDK Stageconfig Files|Stageconfig*.bin";
                }

                if (dlg2.ShowDialog(this) == DialogResult.OK && RSDKver != 2)
                {

                    if (!IsGlobal && RSDKver != 2)
                    {
                        OpenFileDialog dlg3 = new OpenFileDialog();
                        dlg3.Title = "Select Gameconfig Files|Gameconfig*.bin";

                        if (dlg3.ShowDialog(this) == DialogResult.OK)
                        {
                            switch(RSDKver)
                            {
                                case 0:
                                    gcv2 = new RSDKv2.Gameconfig(dlg3.FileName);
                                    break;
                                case 1:
                                    gcvB = new RSDKvB.Gameconfig(dlg3.FileName);
                                    break;
                                case 2:
                                    break;
                            }
                        }

                    }

                    switch (RSDKver)
                    {
                        case 0:
                            bytecodev2.Clear();

                            switch(IsGlobal)
                            {
                                case true:
                                    gcv2 = new RSDKv2.Gameconfig(dlg2.FileName);
                                    bytecodev2.Add(new RSDKv2.Bytecode(new RSDKv2.Reader(dlg.FileName), 1,mobile));

                                    bytecodev2[0].sourceNames = new string[gcv2.ScriptPaths.Count + 1];
                                    bytecodev2[0].typeNames = new string[gcv2.ObjectsNames.Count + 1];

                                    bytecodev2[0].sourceNames[0] = "BlankObject";
                                    bytecodev2[0].typeNames[0] = "BlankObject";

                                    for (int i = 0; i < gcv2.GlobalVariables.Count; i++)
                                    {
                                        bytecodev2[0].globalVariableNames[i] = gcv2.GlobalVariables[i].Name;
                                    }

                                    for (int i = 1; i < gcv2.ObjectsNames.Count + 1; i++)
                                    {
                                        bytecodev2[0].sourceNames[i] = gcv2.ScriptPaths[i - 1];
                                        bytecodev2[0].typeNames[i] = gcv2.ObjectsNames[i - 1];
                                    }
                                    break;
                                case false:

                                    string fle = Path.GetFileName(dlg.FileName);
                                    string dir = dlg.FileName.Replace(fle, "");
                                    string globalcode = dir + "//GS000.bin";
                                    if (!File.Exists(globalcode))
                                    {
                                        globalcode = dir + "//GlobalCode.bin";
                                        if (!File.Exists(globalcode))
                                        {
                                            return;
                                        }
                                    }

                                    RSDKv2.Stageconfig scv2 = new RSDKv2.Stageconfig(dlg2.FileName);
                                    RSDKv2.Bytecode SCbytecode = new RSDKv2.Bytecode(new RSDKv2.Reader(globalcode), 1, mobile);
                                    SCbytecode.LoadStageBytecodeData(new RSDKv2.Reader(dlg.FileName), gcv2.ScriptPaths.Count + 1,mobile);
                                    bytecodev2.Add(SCbytecode);

                                    bytecodev2[0].sourceNames = new string[gcv2.ScriptPaths.Count + scv2.ScriptPaths.Count + 1];
                                    bytecodev2[0].typeNames = new string[gcv2.ScriptPaths.Count + scv2.ObjectsNames.Count + 1];

                                    bytecodev2[0].sourceNames[0] = "BlankObject";
                                    bytecodev2[0].typeNames[0] = "BlankObject";

                                    for (int i = 1; i < gcv2.GlobalVariables.Count; i++)
                                    {
                                        bytecodev2[0].globalVariableNames[i] = gcv2.GlobalVariables[i].Name;
                                    }

                                    int ID = 1;

                                    for (int i = 0; i < gcv2.ObjectsNames.Count; i++)
                                    {
                                        bytecodev2[0].sourceNames[ID] = gcv2.ScriptPaths[i];
                                        bytecodev2[0].typeNames[ID] = gcv2.ObjectsNames[i];
                                        ID++;
                                    }

                                    for (int i = 0; i < scv2.ObjectsNames.Count; i++)
                                    {
                                        bytecodev2[0].sourceNames[ID] = scv2.ScriptPaths[i];
                                        bytecodev2[0].typeNames[ID] = scv2.ObjectsNames[i];
                                        ID++;
                                    }
                                    break;
                            }
                            break;
                        case 1:
                            bytecodevB.Clear();

                            switch (IsGlobal)
                            {
                                case true:
                                    gcvB = new RSDKvB.Gameconfig(dlg2.FileName);
                                    bytecodevB.Add(new RSDKvB.Bytecode(new RSDKvB.Reader(dlg.FileName), 1));

                                    bytecodevB[0].sourceNames = new string[gcvB.ScriptPaths.Count + 1];
                                    bytecodevB[0].typeNames = new string[gcvB.ObjectsNames.Count + 1];

                                    bytecodevB[0].sourceNames[0] = "BlankObject";
                                    bytecodevB[0].typeNames[0] = "BlankObject";

                                    for (int i = 1; i < gcvB.GlobalVariables.Count; i++)
                                    {
                                        bytecodevB[0].globalVariableNames[i] = gcvB.GlobalVariables[i].Name;
                                    }

                                    for (int i = 1; i < gcvB.ObjectsNames.Count + 1; i++)
                                    {
                                        bytecodevB[0].sourceNames[i] = gcvB.ScriptPaths[i - 1];
                                        bytecodevB[0].typeNames[i] = gcvB.ObjectsNames[i - 1];
                                    }
                                    break;
                                case false:

                                    RSDKv2.Stageconfig scvB = new RSDKv2.Stageconfig(dlg2.FileName);
                                    bytecodevB.Add(new RSDKvB.Bytecode(new RSDKvB.Reader(dlg.FileName), gcvB.ScriptPaths.Count + 1));

                                    bytecodev2[0].sourceNames = new string[gcvB.ScriptPaths.Count + scvB.ScriptPaths.Count + 1];
                                    bytecodev2[0].typeNames = new string[gcvB.ScriptPaths.Count + scvB.ObjectsNames.Count + 1];

                                    bytecodev2[0].sourceNames[0] = "BlankObject";
                                    bytecodev2[0].typeNames[0] = "BlankObject";

                                    for (int i = 0; i < gcvB.GlobalVariables.Count; i++)
                                    {
                                        bytecodevB[0].globalVariableNames[i] = gcv2.GlobalVariables[i].Name;
                                    }

                                    int ID = 1;

                                    for (int i = 0; i < gcvB.ObjectsNames.Count; i++)
                                    {
                                        bytecodevB[0].sourceNames[ID] = gcvB.ScriptPaths[i];
                                        bytecodevB[0].typeNames[ID] = gcvB.ObjectsNames[i];
                                        ID++;
                                    }

                                    for (int i = 0; i < scvB.ObjectsNames.Count; i++)
                                    {
                                        bytecodevB[0].sourceNames[ID] = scvB.ScriptPaths[i];
                                        bytecodevB[0].typeNames[ID] = scvB.ObjectsNames[i];
                                        ID++;
                                    }
                                    break;
                            }
                            break;
                        case 2:
                            //Retro-Sonic Stuff
                            break;
                    }
                }
            }
        }

        private void UnpackBCFileButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "Select a folder to export the scripts to!";

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                switch (RSDKver)
                {
                    case 0:
                        bytecodev2[0].UseHex = UseHexCB.Checked;
                        bytecodev2[0].Decompile(dlg.SelectedPath);
                        break;
                    case 1:
                        bytecodevB[0].UseHex = UseHexCB.Checked;
                        bytecodevB[0].Decompile(dlg.SelectedPath);
                        break;
                    case 2:
                        break;
                }
            }
        }
    }
}
