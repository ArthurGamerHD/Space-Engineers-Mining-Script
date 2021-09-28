
#region prog_space

/* Exemple of Preset User Paremeter to Write in This block CustomData Start the copy at next line 0.0 ______________________________

DynamicScanGrid[Yes] -> Set to No and the grid will be scanned only during first script running
                            -> script can crash if blocks removed and recompile needed if any tagging modified

ForceTagForOre[!!WriteWhatUWant!!] -> if nothing Tag is "Ore"
ForceTagForIngot[]                                               "Ingot"
ForceTagForCompo[]                                      "Component"
ForceTagForTool[]                                                "Tool"
ForceTagForToProduce[]
ForceTagForConnectedShip[]
ForceTagForCargoCapacity[]
ForceTagForAlive[]

AssemblerEfficiency[] -> in your world settings 1, 3 or 10

AddToGridBlockName[]     -> Will Add It at beginning of any Block Name in Grid
RemoveFromGridBlockName[]  -> Will Erase It if beginning of any Block Name in Grid 

HideLightGyroThruster***s    -> remove *** to active
CheckInvOnExtGri***d            -> same (will greatly increase complexity if many ships docked)

ManufacturingPeriod[13000];  
RefineryPeriod[11000];  
SortPeriod[7000];  
PrintPeriod[600];      >>>>>>>>>>>>>>>>> continue to copy until big line under this 0.0

Ore{  
Iron[8500] Stone[8500] Ice[15000] Nickel[4300]                  -> Change amount as your wish
Silver[12000] Silicon[8500] Cobalt[15000] Gold[4300]  
Magnesium[8500] Platinum[8500] Uranium[18000]    
}  
Ingot{  
Iron[10000] Stone[10000] Nickel[10000]    
Silver[4000] Silicon[10000] Cobalt[1000] Gold[4000]   
Magnesium[2000] Platinum[1000] Uranium[1500]    
}  
Component{   
SteelPlate[20000] InteriorPlate[10000] Construction[5000]     
Girder[5000] SmallTube[5000] LargeTube[3000]   
Display[2000] Motor[3000] BulletproofGlass[2000]     
Computer[3000] Reactor[2500] Thrust[3000]     
RadioCommunication[50] MetalGrid[800]  
SolarCell[800] PowerCell[500] GravityGenerator[10]  
Medical[200] Detector[500] Explosives[300]  
Superconductor[3000]  
}  
Tool{   
OxygenBottle[20] HydrogenBottle[20]      
NATO_25x184mm[5] NATO_5p56x45mm[5] Missile200mm[5]   
AngleGrinderItem[1] AngleGrinder2Item[1] AngleGrinder3Item[1]   
AngleGrinder4Item[1] WelderItem[1] Welder2Item[1] Welder3Item[1]   
Welder4Item[1] HandDrillItem[1] HandDrill2Item[1] HandDrill3Item[1]    
HandDrill4Item[1] AutomaticRifleItem[1] PreciseAutomaticRifleItem[1]  
RapidFireAutomaticRifleItem[1] UltimateAutomaticRifleItem[1]  
}

End of copy area _____________________________________________________________________________________________0.0*/



// Block List >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  

List<IMyBlockGroup> _AllGroups = new List<IMyBlockGroup>();

List<IMyTerminalBlock> _TypeCargo = new List<IMyTerminalBlock>();
List<IMyTerminalBlock> _TypeLCD = new List<IMyTerminalBlock>();
List<IMyShipConnector> _TypeConnector = new List<IMyShipConnector>();
List<IMyTerminalBlock> _TypeRefinery = new List<IMyTerminalBlock>();
List<IMyAssembler> _TypeAssembler = new List<IMyAssembler>();

List<IMyMechanicalConnectionBlock> _MechConn = new List<IMyMechanicalConnectionBlock>();

List<IMyTerminalBlock> _SortGrp = new List<IMyTerminalBlock>();
List<IMyTerminalBlock> _WithInv = new List<IMyTerminalBlock>();
List<IMyTerminalBlock> _RcvGrp = new List<IMyTerminalBlock>();

List<MyInventoryItem> _Items = new List<MyInventoryItem>();
List<MyInventoryItem> _Items2 = new List<MyInventoryItem>();
List<MyProductionItem> _AnyProdList = new List<MyProductionItem>();

MyDefinitionId ItemRecipe = new MyDefinitionId();

//Define User Tag Class >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

public class c_Tag
{
    public string NativeTag;
    public string UserTag;
    public StringBuilder LCDPrint = new StringBuilder(5000);

    public c_Tag(string UserT, string NativeT)
    {
        this.UserTag = UserT; this.NativeTag = NativeT;
        this.LCDPrint.Append(UserTag + ": Wait 1st Scan \n");
    }
    public void Refresh(StringBuilder str) { this.LCDPrint.Clear(); this.LCDPrint.Append(str); }
}

Dictionary<string, c_Tag> _MainInvTag = new Dictionary<string, c_Tag>();
Dictionary<string, c_Tag> _LCDTag = new Dictionary<string, c_Tag>();
List<c_Tag> _LCDTagFormattedList = new List<c_Tag>();
c_Tag Tag;


//Define Printed LCD Class >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

public class c_PrintedLCD
{
    private int ScrollIndex = 0;
    public StringBuilder Content = new StringBuilder(5000);
    private StringBuilder AffContent = new StringBuilder(5000);
    private StringBuilder TmpStrBld = new StringBuilder(500);
    public IMyTextPanel LCD;

    public c_PrintedLCD(IMyTextPanel xLCD) // Constructor: 
    {
        this.LCD = xLCD;
        LCD.FontSize = 0.9f;
        LCD.Font = "Monospace";
        LCD.ContentType = VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;
    }

    public void Init() { this.Content.Clear(); }

    public void Refresh(StringBuilder NewContent) { this.Init(); this.Content.Append(NewContent); }

    public void Print()
    {
        if (ScrollIndex > Content.Length || Content.Length <= MAXLCDCHAR)
        {
            ScrollIndex = 0;
            LCD.WriteText(this.Content, false);
        }
        else
        {
            AffContent.Clear(); TmpStrBld.Clear();
            AffContent.Append(Content);

            //1st way -> handle with a third StringBuilder and use "AppendSubstring" function
            //TmpStrBld.AppendSubstring(Content, ScrollIndex, AffContent.Length - ScrollIndex);
            //LCD.WriteText(AffContent.Insert(0, TmpStrBld));

            //2nd way -> cast a string directly
            string x = Content.ToString(ScrollIndex, AffContent.Length - ScrollIndex);
            LCD.WriteText(AffContent.Insert(0, x));
        }
        ScrollIndex += MAXLINECHAR;
    }
}

List<c_PrintedLCD> _PrintedLCD = new List<c_PrintedLCD>();
List<c_PrintedLCD> _TempLCD = new List<c_PrintedLCD>();


//Define Receiver Block Class >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

public class c_RcvBlock
{
    public IMyTerminalBlock Block;
    public string RcvItemType;
    public string RcvItemName;
    public int Target;
    public Nullable<int> Needed = null;
    private MyInventoryItem Item;
    private List<MyInventoryItem> _Items = new List<MyInventoryItem>();

    public c_RcvBlock(IMyTerminalBlock xBlock, string Name = "??", string Type = "??", int xTarget = 0)
    { this.Block = xBlock; this.RcvItemName = Name; this.RcvItemType = Type; Target = xTarget; }      // Constructor: 

