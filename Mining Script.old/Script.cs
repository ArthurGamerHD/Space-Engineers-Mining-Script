//Placeholder     

/* 
 
This script has been maded by "The Order Civilian Development Division" in partnership with the "New United Comunist Countries"  
 
*/

//Translation Strings
string String0 = "";
string String1 = " Undocking";
string String2 = " Takes off";
string String3 = " On the way to deposit";
string String4 = " Prepare to mine";
string String5 = " Mining";
string String6 = " Leave from mine";
string String7 = " On the way to home";
string String8 = " Docking";
string String9 = "Program started";
string String10 = "The Order's Mining IA Controller™  ";
string String11 = "      ";
string String12 = " Mining robots of ";
string String13 = "The Order ";
string String14 = "Robots total: ";
string String15 = "To next inventory update: ";
string String16 = "To next blocks list update: ";
string String17 = "Robots list:";
string String18 = "    Diagnostic    ";
string String19 = "Robot ";
string String20 = "Status: ";
string String21 = "Not enough thrusterd!";
string String22 = "Not enough gyroscopes!";
string String23 = "No containers!";
string String24 = "Not enough cameras.";
string String25 = "Not enough drills!";
string String26 = "No connectors!";
string String27 = "No energy source!";
string String28 = "No gas tanks.";
string String29 = "Not enough base connectors.";
string String30 = "No base containers.";
string String31 = "Total: ";
string String32 = "Errors - ";
string String33 = " Warnings - ";
string String34 = "==========";
string String35 = " can't take off ";
string String36 = " detected an obstacle during takeoff ";
string String37 = " Ready";
string String38 = " Refueling";
string String39 = "Speed: ";
string String40 = " m/s";
string String41 = "Distance to: ";
string String42 = "M";
string String43 = "Containers: ";
string String44 = "Thrusters: ";
string String45 = "Drills: ";
string String46 = "Connectors: ";
string String47 = "Battery charge: ";
string String48 = "Fuel: ";
string String49 = "None";
string String50 = "Target: ";
string String51 = " can't find free functional connector ";
string String52 = " can't find asteroid ";
string String53 = " exhausted ore deposit ";
string String54 = " find asteroid end ";
string String55 = " detected an obstacle during docking ";
string String56 = "=__________=";
string String57 = " control lost ";
string String58 = " attacked! ";
string String59 = "     Ore deposit status     ";
string String60 = "Ore deposit ";
string String61 = " exhausted";
string String62 = " in process";
string String63 = " not used";
string String64 = "***********";
string String65 = "    Robots GPS    ";
string String66 = "Not full coordinates ";
string String67 = "Not enough base connectors ";
string String68 = "Ore deposit #";
string String69 = "Goodbye, robot ";
string String70 = ", you will forever be a number one! ";
string String71 = "Drone Full Name:";
string String72 = "Drone Prefix (short name):";
string String73 = "Drone's Block Group:";
string String74 = "Drone Remote Control Name:";
string String75 = "Mothership LCD Name:";
string String76 = "Drone ID Panel";
string String77 = "Mothership's Drones Connectors Name:";
string String78 = "Mothership's Cargo Group Name:";
string String79 = "Drone's Timer for Emergency Sequence:";
string String80 = "Mothership's Raycast Camera Name:";

//Config Pre-set

MyIni _ini = new MyIni();

string FullName = "";
string DClass = "";
string GrName = "";
string RCN = "";
string LCDN = "";
string IDN = "";
string ConnName = "";
string CargoName = "";
string EmergencyTimer = "";
string LockCamName = "";
int ConnRange = 1;
int DampRange = 1;
int MineCount = 1;
int Frequency = 1;
int CargoFreq = 1;
int EmergencyCount = 1;
int ScanRange = 1;
double DampSpeed = 1;
double SearchRange = 1;
double SafetySpeed = 1;
double MineSize = 1;
double MiningDepth = 1;
double uranium = 1;
double MineHeight = 1;
float MiningThrust = 1;
bool Hydrogen = false;
bool Debug = false;