    public bool MatchWithItem(string Name, string Type = "??")
    {
        if (Name != RcvItemName) return false;
        if (Type != RcvItemType && Type != "??") return false;
        if (Target == 0) { Needed = null; return true; }
        this._Items.Clear();
        this.Block.GetInventory(0).GetItems(this._Items);
        this.Item = this._Items.Find(x => x.Type.SubtypeId == this.RcvItemName);
        if (this.Item == null) { Needed = Target; return true; }
        Needed = Target - (int)this.Item.Amount;
        if (Needed <= 0) return false;
        return true;
    }
}

Dictionary<string, List<c_RcvBlock>> __AllByItemRcvBlock = new Dictionary<string, List<c_RcvBlock>>();
Dictionary<string, List<c_RcvBlock>> __AllByTypeRcvBlock = new Dictionary<string, List<c_RcvBlock>>();
List<c_RcvBlock> _SomeRcvBlock = new List<c_RcvBlock>();
c_RcvBlock Receiver;

//Define Item DataBase Class >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

public class c_Item
{
    public string Name;
    public StringBuilder NameStrB = new StringBuilder(20);
    public long Amount, Target, Preset;

    public c_Item(string name, long amount = 0, long preset = 0)
    {       // Constructor:  
        this.Name = name; this.Amount = amount; this.Preset = preset; this.NameStrB.Append(name);
    }

    public void InitScanGrid() { Amount = 0; Target = Preset; }
    public long Ratio { get { return (((Amount) * 100) / Target); } }
    public void AddAmount(long amount) { Amount += amount; }
    public void AddTarget(double target) { Target += (long)target; }
    public bool WithTarget { get { return (Target > 0); } }
    public bool NeedProd { get { return (Amount < Target); } }
    public long AmountNeeded { get { if (this.NeedProd) return (Target - Amount); else return 0; } }
    public void WritePreset(string PresetStr) { long.TryParse(PresetStr, out Preset); }
}


//List Dedicated to Item Database Constructor >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

List<c_Item> _cOreItem = new List<c_Item>(){
new c_Item("Stone"), new c_Item("Iron"), new c_Item("Nickel"),
new c_Item("Silicon"), new c_Item("Cobalt"), new c_Item("Magnesium"), new c_Item("Silver"),
new c_Item("Gold"), new c_Item("Platinum"), new c_Item("Uranium"), new c_Item("Ice")};

List<c_Item> _cIngotItem = new List<c_Item>(){
new c_Item("Stone"), new c_Item("Iron"), new c_Item("Nickel"),
new c_Item("Silicon"), new c_Item("Cobalt"), new c_Item("Magnesium"), new c_Item("Silver"),
new c_Item("Gold"), new c_Item("Platinum"), new c_Item("Uranium", 0, 10)};

enum eIngot { Stone, Iron, Nickel, Silicon, Cobalt, Magnesium, Silver, Gold, Platinum, Uranium };

List<c_Item> _cCompoItem = new List<c_Item>(){
new c_Item("SteelPlate", 0, 1000), new c_Item("InteriorPlate", 0, 500), new c_Item("Construction", 0, 500),
new c_Item("Girder", 0, 500), new c_Item("SmallTube", 0, 500), new c_Item("LargeTube", 0, 200), new c_Item("MetalGrid", 0, 200),
new c_Item("Motor", 0, 200), new c_Item("BulletproofGlass"), new c_Item("Display"), new c_Item("Computer", 0, 500),
new c_Item("RadioCommunication"), new c_Item("Reactor"), new c_Item("Thrust"), new c_Item("Medical"),
new c_Item("Detector"), new c_Item("SolarCell"), new c_Item("PowerCell"), new c_Item("Superconductor"),
new c_Item("Explosives"), new c_Item("GravityGenerator"), new c_Item("Canvas")};

enum eCompo
{
    SteelP, IntP, ConstructC, Girder, STube, LTube, MGrid, Motor, BPGlass, Display, CPU, RadioC, Reactor,
    Thrust, Medic, Detector, SCell, PCell, SupCond, Explo, GravGen, Canvas
};

List<c_Item> _cToolItem = new List<c_Item>(){
new c_Item("OxygenBottle"), new c_Item("HydrogenBottle"),
new c_Item("NATO_25x184mm"), new c_Item("NATO_5p56x45mm"), new c_Item("Missile200mm"),
new c_Item("WelderItem"), new c_Item("Welder2Item"), new c_Item("Welder3Item"), new c_Item("Welder4Item"),
new c_Item("HandDrillItem"), new c_Item("HandDrill2Item"), new c_Item("HandDrill3Item"), new c_Item("HandDrill4Item"),
new c_Item("AngleGrinderItem"), new c_Item("AngleGrinder2Item"), new c_Item("AngleGrinder3Item"), new c_Item("AngleGrinder4Item"),
new c_Item("AutomaticRifleItem"), new c_Item("PreciseAutomaticRifleItem"),
new c_Item("RapidFireAutomaticRifleItem"), new c_Item("UltimateAutomaticRifleItem") };

enum eTool
{
    OxyB, HydroB, Nato184, Nato45, Missile, Welder1, Welder2, Welder3, Welder4, Drill1, Drill2, Drill3, Drill4,
    Grind1, Grind2, Grind3, Grind4, Rifle1, Rifle2, Rifle3, Rifle4
};

Dictionary<string, List<c_Item>> __AllItems = new Dictionary<string, List<c_Item>>();
Dictionary<string, List<c_Item>> __CriticalItems = new Dictionary<string, List<c_Item>>();
List<c_Item> _SomeItems = new List<c_Item>();
List<c_Item> _Ingot = new List<c_Item>();
List<c_Item> _Compo = new List<c_Item>();
List<c_Item> _Tool = new List<c_Item>();

//Any Other Usefull Data >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

Nullable<int> TrAmount = null;
int k, x, indice;
int TmpInt1, TmpInt2;
int AssEfficiency = 1;
const int MAXLINECHAR = 29; const int MAXLCDCHAR = 29 * 19;
String MotherGrid, gStr, TmpStr, ItemName, FunctionExecuted, AliveTag;
StringBuilder gStrB = new StringBuilder(5000);
StringBuilder TmpStrB = new StringBuilder(5000);
const string BLUEPRINT = "MyObjectBuilder_BlueprintDefinition/";
const string MANGEBURNES = "Component";
bool CheckInvOnExt, ColdRestart, DynamicGridScan;
bool DisableSorting, DisableManufacturing, DisablePrinting, DisableRefinery;
bool StopThinking, LastWasInvScanning, StopScript;

static char RedChr = CreateCustomColor(7, 0, 0);
static char GreenChr = CreateCustomColor(0, 7, 0);
static char YellowChr = CreateCustomColor(7, 7, 0);
static char BlueChr = CreateCustomColor(0, 0, 7);

TimeSpan MaxT, CurrentT, SortingT, PrintingT, ManufacturingT, RefineryT, ScanningT;
int MaxInstruct; string LowPerfFunction;
TimeSpan SortingPeriod = new TimeSpan(0, 0, 11);
TimeSpan PrintingPeriod = new TimeSpan(0, 0, 2);
TimeSpan ManufacturingPeriod = new TimeSpan(0, 0, 17);
TimeSpan RefineryPeriod = new TimeSpan(0, 0, 14);
TimeSpan ScanningPeriod = new TimeSpan(0, 0, 3);


public Program()
{  // Call at prog init only  

    ColdRestart = true; StopScript = false;
    MotherGrid = Me.CubeGrid.CustomName;
    Runtime.UpdateFrequency = UpdateFrequency.Update10;

    // Construct Main Tag List
    _MainInvTag.Clear();
    gStr = UserDataEntry(Me.CustomData, "ForceTagForOre", "[", "]"); if (gStr == null) gStr = "Ore";
    _MainInvTag.Add("Ore", new c_Tag(gStr, "Ore"));
    gStr = UserDataEntry(Me.CustomData, "ForceTagForIngot", "[", "]"); if (gStr == null) gStr = "Ingot";
    _MainInvTag.Add("Ingot", new c_Tag(gStr, "Ingot"));
    gStr = UserDataEntry(Me.CustomData, "ForceTagForCompo", "[", "]"); if (gStr == null) gStr = "Component";
    _MainInvTag.Add("Component", new c_Tag(gStr, "Component"));
    gStr = UserDataEntry(Me.CustomData, "ForceTagForTool", "[", "]"); if (gStr == null) gStr = "Tool";
    _MainInvTag.Add("Tool", new c_Tag(gStr, "Tool"));

    _LCDTag = _MainInvTag;
    gStr = UserDataEntry(Me.CustomData, "ForceTagForToProduce", "[", "]"); if (gStr == null) gStr = "ToProduce";
    _LCDTag.Add("ToProduce", new c_Tag(gStr, "ToProduce"));
    gStr = UserDataEntry(Me.CustomData, "ForceTagForConnectedShip", "[", "]"); if (gStr == null) gStr = "ConnectedShip";
    _LCDTag.Add("ConnectedShip", new c_Tag(gStr, "ConnectedShip"));
    gStr = UserDataEntry(Me.CustomData, "ForceTagForCargoCapacity", "[", "]"); if (gStr == null) gStr = "CargoCapacity";
    _LCDTag.Add("CargoCapacity", new c_Tag(gStr, "CargoCapacity"));
    AliveTag = UserDataEntry(Me.CustomData, "ForceTagForAlive", "[", "]"); if (AliveTag == null) AliveTag = "Alive";

    _LCDTagFormattedList.Clear();
    _LCDTagFormattedList = _LCDTag.Values.ToList();

    // Construct Skeleton of Inventory List² >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  

    __AllItems.Clear();
    __AllItems.Add("Ore", new List<c_Item>()); __AllItems["Ore"].AddRange(_cOreItem);
    __AllItems.Add("Ingot", new List<c_Item>()); __AllItems["Ingot"].AddRange(_cIngotItem);
    __AllItems.Add("Component", new List<c_Item>()); __AllItems["Component"].AddRange(_cCompoItem);
    __AllItems.Add("Tool", new List<c_Item>()); __AllItems["Tool"].AddRange(_cToolItem);


    //Get processing user choice >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 

    CheckInvOnExt = Me.CustomData.Contains("CheckInvOnExtGrid");

    DisableSorting = SetPeriod(ref SortingPeriod, "SortPeriod", 5000);
    DisablePrinting = SetPeriod(ref PrintingPeriod, "PrintPeriod", 400);
    DisableManufacturing = SetPeriod(ref ManufacturingPeriod, "ManufacturingPeriod", 9000);
    DisableRefinery = SetPeriod(ref RefineryPeriod, "RefineryPeriod", 7000);

    //Write User Target Amount Choice in Database  >>>>>>>>>>>>>>>>>>>>>>>>>>> 
    foreach (var _AnyInv in __AllItems)
    {
        gStr = UserDataEntry(Me.CustomData, _AnyInv.Key, "{", "}");
        if (gStr == null) continue;

        for (x = 0; x < _AnyInv.Value.Count; x++)
            _AnyInv.Value[x].WritePreset(UserDataEntry(gStr, _AnyInv.Value[x].Name, "[", "]"));
    }

    int.TryParse(UserDataEntry(Me.CustomData, "AssemblerEfficiency", "[", "]"), out AssEfficiency);
    if (AssEfficiency <= 0) AssEfficiency = 1;

    if (UserDataEntry(Me.CustomData, "DynamicScanGrid", "[", "]") == "No") DynamicGridScan = false;
    else DynamicGridScan = true;

}


public void Main(string argument)
{
    if (StopScript) return;
    Refresh_Timers();

    if (ColdRestart)
        ColdRestart_Duties(argument);

    if ((SortingT > SortingPeriod || ManufacturingT > ManufacturingPeriod || RefineryT > RefineryPeriod
                                                                                    || ScanningT > ScanningPeriod) && !LastWasInvScanning)            // Cargo Scanning
        Main_Processing();

    if (PrintingT > PrintingPeriod && !StopThinking)            // LCDs Init & Printing
        Proceed_Printing();

    if (SortingT > SortingPeriod && !StopThinking)              // Main Sorting
        Proceed_Main_Sorting(argument);

    if (RefineryT > RefineryPeriod && !StopThinking)            // Dedicated Refinery Sorting
        Proceed_Refinery_Sorting();

    if (ManufacturingT > ManufacturingPeriod && !StopThinking)  // Automatize Compo Needed Production
        Proceed_Manufacturing();

    if (StopThinking) Perf_Diag_Publishing();
    else LastWasInvScanning = false;

}   //end of main §§§§§§§§§§§§



//Main process function >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

void ColdRestart_Duties(string argument)
{
    ColdRestart = false; FunctionExecuted = "Cold Starting"; StopThinking = true;      // Identify and Rename Subgrids  

    GridTerminalSystem.GetBlocksOfType<IMyMechanicalConnectionBlock>(_MechConn, x => SatisfyGridPolicy(x) && x.IsAttached);
    for (int x = 0; x < _MechConn.Count; x++)
        _MechConn[x].TopGrid.CustomName = MotherGrid + " Sub" + x.ToString();

    // AutoHide Some Blocks 
    List<IMyTerminalBlock> _Temp = new List<IMyTerminalBlock>(); List<IMyTerminalBlock> _Temp2 = new List<IMyTerminalBlock>();
    GridTerminalSystem.GetBlocksOfType<IMyLightingBlock>(_Temp, x => SatisfyGridPolicy(x) && x.ShowInTerminal);
    _Temp2.AddList(_Temp);
    GridTerminalSystem.GetBlocksOfType<IMyGyro>(_Temp, x => SatisfyGridPolicy(x) && x.ShowInTerminal);
    _Temp2.AddList(_Temp);
    GridTerminalSystem.GetBlocksOfType<IMyThrust>(_Temp, x => SatisfyGridPolicy(x) && x.ShowInTerminal);
    _Temp2.AddList(_Temp);
    if (Me.CustomData.Contains("HideLightGyroThrusters"))
        _Temp2.All(c => { c.ShowInTerminal = false; return true; });

    // Tag/Untag All Blocks of Main Grid  
    _Temp.Clear();
    gStr = UserDataEntry(Me.CustomData, "RemoveFromGridBlockName", "[", "]");
    if (gStr != null)
        GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(_Temp, x => SatisfyGridPolicy(x) && x.CustomName.StartsWith(gStr));
    _Temp.All(c => { c.CustomName = c.CustomName.Remove(0, gStr.Length); return true; });

    _Temp.Clear();
    gStr = UserDataEntry(Me.CustomData, "AddToGridBlockName", "[", "]");
    if (gStr != null)
        GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(_Temp, x => SatisfyGridPolicy(x) && !x.CustomName.StartsWith(gStr));
    _Temp.All(c => { c.CustomName = c.CustomName.Insert(0, gStr); return true; });

    if (!DynamicGridScan)
    {
        MainScanningGrid();
        LCDScanningGrid();
        ConstructSortingBlocksLists(argument);
    }
}

void Refresh_Timers()
{
    StopThinking = false;
    CurrentT = Runtime.TimeSinceLastRun; if (MaxT < CurrentT) MaxT = CurrentT;

    if (!DisableSorting) SortingT += CurrentT; if (!DisablePrinting) PrintingT += CurrentT;
    if (!DisableManufacturing) ManufacturingT += CurrentT; if (!DisableRefinery) RefineryT += CurrentT;
    ScanningT += CurrentT;
}