public void load_configuration()
{
    string Pre_FullName = "Mining Drone";
    string Pre_DClass = "[XXX-000]";
    string Pre_GrName = "[XXX-000]";
    string Pre_RCN = "Remote [XXX-000]";
    string Pre_LCDN = "Drones LCD";
    string Pre_IDN = "Drone ID";
    string Pre_ConnName = "Drones connector";
    string Pre_CargoName = "Cargo";
    string Pre_EmergencyTimer = "Emergency";
    string Pre_LockCamName = "Scanning Camera";
    int Pre_ConnRange = 20;
    double Pre_DampSpeed = 0.5;
    int Pre_DampRange = 16;
    int Pre_MineCount = 4;
    int Pre_Frequency = 30;
    int Pre_CargoFreq = 5;
    int Pre_EmergencyCount = 5;
    int Pre_ScanRange = 5000;
    double Pre_SearchRange = -1;
    double Pre_SafetySpeed = 5;
    double Pre_MineSize = 3;
    double Pre_MiningDepth = 20;
    double Pre_uranium = 4;
    bool Pre_Hydrogen = false;
    double Pre_MineHeight = 5;
    float Pre_MiningThrust = 1;
    bool Pre_Debug = false;

    MyIniParseResult result;
    if (!_ini.TryParse(Me.CustomData, out result))
    {
        Echo($"CustomData error:\nLine {result}");
    }


    if (!_ini.ContainsKey("kernel", "FullName")) { _ini.Set("kernel", "FullName", Pre_FullName); _ini.SetComment("kernel", "FullName", $"{String71} Default: {Pre_FullName.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "DClass")) { _ini.Set("kernel", "DClass", Pre_DClass); _ini.SetComment("kernel", "DClass", $"{String72} Default: {Pre_DClass.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "GrName")) { _ini.Set("kernel", "GrName", Pre_GrName); _ini.SetComment("kernel", "GrName", $"{String73} Default: {Pre_GrName.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "RCN")) { _ini.Set("kernel", "RCN", Pre_RCN); _ini.SetComment("kernel", "RCN", $"{String74} Default: {Pre_RCN.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "LCDN")) { _ini.Set("kernel", "LCDN", Pre_LCDN); _ini.SetComment("kernel", "LCDN", $"{String75} Default: {Pre_LCDN.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "IDN")) { _ini.Set("kernel", "IDN", Pre_IDN); _ini.SetComment("kernel", "IDN", $"{String76} Default: {Pre_IDN.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "ConnName")) { _ini.Set("kernel", "ConnName", Pre_ConnName); _ini.SetComment("kernel", "ConnName", $"{String77} Default: {Pre_ConnName.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "CargoName")) { _ini.Set("kernel", "CargoName", Pre_CargoName); _ini.SetComment("kernel", "CargoName", $"{String78} Default: {Pre_CargoName.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "EmergencyTimer")) { _ini.Set("kernel", "EmergencyTimer", Pre_EmergencyTimer); _ini.SetComment("kernel", "EmergencyTimer", $"{String79} Default: {Pre_EmergencyTimer.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "LockCamName")) { _ini.Set("kernel", "LockCamName", Pre_LockCamName); _ini.SetComment("kernel", "LockCamName", $"{String80} Default: {Pre_LockCamName.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "ConnRange")) { _ini.Set("kernel", "ConnRange", Pre_ConnRange); _ini.SetComment("kernel", "ConnRange", $"{String0} Default: {Pre_ConnRange.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "DampRange")) { _ini.Set("kernel", "DampRange", Pre_DampRange); _ini.SetComment("kernel", "DampRange", $"{String0} Default: {Pre_DampRange.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "MineCount")) { _ini.Set("kernel", "MineCount", Pre_MineCount); _ini.SetComment("kernel", "MineCount", $"{String0} Default: {Pre_MineCount.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "Frequency")) { _ini.Set("kernel", "Frequency", Pre_Frequency); _ini.SetComment("kernel", "Frequency", $"{String0} Default: {Pre_Frequency.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "CargoFreq")) { _ini.Set("kernel", "CargoFreq", Pre_CargoFreq); _ini.SetComment("kernel", "CargoFreq", $"{String0} Default: {Pre_CargoFreq.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "EmergencyCount")) { _ini.Set("kernel", "EmergencyCount", Pre_EmergencyCount); _ini.SetComment("kernel", "EmergencyCount", $"{String0} Default: {Pre_EmergencyCount.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "ScanRange")) { _ini.Set("kernel", "ScanRange", Pre_ScanRange); _ini.SetComment("kernel", "ScanRange", $"{String0} Default: {Pre_ScanRange.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "DampSpeed")) { _ini.Set("kernel", "DampSpeed", Pre_DampSpeed); _ini.SetComment("kernel", "DampSpeed", $"{String0} Default: {Pre_DampSpeed.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "SearchRange")) { _ini.Set("kernel", "SearchRange", Pre_SearchRange); _ini.SetComment("kernel", "SearchRange", $"{String0} Default: {Pre_SearchRange.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "SafetySpeed")) { _ini.Set("kernel", "SafetySpeed", Pre_SafetySpeed); _ini.SetComment("kernel", "SafetySpeed", $"{String0} Default: {Pre_SafetySpeed.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "MineSize")) { _ini.Set("kernel", "MineSize", Pre_MineSize); _ini.SetComment("kernel", "MineSize", $"{String0} Default: {Pre_MineSize.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "MiningDepth")) { _ini.Set("kernel", "MiningDepth", Pre_MiningDepth); _ini.SetComment("kernel", "MiningDepth", $"{String0} Default: {Pre_MiningDepth.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "uranium")) { _ini.Set("kernel", "uranium", Pre_uranium); _ini.SetComment("kernel", "uranium", $"{String0} Default: {Pre_uranium.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "MineHeight")) { _ini.Set("kernel", "MineHeight", Pre_MineHeight); _ini.SetComment("kernel", "MineHeight", $"{String0} Default: {Pre_MineHeight.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "MiningThrust")) { _ini.Set("kernel", "MiningThrust", Pre_MiningThrust); _ini.SetComment("kernel", "MiningThrust", $"{String0} Default: {Pre_MiningThrust.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "Hydrogen")) { _ini.Set("kernel", "Hydrogen", Pre_Hydrogen); _ini.SetComment("kernel", "Hydrogen", $"{String0} Default: {Pre_Hydrogen.ToString()}"); }
    if (!_ini.ContainsKey("kernel", "Debug")) { _ini.Set("kernel", "Debug", Pre_Debug); _ini.SetComment("kernel", "Debug", $"{String0} Default: {Pre_Debug.ToString()}"); }
    Me.CustomData = _ini.ToString();

    EmergencyTimer = (_ini.Get("kernel", "EmergencyTimer")).ToString();
    LockCamName = (_ini.Get("kernel", "LockCamName")).ToString();
    CargoName = (_ini.Get("kernel", "CargoName")).ToString();
    FullName = (_ini.Get("kernel", "FullName")).ToString();
    ConnName = (_ini.Get("kernel", "ConnName")).ToString();
    DClass = (_ini.Get("kernel", "DClass")).ToString();
    GrName = (_ini.Get("kernel", "GrName")).ToString();
    LCDN = (_ini.Get("kernel", "LCDN")).ToString();
    RCN = (_ini.Get("kernel", "RCN")).ToString();
    IDN = (_ini.Get("kernel", "IDN")).ToString();
    EmergencyCount = int.Parse((_ini.Get("kernel", "EmergencyCount")).ToString());
    ConnRange = int.Parse((_ini.Get("kernel", "ConnRange")).ToString());
    DampRange = int.Parse((_ini.Get("kernel", "DampRange")).ToString());
    MineCount = int.Parse((_ini.Get("kernel", "MineCount")).ToString());
    Frequency = int.Parse((_ini.Get("kernel", "Frequency")).ToString());
    CargoFreq = int.Parse((_ini.Get("kernel", "CargoFreq")).ToString());
    ScanRange = int.Parse((_ini.Get("kernel", "ScanRange")).ToString());
    SearchRange = double.Parse((_ini.Get("kernel", "SearchRange")).ToString());
    SafetySpeed = double.Parse((_ini.Get("kernel", "SafetySpeed")).ToString());
    MiningDepth = double.Parse((_ini.Get("kernel", "MiningDepth")).ToString());
    MineHeight = double.Parse((_ini.Get("kernel", "MineHeight")).ToString());
    DampSpeed = double.Parse((_ini.Get("kernel", "DampSpeed")).ToString());
    MineSize = double.Parse((_ini.Get("kernel", "MineSize")).ToString());
    uranium = double.Parse((_ini.Get("kernel", "uranium")).ToString());
    MiningThrust = float.Parse((_ini.Get("kernel", "MiningThrust")).ToString());
    Hydrogen = bool.Parse((_ini.Get("kernel", "Hydrogen")).ToString());
    Debug = bool.Parse((_ini.Get("kernel", "Debug")).ToString());
}

// Blocks    
public IMyTimerBlock timer;
public IMyLargeTurretBase turr;
public IMyShipConnector conn;
public IMyGasTank tank;
public IMyBatteryBlock bat;
public IMyReactor react;
public IMyThrust thruster;
public IMyGyro gyro;
public IMyCameraBlock cam;
public IMyShipDrill drill;
public IMyCargoContainer cargo;
public IMyCameraBlock targetcam;
public MyInventoryItem item;
public IMyRadioAntenna antenna;
public List<IMyCargoContainer> basecargos = new List<IMyCargoContainer>();
public List<IMyTerminalBlock> baseconns = new List<IMyTerminalBlock>();
public List<IMyTerminalBlock> LCDs = new List<IMyTerminalBlock>();
public List<IMyTerminalBlock> RCS = new List<IMyTerminalBlock>();
public List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
public List<IMyTerminalBlock> newblocks = new List<IMyTerminalBlock>();
public List<string> names = new List<string>();
public List<string> messages = new List<string>();
public List<string> lastcoords = new List<string>();
public List<int> assign = new List<int>();
public List<int> usedconns = new List<int>();
public List<int> miningnumber = new List<int>();
public List<Vector3D> newcoords = new List<Vector3D>();
public List<Vector3D> newheights = new List<Vector3D>();
public List<MyWaypointInfo> oresgps = new List<MyWaypointInfo>();
public List<MyWaypointInfo> oreheights = new List<MyWaypointInfo>();
public List<Vector3D> unitary = new List<Vector3D>();
public List<IMyTerminalBlock> conns = new List<IMyTerminalBlock>();
public List<IMyTerminalBlock> myblocks = new List<IMyTerminalBlock>();
public List<IMyThrust> thrusters = new List<IMyThrust>();
public List<IMyGyro> gyros = new List<IMyGyro>();
public List<IMyCameraBlock> cameras = new List<IMyCameraBlock>();
public List<IMyShipDrill> drills = new List<IMyShipDrill>();
public List<MyInventoryItem> items = new List<MyInventoryItem>();
public List<MyInventoryItem> items2 = new List<MyInventoryItem>();
public List<IMyCargoContainer> cargos = new List<IMyCargoContainer>();
public List<IMyRadioAntenna> antennas = new List<IMyRadioAntenna>();

//    
// Group    


IMyBlockGroup blocksg;
//    
// Check    
public bool IsLCD(IMyTerminalBlock block)
{
    IMyTextPanel block1;
    block1 = block as IMyTextPanel;
    if (block1 != null) return true;
    return false;
}
public bool IsRC(IMyTerminalBlock block)
{
    IMyRemoteControl block1;
    block1 = block as IMyRemoteControl;
    if (block1 != null) return true;
    return false;
}
public bool IsConn(IMyTerminalBlock block)
{
    IMyShipConnector block1;
    block1 = block as IMyShipConnector;
    if (block1 != null) return true;
    return false;
}
public bool NotContainsMess(string w)
{
    StringBuilder z = new StringBuilder();
    z.Append(w);
    z.AppendLine();
    foreach (string s in messages)
    {
        if (s == z.ToString()) return false;
    }
    return true;
}
public string IsMode(int i)
{
    if (i == -1) return String1;
    if (i == 1) return String2;
    if (i == 2) return String3;
    if (i == 3) return String4;
    if (i == 4) return String5;
    if (i == 5) return String6;
    if (i == 6) return String7;
    if (i == 7) return String8;
    return "?";
}
public string IsLoad(string load)
{
    if (load == "|") return "/";
    if (load == "/") return "−";
    if (load == "−") return "\\";
    if (load == "\\") return "|";
    return "";
}
StringBuilder diagnos = new StringBuilder();
StringBuilder sb = new StringBuilder();
StringBuilder message = new StringBuilder();
MyWaypointInfo waypoint;
MyDetectedEntityInfo target;
Vector3D vector = new Vector3D();
string[] str;
string word;
string load = "|";
int cur = 0;
int Frq = 0;
int Cfrq = 0;
int Mode = 0; // -1 - undocking, 0 - simple, 1 - take-off, 2 - on the way to the mine, 3 - preparation for drilling, 4 - drilling, 5 - escape from the mine, 6 - on the way home, 7 - docking   
double value1 = 0;
double value2 = 0;
double value3 = 0;
bool Ready = false;
public Program()
{
    load_configuration();
    Runtime.UpdateFrequency |= UpdateFrequency.Update10;
    word = String9 + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
    message.Append(word); message.AppendLine(); messages.Add(message.ToString()); message.Clear();
    targetcam = GridTerminalSystem.GetBlockWithName(LockCamName) as IMyCameraBlock;
    blocksg = GridTerminalSystem.GetBlockGroupWithName(GrName);
    if (blocksg != null) blocksg.GetBlocks(blocks);
    blocksg = GridTerminalSystem.GetBlockGroupWithName(CargoName);
    if (blocksg != null) blocksg.GetBlocksOfType(basecargos);
    GridTerminalSystem.SearchBlocksOfName(LCDN, LCDs, IsLCD);
    GridTerminalSystem.SearchBlocksOfName(RCN, RCS, IsRC);
    GridTerminalSystem.SearchBlocksOfName(ConnName, baseconns, IsConn);
    //    
    for (int i = 0; i < RCS.Count; i++)
    {
        unitary.Add(new Vector3D());
        assign.Add(-1);
        miningnumber.Add(0);
        usedconns.Add(-1);
        RCS[i].CustomData = "0";
        lastcoords.Add("wow");
        for (int i1 = MineCount + 1; i1 > 0; i1--) { newcoords.Add(new Vector3D()); newheights.Add(new Vector3D()); }
    }
    //    
    Frq = Frequency;
    Cfrq = CargoFreq;
}
void Main(String args)
{
    Frq--;
    Cfrq--;
    load = IsLoad(load);
    Echo(String10 + load);
    Echo("Arguments: ");
    Echo("RUN:<robot number>");
    Echo("CLEAR");
    Echo("RUNALL");
    Echo("CLEAR:<ore deposit name>");
    Echo("MESSAGESCLEAR");
    Echo("GOODBYE:<robot number>");

    sb.Append(String11 + load + String12 + FullName + String13 + load + String11);
    sb.AppendLine();
    sb.Append(String14 + RCS.Count);
    sb.AppendLine();
    if (Debug)
    {
        sb.Append(String15 + Cfrq);
        sb.AppendLine();
        sb.Append(String16 + Frq);
        sb.AppendLine();
    };
    sb.Append(String17);
    sb.AppendLine();
    if (Frq == 0)
    {
        load_configuration();
        basecargos.Clear();
        targetcam = GridTerminalSystem.GetBlockWithName(LockCamName) as IMyCameraBlock;
        if (targetcam != null) targetcam.EnableRaycast = true;
        blocksg = GridTerminalSystem.GetBlockGroupWithName(GrName);
        if (blocksg != null) blocksg.GetBlocks(newblocks);
        blocksg = GridTerminalSystem.GetBlockGroupWithName(CargoName);
        if (blocksg != null) blocksg.GetBlocksOfType(basecargos);
        for (int i = newblocks.Count - 1; i > -1; i--) { if (!blocks.Contains(newblocks[i])) blocks.Add(newblocks[i]); }
        newblocks.Clear();
        GridTerminalSystem.SearchBlocksOfName(LCDN, newblocks, IsLCD);
        for (int i = newblocks.Count - 1; i > -1; i--) { if (!LCDs.Contains(newblocks[i])) LCDs.Add(newblocks[i]); }
        newblocks.Clear();
        GridTerminalSystem.SearchBlocksOfName(RCN, newblocks, IsRC);
        for (int i = newblocks.Count - 1; i > -1; i--)
        {
            if (!RCS.Contains(newblocks[i]))
            {
                RCS.Add(newblocks[i]);
                unitary.Add(new Vector3D());
                assign.Add(-1);
                miningnumber.Add(0);
                usedconns.Add(-1);
                lastcoords.Add("wow");
                for (int i1 = MineCount + 1; i1 > 0; i1--) { newcoords.Add(new Vector3D()); newheights.Add(new Vector3D()); }
            }
        }
        newblocks.Clear();
        GridTerminalSystem.SearchBlocksOfName(ConnName, newblocks, IsConn);
        for (int i = newblocks.Count - 1; i > -1; i--) { if (!baseconns.Contains(newblocks[i])) baseconns.Add(newblocks[i]); }
        newblocks.Clear();
        for (int i = blocks.Count - 1; i > -1; i--) { if (!blocks[i].IsFunctional) blocks.RemoveAt(i); }
        Frq = Frequency;
    }
    cur = 0;
    diagnos.Append(String18);
    diagnos.AppendLine();
    for (int q = 0; q < RCS.Count; q++)
    {

        IMyRemoteControl RC = RCS[q] as IMyRemoteControl;
        if (RC.IsFunctional)
        {
            int.TryParse(RC.CustomData, out Mode);
            diagnos.Append(String19 + DClass + " #" + (q + 1) + ":");
            diagnos.AppendLine();
            sb.Append(String19 + DClass + " #" + (q + 1) + ":");
            sb.AppendLine();
            sb.Append(String20);
            if (Mode != 0) sb.Append(IsMode(Mode));

            for (int i = blocks.Count - 1; i > -1; i--)
            {
                if (Mode == 0 && SearchRange != -1)
                {
                    if ((blocks[i].GetPosition() - RC.GetPosition()).Length() < SearchRange) myblocks.Add(blocks[i]);
                }
                else
                if (blocks[i].IsSameConstructAs(RC)) myblocks.Add(blocks[i]);
            }
            for (int i = myblocks.Count - 1; i > -1; i--)
            {
                if (IsConn(myblocks[i])) { conns.Add(myblocks[i]); continue; }
                thruster = myblocks[i] as IMyThrust;
                if (thruster != null) { thrusters.Add(thruster); continue; }
                antenna = myblocks[i] as IMyRadioAntenna;
                if (antenna != null) { antennas.Add(antenna); continue; }
                gyro = myblocks[i] as IMyGyro;
                if (gyro != null) { gyros.Add(gyro); continue; }
                cargo = myblocks[i] as IMyCargoContainer;
                if (cargo != null) { cargos.Add(cargo); continue; }
                cam = myblocks[i] as IMyCameraBlock;
                if (cam != null) { cameras.Add(cam); continue; }
                drill = myblocks[i] as IMyShipDrill;
                if (drill != null) { drills.Add(drill); continue; }
                timer = myblocks[i] as IMyTimerBlock;
                if (timer != null && timer.CustomName == EmergencyTimer)
                {
                    if (Mode == 0) { timer.StopCountdown(); timer.TriggerDelay = EmergencyCount; }
                    else { timer.TriggerDelay = EmergencyCount; timer.StartCountdown(); }
                }
            }
            for (int i = 0; i < myblocks.Count; i++)
            {
                IMyRadioAntenna RCantenna = myblocks[i] as IMyRadioAntenna;
                if (RCantenna != null) { RCantenna.CustomName = (DClass + " - " + FullName + " - #" + (q + 1).ToString()); }
                IMyTextPanel RCLCD = myblocks[i] as IMyTextPanel;
                if (RCLCD != null && RCLCD.CustomName == IDN) { RCLCD.ContentType = ContentType.TEXT_AND_IMAGE; RCLCD.WriteText((q + 1).ToString()); }
            }
            value1 = 0;
            value2 = 0;
            if (thrusters.Count < 6)
            {
                diagnos.Append(String21);
                diagnos.AppendLine();
                value1++;
            }
            if (gyros.Count < 1)
            {
                diagnos.Append(String22);
                diagnos.AppendLine();
                value1++;
                foreach (IMyGyro gyro in gyros)
                {
                    gyro.GyroOverride = false;
                    gyro.Enabled = false;
                    gyro.Pitch = 0;
                    gyro.Yaw = 0;
                }
            }
            if (cargos.Count < 1)
            {
                diagnos.Append(String23);
                diagnos.AppendLine();
                value1++;
            }
            if (cameras.Count < 2)
            {
                diagnos.Append(String24);
                diagnos.AppendLine();
                value2++;
            }
            if (drills.Count < 1)
            {
                diagnos.Append(String25);
                diagnos.AppendLine();
                value1++;
            }
            if (conns.Count < 1)
            {
                diagnos.Append(String26);
                diagnos.AppendLine();
                value1++;
            }
            Ready = false;
            foreach (IMyTerminalBlock block in myblocks)
            {
                react = block as IMyReactor;
                if (react != null) { Ready = true; break; }
                bat = block as IMyBatteryBlock;
                if (bat != null) { Ready = true; break; }
            }
            if (!Ready)
            {
                diagnos.Append(String27);
                diagnos.AppendLine();
                value1++;
            }
            Ready = false;
            foreach (IMyTerminalBlock block in myblocks)
            {
                tank = block as IMyGasTank;
                if (tank != null) { Ready = true; break; }
            }
            if (!Ready && Hydrogen)
            {
                diagnos.Append(String28);
                diagnos.AppendLine();
                value2++;
            }
            if (baseconns.Count < RCS.Count)
            {
                diagnos.Append(String29);
                diagnos.AppendLine();
                value2++;
            }
            if (basecargos.Count < 1)
            {
                diagnos.Append(String30);
                diagnos.AppendLine();
                value2++;
            }
            diagnos.Append(String31 + String32 + value1 + String33 + value2);
            diagnos.AppendLine();
            diagnos.Append(String34);
            diagnos.AppendLine();
            if (value1 > 0 && Mode == 0) { Clear(); continue; }

            //Undocking Mode   

            if (Mode == -1)
            {
                Ready = true;
                foreach (IMyGyro gyro in gyros)
                {
                    gyro.GyroOverride = true;
                    gyro.Enabled = true;
                    gyro.Pitch = 0;
                    gyro.Yaw = 0;
                }
                foreach (IMyShipConnector conn in conns) { if (conn.Status == MyShipConnectorStatus.Connected) { Ready = false; break; } }
                if (RC.IsSameConstructAs(Me) || !Ready)
                {
                    word = String19 + DClass + "#" + (q + 1) + String35 + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                    if (NotContainsMess(word.ToString())) { message.Append(word); message.AppendLine(); messages.Add(message.ToString()); message.Clear(); }
                    Mode = 0;
                }
                if (Ready) Mode = 1;
            }
            //    
            if (Mode == 0)
            {
                usedconns[q] = -1;
                Ready = true;
                foreach (IMyThrust thr in thrusters) { thr.Enabled = false; thr.ThrustOverridePercentage = 0; }
                foreach (IMyShipConnector conn in conns) { conn.Enabled = true; conn.Connect(); }
                foreach (IMyShipDrill drill in drills) drill.Enabled = false;
                foreach (IMyCameraBlock cam in cameras) { cam.Enabled = true; cam.EnableRaycast = true; }
                foreach (IMyGyro gyro in gyros)
                {
                    gyro.Pitch = 0;
                    gyro.Yaw = 0;
                    gyro.GyroOverride = false;
                }
                RC.ClearWaypoints();
                for (int i = myblocks.Count - 1; i > -1; i--)
                {
                    bat = myblocks[i] as IMyBatteryBlock;
                    if (bat != null) { bat.ChargeMode = ChargeMode.Recharge; if (bat.CurrentStoredPower < bat.MaxStoredPower - 0.1) Ready = false; continue; }
                    react = myblocks[i] as IMyReactor;
                    if (react != null)
                    {
                        react.Enabled = true;
                        react.UseConveyorSystem = false;
                        if (Cfrq == 0)
                        {
                            if (react.GetInventory().ItemCount > 0)
                            {
                                item = (MyInventoryItem)react.GetInventory().GetItemAt(0);
                                if (item.Amount < (VRage.MyFixedPoint)uranium)
                                {
                                    Ready = false;
                                    for (int i1 = basecargos.Count - 1; i1 > -1; i1--)
                                    {
                                        if (basecargos[i1].GetInventory().ItemCount > 0)
                                        {
                                            basecargos[i1].GetInventory().GetItems(items);
                                            for (int i2 = items.Count - 1; i2 > -1; i2--)
                                            {
                                                if (items[i2].Type == MyItemType.Parse("MyObjectBuilder_Ingot/Uranium"))
                                                { basecargos[i1].GetInventory().TransferItemTo(react.GetInventory(), items[i2], (VRage.MyFixedPoint)uranium - item.Amount); break; }
                                            }
                                        }
                                        items.Clear();
                                    }
                                }
                            }
                            else
                            {
                                Ready = false;
                                for (int i1 = basecargos.Count - 1; i1 > -1; i1--)
                                {
                                    if (basecargos[i1].GetInventory().ItemCount > 0)
                                    {
                                        basecargos[i1].GetInventory().GetItems(items);
                                        for (int i2 = items.Count - 1; i2 > -1; i2--)
                                        {
                                            if (items[i2].Type == MyItemType.Parse("MyObjectBuilder_Ingot/Uranium"))
                                            { basecargos[i1].GetInventory().TransferItemTo(react.GetInventory(), items[i2], (VRage.MyFixedPoint)uranium); break; }
                                        }
                                    }
                                    items.Clear();
                                }
                            }
                        }
                        continue;
                    }
                    tank = myblocks[i] as IMyGasTank;
                    if (tank != null)
                    {
                        tank.Stockpile = true;
                        if (tank.FilledRatio < 0.98) Ready = false;
                        continue;
                    }
                    if (Cfrq == 0)
                    {
                        for (int i1 = cargos.Count - 1; i1 > -1; i1--)
                        {
                            if (cargos[i1].GetInventory().ItemCount > 0)
                            {
                                item = (MyInventoryItem)cargos[i1].GetInventory().GetItemAt(0);
                                Ready = false;
                                for (int i2 = basecargos.Count - 1; i2 > -1; i2--)
                                {
                                    if (!cargos[i1].GetInventory().CanTransferItemTo(basecargos[i2].GetInventory(), item.Type) && basecargos[i2].GetInventory().IsFull) continue;
                                    cargos[i1].GetInventory().TransferItemTo(basecargos[i2].GetInventory(), item, item.Amount); break;
                                }
                            }
                        }
                        for (int i1 = drills.Count - 1; i1 > -1; i1--)
                        {
                            if (drills[i1].GetInventory().ItemCount > 0)
                            {
                                item = (MyInventoryItem)drills[i1].GetInventory().GetItemAt(0);
                                Ready = false;
                                for (int i2 = basecargos.Count - 1; i2 > -1; i2--)
                                {
                                    if (!drills[i1].GetInventory().CanTransferItemTo(basecargos[i2].GetInventory(), item.Type) && basecargos[i2].GetInventory().IsFull) continue;
                                    drills[i1].GetInventory().TransferItemTo(basecargos[i2].GetInventory(), item, item.Amount); break;
                                }
                            }
                        }
                        for (int i1 = conns.Count - 1; i1 > -1; i1--)
                        {
                            if (conns[i1].GetInventory().ItemCount > 0)
                            {
                                item = (MyInventoryItem)conns[i1].GetInventory().GetItemAt(0);
                                Ready = false;
                                for (int i2 = basecargos.Count - 1; i2 > -1; i2--)
                                {
                                    if (!conns[i1].GetInventory().CanTransferItemTo(basecargos[i2].GetInventory(), item.Type) && basecargos[i2].GetInventory().IsFull) continue;
                                    conns[i1].GetInventory().TransferItemTo(basecargos[i2].GetInventory(), item, item.Amount); break;
                                }
                            }
                        }
                    }
                }

                if (drills.Count < 1) Ready = false;
                if (thrusters.Count < 6) Ready = false;
                if (conns.Count < 1) Ready = false;
                if (cargos.Count < 1) Ready = false;
                if (Ready && Cfrq == 0)
                {
                    for (int i = cameras.Count - 1; i > -1; i--)
                    {
                        if (cameras[i].CanScan(RC.GetPosition() + RC.WorldMatrix.Forward * (ConnRange + 20)))
                        {
                            target = cameras[i].Raycast(RC.GetPosition() + RC.WorldMatrix.Forward * (ConnRange + 20));
                            if (target.EntityId == RC.EntityId) continue;
                            if (target.EntityId != 0) { Ready = false; break; }
                            else break;
                        }
                    }
                    if (Ready)
                    {
                        for (int i = 0; i < oresgps.Count; i++)
                        {
                            if (oresgps[i].IsEmpty() || oresgps[i].Coords == new Vector3D()) continue;
                            if (!assign.Contains(i)) { assign[q] = i; for (int i1 = cur; i1 < cur + MineCount + 1; i1++) { newcoords[i1] = new Vector3D(); newheights[i1] = new Vector3D(); } break; }
                        }
                        if (assign[q] != -1)
                        {
                            Mode = -1;
                            foreach (IMyThrust thr in thrusters) { thr.Enabled = true; thr.ThrustOverridePercentage = 0; }
                            for (int i = myblocks.Count - 1; i > -1; i--)
                            {
                                bat = myblocks[i] as IMyBatteryBlock;
                                if (bat != null) { bat.ChargeMode = ChargeMode.Discharge; if (bat.CurrentStoredPower != bat.MaxStoredPower) continue; }
                                react = myblocks[i] as IMyReactor;
                                if (react != null)
                                {
                                    react.Enabled = true;
                                    continue;
                                }
                                tank = myblocks[i] as IMyGasTank;
                                if (tank != null) tank.Stockpile = false;
                            }
                            foreach (IMyShipConnector conn in conns) { conn.Enabled = false; conn.Disconnect(); }
                            unitary[q] = RC.GetPosition();
                        }
                    }
                    else
                    {
                        word = String19 + DClass + "#" + (q + 1) + String36 + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                        if (NotContainsMess(word.ToString())) { message.Append(word); message.AppendLine(); messages.Add(message.ToString()); message.Clear(); }
                    }
                }
                if (Ready) { sb.Append(String37); }
                if (!Ready) { sb.Append(String38); }
            }
            sb.AppendLine();
            sb.Append(String39 + ((float)RC.GetShipSpeed()).ToString("0.0") + String40);
            sb.AppendLine();
            sb.Append(String41 + ((float)(RC.GetPosition() - Me.GetPosition()).Length()).ToString("0.00") + String42);
            sb.AppendLine();
            if (Debug)
            {
                sb.Append(String43 + cargos.Count);
                sb.AppendLine();
                sb.Append(String44 + thrusters.Count);
                sb.AppendLine();
                sb.Append(String45 + drills.Count);
                sb.AppendLine();
                sb.Append(String46 + conns.Count);
                sb.AppendLine();
            }
            value1 = 0;
            value2 = 0;
            foreach (IMyTerminalBlock block in myblocks)
            {
                bat = block as IMyBatteryBlock;
                if (bat != null) { value1 += bat.MaxStoredPower; value2 += bat.CurrentStoredPower; }
            }
            if (value1 != 0)
            {
                sb.Append(String47 + ((float)(value2 / value1) * 100).ToString("0") + "%");
                sb.AppendLine();
            }
            value1 = 0;
            value2 = 0;
            foreach (IMyTerminalBlock block in myblocks)
            {
                tank = block as IMyGasTank;
                if (tank != null) { value1 += tank.FilledRatio; value2++; }
            }
            if (value1 != 0)
            {
                sb.Append(String48 + (float)(value1 / value2) * 100 + "%");
                sb.AppendLine();
            }
            if (assign[q] != -1) word = oresgps[assign[q]].Name;
            else word = String49;
            sb.Append(String50 + word);
            sb.AppendLine();
            //    
            if (Mode == 1)
            {
                foreach (IMyRadioAntenna antenna in antennas)
                {
                    antenna.Enabled = true;
                }
                newcoords[cur] = oresgps[assign[q]].Coords;
                newheights[cur] = oreheights[assign[q]].Coords;
                RC.ClearWaypoints();
                RC.FlightMode = FlightMode.OneWay;
                vector = unitary[q];
                foreach (IMyThrust thr in thrusters) { thr.Enabled = true; if (thr.WorldMatrix.Forward == RC.WorldMatrix.Backward) thr.ThrustOverridePercentage = (float)(1 - RC.GetShipSpeed() / SafetySpeed); }
                if ((vector - RC.GetPosition()).Length() >= ConnRange) Mode = 2;
            }
            //    
            if (Mode == 2)
            {
                foreach (IMyThrust thr in thrusters) { thr.Enabled = true; thr.ThrustOverridePercentage = 0; }
                MyWaypointInfo.TryParse("GPS: " + "ORE" + " #1:" + newheights[cur + miningnumber[q]].X + ":" + newheights[cur + miningnumber[q]].Y + ":" + newheights[cur + miningnumber[q]].Z + ":", out waypoint);
                RC.AddWaypoint(waypoint);
                RC.SetAutoPilotEnabled(true);
                RC.SetCollisionAvoidance(true);
                if ((RC.GetPosition() - newheights[cur + miningnumber[q]]).Length() < 25) Mode = 3;
            }
            //    
            if (Mode == 3)
            {
                Ready = true;
                RC.SetAutoPilotEnabled(false);
                for (int i = cameras.Count - 1; i > -1; i--)
                {
                    if (cameras[i].CanScan(newcoords[cur + miningnumber[q]] + Vector3D.Normalize(newcoords[cur + miningnumber[q]] - cameras[i].GetPosition()) * 100))
                    {
                        target = cameras[i].Raycast(newcoords[cur + miningnumber[q]] + Vector3D.Normalize(newcoords[cur + miningnumber[q]] - cameras[i].GetPosition()) * 100);
                        if (target.EntityId == RC.CubeGrid.EntityId) continue;
                        if (target.Type == MyDetectedEntityType.Asteroid || target.Type == MyDetectedEntityType.Planet) break;
                        else if (i == 0) Ready = false;
                    }
                }
                if (!Ready)
                {
                    for (int i = 0; i < baseconns.Count; i++)
                    {
                        conn = baseconns[i] as IMyShipConnector;
                        if (conn != null && (conn.Status != MyShipConnectorStatus.Unconnected || !conn.IsFunctional)) { continue; }
                        if (!usedconns.Contains(i)) { usedconns[q] = i; break; }
                    }
                    if (usedconns[q] != -1) Mode = 6;
                    else
                    {
                        word = String19 + DClass + "#" + (q + 1) + String51 + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                        if (NotContainsMess(word.ToString())) { message.Append(word); message.AppendLine(); messages.Add(message.ToString()); message.Clear(); }
                    }
                    word = String19 + DClass + "#" + (q + 1) + String52 + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                    if (NotContainsMess(word.ToString())) { message.Append(word); message.AppendLine(); messages.Add(message.ToString()); message.Clear(); }
                    RC.CustomData = Mode.ToString();
                    Clear();
                    continue;
                }
                Ready = false;
                foreach (IMyGyro gyro in gyros)
                {
                    if (gyro.WorldMatrix.Forward != RC.WorldMatrix.Forward) continue;
                    vector = newcoords[cur] - RC.GetPosition();
                    value1 = (Vector3D.Dot(gyro.WorldMatrix.Forward, Vector3D.Normalize(vector)) / vector.Length() * 100);
                    value2 = (Vector3D.Dot(gyro.WorldMatrix.Right, Vector3D.Normalize(vector)) / vector.Length() * 100);
                    value3 = (Vector3D.Dot(gyro.WorldMatrix.Up, Vector3D.Normalize(vector)) / vector.Length() * 100);
                    gyro.GyroOverride = true;
                    gyro.Enabled = true;
                    gyro.Pitch = (float)(Math.Atan2(-value3, value1));
                    gyro.Yaw = (float)(Math.Atan2(value2, value1));
                    if (Vector3D.Reject(RC.WorldMatrix.Forward, Vector3D.Normalize(vector)).Length() <= 0.08) Ready = true;
                    else Ready = false;
                }
                if (Ready)
                {
                    value1 = MineSize;
                    for (int i = 1; i < MineCount + 1;)
                    {
                        if (newcoords[cur + i] == new Vector3D())
                            newcoords[cur + i] = RC.GetPosition() + RC.WorldMatrix.Forward * (oresgps[assign[q]].Coords - RC.GetPosition()).Length() + RC.WorldMatrix.Left * value1;
                        if (newheights[cur + i] == new Vector3D())
                            newheights[cur + i] = RC.GetPosition() + RC.WorldMatrix.Forward * 1 + RC.WorldMatrix.Left * value1;
                        i++;
                        if (i > MineCount) break;
                        if (newcoords[cur + i] == new Vector3D())
                            newcoords[cur + i] = RC.GetPosition() + RC.WorldMatrix.Forward * (oresgps[assign[q]].Coords - RC.GetPosition()).Length() + RC.WorldMatrix.Right * value1;
                        if (newheights[cur + i] == new Vector3D())
                            newheights[cur + i] = RC.GetPosition() + RC.WorldMatrix.Forward * 1 + RC.WorldMatrix.Right * value1;
                        i++;
                        if (i > MineCount) break;
                        if (newcoords[cur + i] == new Vector3D())
                            newcoords[cur + i] = RC.GetPosition() + RC.WorldMatrix.Forward * (oresgps[assign[q]].Coords - RC.GetPosition()).Length() + RC.WorldMatrix.Up * value1;
                        if (newheights[cur + i] == new Vector3D())
                            newheights[cur + i] = RC.GetPosition() + RC.WorldMatrix.Forward * 1 + RC.WorldMatrix.Up * value1;
                        i++;
                        if (i > MineCount) break;
                        if (newcoords[cur + i] == new Vector3D())
                            newcoords[cur + i] = RC.GetPosition() + RC.WorldMatrix.Forward * (oresgps[assign[q]].Coords - RC.GetPosition()).Length() + RC.WorldMatrix.Down * value1;
                        if (newheights[cur + i] == new Vector3D())
                            newheights[cur + i] = RC.GetPosition() + RC.WorldMatrix.Forward * 1 + RC.WorldMatrix.Down * value1;
                        i++;
                        if (i > MineCount) break;
                        if (newcoords[cur + i] == new Vector3D())
                            newcoords[cur + i] = RC.GetPosition() + RC.WorldMatrix.Forward * (oresgps[assign[q]].Coords - RC.GetPosition()).Length() + RC.WorldMatrix.Left * value1 / 2 + RC.WorldMatrix.Up * value1 / 2;
                        if (newheights[cur + i] == new Vector3D())
                            newheights[cur + i] = RC.GetPosition() + RC.WorldMatrix.Forward * 1 + RC.WorldMatrix.Left * value1 / 2 + RC.WorldMatrix.Up * value1 / 2;
                        i++;
                        if (i > MineCount) break;
                        if (newcoords[cur + i] == new Vector3D())
                            newcoords[cur + i] = RC.GetPosition() + RC.WorldMatrix.Forward * (oresgps[assign[q]].Coords - RC.GetPosition()).Length() + RC.WorldMatrix.Left * value1 / 2 + RC.WorldMatrix.Down * value1 / 2;
                        if (newheights[cur + i] == new Vector3D())
                            newheights[cur + i] = RC.GetPosition() + RC.WorldMatrix.Forward * 1 + RC.WorldMatrix.Left * value1 / 2 + RC.WorldMatrix.Down * value1 / 2;
                        i++;
                        if (i > MineCount) break;
                        if (newcoords[cur + i] == new Vector3D())
                            newcoords[cur + i] = RC.GetPosition() + RC.WorldMatrix.Forward * (oresgps[assign[q]].Coords - RC.GetPosition()).Length() + RC.WorldMatrix.Right * value1 / 2 + RC.WorldMatrix.Up * value1 / 2;
                        if (newheights[cur + i] == new Vector3D())
                            newheights[cur + i] = RC.GetPosition() + RC.WorldMatrix.Forward * 1 + RC.WorldMatrix.Right * value1 / 2 + RC.WorldMatrix.Up * value1 / 2;
                        i++;
                        if (i > MineCount) break;
                        if (newcoords[cur + i] == new Vector3D())
                            newcoords[cur + i] = RC.GetPosition() + RC.WorldMatrix.Forward * (oresgps[assign[q]].Coords - RC.GetPosition()).Length() + RC.WorldMatrix.Right * value1 / 2 + RC.WorldMatrix.Down * value1 / 2;
                        if (newheights[cur + i] == new Vector3D())
                            newheights[cur + i] = RC.GetPosition() + RC.WorldMatrix.Forward * 1 + RC.WorldMatrix.Right * value1 / 2 + RC.WorldMatrix.Down * value1 / 2;
                        i++;
                        if (i > MineCount) break;
                        value1 += MineSize;
                    }
                    unitary[q] = newcoords[cur + miningnumber[q]] + Vector3D.Normalize(newcoords[cur + miningnumber[q]] - RC.GetPosition()) * (MiningDepth * 2); //RC.GetPosition() + RC.WorldMatrix.Forward * (MiningDepth * 2 + (newcoords[cur + miningnumber[q]] - newheights[cur + miningnumber[q]]).Length());   
                    Mode = 4;
                }
            }

            // Mining Operation    

            if (Mode == 4)
            {
                Ready = true;
                for (int i = cameras.Count - 1; i > -1; i--)
                {
                    if (cameras[i].CanScan(RC.GetPosition() + RC.WorldMatrix.Forward * 100))
                    {
                        target = cameras[i].Raycast(RC.GetPosition() + RC.WorldMatrix.Forward * 100);
                        if (target.EntityId == RC.CubeGrid.EntityId) continue;
                        if (target.Type == MyDetectedEntityType.Asteroid || target.Type == MyDetectedEntityType.Planet) break;
                        else Ready = false;
                    }
                }
                if (!Ready)
                {
                    Mode = 5;
                    if (miningnumber[q] == MineCount)
                    {
                        miningnumber[q] = 0;
                        oresgps[assign[q]] = new MyWaypointInfo();
                        oreheights[assign[q]] = new MyWaypointInfo();
                        assign[q] = -1;
                        word = String19 + DClass + "#" + (q + 1) + String53 + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                        if (NotContainsMess(word.ToString())) { message.Append(word); message.AppendLine(); messages.Add(message.ToString()); message.Clear(); }
                    }
                    else miningnumber[q]++;
                    word = String19 + DClass + "#" + (q + 1) + String54 + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                    if (NotContainsMess(word.ToString())) { message.Append(word); message.AppendLine(); messages.Add(message.ToString()); message.Clear(); }
                    unitary[q] = RC.GetPosition();
                    foreach (IMyThrust thrust in thrusters) thrust.ThrustOverridePercentage = 0;
                    RC.CustomData = Mode.ToString();
                    Clear();
                    continue;
                }
                vector = unitary[q] - RC.GetPosition();
                foreach (IMyGyro gyro in gyros)
                {
                    if (gyro.WorldMatrix.Forward != RC.WorldMatrix.Forward) continue;
                    value1 = (Vector3D.Dot(gyro.WorldMatrix.Forward, Vector3D.Normalize(vector)) / vector.Length() * 100);
                    value2 = (Vector3D.Dot(gyro.WorldMatrix.Right, Vector3D.Normalize(vector)) / vector.Length() * 100);
                    value3 = (Vector3D.Dot(gyro.WorldMatrix.Up, Vector3D.Normalize(vector)) / vector.Length() * 100);
                    gyro.GyroOverride = true;
                    gyro.Enabled = true;
                    gyro.Pitch = (float)(Math.Atan2(-value3, value1));
                    gyro.Yaw = (float)(Math.Atan2(value2, value1));
                }
                if (((Vector3D)target.HitPosition - RC.GetPosition()).Length() > DampRange)
                {
                    foreach (IMyThrust thrust in thrusters)
                    {
                        thrust.Enabled = true;
                        if (thrust.WorldMatrix.Forward != RC.WorldMatrix.Forward) thrust.ThrustOverridePercentage = (float)-thrust.WorldMatrix.Forward.Dot(vector - RC.GetShipVelocities().LinearVelocity);
                        if (thrust.WorldMatrix.Forward == RC.WorldMatrix.Backward) thrust.ThrustOverridePercentage = (float)(1 - RC.GetShipSpeed() / SafetySpeed);
                    }
                }
                else
                {
                    Ready = false;
                    if (Cfrq == 0)
                    {
                        Ready = true;
                        foreach (IMyCargoContainer cargo in cargos) { if (!cargo.GetInventory().IsFull) { Ready = false; break; } }
                    }

                    if (vector.Length() > MiningDepth && !Ready)
                    {
                        foreach (IMyThrust thrust in thrusters)
                        {
                            thrust.Enabled = true;
                            if (thrust.WorldMatrix.Forward != RC.WorldMatrix.Forward) thrust.ThrustOverridePercentage = 0;
                            if (thrust.WorldMatrix.Forward == RC.WorldMatrix.Backward) thrust.ThrustOverridePercentage = (float)(1 - RC.GetShipSpeed() / MiningThrust);
                            if (RC.GetShipSpeed() > MiningThrust) { if (thrust.WorldMatrix.Forward == RC.WorldMatrix.Backward) thrust.ThrustOverridePercentage = 0; }
                        }
                        foreach (IMyShipDrill drill in drills) { drill.Enabled = true; }
                    }
                    if (vector.Length() < MiningDepth || Ready)
                    {
                        foreach (IMyThrust thrust in thrusters) { thrust.ThrustOverridePercentage = 0; }
                        if (miningnumber[q] == MineCount)
                        {
                            miningnumber[q] = 0;
                            oresgps[assign[q]] = new MyWaypointInfo();
                            oreheights[assign[q]] = new MyWaypointInfo();
                            assign[q] = -1;
                            word = String19 + DClass + "#" + (q + 1) + String53 + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                            if (NotContainsMess(word.ToString())) { message.Append(word); message.AppendLine(); messages.Add(message.ToString()); message.Clear(); }
                        }
                        else
                            miningnumber[q]++;
                        Mode = 5;
                        unitary[q] = RC.GetPosition();
                    }
                }
            }


            //Leaving Mining Site    

            if (Mode == 5)
            {
                vector = unitary[q] - RC.GetPosition();

                foreach (IMyGyro gyro in gyros)
                {
                      
                    if (gyro.WorldMatrix.Forward != RC.WorldMatrix.Forward) continue;
                    gyro.GyroOverride = true;
                    gyro.Enabled = true;
                    value1 = (Vector3D.Dot(gyro.WorldMatrix.Forward, Vector3D.Normalize(vector)) / vector.Length() * 100);   
                    value2 = (Vector3D.Dot(gyro.WorldMatrix.Right, Vector3D.Normalize(vector)) / vector.Length() * 100);   
                    value3 = (Vector3D.Dot(gyro.WorldMatrix.Up, Vector3D.Normalize(vector)) / vector.Length() * 100);   
                    
                    foreach (IMyShipDrill drill in drills) { drill.Enabled = false; }
                    /* 
                    gyro.Pitch = 0;
                    gyro.Yaw = 0;
                    */
                }

                foreach (IMyThrust thrust in thrusters)
                {
                    thrust.Enabled = true;
                    if (thrust.WorldMatrix.Forward != RC.WorldMatrix.Backward) thrust.ThrustOverridePercentage = 0; //(float)-Vector3D.Dot(thrust.WorldMatrix.Forward, Vector3D.Normalize(Vector3D.Reject(thrust.GetPosition() + thrust.WorldMatrix.Forward, Vector3D.Normalize(vector - RC.GetShipVelocities().LinearVelocity)))) / 35;   
                    if (thrust.WorldMatrix.Forward == RC.WorldMatrix.Forward) thrust.ThrustOverridePercentage = (float)(1 - RC.GetShipSpeed() / SafetySpeed);
                }
                if ((unitary[q] - RC.GetPosition()).Length() > MiningDepth * 2 + (newcoords[cur + miningnumber[q]] - newheights[cur + miningnumber[q]]).Length())
                {
                    foreach (IMyGyro gyro in gyros)
                    {
                        gyro.Pitch = 0;
                        gyro.Yaw = 0;
                        gyro.GyroOverride = false;
                    }
                    foreach (IMyThrust thrust in thrusters)
                    {
                        thrust.ThrustOverridePercentage = 0;
                    }
                    RC.ClearWaypoints();
                    for (int i = 0; i < baseconns.Count; i++)
                    {
                        conn = baseconns[i] as IMyShipConnector;
                        if (conn != null && (conn.Status != MyShipConnectorStatus.Unconnected || !conn.IsFunctional)) { continue; }
                        if (!usedconns.Contains(i)) usedconns[q] = i;
                    }
                    if (usedconns[q] != -1)
                    {
                        Mode = 6;
                    }
                    else
                    {
                        word = String19 + DClass + "#" + (q + 1) + String51 + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                        if (NotContainsMess(word.ToString())) { message.Append(word); message.AppendLine(); messages.Add(message.ToString()); message.Clear(); }
                    }
                }
            }

            //Returning Home    


            if (Mode == 6)
            {
                vector = baseconns[usedconns[q]].GetPosition() + baseconns[usedconns[q]].WorldMatrix.Forward * ConnRange;
                MyWaypointInfo.TryParse("GPS: " + "CONNECTOR" + " #1:" + vector.X + ":" + vector.Y + ":" + vector.Z + ":", out waypoint);
                if (Cfrq == 0) RC.ClearWaypoints();
                RC.AddWaypoint(waypoint);
                RC.SetAutoPilotEnabled(true);
                RC.SetCollisionAvoidance(true);
                if ((RC.GetPosition() - waypoint.Coords).Length() < 3)
                { Mode = 7; }
            }
            //    
            if (Mode == 7)
            {
                unitary[q] = baseconns[usedconns[q]].GetPosition();
                RC.SetAutoPilotEnabled(false);
                Ready = true;
                for (int i = cameras.Count - 1; i > -1; i--)
                    {   
                        if (cameras[i].CanScan(unitary[q]))   
                        {   
                            target = cameras[i].Raycast(unitary[q]);   
                            if (target.EntityId == RC.CubeGrid.EntityId) continue;   
                            if (target.EntityId == 0 || target.EntityId == baseconns[usedconns[q]].CubeGrid.EntityId || ((Vector3D)target.HitPosition - unitary[q]).Length() < 2) break;   
                            else { Ready = false; break; }   
                        }   
                    }
                    if (!Ready)
                    {
                        word = String19 + DClass + "#" + (q + 1) + String55 + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                        if (NotContainsMess(word.ToString())) { message.Append(word); message.AppendLine(); messages.Add(message.ToString()); message.Clear(); }
                        Mode = 6;
                        RC.CustomData = Mode.ToString();
                        Clear();
                        continue;
                    }
                vector = baseconns[usedconns[q]].GetPosition() + baseconns[usedconns[q]].WorldMatrix.Forward * ConnRange;
                if ((vector - RC.GetPosition()).Length() > ConnRange + 15) { foreach (IMyThrust thrust in thrusters) thrust.ThrustOverridePercentage = 0; Mode = 6; RC.CustomData = Mode.ToString(); continue; }
                Ready = false;
                vector = unitary[q] - RC.GetPosition();
                foreach (IMyThrust thrust in thrusters)
                {
                    thrust.Enabled = true;
                    thrust.ThrustOverridePercentage = 0;
                }
                foreach (IMyGyro gyro in gyros)
                {
                    if (gyro.WorldMatrix.Forward != RC.WorldMatrix.Forward) continue;
                    value1 = (Vector3D.Dot(gyro.WorldMatrix.Forward, Vector3D.Normalize(-vector)) / vector.Length() * 100);
                    value2 = (Vector3D.Dot(gyro.WorldMatrix.Right, Vector3D.Normalize(-vector)) / vector.Length() * 100);
                    value3 = (Vector3D.Dot(gyro.WorldMatrix.Up, Vector3D.Normalize(-vector)) / vector.Length() * 100);
                    gyro.GyroOverride = true;
                    gyro.Enabled = true;
                    gyro.Pitch = (float)(Math.Atan2(-value3, value1));
                    gyro.Yaw = (float)(Math.Atan2(value2, value1));
                    if (Vector3D.Reject(RC.WorldMatrix.Backward, Vector3D.Normalize(vector)).Length() <= 0.08) Ready = true;
                }
                if (Ready)
                {
                    Ready = true;
                    foreach (IMyThrust thrust in thrusters)
                    {
                        thrust.Enabled = true;
                        if (thrust.WorldMatrix.Forward != RC.WorldMatrix.Forward && thrust.WorldMatrix.Forward != RC.WorldMatrix.Backward) { thrust.ThrustOverridePercentage = (float)-thrust.WorldMatrix.Forward.Dot(vector - RC.GetShipVelocities().LinearVelocity); if (RC.GetShipSpeed() > SafetySpeed) Ready = false; }
                    }
                    if (Ready)
                    {
                        if ((baseconns[usedconns[q]].GetPosition() - RC.GetPosition()).Length() > DampRange)
                        {
                            foreach (IMyThrust thrust in thrusters)
                            {
                                thrust.Enabled = true;
                                if (thrust.WorldMatrix.Forward != RC.WorldMatrix.Backward) thrust.ThrustOverridePercentage = (float)-thrust.WorldMatrix.Forward.Dot(vector - RC.GetShipVelocities().LinearVelocity);
                                if (thrust.WorldMatrix.Forward == RC.WorldMatrix.Forward) thrust.ThrustOverridePercentage = (float)(1 - RC.GetShipSpeed() / SafetySpeed);
                            }
                        }
                        else
                        {
                            foreach (IMyShipConnector conn in conns)
                            {
                                foreach (IMyThrust thrust in thrusters)
                                {
                                    thrust.Enabled = true;
                                    if (!conn.IsFunctional | thrust.WorldMatrix.Forward != RC.WorldMatrix.Forward || RC.GetShipSpeed() >= DampSpeed) thrust.ThrustOverridePercentage = 0;
                                    else thrust.ThrustOverridePercentage = (float)(1 - RC.GetShipSpeed() / DampSpeed);
                                }

                                conn.Enabled = true; conn.Connect(); if (conn.Status == MyShipConnectorStatus.Connected)
                                {
                                    foreach (IMyRadioAntenna antenna in antennas)
                                    {
                                        antenna.Enabled = false;
                                    }
                                    Mode = 0; break;
                                }
                            }
                        }
                    }
                }

            }
            //    
            sb.Append(String56);
            sb.AppendLine();
        }
        else
        {
            word = String19 + DClass + "#" + (q + 1) + String57 + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
            if (NotContainsMess(word.ToString())) { message.Append(word); message.AppendLine(); messages.Add(message.ToString()); message.Clear(); }
        }
        for (int i = myblocks.Count - 1; i > -1; i--)
        {
            turr = myblocks[i] as IMyLargeTurretBase;
            if (turr != null && !turr.GetTargetedEntity().IsEmpty())
            {
                word = String19 + DClass + "#" + (q + 1) + String58 + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                if (NotContainsMess(word.ToString())) { message.Append(word); message.AppendLine(); messages.Add(message.ToString()); message.Clear(); }
            }
        }
        RC.CustomData = Mode.ToString();
        conns.Clear();
        thrusters.Clear();
        gyros.Clear();
        cargos.Clear();
        drills.Clear();
        antennas.Clear();
        cameras.Clear();
        myblocks.Clear();
        items.Clear();
        items2.Clear();
        bat = null;
        react = null;
        thruster = null;
        gyro = null;
        antenna = null;
        cam = null;
        drill = null;
        cargo = null;
        tank = null;
        conn = null;
        waypoint = new MyWaypointInfo();
        item = new MyInventoryItem();
        vector = new Vector3D();
        cur += 1 + MineCount;
    }

    foreach (IMyTextPanel lcd in LCDs)
    {
        if (lcd.CustomName.Contains(" 2")) { lcd.ContentType = ContentType.TEXT_AND_IMAGE; lcd.WriteText(sb.ToString()); continue; }
        if (lcd.CustomName.Contains(" 3"))
        {
            message.Append(String59);
            message.AppendLine();
            for (int i = 0; i < oresgps.Count; i++)
            {
                message.Append(String60 + "''" + names[i] + "''");
                if (oresgps[i].IsEmpty()) message.Append(String61);
                else
                {
                    if (assign.Contains(i)) message.Append(String62);
                    else message.Append(String63);
                }
                message.AppendLine();
                message.Append(String64);
                message.AppendLine();
            }
            lcd.ContentType = ContentType.TEXT_AND_IMAGE;
            lcd.WriteText(message.ToString());
            message.Clear();
            continue;
        }
        if (lcd.CustomName.Contains(" 5"))
        {
            lcd.ContentType = ContentType.TEXT_AND_IMAGE;
            lcd.WriteText(diagnos.ToString());
            continue;
        }
        if (lcd.CustomName.Contains(" 6"))
        {
            for (int i = 0; i < RCS.Count; i++)
            {
                message.Append("GPS:" + DClass + "#" + (i + 1) + ":" + (RCS[i].GetPosition().X).ToString("0.0") + ":" + (RCS[i].GetPosition().Y).ToString("0.0") + ":" + (RCS[i].GetPosition().Z).ToString("0.0") + ":");
                message.AppendLine();
                if (RCS[i].GetPosition() != new Vector3D() && RCS[i].GetPosition() != null) lastcoords[i] = message.ToString();
                message.Clear();
            }
            message.Append(String65);
            message.AppendLine();
            lcd.ContentType = ContentType.TEXT_AND_IMAGE;
            for (int i = 0; i < lastcoords.Count; i++) { message.Append(lastcoords[i]); message.AppendLine(); }
            lcd.WriteText(message.ToString());
            message.Clear();
            continue;
        }
        if (lcd.CustomName.Contains(" 4"))
        {
            for (int i = messages.Count - 1; i > -1; i--)
            {
                message.Append(messages[i]);
            }
            lcd.ContentType = ContentType.TEXT_AND_IMAGE;
            lcd.WriteText(message.ToString());
            message.Clear();
        }
    }
    sb.Clear();
    foreach (IMyTextPanel lcd in LCDs)
    {
        if (lcd.CustomName.Contains(" 1")) lcd.ReadText(sb);
        else continue;
        if (sb != null)
        {
            str = sb.ToString().Split('@');
            for (int i = 0; i < str.Length; i += 2)
            {
                if (str[i].Length > 0)
                {
                    MyWaypointInfo.TryParse(str[i], out waypoint);
                    if (waypoint.IsEmpty() || str.Length < 1) { continue; }
                    oresgps.Add(waypoint);
                    MyWaypointInfo.TryParse(str[i + 1], out waypoint);
                    if (waypoint.IsEmpty())
                    {
                        oresgps.RemoveAt(oresgps.Count - 1);
                        word = String66 + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                        if (NotContainsMess(word.ToString())) { message.Append(word); message.AppendLine(); messages.Add(message.ToString()); message.Clear(); }
                        continue;
                    }
                    oreheights.Add(waypoint);
                    lcd.CustomData = lcd.CustomData + "\n" + oresgps[oresgps.Count - 1] + "@" + oreheights[oreheights.Count - 1];
                    names.Add(oresgps[oresgps.Count - 1].Name);
                    sb.Clear();
                }
            }
        }
        lcd.WriteText("----[Manual Coordinates Input]---- \nReplace this message with: \n'GPS:Ore_1:X.X:Y.Y:Z.Z:'+'@'+'GPS:Rendezvous_1:X.X:Y.Y:Z.Z:'+'@' \n \nRemove the GPS color code before use on this script (#ffffff) \n Rendezvous Point needs to be at least 5 meters from the Voxel or any Base \nYou can add multiple coordinates, example: \n\nGPS:Vein_1:XXX.X:YYY.Y:ZZZ.Z:@GPS:rendezvous_1:XXX.X:YYY.Y:ZZZ.Z:@ \nGPS:Vein_2:XXX.X:YYY.Y:ZZZ.Z:@GPS:rendezvous_2:XXX.X:YYY.Y:ZZZ.Z:@ \n\nThe coordinates will be stored in memory and removed from the display.\nA copy of the coordinates will be saved in the display's CustomData.");
    }

    sb.Clear();
    diagnos.Clear();
    switch (args)
    {
        case "CLEAR":
            for (int q = 0; q < RCS.Count; q++)
            {
                IMyRemoteControl RC = (IMyRemoteControl)RCS[q];
                for (int i1 = 0; i1 < oresgps.Count; i1++) oresgps[i1] = new MyWaypointInfo();
                for (int i1 = 0; i1 < oreheights.Count; i1++) oreheights[i1] = new MyWaypointInfo();
                assign[q] = -1;
                miningnumber[q] = 0;
                int.TryParse(RC.CustomData, out Mode);
                if (Mode == 4) Mode = 4;
                if (Mode == 1 || Mode == 2 || Mode == 3)
                {
                    RC.ClearWaypoints();
                    for (int i = 0; i < baseconns.Count; i++)
                    {
                        conn = baseconns[i] as IMyShipConnector;
                        if (conn != null && (conn.Status != MyShipConnectorStatus.Unconnected || !conn.IsFunctional)) { continue; }
                        if (!usedconns.Contains(i)) usedconns[q] = i;
                    }
                    if (usedconns[q] != -1)
                    {
                        Mode = 6;
                    }
                    else
                    {
                        word = String19 + DClass + "#" + (q + 1) + String51 + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                        if (NotContainsMess(word.ToString())) { message.Append(word); message.AppendLine(); messages.Add(message.ToString()); message.Clear(); }
                    }
                }
                RC.CustomData = Mode.ToString();
            }
    ; break;
        case "RUNALL":
            if (baseconns.Count > RCS.Count - 1)
                for (int q = 0; q < RCS.Count; q++)
                {
                    IMyRemoteControl RC = (IMyRemoteControl)RCS[q];
                    int.TryParse(RC.CustomData, out Mode);
                    if (Mode == 4) Mode = 4;
                    if (Mode == 1 || Mode == 2 || Mode == 3)
                    {
                        RC.ClearWaypoints();
                        for (int i = 0; i < baseconns.Count; i++)
                        {
                            conn = baseconns[i] as IMyShipConnector;
                            if (conn != null && (conn.Status != MyShipConnectorStatus.Unconnected || !conn.IsFunctional)) { continue; }
                            if (!usedconns.Contains(i)) usedconns[q] = i;
                        }
                        if (usedconns[q] != -1)
                        {
                            Mode = 6;
                        }
                        else
                        {
                            word = String19 + DClass + "#" + (q + 1) + String51 + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                            if (NotContainsMess(word.ToString())) { message.Append(word); message.AppendLine(); messages.Add(message.ToString()); message.Clear(); }
                        }
                    }
                    RC.CustomData = Mode.ToString();
                }
            else
            {
                word = String67 + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                if (NotContainsMess(word.ToString())) { message.Append(word); message.AppendLine(); messages.Add(message.ToString()); message.Clear(); }
            }
    ; break;
        case "MESSAGESCLEAR": messages.Clear(); break;
        case "SCAN":
            if (targetcam != null)
            {
                target = targetcam.Raycast(ScanRange, 0, 0);
                if (target.EntityId != 0)
                {
                    vector = (Vector3D)target.HitPosition;
                    if (target.Type == MyDetectedEntityType.Asteroid || target.Type == MyDetectedEntityType.Planet)
                    {
                        word = "GPS:" + String68 + (oresgps.Count + 1) + ":" + vector.X + ":" + vector.Y + ":" + vector.Z + ":";
                        MyWaypointInfo.TryParse(word, out waypoint);
                        oresgps.Add(waypoint);
                        vector -= Vector3D.Normalize(vector - targetcam.GetPosition()) * MineHeight;
                        word = "GPS:" + String68 + (oresgps.Count + 1) + ":" + vector.X + ":" + vector.Y + ":" + vector.Z + ":";
                        MyWaypointInfo.TryParse(word, out waypoint);
                        oreheights.Add(waypoint);
                        names.Add(String68 + oresgps.Count);
                    }
                }
            }
    ; break;
    }
    if (args.Contains("GOODBYE:"))
    {
        word = args.Remove(0, 8);
        int.TryParse(word, out cur);
        if (cur > 0)
        {
            word = String69 + DClass + "#" + (cur) + String70 + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
            if (NotContainsMess(word.ToString())) { message.Append(word); message.AppendLine(); messages.Add(message.ToString()); message.Clear(); }
            cur--;
            IMyRemoteControl RC = (IMyRemoteControl)RCS[cur];
            RC.CustomData = "0";
            RC.ClearWaypoints();
            miningnumber.RemoveAt(cur);
            RCS.RemoveAt(cur);
            usedconns.RemoveAt(cur);
            assign.RemoveAt(cur);
            lastcoords.RemoveAt(cur);
            if (cur == 0)
                for (int i1 = MineCount; i1 > -1; i1--) { newcoords.RemoveAt(i1); newheights.RemoveAt(i1); }
            else
                for (int i1 = (cur + 1) * MineCount + 1; i1 > cur * MineCount; i1--) { newcoords.RemoveAt(i1); newheights.RemoveAt(i1); }
        }
    }
    if (args.Contains("CLEAR:"))
    {
        word = args.Remove(0, 6);
        for (int i = names.Count - 1; i > -1; i--)
        {
            if (names[i] == word)
            {
                names.RemoveAt(i);
                oresgps[i] = new MyWaypointInfo();
                oreheights[i] = new MyWaypointInfo();
                for (int i1 = assign.Count - 1; i1 > -1; i1--)
                {
                    if (assign[i1] == i)
                    {
                        IMyRemoteControl RC = (IMyRemoteControl)RCS[i1];
                        miningnumber[i1] = 0;
                        int.TryParse(RC.CustomData, out Mode);
                        if (Mode == 4) Mode = 5;
                        if (Mode == 1 || Mode == 2 || Mode == 3)
                        {
                            RC.ClearWaypoints();
                            for (int i2 = 0; i2 < baseconns.Count; i2++)
                            {
                                conn = baseconns[i2] as IMyShipConnector;
                                if (conn != null && (conn.Status != MyShipConnectorStatus.Unconnected || !conn.IsFunctional)) { continue; }
                                if (!usedconns.Contains(i2)) usedconns[i1] = i2;
                            }
                            if (usedconns[i1] != -1)
                            {
                                Mode = 6;
                            }
                            else
                            {
                                word = String19 + DClass + "#" + (i1) + String51 + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                                if (NotContainsMess(word.ToString())) { message.Append(word); message.AppendLine(); messages.Add(message.ToString()); message.Clear(); }
                            }
                        }
                        RC.CustomData = Mode.ToString();
                        assign[i1] = -1;
                    }
                }
            }
        }
    }
    if (args.Contains("RUN:"))
    {
        word = args.Remove(0, 4);
        int.TryParse(word, out cur);
        if (cur > 0 && cur < RCS.Count + 1)
        {
            IMyRemoteControl RC = (IMyRemoteControl)RCS[cur - 1];
            int.TryParse(RC.CustomData, out Mode);
            miningnumber[cur - 1] = 0;
            if (Mode == 4) Mode = 5;
            if (Mode == 1 || Mode == 2 || Mode == 3)
            {
                RC.ClearWaypoints();
                for (int i = 0; i < baseconns.Count; i++)
                {
                    conn = baseconns[i] as IMyShipConnector;
                    if (conn != null && (conn.Status != MyShipConnectorStatus.Unconnected || !conn.IsFunctional)) { continue; }
                    if (!usedconns.Contains(i)) usedconns[cur - 1] = i;
                }
                if (usedconns[cur - 1] != -1)
                {
                    Mode = 6;
                }
                else
                {
                    word = String19 + DClass + "#" + (cur) + String51 + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                    if (NotContainsMess(word.ToString())) { message.Append(word); message.AppendLine(); messages.Add(message.ToString()); message.Clear(); }
                }
            }
            RC.CustomData = Mode.ToString();
        }
    }
    if (Cfrq == 0) Cfrq = CargoFreq;
}
void Clear()
{
    conns.Clear();
    thrusters.Clear();
    gyros.Clear();
    antennas.Clear();
    cargos.Clear();
    drills.Clear();
    cameras.Clear();
    myblocks.Clear();
    items.Clear();
    items2.Clear();
    bat = null;
    react = null;
    thruster = null;
    gyro = null;
    cam = null;
    drill = null;
    cargo = null;
    tank = null;
    antenna = null;
    conn = null;
    waypoint = new MyWaypointInfo();
    item = new MyInventoryItem();
    vector = new Vector3D();
    cur += 1 + MineCount;
}