void Main_Processing()
{
    ScanningT = TimeSpan.Zero; LastWasInvScanning = true; StopThinking = true; FunctionExecuted = "Inv Scanning";

    if (DynamicGridScan)
        MainScanningGrid();

    // Prepare Inventory List for a new scan >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  

    __AllItems["Ore"].All(c => { c.InitScanGrid(); return true; }); __AllItems["Ingot"].All(c => { c.InitScanGrid(); return true; });
    __AllItems["Component"].All(c => { c.InitScanGrid(); return true; }); __AllItems["Tool"].All(c => { c.InitScanGrid(); return true; });

    __CriticalItems.Clear();
    __CriticalItems.Add("Ore", new List<c_Item>()); __CriticalItems.Add("Ingot", new List<c_Item>());
    __CriticalItems.Add("Component", new List<c_Item>()); __CriticalItems.Add("Tool", new List<c_Item>());


    gStrB.Clear();
    gStrB.Append("-".PadRight(28, '-') + "\n" + ("Containers Capacity :").PadRight(28) + "\n" + "-".PadRight(28, '-') + "\n");
    _WithInv.All(c => { ScanItemAndCargoFilling(c); return true; }); //Scan Items and Cargo filling of blocks 
    (_LCDTag["CargoCapacity"]).Refresh(gStrB); // Write Cargo filling LCD Formatted List

    UpdateDatabaseWithRecipeofNeededComponent();

    gStrB.Clear();
    __AllItems.All(c => { BuildCritList(c.Key, c.Value); return true; }); //Write Needed Item List
    (_LCDTag["ToProduce"]).Refresh(gStrB); //Write LCD Formatted Needed Item List

    gStrB.Clear();
    gStrB.Append("-".PadRight(28, '-') + "\n" + ("Connected Ships :").PadRight(28) + "\n" + "-".PadRight(28, '-') + "\n");
    _TypeConnector.All(c => { BuildConnectedShipList(c); return true; }); //Write LCD Formatted Connected Ship List
    (_LCDTag["ConnectedShip"]).Refresh(gStrB);

    __AllItems.All(c => { BuildItemList(c.Key, c.Value); return true; }); //Write Main Item Lists and related LCD formatted lists

    if (_TypeLCD.Count >= 1) LCD_ContentDispatching(); //Dispatch Content to Publish on all Tagged LCDs
}


void Proceed_Printing()
{

    PrintingT = TimeSpan.Zero; StopThinking = true; FunctionExecuted = "Print Processing"; LastWasInvScanning = false;

    if (DynamicGridScan)
        LCDScanningGrid();

    if (_TypeLCD.Count == _PrintedLCD.Count)
        _PrintedLCD.All(c => { c.Print(); return true; });
}


void Proceed_Main_Sorting(string argument)
{
    SortingT = TimeSpan.Zero; StopThinking = true; FunctionExecuted = "Sort Processing"; LastWasInvScanning = false;

    if (DynamicGridScan)
        ConstructSortingBlocksLists(argument);

    foreach (IMyTerminalBlock Sender in _SortGrp) //Sort Items -> Dispatching Items from Global Sender List to the 2 Skeletons   
    {
        indice = Sender.InventoryCount - 1;  //point to the 2nd inventory for sorting factories

        _Items.Clear();
        Sender.GetInventory(indice).GetItems(_Items);
        k = 0;
        for (x = 0; x < _Items.Count; x++)
            SortProcessing(Sender);
    }
}


void Proceed_Refinery_Sorting()
{
    RefineryT = TimeSpan.Zero; StopThinking = true; FunctionExecuted = "Force Refinery Processing"; LastWasInvScanning = false;

    _SomeItems.Clear();
    _SomeItems.AddRange(__CriticalItems["Ingot"]);

    _TypeRefinery.RemoveAll(x => (!IsIdentifiedAs(x, AliveTag)));

    _SomeRcvBlock.Clear();
    if (__AllByTypeRcvBlock.ContainsKey("Ore")) _SomeRcvBlock.AddRange(__AllByTypeRcvBlock["Ore"]);
    if (__AllByItemRcvBlock.ContainsKey("Ore")) _SomeRcvBlock.AddRange(__AllByItemRcvBlock["Ore"]);

    bool GoToNextRef;
    foreach (IMyRefinery AnyRef in _TypeRefinery)
    {
        GoToNextRef = false;
        _Items.Clear();
        AnyRef.GetInventory(0).GetItems(_Items); //Check Actual Refinery Inventory
        if (_Items.Count > 0)
        {
            if (_SomeItems.Exists(c => c.Name == _Items[0].Type.SubtypeId)) continue;
            for (x = 1; x < _Items.Count; x++)
            {
                if (!_SomeItems.Exists(c => c.Name == _Items[x].Type.SubtypeId)) continue;
                (AnyRef).GetInventory(0).TransferItemTo((AnyRef).GetInventory(0), x, 0, true, null);
                GoToNextRef = true; break;
            }
        }
        if (GoToNextRef) continue;
        if (_SomeRcvBlock.Count != 0)
        {
            foreach (c_RcvBlock NowSender in _SomeRcvBlock) //Ore Rcv Block will now become potential sender for refinery
            {
                _Items.Clear();
                NowSender.Block.GetInventory(0).GetItems(_Items);
                for (x = 0; x < _Items.Count; x++)
                {
                    if (!_Items[x].Type.TypeId.EndsWith("Ore")) continue;
                    if (!_SomeItems.Exists(c => c.Name == _Items[x].Type.SubtypeId)) continue;
                    NowSender.Block.GetInventory(0).TransferItemTo((AnyRef).GetInventory(0), x, 0, true, null);
                    GoToNextRef = true; break;
                }
            }
        }
        if (GoToNextRef) continue;
        foreach (IMyRefinery NowSender in _TypeRefinery) //Other refinery could now share 1st inv slot 
        {
            if (NowSender.CustomName == AnyRef.CustomName) continue;
            _Items.Clear();
            NowSender.GetInventory(0).GetItems(_Items);
            if (_Items.Count > 0)
            {
                if (!_SomeItems.Exists(c => c.Name == _Items[0].Type.SubtypeId)) continue;
                VRage.MyFixedPoint Tr_Amount = _Items[0].Amount * 0.5f;
                NowSender.GetInventory(0).TransferItemTo((AnyRef).GetInventory(0), 0, 0, true, Tr_Amount);
                break;
            }
        }
    }
}


void Proceed_Manufacturing()
{
    ManufacturingT = TimeSpan.Zero; StopThinking = true; FunctionExecuted = "Prod Processing"; LastWasInvScanning = false;

    _SomeItems.Clear();
    _SomeItems.AddRange(__CriticalItems["Tool"]);
    _SomeItems.InsertRange(0, __CriticalItems["Component"]);

    IMyAssembler Producer = null;
    List<MyProductionItem> _ProducerProd = new List<MyProductionItem>();

    _TypeAssembler.RemoveAll(x => (!IsIdentifiedAs(x, AliveTag)));

    foreach (IMyAssembler AnyAss in _TypeAssembler) //Remove All Item Needed from the list if already in production
    {
        if (_SomeItems.Count == 0) break;
        AnyAss.CooperativeMode = false; AnyAss.Repeating = false;
        if (AnyAss.IsQueueEmpty) continue;

        AnyAss.GetQueue(_AnyProdList);
        TmpStr = _AnyProdList[0].BlueprintId.ToString();

        if (Producer != null)
            if (TmpStr == _ProducerProd[0].BlueprintId.ToString()) Producer = null; // Forget producer with fragmented production

        if (!AnyAss.IsProducing) AnyAss.ClearQueue(); //Free assembler from incomplete recipe
        else if (Producer == null && _SomeItems.Exists(x => (TmpStr.Contains(x.Name.Replace("Item", "")))))
        {
            Producer = AnyAss; //Remember a producer
            _ProducerProd = _AnyProdList;
        }
        _SomeItems.RemoveAll(x => (TmpStr.Contains(x.Name.Replace("Item", ""))));
    }

    _TypeAssembler.RemoveAll(x => (!x.IsQueueEmpty));
    if (_TypeAssembler.Count < 1) return;

    foreach (IMyAssembler AnyAss in _TypeAssembler) //Match Assembler with Needed Items
    {
        if (_SomeItems.Count <= 1) break;
        SetQueue(AnyAss, BluePrintBuild(_SomeItems[0].Name), _SomeItems[0].AmountNeeded);
        _SomeItems.RemoveAt(0);
    }

    _TypeAssembler.RemoveAll(x => (!x.IsQueueEmpty));
    if (_TypeAssembler.Count < 1) return; // All assemblers working = exit
    if (_SomeItems.Count < 1)
    {
        if (Producer != null) Producer.ClearQueue(); //Kill a current production to permit a share next cycle
        return;     // No Items to produce = exit
    }

    TmpStr = BluePrintBuild(_SomeItems[0].Name);
    double ProdAmount = _SomeItems[0].AmountNeeded / _TypeAssembler.Count;
    if (ProdAmount < 1)
        SetQueue(_TypeAssembler[0], TmpStr, (double)_SomeItems[0].AmountNeeded);
    else
        _TypeAssembler.All(c => { SetQueue(c, TmpStr, ProdAmount); return true; }); //Share last production between last assemblers

}


void Perf_Diag_Publishing()
{
    if (!DisablePrinting) Echo("Printing -> " + PrintingPeriod.TotalSeconds.ToString() + "s"); else Echo("Printing Disabled");
    if (!DisableSorting) Echo("Sorting -> " + SortingPeriod.TotalSeconds.ToString() + "s"); else Echo("Sorting Disabled");
    if (!DisableManufacturing) Echo("Manufacturing -> " + ManufacturingPeriod.TotalSeconds.ToString() + "s");
    else Echo("Auto Manufacturing Inactive");
    if (!DisableRefinery) Echo("Refinery Forcing -> " + RefineryPeriod.TotalSeconds.ToString() + "s");
    else Echo("Refinery Forcing Inactive");
    if (CheckInvOnExt) Echo("Ext Grid Scanned"); else Echo("Ext Grid Not Scanned");
    if (DynamicGridScan) Echo("Dynamic Grid Scan"); else Echo("Unique Grid Scan");

    Echo(FunctionExecuted);
    Echo("---------------------"); Echo("Perf Diagnostic"); Echo("---------------------");
    Echo("Last : " + CurrentT.TotalMilliseconds.ToString() + "ms");
    Echo("Worst : " + MaxT.TotalMilliseconds.ToString() + "ms");

    Echo("Last : " + Runtime.CurrentInstructionCount.ToString() + " // " + Runtime.MaxInstructionCount.ToString());

    if (MaxInstruct < Runtime.CurrentInstructionCount) { MaxInstruct = Runtime.CurrentInstructionCount; LowPerfFunction = FunctionExecuted; }

    Echo("Worst : " + MaxInstruct.ToString());
    Echo("During : " + LowPerfFunction);
}


//Utilitary functions >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

String UserDataEntry(String ScanField, String Pattern, String FlagStart, String FlagStop)
{
    TmpInt1 = ScanField.IndexOf(Pattern + FlagStart); // Find OffSet 
    if (TmpInt1 >= 0)
    {
        TmpInt1 += Pattern.Length + 1;
        TmpInt2 = ScanField.IndexOf(FlagStop, TmpInt1) - TmpInt1; // Find Length
        if (TmpInt2 > 0)
            return ScanField.Substring(TmpInt1, TmpInt2);
    }
    return null;
}

bool SetPeriod(ref TimeSpan Period, String TagParam, int MinPeriod = 500)
{
    int MSecNb = -1;
    if (!int.TryParse(UserDataEntry(Me.CustomData, TagParam, "[", "]"), out MSecNb)) MSecNb = MinPeriod;
    if (MSecNb == 0) return true;
    if (MSecNb < MinPeriod) MSecNb = MinPeriod;
    Period = new TimeSpan(0, 0, 0, 0, MSecNb);
    return false;
}

//DATABASE & TAGGING MANAGEMENT >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>


bool IsIdentifiedAs(IMyTerminalBlock TestSubject, string Pattern)
{
    if (TestSubject.CustomName.Contains(Pattern)
        || TestSubject.CustomData.Contains(Pattern)) return true;
    else return false;
}


bool IsInList(IMyTerminalBlock TestSubject, List<c_Tag> AnyTagList)
{
    return AnyTagList.Exists(c => IsIdentifiedAs(TestSubject, c.UserTag));
}


bool SatisfyGridPolicy(IMyTerminalBlock TestSubject, bool Policy = false)
{
    if (TestSubject == null) return false;
    return (TestSubject.CubeGrid.CustomName.StartsWith(MotherGrid) || Policy);
}


c_Tag TagForThisItem(MyInventoryItem Item)
{
    foreach (var Tag in _MainInvTag)
    {
        if (Item.Type.TypeId.ToString().EndsWith(Tag.Key) || Tag.Key == "Tool")
            return Tag.Value;
    }
    return _MainInvTag["Tool"];
}


void AddItemOnList(MyInventoryItem TestItem)
{
    Tag = TagForThisItem(TestItem);
    _SomeItems.Clear();
    _SomeItems.AddRange(__AllItems[Tag.NativeTag]);
    if (_SomeItems.Exists(c => c.Name == TestItem.Type.SubtypeId))
        _SomeItems.Find(c => c.Name == TestItem.Type.SubtypeId).AddAmount((long)TestItem.Amount);
    else
        _SomeItems.Add(new c_Item(TestItem.Type.SubtypeId, (long)TestItem.Amount));
}

void ScanItemAndCargoFilling(IMyTerminalBlock Scanned)
{
    WriteInvCapacity(Scanned.CustomName, (Scanned).GetInventory(0), ref gStrB);
    _Items.Clear();
    Scanned.GetInventory(0).GetItems(_Items);
    if (Scanned.InventoryCount == 2)
    {
        WriteInvCapacity("|".PadLeft(19), (Scanned).GetInventory(1), ref gStrB);
        _Items2.Clear();
        Scanned.GetInventory(1).GetItems(_Items2);
        _Items.AddRange(_Items2);
    }
    _Items.All(c => { AddItemOnList(c); return true; });
}

void BuildCritList(string KeyTag, List<c_Item> _List)
{
    _List = _List.FindAll(x => x.NeedProd);  //Prepare DataBase for next work  
    if (_List.Count == 0) return;

    gStrB.Append("-".PadRight(28, '-') + "\n" + (KeyTag + " to Produce : ").PadRight(28) + "\n" + "-".PadRight(28, '-') + "\n");
    for (x = 0; x < _List.Count; x++)
    {
        TmpStrB.Clear();
        TmpStrB.Append(FilterName(_List[x].NameStrB));
        while (TmpStrB.Length < 13) TmpStrB.Append('-'); // replace padright
        gStrB.Append(TmpStrB + "> ");
        gStrB.Append(FormatNb(_List[x].AmountNeeded) + "  " + BlueChr + " ");
        gStrB.Append(FormatNb(_List[x].Ratio) + "%\n");
    }

(__CriticalItems[KeyTag]).AddRange(_List);
}

void BuildItemList(string KeyTag, List<c_Item> _List)
{
    gStrB.Clear();
    gStrB.Append("-".PadRight(28, '-') + "\n" + KeyTag.PadRight(28, ' ') + "\n" + "-".PadRight(28, '-') + "\n");
    Tag = _MainInvTag[KeyTag];
    foreach (c_Item AnyItem in _List)
    {
        TmpStrB.Clear();
        TmpStrB.Append(FilterName(AnyItem.NameStrB));
        while (TmpStrB.Length < 13) TmpStrB.Append('-');
        gStrB.Append(TmpStrB + "> ");
        gStrB.Append(FormatNb(AnyItem.Amount) + "  ");

        if (AnyItem.WithTarget)
        {
            if (AnyItem.NeedProd) gStrB.Append(RedChr + " " + FormatNb(AnyItem.Ratio) + "%\n");
            else gStrB.Append(GreenChr + " " + FormatNb(AnyItem.Ratio) + "%\n");
        }
        else
            gStrB.Append(YellowChr + "  --- \n");
    }
    Tag.Refresh(gStrB);
}

void BuildConnectedShipList(IMyShipConnector Connector)
{
    if (!SatisfyGridPolicy(Connector) && SatisfyGridPolicy(Connector.OtherConnector))
        gStrB.Append(("-> " + Connector.CubeGrid.CustomName).PadRight(28) + "\n");
}

//BLOCKS MANAGEMENT >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

void MainScanningGrid()   //Getting by type block groups 
{
    GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(_WithInv, x => x.HasInventory
                                                            && SatisfyGridPolicy(x, CheckInvOnExt));
    GridTerminalSystem.GetBlocksOfType<IMyCargoContainer>(_TypeCargo);
    GridTerminalSystem.GetBlocksOfType<IMyRefinery>(_TypeRefinery, x => SatisfyGridPolicy(x));
    GridTerminalSystem.GetBlocksOfType<IMyAssembler>(_TypeAssembler, x => SatisfyGridPolicy(x));
    GridTerminalSystem.GetBlocksOfType<IMyShipConnector>(_TypeConnector);
}

void LCDScanningGrid()  //Getting Tagged LCD blocks
{
    GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(_TypeLCD, x => SatisfyGridPolicy(x)
                            && (IsInList(x, _LCDTagFormattedList)));
}

void ConstructSortingBlocksLists(string argument = "")
{
    //Check if argument is pertinent to be used as inventory group >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    GridTerminalSystem.GetBlockGroups(_AllGroups);
    _SortGrp.Clear();

    if (_AllGroups.Exists(x => x.Name.Equals(argument)))
    {
        GridTerminalSystem.GetBlockGroupWithName(argument).GetBlocks(_SortGrp);
        _SortGrp.RemoveAll(x => !x.HasInventory);
    }

    if (_SortGrp.Count == 0)
    {
        _SortGrp.AddRange(_TypeCargo);
        _SortGrp.AddRange(_TypeRefinery);
        _SortGrp.AddRange(_TypeAssembler);
        _SortGrp.AddRange(_TypeConnector);
    }

    _SortGrp.RemoveAll(x => (!SatisfyGridPolicy(x) && !IsIdentifiedAs(x, "FreeTaking")));
    _RcvGrp = _SortGrp.FindAll(x => (SatisfyGridPolicy(x) && x.InventoryCount == 1));

    __AllByTypeRcvBlock.Clear();  // Construct Skeleton of By Type Cargo Receivers List² and fill it >>>>>>>>>>
    foreach (var Tag in _MainInvTag)
    {
        __AllByTypeRcvBlock.Add(Tag.Key, new List<c_RcvBlock>());
        foreach (IMyTerminalBlock AnyBlock in _RcvGrp)
        {
            if (IsIdentifiedAs(AnyBlock, Tag.Value.UserTag))
                __AllByTypeRcvBlock[Tag.Key].Add(new c_RcvBlock(AnyBlock));
        }
    }

    __AllByItemRcvBlock.Clear();  // Construct Skeleton of By Item Cargo Receivers List² and fill it >>>>>>>>>>
    foreach (var _AnyInv in __AllItems)
    {
        __AllByItemRcvBlock.Add(_AnyInv.Key, new List<c_RcvBlock>());
        foreach (IMyTerminalBlock AnyBlock in _RcvGrp)
        {
            gStr = UserDataEntry(AnyBlock.CustomData, _AnyInv.Key, "{", "}");
            if (gStr == null) continue;

            foreach (c_Item Item in _AnyInv.Value)
            {
                if (gStr.Contains(Item.Name))
                {
                    int Target = 0;
                    int.TryParse(UserDataEntry(gStr, Item.Name, "[", "]"), out Target);
                    //if (x < 0) x = 0;
                    __AllByItemRcvBlock[_AnyInv.Key].Add(new c_RcvBlock(AnyBlock, Item.Name, _AnyInv.Key, Target));
                }
            }
        }
    }
}



void LCD_ContentDispatching() //Prepare Content to Publish on Tagged LCDs
{
    if (_TypeLCD.Count != _PrintedLCD.Count)
    {
        _PrintedLCD.Clear();
        foreach (IMyTextPanel AnyLCD in _TypeLCD)
            _PrintedLCD.Add(new c_PrintedLCD(AnyLCD));
    }

    _PrintedLCD.All(c => { c.Init(); return true; });

    foreach (var AnyTag in _LCDTag)
    {
        _TempLCD = _PrintedLCD.FindAll(x => (IsIdentifiedAs(x.LCD, AnyTag.Value.UserTag)));
        _TempLCD.All(c => { c.Content.Append(AnyTag.Value.LCDPrint); return true; });
    }
}

c_RcvBlock FirstBlockNotFull(List<c_RcvBlock> _List)
{
    if (_List == null)
        return null;
    if (_List.Exists(x => !x.Block.GetInventory(0).IsFull))
        return (_List.Find(x => !x.Block.GetInventory(0).IsFull));
    else
        return null;
}

void SortProcessing(IMyTerminalBlock Sender)
{
    TrAmount = null;
    Tag = TagForThisItem(_Items[x]);
    ItemName = _Items[x].Type.SubtypeId;
    if ((" " + UserDataEntry(Sender.CustomData, Tag.NativeTag, "{", "}")).Contains(ItemName)) return;
    _SomeRcvBlock = __AllByItemRcvBlock[Tag.NativeTag].FindAll(x => x.MatchWithItem(ItemName));
    Receiver = FirstBlockNotFull(_SomeRcvBlock);
    if (Receiver != null) TrAmount = Receiver.Needed;

    if (Receiver == null && !IsIdentifiedAs(Sender, Tag.UserTag))
        Receiver = FirstBlockNotFull(__AllByTypeRcvBlock[Tag.NativeTag]);

    if (Receiver == null) return;
    (Sender).GetInventory(indice).TransferItemTo((Receiver.Block).GetInventory(0), x - k, null, true, TrAmount);
    _Items2.Clear();
    Sender.GetInventory(indice).GetItems(_Items2);
    k = _Items.Count - _Items2.Count;
}

void UpdateDatabaseWithRecipeofNeededComponent()   //Add Item Recipe to global needed item list
{
    _Ingot = __AllItems["Ingot"];
    _Compo = __AllItems["Component"];
    _Tool = __AllItems["Tool"];

    _Ingot[(int)eIngot.Stone].AddTarget((20 * _Compo[(int)eCompo.Reactor].AmountNeeded) / AssEfficiency);

    double TmpAmount = 21 * _Compo[(int)eCompo.SteelP].AmountNeeded + 3.5 * _Compo[(int)eCompo.IntP].AmountNeeded + 10 * _Compo[(int)eCompo.ConstructC].AmountNeeded
        + 7 * _Compo[(int)eCompo.Girder].AmountNeeded + 5 * _Compo[(int)eCompo.STube].AmountNeeded + 30 * _Compo[(int)eCompo.LTube].AmountNeeded
        + 12 * _Compo[(int)eCompo.MGrid].AmountNeeded + 20 * _Compo[(int)eCompo.Motor].AmountNeeded + 1 * _Compo[(int)eCompo.Display].AmountNeeded
        + 0.5 * _Compo[(int)eCompo.CPU].AmountNeeded + 8 * _Compo[(int)eCompo.RadioC].AmountNeeded + 15 * _Compo[(int)eCompo.Reactor].AmountNeeded
        + 30 * _Compo[(int)eCompo.Thrust].AmountNeeded + 60 * _Compo[(int)eCompo.Medic].AmountNeeded + 5 * _Compo[(int)eCompo.Detector].AmountNeeded
        + 10 * _Compo[(int)eCompo.PCell].AmountNeeded + 10 * _Compo[(int)eCompo.SupCond].AmountNeeded + 600 * _Compo[(int)eCompo.GravGen].AmountNeeded
        + 2 * _Compo[(int)eCompo.Canvas].AmountNeeded;
    TmpAmount += 80 * _Tool[(int)eTool.OxyB].AmountNeeded + 80 * _Tool[(int)eTool.HydroB].AmountNeeded + 5 * _Tool[(int)eTool.Welder1].AmountNeeded
                + 5 * _Tool[(int)eTool.Welder2].AmountNeeded + 5 * _Tool[(int)eTool.Welder3].AmountNeeded + 5 * _Tool[(int)eTool.Welder4].AmountNeeded
                + 3 * _Tool[(int)eTool.Grind1].AmountNeeded + 3 * _Tool[(int)eTool.Grind2].AmountNeeded + 3 * _Tool[(int)eTool.Grind3].AmountNeeded
                + 3 * _Tool[(int)eTool.Grind4].AmountNeeded + 20 * _Tool[(int)eTool.Drill1].AmountNeeded + 20 * _Tool[(int)eTool.Drill2].AmountNeeded
                + 20 * _Tool[(int)eTool.Drill3].AmountNeeded + 20 * _Tool[(int)eTool.Drill4].AmountNeeded + 3 * _Tool[(int)eTool.Rifle1].AmountNeeded
                + 20 * _Tool[(int)eTool.Rifle2].AmountNeeded + 20 * _Tool[(int)eTool.Rifle3].AmountNeeded + 20 * _Tool[(int)eTool.Rifle4].AmountNeeded
                + 0.8 * _Tool[(int)eTool.Nato45].AmountNeeded + 40 * _Tool[(int)eTool.Nato184].AmountNeeded + 55 * _Tool[(int)eTool.Missile].AmountNeeded;
    _Ingot[(int)eIngot.Iron].AddTarget(TmpAmount / AssEfficiency);

    TmpAmount = 5 * _Compo[(int)eCompo.MGrid].AmountNeeded + 5 * _Compo[(int)eCompo.Motor].AmountNeeded + 70 * _Compo[(int)eCompo.Medic].AmountNeeded
        + 15 * _Compo[(int)eCompo.Detector].AmountNeeded + 10 * _Compo[(int)eCompo.SCell].AmountNeeded + 2 * _Compo[(int)eCompo.PCell].AmountNeeded;
    TmpAmount += 30 * _Tool[(int)eTool.OxyB].AmountNeeded + 30 * _Tool[(int)eTool.HydroB].AmountNeeded + 1 * _Tool[(int)eTool.Welder1].AmountNeeded
                + 1 * _Tool[(int)eTool.Welder2].AmountNeeded + 1 * _Tool[(int)eTool.Welder3].AmountNeeded + 1 * _Tool[(int)eTool.Welder4].AmountNeeded
                + 1 * _Tool[(int)eTool.Grind1].AmountNeeded + 1 * _Tool[(int)eTool.Grind2].AmountNeeded + 1 * _Tool[(int)eTool.Grind3].AmountNeeded
                + 1 * _Tool[(int)eTool.Grind4].AmountNeeded + 3 * _Tool[(int)eTool.Drill1].AmountNeeded + 3 * _Tool[(int)eTool.Drill2].AmountNeeded
                + 3 * _Tool[(int)eTool.Drill3].AmountNeeded + 3 * _Tool[(int)eTool.Drill4].AmountNeeded + 1 * _Tool[(int)eTool.Rifle1].AmountNeeded
                + 8 * _Tool[(int)eTool.Rifle2].AmountNeeded + 1 * _Tool[(int)eTool.Rifle3].AmountNeeded + 1 * _Tool[(int)eTool.Rifle4].AmountNeeded
                + 0.2 * _Tool[(int)eTool.Nato45].AmountNeeded + 5 * _Tool[(int)eTool.Nato184].AmountNeeded + 7 * _Tool[(int)eTool.Missile].AmountNeeded;
    _Ingot[(int)eIngot.Nickel].AddTarget(TmpAmount / AssEfficiency);

    TmpAmount = 15 * _Compo[(int)eCompo.BPGlass].AmountNeeded + 5 * _Compo[(int)eCompo.Display].AmountNeeded + 0.2 * _Compo[(int)eCompo.CPU].AmountNeeded
        + 1 * _Compo[(int)eCompo.RadioC].AmountNeeded + 8 * _Compo[(int)eCompo.SCell].AmountNeeded + 1 * _Compo[(int)eCompo.PCell].AmountNeeded
        + 0.5 * _Compo[(int)eCompo.Explo].AmountNeeded + 35 * _Compo[(int)eCompo.Canvas].AmountNeeded;
    TmpAmount += 2 * _Tool[(int)eTool.Welder2].AmountNeeded
                + 1 * _Tool[(int)eTool.Grind1].AmountNeeded + 6 * _Tool[(int)eTool.Grind2].AmountNeeded + 2 * _Tool[(int)eTool.Grind3].AmountNeeded
                + 2 * _Tool[(int)eTool.Grind4].AmountNeeded + 3 * _Tool[(int)eTool.Drill1].AmountNeeded + 5 * _Tool[(int)eTool.Drill2].AmountNeeded
                + 3 * _Tool[(int)eTool.Drill3].AmountNeeded + 3 * _Tool[(int)eTool.Drill4].AmountNeeded + 0.2 * _Tool[(int)eTool.Missile].AmountNeeded;
    _Ingot[(int)eIngot.Silicon].AddTarget(TmpAmount / AssEfficiency);

    TmpAmount = 3 * _Compo[(int)eCompo.MGrid].AmountNeeded + 10 * _Compo[(int)eCompo.Thrust].AmountNeeded + 220 * _Compo[(int)eCompo.GravGen].AmountNeeded;
    TmpAmount += 0.2 * _Tool[(int)eTool.Welder1].AmountNeeded
        + 0.2 * _Tool[(int)eTool.Welder2].AmountNeeded + 0.2 * _Tool[(int)eTool.Welder3].AmountNeeded + 0.2 * _Tool[(int)eTool.Welder4].AmountNeeded
        + 1 * _Tool[(int)eTool.Grind1].AmountNeeded + 2 * _Tool[(int)eTool.Grind2].AmountNeeded + 1 * _Tool[(int)eTool.Grind3].AmountNeeded
        + 1 * _Tool[(int)eTool.Grind4].AmountNeeded + 5 * _Tool[(int)eTool.Rifle3].AmountNeeded;
    _Ingot[(int)eIngot.Cobalt].AddTarget(TmpAmount / AssEfficiency);

    TmpAmount = 2 * _Compo[(int)eCompo.Explo].AmountNeeded;
    TmpAmount += 0.15 * _Tool[(int)eTool.Nato45].AmountNeeded + 3 * _Tool[(int)eTool.Nato184].AmountNeeded + 1.2 * _Tool[(int)eTool.Missile].AmountNeeded;
    _Ingot[(int)eIngot.Magnesium].AddTarget(TmpAmount / AssEfficiency);

    TmpAmount = 5 * _Compo[(int)eCompo.Reactor].AmountNeeded + 20 * _Compo[(int)eCompo.Medic].AmountNeeded + 5 * _Compo[(int)eCompo.GravGen].AmountNeeded;
    TmpAmount += 10 * _Tool[(int)eTool.OxyB].AmountNeeded + 10 * _Tool[(int)eTool.HydroB].AmountNeeded + 2 * _Tool[(int)eTool.Welder3].AmountNeeded
                + 2 * _Tool[(int)eTool.Grind3].AmountNeeded + 2 * _Tool[(int)eTool.Drill3].AmountNeeded + 6 * _Tool[(int)eTool.Rifle4].AmountNeeded;
    _Ingot[(int)eIngot.Silver].AddTarget(TmpAmount / AssEfficiency);

    TmpAmount = 1 * _Compo[(int)eCompo.Thrust].AmountNeeded + 2 * _Compo[(int)eCompo.SupCond].AmountNeeded + 10 * _Compo[(int)eCompo.GravGen].AmountNeeded;
    _Ingot[(int)eIngot.Gold].AddTarget(TmpAmount / AssEfficiency);

    TmpAmount = 0.4 * _Compo[(int)eCompo.Thrust].AmountNeeded;
    TmpAmount += 2 * _Tool[(int)eTool.Welder4].AmountNeeded + 0.04 * _Tool[(int)eTool.Missile].AmountNeeded
                + 2 * _Tool[(int)eTool.Grind4].AmountNeeded + 2 * _Tool[(int)eTool.Drill4].AmountNeeded + 4 * _Tool[(int)eTool.Rifle4].AmountNeeded;
    _Ingot[(int)eIngot.Platinum].AddTarget(TmpAmount / AssEfficiency);

    _Ingot[(int)eIngot.Uranium].AddTarget((0.1 * _Tool[(int)eTool.Missile].AmountNeeded) / AssEfficiency);

    __AllItems["Ore"][(int)eIngot.Stone].AddTarget(_Ingot[(int)eIngot.Stone].AmountNeeded * 100);
    __AllItems["Ore"][(int)eIngot.Iron].AddTarget(_Ingot[(int)eIngot.Iron].AmountNeeded * 1.79);
    __AllItems["Ore"][(int)eIngot.Nickel].AddTarget(_Ingot[(int)eIngot.Nickel].AmountNeeded * 3.14);
    __AllItems["Ore"][(int)eIngot.Silicon].AddTarget(_Ingot[(int)eIngot.Silicon].AmountNeeded * 1.79);
    __AllItems["Ore"][(int)eIngot.Cobalt].AddTarget(_Ingot[(int)eIngot.Cobalt].AmountNeeded * 4.17);
    __AllItems["Ore"][(int)eIngot.Magnesium].AddTarget(_Ingot[(int)eIngot.Magnesium].AmountNeeded * 178.6);
    __AllItems["Ore"][(int)eIngot.Silver].AddTarget(_Ingot[(int)eIngot.Silver].AmountNeeded * 12.5);
    __AllItems["Ore"][(int)eIngot.Gold].AddTarget(_Ingot[(int)eIngot.Gold].AmountNeeded * 125);
    __AllItems["Ore"][(int)eIngot.Platinum].AddTarget(_Ingot[(int)eIngot.Platinum].AmountNeeded * 250);
    __AllItems["Ore"][(int)eIngot.Uranium].AddTarget(_Ingot[(int)eIngot.Uranium].AmountNeeded * 178.6);
}


//PRINTING FUNCTIONS & STRING MANAGEMENT >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>


StringBuilder FormatNb(long Data)
{
    TmpInt1 = ((int)Math.Ceiling(Math.Log10(Data + 1)) - 1) / 3; // digit pack = digit number / 3
    TmpStrB.Clear();
    TmpStrB.Append(((Data) / (int)Math.Pow(1000, TmpInt1)).ToString()); // FormatNb = Data / (1000 ^ digit pack)

    switch (TmpInt1)
    {
        case 1: TmpStrB.Append("K"); break;
        case 2: TmpStrB.Append("M"); break;
        case 3: TmpStrB.Append("G"); break;
        case 4: TmpStrB.Append("T"); break;
    }

    while (TmpStrB.Length < 4) TmpStrB.Insert(0, ' '); // replace padleft
    return (TmpStrB);
}


StringBuilder FilterName(StringBuilder Subject)
{

    Subject = Subject.Replace("NATO_", "");
    Subject = Subject.Replace("Item", "");
    Subject = Subject.Replace("Angle", "");
    Subject = Subject.Replace("Automatic", "");

    Subject = Subject.Replace("RadioCommunication", "RadioCom");
    Subject = Subject.Replace("BulletproofGlass", "BulletpGlass");
    Subject = Subject.Replace("GravityGenerator", "GravityGen");
    Subject = Subject.Replace("Superconductor", "Supconductor");
    Subject = Subject.Replace("InteriorPlate", "IntPlate");
    Subject = Subject.Replace("RapidFireRifle", "RapidRifle");
    Subject = Subject.Replace("UltimateRifle", "RoxxorRifle");
    Subject = Subject.Replace("OxygenBottle", "OxyBottle");
    Subject = Subject.Replace("HydrogenBottle", "HydroBottle");

    return Subject;
}


void WriteInvCapacity(string Title, IMyInventory Inv, ref StringBuilder str)
{
    TmpInt1 = (int)(((float)Inv.CurrentVolume * 100) / (float)Inv.MaxVolume); // Cargo Capacity
    str.Append(Title.PadRight(20, '-').Remove(19) + "-> " + TmpInt1.ToString().PadLeft(3) + "% ");
    if (TmpInt1 >= 80) str.Append(YellowChr + "\n");
    else str.Append(GreenChr + "\n");
}


static char CreateCustomColor(int r, int g, int b)
{
    return (char)(0xE100 + (MathHelper.Clamp(r, 0, 7) << 6) + (MathHelper.Clamp(g, 0, 7) << 3) + MathHelper.Clamp(b, 0, 7));
}


//PRODUCTION & BLUEPRINT MANAGEMENT >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

string BluePrintBuild(string Subject)
{

    Subject = Subject.Replace("Item", "");

    switch (Subject)
    {
        case "RadioCommunication": Subject += MANGEBURNES; break;
        case "Computer": Subject += MANGEBURNES; break;
        case "Reactor": Subject += MANGEBURNES; break;
        case "Detector": Subject += MANGEBURNES; break;
        case "Construction": Subject += MANGEBURNES; break;
        case "Thrust": Subject += MANGEBURNES; break;
        case "Motor": Subject += MANGEBURNES; break;
        case "Explosives": Subject += MANGEBURNES; break;
        case "Girder": Subject += MANGEBURNES; break;
        case "GravityGenerator": Subject += MANGEBURNES; break;
        case "Medical": Subject += MANGEBURNES; break;
        case "NATO_25x184mm": Subject += "Magazine"; break;
        case "NATO_5p56x45mm": Subject += "Magazine"; break;
    }

    Subject = BLUEPRINT + Subject;
    return Subject;
}


void SetQueue(IMyAssembler assembler, string BPid, double Amount)
{
    if (MyDefinitionId.TryParse(BPid, out ItemRecipe))
        assembler.AddQueueItem(ItemRecipe, Amount);
}

#endregion
