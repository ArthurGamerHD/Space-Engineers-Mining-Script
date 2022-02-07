/* v:2.0178
* Automatic LCDs 2 - In-game script by MMaster
*
*  Última Atualização: Armas e Muniçoes atualizadas para igualar com as diferentes versões atualmente no jogo (confira o guia do comando "Inventory" para lista completa) 
*  Corrigido LCDs wide que quando rotacionados que não cobriam a tela inteira
*  Opção adicionada "SKIP_CONTENT_TYPE" para desativar a troca automática do tipo de conteúdo LCD (verifique a aba "Tips & Tricks" do guia)
*  As opções SlowMode, SKIP_CONTENT_TYPE, SCROLL_LINES e ENABLE_BOOT agora pode ser definidas na aba "custom data" do "bloco programável"
*  
*  Updates Notaveis que valem ser citados:
*  Suporte a painéis LCDs do cockpit (e outros blocos com superficie editaveis) - leia a seção guia 'How to use with cockpits?'
*  Otimizações para servidores executando "limitador de scripts" - use o SlowMode!
*  Configuração de SlowMode adicionada para diminuir consideravelmente as atualizações do script (4-5 times less processing per second)
*  Agora usando MDK!
* 
*  Para updates antigos, Verifique a pagina original do Script na Steam */

/* Personalize aqui: */

// Use esta tag para identificar quais LCDs serão gerenciados por este script
// Você pode definir uma regra para filtrar entre: "G:Nome Do Grupo" ou "T:[meu LCD]"
public string LCD_TAG = "T:[LCD]";

// Defina como "True" para habilitar o modo lento (padrão "false")
public bool SlowMode = false;

// Defina quantas linhas irá rolar por vez
public int SCROLL_LINES = 1;

// Script define automaticamente se o LCD está usando fonte monoespaçada
// se você usar a rolagem de fonte personalizada, vá até a parte inferior, suba um pouco até encontrar a linha "AddCharsSize"
// nome da fonte monoespaçada e definição de tamanho estão logo acima

// Habilitar sequencia de Booting (após compilar ou recarregar o mundo)

public bool ENABLE_BOOT = true;

// Defina como "true" para IMPEDIR o script de alterar o Tipo de conteudo dos LCDs
public bool SKIP_CONTENT_TYPE = false;

/* (falas do criador do script)

LEIA O GUIA COMPLETO (em Inglês)
http://steamcommunity.com/sharedfiles/filedetails/?id=407158161

Guia basico em vídeo
Por favor, assista ao vídeo guia mesmo que você não entenda meu inglês. Você pode ver como as coisas são feitas lá.

https://youtu.be/vqpPQ_20Xso


Por favor, leia atentamente o guia completo antes de fazer perguntas, ja que eu tive que remover guia a partir daqui para poder adicionar mais recursos :(

Por favor, NÃO publique este script ou seus derivados sem a minha permissão! Sinta-se à vontade para usá-lo em suas Naves e Blueprints!

Agradecimentos especiais
Keen Software House por esse incrível jogo Space Engineers
Malware por contribuir com código dos blocos programáveis e MDK!

veja meu Twitter: https://twitter.com/MattsPlayCorner
e o meu Facebook: https://www.facebook.com/MattsPlayCorner1080p
para mais coisas loucas feitas por mim no futuro :)

Se você quiser fazer scripts para Space Engineers, confira MDK feito pelo @Malware:
https://github.com/malware-dev/MDK-SE/wiki/Quick-Introduction-to-Space-Engineers-Ingame-Scripts
*/
bool MDK_IS_GREAT = true;   // << (Isso é uma piada não necessario para o script, mas é bom mesmo)
/*Personalize os caracteres usados pelo script */
class MMStyle
{
    // Caracteres de fonte monoespaçada (\uXXXX é um código de caractere especial)
    public const char BAR_MONO_START = '[';
    public const char BAR_MONO_END = ']';
    public const char BAR_MONO_EMPTY = '\u2591'; // 25% rect
    public const char BAR_MONO_FILL = '\u2588'; // full rect

    // Classic (Debug) font characters
    // Start and end characters of progress bar need to be the same width!
    public const char BAR_START = '[';
    public const char BAR_END = ']';
    // Empty and fill characters of progress bar need to be the same width!
    public const char BAR_EMPTY = '\'';
    public const char BAR_FILL = '|';
}
// (for developer) Debug level to show
public const int DebugLevel = 0;

// (for modded lcds) Affects all LCDs managed by this programmable block
/* LCD height modifier
0.5f makes the LCD have only 1/2 the lines of normal LCD
2.0f makes it fit 2x more lines on LCD */
public const float HEIGHT_MOD = 1.0f;

/* line width modifier
0.5f moves the right edge to 50% of normal LCD width
2.0f makes it fit 200% more text on line */
public const float WIDTH_MOD = 1.0f;

List<string> BOOT_FRAMES = new List<string>() {
/* BOOT FRAMES
* Each @"<text>" marks single frame, add as many as you want each will be displayed for one second
* @"" is multiline string so you can write multiple lines */
@"
Initializing systems"
,
@"
Verifying connections"
,
@"
Loading commands"
};

void ItemsConf() {
    // ITEMS AND QUOTAS LIST
    // (subType, mainType, quota, display name, short name)
    // VANILLA ITEMS
    Add("Stone", "Ore", 0, "Pedra");
    Add("Iron", "Ore", 0, "Ferro");
    Add("Nickel", "Ore", 0, "Níquel");
    Add("Cobalt", "Ore", 0, "Cobalto");
    Add("Magnesium", "Ore", 0, "Magnésio");
    Add("Silicon", "Ore", 0, "Silício");
    Add("Silver", "Ore", 0, "Prata");
    Add("Gold", "Ore", 0, "Ouro");
    Add("Platinum", "Ore", 0, "Platina");
    Add("Uranium", "Ore", 0, "Urânio");
    Add("Ice", "Ore", 0, "Gelo");
    Add("Scrap", "Ore", 0, "Sucata");
    Add("Stone", "Ingot", 40000, "Cascalho", "gravel");
    Add("Iron", "Ingot", 300000, "Ferro");
    Add("Nickel", "Ingot", 900000, "Níquel");
    Add("Cobalt", "Ingot", 120000, "Cobalto");
    Add("Magnesium", "Ingot", 80000, "Magnésio");
    Add("Silicon", "Ingot", 80000, "Silício");
    Add("Silver", "Ingot", 800000, "Prata");
    Add("Gold", "Ingot", 80000, "Ouro");
    Add("Platinum", "Ingot", 45000, "Platina");
    Add("Uranium", "Ingot", 12000, "Urânio");
    Add("SemiAutoPistolItem", "Tool", 0, "Pistola S-10", "s-10");
    Add("ElitePistolItem", "Tool", 0, "Pistola S-10E", "s-10e");
    Add("FullAutoPistolItem", "Tool", 0, "Pistola S-20A", "s-20a");
    Add("AutomaticRifleItem", "Tool", 0, "Rifle MR-20", "mr-20");
    Add("PreciseAutomaticRifleItem", "Tool", 0, "Rifle MR-8P", "mr-8p");
    Add("RapidFireAutomaticRifleItem", "Tool", 0, "Rifle MR-50A", "mr-50a");
    Add("UltimateAutomaticRifleItem", "Tool", 0, "Rifle MR - 30E", "mr-30e");
    Add("BasicHandHeldLauncherItem", "Tool", 0, "Lançador de Foguetes RO-1", "ro-1");
    Add("AdvancedHandHeldLauncherItem", "Tool", 0, "Lançador de Foguetes PRO-1", "pro-1");
    Add("WelderItem", "Tool", 0, "Solda");
    Add("Welder2Item", "Tool", 0, "* Enh. Welder");
    Add("Welder3Item", "Tool", 0, "** Prof. Welder");
    Add("Welder4Item", "Tool", 0, "*** Elite Welder");
    Add("AngleGrinderItem", "Tool", 0, "Angle Grinder");
    Add("AngleGrinder2Item", "Tool", 0, "* Enh. Grinder");
    Add("AngleGrinder3Item", "Tool", 0, "** Prof. Grinder");
    Add("AngleGrinder4Item", "Tool", 0, "*** Elite Grinder");
    Add("HandDrillItem", "Tool", 0, "Furadeira de Mão");
    Add("HandDrill2Item", "Tool", 0, "* Enh. Drill");
    Add("HandDrill3Item", "Tool", 0, "** Prof. Drill");
    Add("HandDrill4Item", "Tool", 0, "*** Elite Drill");
    Add("Construction", "Component", 50000, "Componente de Construção");
    Add("MetalGrid", "Component", 15500, "Grade De Metal");
    Add("InteriorPlate", "Component", 55000, "Placa Interior");
    Add("SteelPlate", "Component", 300000, "Placa de Aço");
    Add("Girder", "Component", 3500, "Viga");
    Add("SmallTube", "Component", 26000, "Tubo Pequeno");
    Add("LargeTube", "Component", 6000, "Tubo Grande");
    Add("Motor", "Component", 16000);
    Add("Display", "Component", 500, "Tela");
    Add("BulletproofGlass", "Component", 12000, "Vidro a prova de balas", "bpglass");
    Add("Computer", "Component", 6500, "Computador");
    Add("Reactor", "Component", 10000, "Componente de Reator");
    Add("Thrust", "Component", 16000, "Componente de Propulsor", "thruster");
    Add("GravityGenerator", "Component", 250, "Componente do gerador de Gravidade", "gravgen");
    Add("Medical", "Component", 120, "Componentes Médicos");
    Add("RadioCommunication", "Component", 250, "Rádio-com", "radio");
    Add("Detector", "Component", 400, "Componentes Detectores");
    Add("Explosives", "Component", 500, "Explosivos");
    Add("SolarCell", "Component", 2800, "Células Solar");
    Add("PowerCell", "Component", 2800, "Células de Energia");
    Add("Superconductor", "Component", 3000, "Super Condutor");
    Add("Canvas", "Component", 300);
    Add("ZoneChip", "Component", 100, "Zone Chip");
    Add("Datapad", "Datapad", 0, "Tablet");
    Add("Medkit", "ConsumableItem", 0);
    Add("Powerkit", "ConsumableItem", 0);
    Add("SpaceCredit", "PhysicalObject", 0, "Créditos Espaciais");
    Add("NATO_5p56x45mm", "Ammo", 8000, "5.56x45mm", "5.56x45mm", false);
    Add("SemiAutoPistolMagazine", "Ammo", 500, "S-10 Mag", "s-10mag");
    Add("ElitePistolMagazine", "Ammo", 500, "S-10E Mag", "s-10emag");
    Add("FullAutoPistolMagazine", "Ammo", 500, "S-20A Mag", "s-20amag");
    Add("AutomaticRifleGun_Mag_20rd", "Ammo", 1000, "MR-20 Mag", "mr-20mag");
    Add("PreciseAutomaticRifleGun_Mag_5rd", "Ammo", 1000, "MR-8P Mag", "mr-8pmag");
    Add("RapidFireAutomaticRifleGun_Mag_50rd", "Ammo", 8000, "MR-50A Mag", "mr-50amag");
    Add("UltimateAutomaticRifleGun_Mag_30rd", "Ammo", 1000, "MR-30E Mag", "mr-30emag");
    Add("NATO_25x184mm", "Ammo", 2500, "25x184mm", "25x184mm");
    Add("Missile200mm", "Ammo", 1600, "200mm Missile", "200mmmissile");
    Add("OxygenBottle", "OxygenContainerObject", 5, "Cilindro de Oxigênio");
    Add("HydrogenBottle", "GasContainerObject", 5, "Cilindro de Hidrogenio");

    // MODDED ITEMS
    // (subType, mainType, quota, display name, short name, used)
    // * if used is true, item will be shown in inventory even for 0 items
    // * if used is false, item will be used only for display name and short name
    // AzimuthSupercharger
    Add("AzimuthSupercharger", "Component", 1600, "Supercharger", "supercharger", false);
    // OKI Ammo
    Add("OKI23mmAmmo", "Ammo", 500, "23x180mm", "23x180mm", false);
    Add("OKI50mmAmmo", "Ammo", 500, "50x450mm", "50x450mm", false);
    Add("OKI122mmAmmo", "Ammo", 200, "122x640mm", "122x640mm", false);
    Add("OKI230mmAmmo", "Ammo", 100, "230x920mm", "230x920mm", false);

    // REALLY REALLY REALLY
    // DO NOT MODIFY ANYTHING BELOW THIS (TRANSLATION STRINGS ARE AT THE BOTTOM)
}
void Add(string sT, string mT, int q = 0, string dN = "", string sN = "", bool u = true) { ƒ.Ƅ(sT, mT, q, dN, sN, u); }
ƈ ƒ;ȕ ƞ;Ģ ϖ;ɯ k=null;void ϕ(string Ɯ){}bool ϔ(string ϒ){return ϒ.ǯ("true")?true:false;}void ϓ(string ϗ,string ϒ){string
Ǻ=ϗ.ToLower();switch(Ǻ){case"lcd_tag":LCD_TAG=ϒ;break;case"slowmode":SlowMode=ϔ(ϒ);break;case"enable_boot":ENABLE_BOOT=ϔ(
ϒ);break;case"skip_content_type":SKIP_CONTENT_TYPE=ϔ(ϒ);break;case"scroll_lines":int ϑ=0;if(int.TryParse(ϒ,out ϑ)){
SCROLL_LINES=ϑ;}break;}}void ϐ(){string[]ǌ=Me.CustomData.Split('\n');for(int D=0;D<ǌ.Length;D++){string ǒ=ǌ[D];int ţ=ǒ.IndexOf('=');
if(ţ<0){ϕ(ǒ);continue;}string Ϗ=ǒ.Substring(0,ţ).Trim();string Ǯ=ǒ.Substring(ţ+1).Trim();ϓ(Ϗ,Ǯ);}}void ώ(ȕ ƞ){ƒ=new ƈ();
ItemsConf();ϐ();k=new ɯ(this,DebugLevel,ƞ){ƒ=ƒ,ɩ=LCD_TAG,ɨ=SCROLL_LINES,ɧ=ENABLE_BOOT,ɦ=BOOT_FRAMES,ɥ=!MDK_IS_GREAT,ɢ=HEIGHT_MOD,
ɤ=WIDTH_MOD};k.ǣ();}void ύ(){ƞ.ǆ=this;k.ǆ=this;}Program(){Runtime.UpdateFrequency=UpdateFrequency.Update1;}void Main(
string ą,UpdateType ι){try{if(ƞ==null){ƞ=new ȕ(this,DebugLevel,SlowMode);ώ(ƞ);ϖ=new Ģ(k);ƞ.ȋ(ϖ,0);}else{ύ();k.œ.Ѕ();}if(ą.
Length==0&&(ι&(UpdateType.Update1|UpdateType.Update10|UpdateType.Update100))==0){ƞ.ȣ();return;}if(ą!=""){if(ϖ.ć(ą)){ƞ.ȣ();
return;}}ϖ.Ġ=0;ƞ.Ȣ();}catch(Exception ex){Echo("ERROR DESCRIPTION:\n"+ex.ToString());Me.Enabled=false;}}class θ:ɕ{Ģ Ĝ;ɯ k;
string ą="";public θ(ɯ Ý,Ģ Ē,string Ĵ){ɐ=-1;ɔ="ArgScroll";ą=Ĵ;Ĝ=Ē;k=Ý;}int ů;δ η;public override void ɇ(){η=new δ(Ɩ,k.œ);}int
ζ=0;int ĝ=0;ˣ Ɯ;public override bool Ɏ(bool õ){if(!õ){ĝ=0;η.ŵ();Ɯ=new ˣ(Ɩ);ζ=0;}if(ĝ==0){if(!Ɯ.ʚ(ą,õ))return false;if(Ɯ.ˢ
.Count>0){if(!int.TryParse(Ɯ.ˢ[0].Ĵ,out ů))ů=1;else if(ů<1)ů=1;}if(Ɯ.ˡ.EndsWith("up"))ů=-ů;else if(!Ɯ.ˡ.EndsWith("down"))
ů=0;ĝ++;õ=false;}if(ĝ==1){if(!η.ϵ("textpanel",Ɯ.ˠ,õ))return false;ĝ++;õ=false;}ë j;for(;ζ<η.Ћ();ζ++){if(!Ɩ.Ȟ(20))return
false;IMyTextPanel ε=η.λ[ζ]as IMyTextPanel;if(!Ĝ.û.TryGetValue(ε,out j))continue;if(j==null||j.ç!=ε)continue;if(j.á)j.è.ļ=10;
if(ů>0)j.è.Ŀ(ů);else if(ů<0)j.è.Ł(-ů);else j.è.Ļ();j.K();}return true;}}class δ{ȕ Ɩ;Љ γ;IMyCubeGrid κ{get{return Ɩ.ǆ.Me.
CubeGrid;}}IMyGridTerminalSystem ǂ{get{return Ɩ.ǆ.GridTerminalSystem;}}public List<IMyTerminalBlock>λ=new List<IMyTerminalBlock>
();public δ(ȕ ƞ,Љ ϋ){Ɩ=ƞ;γ=ϋ;}int ϊ=0;public double ω(ref double ψ,ref double χ,bool õ){if(!õ)ϊ=0;for(;ϊ<λ.Count;ϊ++){if(
!Ɩ.Ȟ(4))return Double.NaN;IMyInventory σ=λ[ϊ].GetInventory(0);if(σ==null)continue;ψ+=(double)σ.CurrentVolume;χ+=(double)σ
.MaxVolume;}ψ*=1000;χ*=1000;return(χ>0?ψ/χ*100:100);}int φ=0;double υ=0;public double ό(bool õ){if(!õ){φ=0;υ=0;}for(;φ<λ.
Count;φ++){if(!Ɩ.Ȟ(6))return Double.NaN;for(int τ=0;τ<2;τ++){IMyInventory σ=λ[φ].GetInventory(τ);if(σ==null)continue;υ+=(
double)σ.CurrentMass;}}return υ*1000;}int ς=0;private bool ρ(bool õ=false){if(!õ)ς=0;while(ς<λ.Count){if(!Ɩ.Ȟ(4))return false;
if(λ[ς].CubeGrid!=κ){λ.RemoveAt(ς);continue;}ς++;}return true;}List<IMyBlockGroup>π=new List<IMyBlockGroup>();List<
IMyTerminalBlock>ο=new List<IMyTerminalBlock>();int ξ=0;public bool ν(string ˠ,bool õ){int μ=ˠ.IndexOf(':');string Ϙ=(μ>=1&&μ<=2?ˠ.
Substring(0,μ):"");bool ϴ=Ϙ.Contains("T");if(Ϙ!="")ˠ=ˠ.Substring(μ+1);if(ˠ==""||ˠ=="*"){if(!õ){ο.Clear();ǂ.GetBlocks(ο);λ.AddList
(ο);}if(ϴ)if(!ρ(õ))return false;return true;}string ϳ=(Ϙ.Contains("G")?ˠ.Trim():"");if(ϳ!=""){if(!õ){π.Clear();ǂ.
GetBlockGroups(π);ξ=0;}for(;ξ<π.Count;ξ++){IMyBlockGroup Ϻ=π[ξ];if(string.Compare(Ϻ.Name,ϳ,true)==0){if(!õ){ο.Clear();Ϻ.GetBlocks(ο);λ
.AddList(ο);}if(ϴ)if(!ρ(õ))return false;return true;}}return true;}if(!õ){ο.Clear();ǂ.SearchBlocksOfName(ˠ,ο);λ.AddList(ο
);}if(ϴ)if(!ρ(õ))return false;return true;}List<IMyBlockGroup>Ͽ=new List<IMyBlockGroup>();List<IMyTerminalBlock>Ͼ=new
List<IMyTerminalBlock>();int Ͻ=0;int ϼ=0;public bool ϻ(string ʓ,string ϳ,bool ϴ,bool õ){if(!õ){Ͽ.Clear();ǂ.GetBlockGroups(Ͽ)
;Ͻ=0;}for(;Ͻ<Ͽ.Count;Ͻ++){IMyBlockGroup Ϻ=Ͽ[Ͻ];if(string.Compare(Ϻ.Name,ϳ,true)==0){if(!õ){ϼ=0;Ͼ.Clear();Ϻ.GetBlocks(Ͼ);}
else õ=false;for(;ϼ<Ͼ.Count;ϼ++){if(!Ɩ.Ȟ(6))return false;if(ϴ&&Ͼ[ϼ].CubeGrid!=κ)continue;if(γ.ϣ(Ͼ[ϼ],ʓ))λ.Add(Ͼ[ϼ]);}return
true;}}return true;}List<IMyTerminalBlock>ϸ=new List<IMyTerminalBlock>();int Ϸ=0;public bool ϵ(string ʓ,string ˠ,bool õ){int
μ=ˠ.IndexOf(':');string Ϙ=(μ>=1&&μ<=2?ˠ.Substring(0,μ):"");bool ϴ=Ϙ.Contains("T");if(Ϙ!="")ˠ=ˠ.Substring(μ+1);if(!õ){ϸ.
Clear();Ϸ=0;}string ϳ=(Ϙ.Contains("G")?ˠ.Trim():"");if(ϳ!=""){if(!ϻ(ʓ,ϳ,ϴ,õ))return false;return true;}if(!õ)γ.ϲ(ref ϸ,ʓ);if(
ˠ==""||ˠ=="*"){if(!õ)λ.AddList(ϸ);if(ϴ)if(!ρ(õ))return false;return true;}for(;Ϸ<ϸ.Count;Ϸ++){if(!Ɩ.Ȟ(4))return false;if(
ϴ&&ϸ[Ϸ].CubeGrid!=κ)continue;if(ϸ[Ϸ].CustomName.Contains(ˠ))λ.Add(ϸ[Ϸ]);}return true;}public void Ϲ(δ Ѐ){λ.AddList(Ѐ.λ);}
public void ŵ(){λ.Clear();}public int Ћ(){return λ.Count;}}class Љ{ȕ Ɩ;ɯ k;public MyGridProgram ǆ{get{return Ɩ.ǆ;}}public
IMyGridTerminalSystem ǂ{get{return Ɩ.ǆ.GridTerminalSystem;}}public Љ(ȕ ƞ,ɯ Ý){Ɩ=ƞ;k=Ý;}void Ј<Ǥ>(List<IMyTerminalBlock>Њ,Func<
IMyTerminalBlock,bool>Ї=null)where Ǥ:class,IMyTerminalBlock{ǂ.GetBlocksOfType<Ǥ>(Њ,Ї);}public Dictionary<string,Action<List<
IMyTerminalBlock>,Func<IMyTerminalBlock,bool>>>І;public void Ѕ(){if(І!=null)return;І=new Dictionary<string,Action<List<IMyTerminalBlock>
,Func<IMyTerminalBlock,bool>>>(){{"CargoContainer",Ј<IMyCargoContainer>},{"TextPanel",Ј<IMyTextPanel>},{"Assembler",Ј<
IMyAssembler>},{"Refinery",Ј<IMyRefinery>},{"Reactor",Ј<IMyReactor>},{"SolarPanel",Ј<IMySolarPanel>},{"BatteryBlock",Ј<
IMyBatteryBlock>},{"Beacon",Ј<IMyBeacon>},{"RadioAntenna",Ј<IMyRadioAntenna>},{"AirVent",Ј<IMyAirVent>},{"ConveyorSorter",Ј<
IMyConveyorSorter>},{"OxygenTank",Ј<IMyGasTank>},{"OxygenGenerator",Ј<IMyGasGenerator>},{"OxygenFarm",Ј<IMyOxygenFarm>},{"LaserAntenna",Ј
<IMyLaserAntenna>},{"Thrust",Ј<IMyThrust>},{"Gyro",Ј<IMyGyro>},{"SensorBlock",Ј<IMySensorBlock>},{"ShipConnector",Ј<
IMyShipConnector>},{"ReflectorLight",Ј<IMyReflectorLight>},{"InteriorLight",Ј<IMyInteriorLight>},{"LandingGear",Ј<IMyLandingGear>},{
"ProgrammableBlock",Ј<IMyProgrammableBlock>},{"TimerBlock",Ј<IMyTimerBlock>},{"MotorStator",Ј<IMyMotorStator>},{"PistonBase",Ј<
IMyPistonBase>},{"Projector",Ј<IMyProjector>},{"ShipMergeBlock",Ј<IMyShipMergeBlock>},{"SoundBlock",Ј<IMySoundBlock>},{"Collector",Ј<
IMyCollector>},{"JumpDrive",Ј<IMyJumpDrive>},{"Door",Ј<IMyDoor>},{"GravityGeneratorSphere",Ј<IMyGravityGeneratorSphere>},{
"GravityGenerator",Ј<IMyGravityGenerator>},{"ShipDrill",Ј<IMyShipDrill>},{"ShipGrinder",Ј<IMyShipGrinder>},{"ShipWelder",Ј<IMyShipWelder>}
,{"Parachute",Ј<IMyParachute>},{"LargeGatlingTurret",Ј<IMyLargeGatlingTurret>},{"LargeInteriorTurret",Ј<
IMyLargeInteriorTurret>},{"LargeMissileTurret",Ј<IMyLargeMissileTurret>},{"SmallGatlingGun",Ј<IMySmallGatlingGun>},{
"SmallMissileLauncherReload",Ј<IMySmallMissileLauncherReload>},{"SmallMissileLauncher",Ј<IMySmallMissileLauncher>},{"VirtualMass",Ј<IMyVirtualMass>}
,{"Warhead",Ј<IMyWarhead>},{"FunctionalBlock",Ј<IMyFunctionalBlock>},{"LightingBlock",Ј<IMyLightingBlock>},{
"ControlPanel",Ј<IMyControlPanel>},{"Cockpit",Ј<IMyCockpit>},{"CryoChamber",Ј<IMyCryoChamber>},{"MedicalRoom",Ј<IMyMedicalRoom>},{
"RemoteControl",Ј<IMyRemoteControl>},{"ButtonPanel",Ј<IMyButtonPanel>},{"CameraBlock",Ј<IMyCameraBlock>},{"OreDetector",Ј<
IMyOreDetector>},{"ShipController",Ј<IMyShipController>},{"SafeZoneBlock",Ј<IMySafeZoneBlock>},{"Decoy",Ј<IMyDecoy>}};}public void Є(
ref List<IMyTerminalBlock>Ă,string Ѓ){Action<List<IMyTerminalBlock>,Func<IMyTerminalBlock,bool>>Ђ;if(Ѓ=="SurfaceProvider"){
ǂ.GetBlocksOfType<IMyTextSurfaceProvider>(Ă);return;}if(І.TryGetValue(Ѓ,out Ђ))Ђ(Ă,null);else{if(Ѓ=="WindTurbine"){ǂ.
GetBlocksOfType<IMyPowerProducer>(Ă,(Ё)=>Ё.BlockDefinition.TypeIdString.EndsWith("WindTurbine"));return;}if(Ѓ=="HydrogenEngine"){ǂ.
GetBlocksOfType<IMyPowerProducer>(Ă,(Ё)=>Ё.BlockDefinition.TypeIdString.EndsWith("HydrogenEngine"));return;}if(Ѓ=="StoreBlock"){ǂ.
GetBlocksOfType<IMyFunctionalBlock>(Ă,(Ё)=>Ё.BlockDefinition.TypeIdString.EndsWith("StoreBlock"));return;}if(Ѓ=="ContractBlock"){ǂ.
GetBlocksOfType<IMyFunctionalBlock>(Ă,(Ё)=>Ё.BlockDefinition.TypeIdString.EndsWith("ContractBlock"));return;}if(Ѓ=="VendingMachine"){ǂ.
GetBlocksOfType<IMyFunctionalBlock>(Ă,(Ё)=>Ё.BlockDefinition.TypeIdString.EndsWith("VendingMachine"));return;}}}public void ϲ(ref List<
IMyTerminalBlock>Ă,string ϙ){Є(ref Ă,ϡ(ϙ.Trim()));}public bool ϣ(IMyTerminalBlock ç,string ϙ){string Ϣ=ϡ(ϙ);switch(Ϣ){case
"FunctionalBlock":return true;case"ShipController":return(ç as IMyShipController!=null);default:return ç.BlockDefinition.TypeIdString.
Contains(ϡ(ϙ));}}public string ϡ(string Ϡ){if(Ϡ=="surfaceprovider")return"SurfaceProvider";if(Ϡ.ǰ("carg")||Ϡ.ǰ("conta"))return
"CargoContainer";if(Ϡ.ǰ("text")||Ϡ.ǰ("lcd"))return"TextPanel";if(Ϡ.ǰ("ass"))return"Assembler";if(Ϡ.ǰ("refi"))return"Refinery";if(Ϡ.ǰ(
"reac"))return"Reactor";if(Ϡ.ǰ("solar"))return"SolarPanel";if(Ϡ.ǰ("wind"))return"WindTurbine";if(Ϡ.ǰ("hydro")&&Ϡ.Contains(
"eng"))return"HydrogenEngine";if(Ϡ.ǰ("bat"))return"BatteryBlock";if(Ϡ.ǰ("bea"))return"Beacon";if(Ϡ.ǯ("vent"))return"AirVent";
if(Ϡ.ǯ("sorter"))return"ConveyorSorter";if(Ϡ.ǯ("tank"))return"OxygenTank";if(Ϡ.ǯ("farm")&&Ϡ.ǯ("oxy"))return"OxygenFarm";if
(Ϡ.ǯ("gene")&&Ϡ.ǯ("oxy"))return"OxygenGenerator";if(Ϡ.ǯ("cryo"))return"CryoChamber";if(string.Compare(Ϡ,"laserantenna",
true)==0)return"LaserAntenna";if(Ϡ.ǯ("antenna"))return"RadioAntenna";if(Ϡ.ǰ("thrust"))return"Thrust";if(Ϡ.ǰ("gyro"))return
"Gyro";if(Ϡ.ǰ("sensor"))return"SensorBlock";if(Ϡ.ǯ("connector"))return"ShipConnector";if(Ϡ.ǰ("reflector")||Ϡ.ǰ("spotlight"))
return"ReflectorLight";if((Ϡ.ǰ("inter")&&Ϡ.ǭ("light")))return"InteriorLight";if(Ϡ.ǰ("land"))return"LandingGear";if(Ϡ.ǰ(
"program"))return"ProgrammableBlock";if(Ϡ.ǰ("timer"))return"TimerBlock";if(Ϡ.ǰ("motor")||Ϡ.ǰ("rotor"))return"MotorStator";if(Ϡ.ǰ(
"piston"))return"PistonBase";if(Ϡ.ǰ("proj"))return"Projector";if(Ϡ.ǯ("merge"))return"ShipMergeBlock";if(Ϡ.ǰ("sound"))return
"SoundBlock";if(Ϡ.ǰ("col"))return"Collector";if(Ϡ.ǯ("jump"))return"JumpDrive";if(string.Compare(Ϡ,"door",true)==0)return"Door";if((Ϡ
.ǯ("grav")&&Ϡ.ǯ("sphe")))return"GravityGeneratorSphere";if(Ϡ.ǯ("grav"))return"GravityGenerator";if(Ϡ.ǭ("drill"))return
"ShipDrill";if(Ϡ.ǯ("grind"))return"ShipGrinder";if(Ϡ.ǭ("welder"))return"ShipWelder";if(Ϡ.ǰ("parach"))return"Parachute";if((Ϡ.ǯ(
"turret")&&Ϡ.ǯ("gatl")))return"LargeGatlingTurret";if((Ϡ.ǯ("turret")&&Ϡ.ǯ("inter")))return"LargeInteriorTurret";if((Ϡ.ǯ("turret"
)&&Ϡ.ǯ("miss")))return"LargeMissileTurret";if(Ϡ.ǯ("gatl"))return"SmallGatlingGun";if((Ϡ.ǯ("launcher")&&Ϡ.ǯ("reload")))
return"SmallMissileLauncherReload";if((Ϡ.ǯ("launcher")))return"SmallMissileLauncher";if(Ϡ.ǯ("mass"))return"VirtualMass";if(
string.Compare(Ϡ,"warhead",true)==0)return"Warhead";if(Ϡ.ǰ("func"))return"FunctionalBlock";if(string.Compare(Ϡ,"shipctrl",true
)==0)return"ShipController";if(Ϡ.ǰ("light"))return"LightingBlock";if(Ϡ.ǰ("contr"))return"ControlPanel";if(Ϡ.ǰ("coc"))
return"Cockpit";if(Ϡ.ǰ("medi"))return"MedicalRoom";if(Ϡ.ǰ("remote"))return"RemoteControl";if(Ϡ.ǰ("but"))return"ButtonPanel";if
(Ϡ.ǰ("cam"))return"CameraBlock";if(Ϡ.ǯ("detect"))return"OreDetector";if(Ϡ.ǰ("safe"))return"SafeZoneBlock";if(Ϡ.ǰ("store")
)return"StoreBlock";if(Ϡ.ǰ("contract"))return"ContractBlock";if(Ϡ.ǰ("vending"))return"VendingMachine";if(Ϡ.ǰ("decoy"))
return"Decoy";return"Unknown";}public string ϟ(IMyBatteryBlock ŏ){string Ϟ="";if(ŏ.ChargeMode==ChargeMode.Recharge)Ϟ="(+) ";
else if(ŏ.ChargeMode==ChargeMode.Discharge)Ϟ="(-) ";else Ϟ="(±) ";return Ϟ+k.Ȁ((ŏ.CurrentStoredPower/ŏ.MaxStoredPower)*
100.0f)+"%";}Dictionary<MyLaserAntennaStatus,string>ϝ=new Dictionary<MyLaserAntennaStatus,string>(){{MyLaserAntennaStatus.Idle
,"IDLE"},{MyLaserAntennaStatus.Connecting,"CONECTANDO"},{MyLaserAntennaStatus.Connected,"CONECTADA"},{
MyLaserAntennaStatus.OutOfRange,"FORA DE ALCANCE"},{MyLaserAntennaStatus.RotatingToTarget,"ROTACIONANDO"},{MyLaserAntennaStatus.
SearchingTargetForAntenna,"PROCURANDO"}};public string Ϥ(IMyLaserAntenna ō){return ϝ[ō.Status];}public double Ϝ(IMyJumpDrive Ŏ,out double ʙ,out
double Ÿ){ʙ=Ŏ.CurrentStoredPower;Ÿ=Ŏ.MaxStoredPower;return(Ÿ>0?ʙ/Ÿ*100:0);}public double ϛ(IMyJumpDrive Ŏ){double ʙ=Ŏ.
CurrentStoredPower;double Ÿ=Ŏ.MaxStoredPower;return(Ÿ>0?ʙ/Ÿ*100:0);}}class Ϛ:ɕ{ɯ k;Ģ Ĝ;public int Ϧ=0;public Ϛ(ɯ Ý,Ģ f){ɔ="BootPanelsTask"
;ɐ=1;k=Ý;Ĝ=f;if(!k.ɧ){Ϧ=int.MaxValue;Ĝ.Ć=true;}}Ǖ Đ;public override void ɇ(){Đ=k.Đ;}public override bool Ɏ(bool õ){if(Ϧ>k
.ɦ.Count){Ɉ();return true;}if(!õ&&Ϧ==0){Ĝ.Ć=false;}if(!ϯ(õ))return false;Ϧ++;return true;}public override void ɣ(){Ĝ.Ć=
true;}public void ϱ(){ƃ é=Ĝ.é;for(int D=0;D<é.µ();D++){ë j=é.w(D);j.F();}Ϧ=(k.ɧ?0:int.MaxValue);}int D;ş ϰ=null;public bool
ϯ(bool õ){ƃ é=Ĝ.é;if(!õ)D=0;int Ϯ=0;for(;D<é.µ();D++){if(!Ɩ.Ȟ(40)||Ϯ>5)return false;ë j=é.w(D);ϰ=k.Ǔ(ϰ,j);float?ϭ=j.ê?.
FontSize;if(ϭ!=null&&ϭ>3f)continue;if(ϰ.Ř.Count<=0)ϰ.Ũ(k.Ǐ(null,j));else k.Ǐ(ϰ.Ř[0],j);k.ý();k.Ʀ(Đ.Ǥ("B1"));double ʕ=(double)Ϧ/k
.ɦ.Count*100;k.Ʋ(ʕ);if(Ϧ==k.ɦ.Count){k.Ǒ("");k.Ʀ("Automatic LCDs 2");k.Ʀ("por MMaster"); k.Ʀ("Traduzido Por Arthur_Gamer_HD");}else k.ǐ(k.ɦ[Ϧ]);bool á=j.á;j.á=
false;k.Ǵ(j,ϰ);j.á=á;Ϯ++;}return true;}public bool Ϭ(){return Ϧ<=k.ɦ.Count;}}public enum ϫ{Ϫ=0,ϩ=1,Ϩ=2,ϧ=3,β=4,ʾ=5,ʝ=6,ʼ=7,ʻ=
8,ʺ=9,ʹ=10,ʸ=11,ʷ=12,ʶ=13,ʵ=14,ʴ=15,ʳ=16,ʽ=17,ʲ=18,ʰ=19,ʯ=20,ʮ=21,ʭ=22,ʬ=23,ʫ=24,ʪ=25,ʩ=26,ʨ=27,ʧ=28,ʱ=29,ʿ=30,ˋ=31,}
class ˣ{ȕ Ɩ;public string ˡ="";public string ˠ="";public string ˑ="";public string ː="";public ϫ ˏ=ϫ.Ϫ;public ˣ(ȕ ƞ){Ɩ=ƞ;}ϫ ˎ
(){if(ˡ=="echo"||ˡ=="center"||ˡ=="right")return ϫ.ϩ;if(ˡ.StartsWith("hscroll"))return ϫ.ʿ;if(ˡ.StartsWith("inventory")||ˡ
.StartsWith("missing")||ˡ.StartsWith("invlist"))return ϫ.Ϩ;if(ˡ.StartsWith("working"))return ϫ.ʲ;if(ˡ.StartsWith("cargo")
)return ϫ.ϧ;if(ˡ.StartsWith("mass"))return ϫ.β;if(ˡ.StartsWith("shipmass"))return ϫ.ʬ;if(ˡ=="oxygen")return ϫ.ʾ;if(ˡ.
StartsWith("tanks"))return ϫ.ʝ;if(ˡ.StartsWith("powertime"))return ϫ.ʼ;if(ˡ.StartsWith("powerused"))return ϫ.ʻ;if(ˡ.StartsWith(
"power"))return ϫ.ʺ;if(ˡ.StartsWith("speed"))return ϫ.ʹ;if(ˡ.StartsWith("accel"))return ϫ.ʸ;if(ˡ.StartsWith("alti"))return ϫ.ʪ;
if(ˡ.StartsWith("charge"))return ϫ.ʷ;if(ˡ.StartsWith("docked"))return ϫ.ˋ;if(ˡ.StartsWith("time")||ˡ.StartsWith("date"))
return ϫ.ʶ;if(ˡ.StartsWith("countdown"))return ϫ.ʵ;if(ˡ.StartsWith("textlcd"))return ϫ.ʴ;if(ˡ.EndsWith("count"))return ϫ.ʳ;if(
ˡ.StartsWith("dampeners")||ˡ.StartsWith("occupied"))return ϫ.ʽ;if(ˡ.StartsWith("damage"))return ϫ.ʰ;if(ˡ.StartsWith(
"amount"))return ϫ.ʯ;if(ˡ.StartsWith("pos"))return ϫ.ʮ;if(ˡ.StartsWith("distance"))return ϫ.ʫ;if(ˡ.StartsWith("details"))return
ϫ.ʭ;if(ˡ.StartsWith("stop"))return ϫ.ʩ;if(ˡ.StartsWith("gravity"))return ϫ.ʨ;if(ˡ.StartsWith("customdata"))return ϫ.ʧ;if(
ˡ.StartsWith("prop"))return ϫ.ʱ;return ϫ.Ϫ;}public Ɲ ˍ(){switch(ˏ){case ϫ.ϩ:return new Ҕ();case ϫ.Ϩ:return new ң();case ϫ
.ϧ:return new ʦ();case ϫ.β:return new Ӂ();case ϫ.ʾ:return new ҭ();case ϫ.ʝ:return new э();case ϫ.ʼ:return new ч();case ϫ.
ʻ:return new Т();case ϫ.ʺ:return new ҳ();case ϫ.ʹ:return new ѓ();case ϫ.ʸ:return new ʗ();case ϫ.ʷ:return new Ξ();case ϫ.ʶ
:return new Ͱ();case ϫ.ʵ:return new Ϊ();case ϫ.ʴ:return new ĺ();case ϫ.ʳ:return new ʒ();case ϫ.ʽ:return new Ѫ();case ϫ.ʲ:
return new ĸ();case ϫ.ʰ:return new ͻ();case ϫ.ʯ:return new ҽ();case ϫ.ʮ:return new ҩ();case ϫ.ʭ:return new Γ();case ϫ.ʬ:return
new Ѩ();case ϫ.ʫ:return new Ή();case ϫ.ʪ:return new ʔ();case ϫ.ʩ:return new ѐ();case ϫ.ʨ:return new ғ();case ϫ.ʧ:return new
Υ();case ϫ.ʱ:return new ў();case ϫ.ʿ:return new ҧ();case ϫ.ˋ:return new Ҙ();default:return new Ɲ();}}public List<ˆ>ˢ=new
List<ˆ>();string[]ˌ=null;string ˊ="";bool ˉ=false;int Ķ=1;public bool ʚ(string ˈ,bool õ){if(!õ){ˏ=ϫ.Ϫ;ˠ="";ˡ="";ˑ=ˈ.
TrimStart(' ');ˢ.Clear();if(ˑ=="")return true;int ˇ=ˑ.IndexOf(' ');if(ˇ<0||ˇ>=ˑ.Length-1)ː="";else ː=ˑ.Substring(ˇ+1);ˌ=ˑ.Split(
' ');ˊ="";ˉ=false;ˡ=ˌ[0].ToLower();Ķ=1;}for(;Ķ<ˌ.Length;Ķ++){if(!Ɩ.Ȟ(40))return false;string Ĵ=ˌ[Ķ];if(Ĵ=="")continue;if(Ĵ[
0]=='{'&&Ĵ[Ĵ.Length-1]=='}'){Ĵ=Ĵ.Substring(1,Ĵ.Length-2);if(Ĵ=="")continue;if(ˠ=="")ˠ=Ĵ;else ˢ.Add(new ˆ(Ĵ));continue;}if
(Ĵ[0]=='{'){ˉ=true;ˊ=Ĵ.Substring(1);continue;}if(Ĵ[Ĵ.Length-1]=='}'){ˉ=false;ˊ+=' '+Ĵ.Substring(0,Ĵ.Length-1);if(ˠ=="")ˠ=
ˊ;else ˢ.Add(new ˆ(ˊ));continue;}if(ˉ){if(ˊ.Length!=0)ˊ+=' ';ˊ+=Ĵ;continue;}if(ˠ=="")ˠ=Ĵ;else ˢ.Add(new ˆ(Ĵ));}ˏ=ˎ();
return true;}}class ˆ{public string ˁ="";public string ˀ="";public string Ĵ="";public List<string>ʜ=new List<string>();public
ˆ(string ʛ){Ĵ=ʛ;}public void ʚ(){if(Ĵ==""||ˁ!=""||ˀ!=""||ʜ.Count>0)return;string ʙ=Ĵ.Trim();if(ʙ[0]=='+'||ʙ[0]=='-'){ˁ+=ʙ
[0];ʙ=Ĵ.Substring(1);}string[]Ƣ=ʙ.Split('/');string ʘ=Ƣ[0];if(Ƣ.Length>1){ˀ=Ƣ[0];ʘ=Ƣ[1];}else ˀ="";if(ʘ.Length>0){string[
]Ĉ=ʘ.Split(',');for(int D=0;D<Ĉ.Length;D++)if(Ĉ[D]!="")ʜ.Add(Ĉ[D]);}}}class ʗ:Ɲ{public ʗ(){ɐ=0.5;ɔ="CmdAccel";}public
override bool ƙ(bool õ){double ʖ=0;if(Ɯ.ˠ!="")double.TryParse(Ɯ.ˠ.Trim(),out ʖ);k.Ƅ(Đ.Ǥ("AC1")+" ");k.Ʒ(k.Ǆ.ʀ.ToString("F1")+
" m/s²");if(ʖ>0){double ʕ=k.Ǆ.ʀ/ʖ*100;k.Ʋ(ʕ);}return true;}}class ʔ:Ɲ{public ʔ(){ɐ=1;ɔ="CmdAltitude";}public override bool ƙ(
bool õ){string ʓ=(Ɯ.ˡ.EndsWith("sea")?"sea":"ground");switch(ʓ){case"sea":k.Ƅ(Đ.Ǥ("ALT1"));k.Ʒ(k.Ǆ.ɶ.ToString("F0")+" m");
break;default:k.Ƅ(Đ.Ǥ("ALT2"));k.Ʒ(k.Ǆ.ɴ.ToString("F0")+" m");break;}return true;}}class ʒ:Ɲ{public ʒ(){ɐ=15;ɔ=
"CmdBlockCount";}δ ķ;public override void ɇ(){ķ=new δ(Ɩ,k.œ);}bool ʑ;bool ʐ;int Ķ=0;int ĝ=0;public override bool ƙ(bool õ){if(!õ){ʑ=(Ɯ.
ˡ=="enabledcount");ʐ=(Ɯ.ˡ=="prodcount");Ķ=0;ĝ=0;}if(Ɯ.ˢ.Count==0){if(ĝ==0){if(!õ)ķ.ŵ();if(!ķ.ν(Ɯ.ˠ,õ))return false;ĝ++;õ=
false;}if(!ʟ(ķ,"blocks",ʑ,ʐ,õ))return false;return true;}for(;Ķ<Ɯ.ˢ.Count;Ķ++){ˆ Ĵ=Ɯ.ˢ[Ķ];if(!õ)Ĵ.ʚ();if(!ł(Ĵ,õ))return false
;õ=false;}return true;}int Ĳ=0;int ĳ=0;bool ł(ˆ Ĵ,bool õ){if(!õ){Ĳ=0;ĳ=0;}for(;Ĳ<Ĵ.ʜ.Count;Ĳ++){if(ĳ==0){if(!õ)ķ.ŵ();if(!
ķ.ϵ(Ĵ.ʜ[Ĳ],Ɯ.ˠ,õ))return false;ĳ++;õ=false;}if(!ʟ(ķ,Ĵ.ʜ[Ĳ],ʑ,ʐ,õ))return false;ĳ=0;õ=false;}return true;}Dictionary<
string,int>ʤ=new Dictionary<string,int>();Dictionary<string,int>ʣ=new Dictionary<string,int>();List<string>ʢ=new List<string>(
);int í=0;int ʡ=0;int ʥ=0;ʌ ʠ=new ʌ();bool ʟ(δ Ă,string ʓ,bool ʑ,bool ʐ,bool õ){if(Ă.Ћ()==0){ʠ.ŵ().ʈ(char.ToUpper(ʓ[0])).
ʈ(ʓ.ToLower(),1,ʓ.Length-1);k.Ƅ(ʠ.ʈ(" ").ʈ(Đ.Ǥ("C1")).ʈ(" "));string ʞ=(ʑ||ʐ?"0 / 0":"0");k.Ʒ(ʞ);return true;}if(!õ){ʤ.
Clear();ʣ.Clear();ʢ.Clear();í=0;ʡ=0;ʥ=0;}if(ʥ==0){for(;í<Ă.Ћ();í++){if(!Ɩ.Ȟ(15))return false;IMyProductionBlock Œ=Ă.λ[í]as
IMyProductionBlock;ʠ.ŵ().ʈ(Ă.λ[í].DefinitionDisplayNameText);string Ǻ=ʠ.Ɇ();if(ʢ.Contains(Ǻ)){ʤ[Ǻ]++;if((ʑ&&Ă.λ[í].IsWorking)||(ʐ&&Œ!=null
&&Œ.IsProducing))ʣ[Ǻ]++;}else{ʤ.Add(Ǻ,1);ʢ.Add(Ǻ);if(ʑ||ʐ)if((ʑ&&Ă.λ[í].IsWorking)||(ʐ&&Œ!=null&&Œ.IsProducing))ʣ.Add(Ǻ,1)
;else ʣ.Add(Ǻ,0);}}ʥ++;õ=false;}for(;ʡ<ʤ.Count;ʡ++){if(!Ɩ.Ȟ(8))return false;k.Ƅ(ʢ[ʡ]+" "+Đ.Ǥ("C1")+" ");string ʞ=(ʑ||ʐ?ʣ[
ʢ[ʡ]]+" / ":"")+ʤ[ʢ[ʡ]];k.Ʒ(ʞ);}return true;}}class ʦ:Ɲ{δ ķ;public ʦ(){ɐ=2;ɔ="CmdCargo";}public override void ɇ(){ķ=new δ
(Ɩ,k.œ);}bool Τ=true;bool ͺ=false;bool Σ=false;bool Λ=false;double Ρ=0;double Π=0;int ĝ=0;public override bool ƙ(bool õ){
if(!õ){ķ.ŵ();Τ=Ɯ.ˡ.Contains("all");Λ=Ɯ.ˡ.EndsWith("bar");ͺ=(Ɯ.ˡ[Ɯ.ˡ.Length-1]=='x');Σ=(Ɯ.ˡ[Ɯ.ˡ.Length-1]=='p');Ρ=0;Π=0;ĝ=0
;}if(ĝ==0){if(Τ){if(!ķ.ν(Ɯ.ˠ,õ))return false;}else{if(!ķ.ϵ("cargocontainer",Ɯ.ˠ,õ))return false;}ĝ++;õ=false;}double Ο=ķ.
ω(ref Ρ,ref Π,õ);if(Double.IsNaN(Ο))return false;if(Λ){k.Ʋ(Ο);return true;}k.Ƅ(Đ.Ǥ("C2")+" ");if(!ͺ&&!Σ){k.Ʒ(k.ȉ(Ρ)+
"L / "+k.ȉ(Π)+"L");k.Ƹ(Ο,1.0f,k.ə);k.Ǒ(' '+k.Ȁ(Ο)+"%");}else if(Σ){k.Ʒ(k.Ȁ(Ο)+"%");k.Ʋ(Ο);}else k.Ʒ(k.Ȁ(Ο)+"%");return true;}}
class Ξ:Ɲ{public Ξ(){ɐ=3;ɔ="CmdCharge";}δ ķ;public override void ɇ(){ķ=new δ(Ɩ,k.œ);}int ĝ=0;int í=0;bool ͺ=false;bool Μ=
false;bool Λ=false;Dictionary<long,double>b=new Dictionary<long,double>();Dictionary<long,double>Κ=new Dictionary<long,double
>();Dictionary<long,double>Ι=new Dictionary<long,double>();Dictionary<long,double>Θ=new Dictionary<long,double>();
Dictionary<long,double>Η=new Dictionary<long,double>();double Ζ(long Ν,double ʙ,double Ÿ){double α=0;double ί=0;double ή=0;double
έ=0;if(Κ.TryGetValue(Ν,out ή)){έ=Θ[Ν];}if(b.TryGetValue(Ν,out α)){ί=Ι[Ν];}double ά=(Ɩ.ȑ-ή);double Ϋ=0;if(ά>0)Ϋ=(ʙ-έ)/ά;if
(Ϋ<0){if(!Η.TryGetValue(Ν,out Ϋ))Ϋ=0;}else Η[Ν]=Ϋ;if(α>0){Κ[Ν]=b[Ν];Θ[Ν]=Ι[Ν];}b[Ν]=Ɩ.ȑ;Ι[Ν]=ʙ;return(Ϋ>0?(Ÿ-ʙ)/Ϋ:0);}
public override bool ƙ(bool õ){if(!õ){ķ.ŵ();Λ=Ɯ.ˡ.EndsWith("bar");ͺ=Ɯ.ˡ.Contains("x");Μ=Ɯ.ˡ.Contains("time");í=0;ĝ=0;}if(ĝ==0)
{if(!ķ.ϵ("jumpdrive",Ɯ.ˠ,õ))return false;if(ķ.Ћ()<=0){k.Ǒ("Charge: "+Đ.Ǥ("D2"));return true;}ĝ++;õ=false;}for(;í<ķ.Ћ();í
++){if(!Ɩ.Ȟ(25))return false;IMyJumpDrive Ŏ=ķ.λ[í]as IMyJumpDrive;double ʙ,Ÿ,ʕ;ʕ=k.œ.Ϝ(Ŏ,out ʙ,out Ÿ);if(Λ){k.Ʋ(ʕ);}else{k
.Ƅ(Ŏ.CustomName+" ");if(Μ){TimeSpan ΰ=TimeSpan.FromSeconds(Ζ(Ŏ.EntityId,ʙ,Ÿ));k.Ʒ(k.ǅ.ȗ(ΰ));if(!ͺ){k.Ƹ(ʕ,1.0f,k.ə);k.Ʒ(
' '+ʕ.ToString("0.0")+"%");}}else{if(!ͺ){k.Ʒ(k.ȉ(ʙ)+"Wh / "+k.ȉ(Ÿ)+"Wh");k.Ƹ(ʕ,1.0f,k.ə);}k.Ʒ(' '+ʕ.ToString("0.0")+"%");}}
}return true;}}class Ϊ:Ɲ{public Ϊ(){ɐ=1;ɔ="CmdCountdown";}public override bool ƙ(bool õ){bool ͽ=Ɯ.ˡ.EndsWith("c");bool Ω=
Ɯ.ˡ.EndsWith("r");string Ψ="";int Δ=Ɯ.ˑ.IndexOf(' ');if(Δ>=0)Ψ=Ɯ.ˑ.Substring(Δ+1).Trim();DateTime Χ=DateTime.Now;DateTime
Φ;if(!DateTime.TryParseExact(Ψ,"H:mm d.M.yyyy",System.Globalization.CultureInfo.InvariantCulture,System.Globalization.
DateTimeStyles.None,out Φ)){k.Ǒ(Đ.Ǥ("C3"));k.Ǒ("  Countdown 19:02 28.2.2015");return true;}TimeSpan Ε=Φ-Χ;string Ĺ="";if(Ε.Ticks<=0)Ĺ=
Đ.Ǥ("C4");else{if((int)Ε.TotalDays>0)Ĺ+=(int)Ε.TotalDays+" "+Đ.Ǥ("C5")+" ";if(Ε.Hours>0||Ĺ!="")Ĺ+=Ε.Hours+"h ";if(Ε.
Minutes>0||Ĺ!="")Ĺ+=Ε.Minutes+"m ";Ĺ+=Ε.Seconds+"s";}if(ͽ)k.Ʀ(Ĺ);else if(Ω)k.Ʒ(Ĺ);else k.Ǒ(Ĺ);return true;}}class Υ:Ɲ{public Υ(
){ɐ=1;ɔ="CmdCustomData";}public override bool ƙ(bool õ){string Ĺ="";if(Ɯ.ˠ!=""&&Ɯ.ˠ!="*"){IMyTerminalBlock ͼ=k.ǂ.
GetBlockWithName(Ɯ.ˠ)as IMyTerminalBlock;if(ͼ==null){k.Ǒ("CustomData: "+Đ.Ǥ("CD1")+Ɯ.ˠ);return true;}Ĺ=ͼ.CustomData;}else{k.Ǒ(
"CustomData:"+Đ.Ǥ("CD2"));return true;}if(Ĺ.Length==0)return true;k.ǐ(Ĺ);return true;}}class ͻ:Ɲ{public ͻ(){ɐ=5;ɔ="CmdDamage";}δ ķ;
public override void ɇ(){ķ=new δ(Ɩ,k.œ);}bool Ơ=false;int í=0;public override bool ƙ(bool õ){bool ͺ=Ɯ.ˡ.StartsWith("damagex");
bool ͷ=Ɯ.ˡ.EndsWith("noc");bool Ͷ=(!ͷ&&Ɯ.ˡ.EndsWith("c"));float ʹ=100;if(!õ){ķ.ŵ();Ơ=false;í=0;}if(!ķ.ν(Ɯ.ˠ,õ))return false;
if(Ɯ.ˢ.Count>0){if(!float.TryParse(Ɯ.ˢ[0].Ĵ,out ʹ))ʹ=100;}ʹ-=0.00001f;for(;í<ķ.Ћ();í++){if(!Ɩ.Ȟ(30))return false;
IMyTerminalBlock ç=ķ.λ[í];IMySlimBlock ͳ=ç.CubeGrid.GetCubeBlock(ç.Position);if(ͳ==null)continue;float Ͳ=(ͷ?ͳ.MaxIntegrity:ͳ.
BuildIntegrity);if(!Ͷ)Ͳ-=ͳ.CurrentDamage;float ʕ=100*(Ͳ/ͳ.MaxIntegrity);if(ʕ>=ʹ)continue;Ơ=true;string ͱ=k.ǟ(ͳ.FatBlock.
DisplayNameText,k.ɠ*0.69f-k.ə);k.Ƅ(ͱ+' ');if(!ͺ){k.ƭ(k.ȉ(Ͳ)+" / ",0.69f);k.Ƅ(k.ȉ(ͳ.MaxIntegrity));}k.Ʒ(' '+ʕ.ToString("0.0")+'%');k.Ʋ(ʕ
);}if(!Ơ)k.Ǒ(Đ.Ǥ("D3"));return true;}}class Ͱ:Ɲ{public Ͱ(){ɐ=1;ɔ="CmdDateTime";}public override bool ƙ(bool õ){bool ˮ=(Ɯ.
ˡ.StartsWith("datetime"));bool ˬ=(Ɯ.ˡ.StartsWith("date"));bool ͽ=Ɯ.ˡ.Contains("c");int ˤ=Ɯ.ˡ.IndexOf('+');if(ˤ<0)ˤ=Ɯ.ˡ.
IndexOf('-');float Ά=0;if(ˤ>=0)float.TryParse(Ɯ.ˡ.Substring(ˤ),out Ά);DateTime Ε=DateTime.Now.AddHours(Ά);string Ĺ="";int Δ=Ɯ.ˑ
.IndexOf(' ');if(Δ>=0)Ĺ=Ɯ.ˑ.Substring(Δ+1);if(!ˮ){if(!ˬ)Ĺ+=Ε.ToShortTimeString();else Ĺ+=Ε.ToShortDateString();}else{if(Ĺ
=="")Ĺ=String.Format("{0:d} {0:t}",Ε);else{Ĺ=Ĺ.Replace("/","\\/");Ĺ=Ĺ.Replace(":","\\:");Ĺ=Ĺ.Replace("\"","\\\"");Ĺ=Ĺ.
Replace("'","\\'");Ĺ=Ε.ToString(Ĺ+' ');Ĺ=Ĺ.Substring(0,Ĺ.Length-1);}}if(ͽ)k.Ʀ(Ĺ);else k.Ǒ(Ĺ);return true;}}class Γ:Ɲ{public Γ()
{ɐ=5;ɔ="CmdDetails";}string Β="";δ ķ;public override void ɇ(){ķ=new δ(Ɩ,k.œ);if(Ɯ.ˢ.Count>0)Β=Ɯ.ˢ[0].Ĵ.Trim();}int ĝ=0;
int í=1;bool Α=false;IMyTerminalBlock ç;public override bool ƙ(bool õ){if(Ɯ.ˠ==""||Ɯ.ˠ=="*"){k.Ǒ("Details: "+Đ.Ǥ("D1"));
return true;}if(!õ){ķ.ŵ();Α=Ɯ.ˡ.Contains("non");ĝ=0;í=1;}if(ĝ==0){if(!ķ.ν(Ɯ.ˠ,õ))return true;if(ķ.Ћ()<=0){k.Ǒ("Details: "+Đ.Ǥ(
"D2"));return true;}ĝ++;õ=false;}int ΐ=(Ɯ.ˡ.EndsWith("x")?1:0);if(ĝ==1){if(!õ){ç=ķ.λ[0];if(!Α)k.Ǒ(ç.CustomName);}if(!Ό(ç,ΐ,õ
))return false;ĝ++;õ=false;}for(;í<ķ.Ћ();í++){if(!õ){ç=ķ.λ[í];if(!Α){k.Ǒ("");k.Ǒ(ç.CustomName);}}if(!Ό(ç,ΐ,õ))return
false;õ=false;}return true;}string[]ǌ;int Ώ=0;bool Ύ=false;ʌ Ư=new ʌ();bool Ό(IMyTerminalBlock ç,int Ί,bool õ){if(!õ){ǌ=Ư.ŵ()
.ʈ(ç.DetailedInfo).ʈ('\n').ʈ(ç.CustomInfo).Ɇ().Split('\n');Ώ=Ί;Ύ=(Β.Length==0);}for(;Ώ<ǌ.Length;Ώ++){if(!Ɩ.Ȟ(5))return
false;if(ǌ[Ώ].Length==0)continue;if(!Ύ){if(!ǌ[Ώ].Contains(Β))continue;Ύ=true;}k.Ǒ(Ư.ŵ().ʈ("  ").ʈ(ǌ[Ώ]));}return true;}}class
Ή:Ɲ{public Ή(){ɐ=1;ɔ="CmdDistance";}string ϥ="";string[]Έ;Vector3D Ќ;string ҝ="";bool Ҝ=false;public override void ɇ(){Ҝ=
false;if(Ɯ.ˢ.Count<=0)return;ϥ=Ɯ.ˢ[0].Ĵ.Trim();Έ=ϥ.Split(':');if(Έ.Length<5||Έ[0]!="GPS")return;double қ,Қ,ҙ;if(!double.
TryParse(Έ[2],out қ))return;if(!double.TryParse(Έ[3],out Қ))return;if(!double.TryParse(Έ[4],out ҙ))return;Ќ=new Vector3D(қ,Қ,ҙ);
ҝ=Έ[1];Ҝ=true;}public override bool ƙ(bool õ){if(!Ҝ){k.Ǒ("Distance: "+Đ.Ǥ("DTU")+" '"+ϥ+"'.");return true;}
IMyTerminalBlock ç=f.C.ç;if(Ɯ.ˠ!=""&&Ɯ.ˠ!="*"){ç=k.ǂ.GetBlockWithName(Ɯ.ˠ);if(ç==null){k.Ǒ("Distance: "+Đ.Ǥ("P1")+": "+Ɯ.ˠ);return true;
}}double я=Vector3D.Distance(ç.GetPosition(),Ќ);k.Ƅ(ҝ+": ");k.Ʒ(k.ȉ(я)+"m ");return true;}}class Ҙ:Ɲ{δ ķ;public Ҙ(){ɐ=2;ɔ
="CmdDocked";}public override void ɇ(){ķ=new δ(Ɩ,k.œ);}int ĝ=0;int җ=0;bool Җ=false;bool ҕ=false;IMyShipConnector Ō;
public override bool ƙ(bool õ){if(!õ){if(Ɯ.ˡ.EndsWith("e"))Җ=true;if(Ɯ.ˡ.Contains("cn"))ҕ=true;ķ.ŵ();ĝ=0;}if(ĝ==0){if(!ķ.ϵ(
"connector",Ɯ.ˠ,õ))return false;ĝ++;җ=0;õ=false;}if(ķ.Ћ()<=0){k.Ǒ("Docked: "+Đ.Ǥ("DO1"));return true;}for(;җ<ķ.Ћ();җ++){Ō=ķ.λ[җ]as
IMyShipConnector;if(Ō.Status==MyShipConnectorStatus.Connected){if(ҕ){k.Ƅ(Ō.CustomName+":");k.Ʒ(Ō.OtherConnector.CubeGrid.CustomName);}
else{k.Ǒ(Ō.OtherConnector.CubeGrid.CustomName);}}else{if(Җ){if(ҕ){k.Ƅ(Ō.CustomName+":");k.Ʒ("-");}else k.Ǒ("-");}}}return
true;}}class Ҕ:Ɲ{public Ҕ(){ɐ=30;ɔ="CmdEcho";}public override bool ƙ(bool õ){string ʓ=(Ɯ.ˡ=="center"?"c":(Ɯ.ˡ=="right"?"r":
"n"));switch(ʓ){case"c":k.Ʀ(Ɯ.ː);break;case"r":k.Ʒ(Ɯ.ː);break;default:k.Ǒ(Ɯ.ː);break;}return true;}}class ғ:Ɲ{public ғ(){ɐ=
1;ɔ="CmdGravity";}public override bool ƙ(bool õ){string ʓ=(Ɯ.ˡ.Contains("nat")?"n":(Ɯ.ˡ.Contains("art")?"a":(Ɯ.ˡ.Contains
("tot")?"t":"s")));Vector3D Ϻ;if(k.Ǆ.ɱ==null){k.Ǒ("Gravity: "+Đ.Ǥ("GNC"));return true;}switch(ʓ){case"n":k.Ƅ(Đ.Ǥ("G2")+
" ");Ϻ=k.Ǆ.ɱ.GetNaturalGravity();k.Ʒ(Ϻ.Length().ToString("F1")+" m/s²");break;case"a":k.Ƅ(Đ.Ǥ("G3")+" ");Ϻ=k.Ǆ.ɱ.
GetArtificialGravity();k.Ʒ(Ϻ.Length().ToString("F1")+" m/s²");break;case"t":k.Ƅ(Đ.Ǥ("G1")+" ");Ϻ=k.Ǆ.ɱ.GetTotalGravity();k.Ʒ(Ϻ.Length().
ToString("F1")+" m/s²");break;default:k.Ƅ(Đ.Ǥ("GN"));k.ƭ(" | ",0.33f);k.ƭ(Đ.Ǥ("GA")+" | ",0.66f);k.Ʒ(Đ.Ǥ("GT"),1.0f);k.Ƅ("");Ϻ=k
.Ǆ.ɱ.GetNaturalGravity();k.ƭ(Ϻ.Length().ToString("F1")+" | ",0.33f);Ϻ=k.Ǆ.ɱ.GetArtificialGravity();k.ƭ(Ϻ.Length().
ToString("F1")+" | ",0.66f);Ϻ=k.Ǆ.ɱ.GetTotalGravity();k.Ʒ(Ϻ.Length().ToString("F1")+" ");break;}return true;}}class ҧ:Ɲ{public ҧ
(){ɐ=0.5;ɔ="CmdHScroll";}ʌ Ҧ=new ʌ();int ҥ=1;public override bool ƙ(bool õ){if(Ҧ.ʊ==0){string Ĺ=Ɯ.ː+"  ";if(Ĺ.Length==0)
return true;float Ҥ=k.ɠ;float ƪ=k.ǡ(Ĺ,k.Ƽ);float ђ=Ҥ/ƪ;if(ђ>1)Ҧ.ʈ(string.Join("",Enumerable.Repeat(Ĺ,(int)Math.Ceiling(ђ))));
else Ҧ.ʈ(Ĺ);if(Ĺ.Length>40)ҥ=3;else if(Ĺ.Length>5)ҥ=2;else ҥ=1;k.Ǒ(Ҧ);return true;}bool Ω=Ɯ.ˡ.EndsWith("r");if(Ω){Ҧ.Ư.Insert
(0,Ҧ.Ɇ(Ҧ.ʊ-ҥ,ҥ));Ҧ.ʇ(Ҧ.ʊ-ҥ,ҥ);}else{Ҧ.ʈ(Ҧ.Ɇ(0,ҥ));Ҧ.ʇ(0,ҥ);}k.Ǒ(Ҧ);return true;}}class ң:Ɲ{public ң(){ɐ=7;ɔ="CmdInvList";
}float Ң=-1;float ҡ=-1;public override void ɇ(){ķ=new δ(Ɩ,k.œ);ґ=new Ƙ(Ɩ,k);}ʌ Ư=new ʌ(100);Dictionary<string,string>Ҡ=
new Dictionary<string,string>();void ҟ(string ȧ,double ѳ,int Y){if(Y>0){if(!Ҍ)k.Ƹ(Math.Min(100,100*ѳ/Y),0.3f);string ͱ;if(Ҡ
.ContainsKey(ȧ)){ͱ=Ҡ[ȧ];}else{if(!ҍ)ͱ=k.ǟ(ȧ,k.ɠ*0.5f-Ҋ-ҡ);else{if(!Ҍ)ͱ=k.ǟ(ȧ,k.ɠ*0.5f);else ͱ=k.ǟ(ȧ,k.ɠ*0.9f);}Ҡ[ȧ]=ͱ;}Ư.
ŵ();if(!Ҍ)Ư.ʈ(' ');if(!ҍ){k.Ƅ(Ư.ʈ(ͱ).ʈ(' '));k.ƭ(k.ȉ(ѳ),1.0f,Ҋ+ҡ);k.Ǒ(Ư.ŵ().ʈ(" / ").ʈ(k.ȉ(Y)));}else{k.Ǒ(Ư.ʈ(ͱ));}}else{
if(!ҍ){k.Ƅ(Ư.ŵ().ʈ(ȧ).ʈ(':'));k.Ʒ(k.ȉ(ѳ),1.0f,Ң);}else k.Ǒ(Ư.ŵ().ʈ(ȧ));}}void Ҟ(string ȧ,double ѳ,double Ѿ,int Y){if(Y>0){
if(!ҍ){k.Ƅ(Ư.ŵ().ʈ(ȧ).ʈ(' '));k.ƭ(k.ȉ(ѳ),0.51f);k.Ƅ(Ư.ŵ().ʈ(" / ").ʈ(k.ȉ(Y)));k.Ʒ(Ư.ŵ().ʈ(" +").ʈ(k.ȉ(Ѿ)).ʈ(" ").ʈ(Đ.Ǥ(
"I1")),1.0f);}else k.Ǒ(Ư.ŵ().ʈ(ȧ));if(!Ҍ)k.Ʋ(Math.Min(100,100*ѳ/Y));}else{if(!ҍ){k.Ƅ(Ư.ŵ().ʈ(ȧ).ʈ(':'));k.ƭ(k.ȉ(ѳ),0.51f);k.
Ʒ(Ư.ŵ().ʈ(" +").ʈ(k.ȉ(Ѿ)).ʈ(" ").ʈ(Đ.Ǥ("I1")),1.0f);}else{k.Ǒ(Ư.ŵ().ʈ(ȧ));}}}float Ҋ=0;bool ѽ(ź Ž){int Y=(ҏ?Ž.Ź:Ž.Ÿ);if(Y
<0)return true;float Ƶ=k.ǡ(k.ȉ(Y),k.Ƽ);if(Ƶ>Ҋ)Ҋ=Ƶ;return true;}List<ź>Ѽ;int ѻ=0;int Ѻ=0;bool ѹ(bool õ,bool Ѹ,string Ñ,
string С){if(!õ){Ѻ=0;ѻ=0;}if(Ѻ==0){if(ҁ){if((Ѽ=ґ.ſ(Ñ,õ,ѽ))==null)return false;}else{if((Ѽ=ґ.ſ(Ñ,õ))==null)return false;}Ѻ++;õ=
false;}if(Ѽ.Count>0){if(!Ѹ&&!õ){if(!k.ƻ)k.Ǒ();k.Ʀ(Ư.ŵ().ʈ("<< ").ʈ(С).ʈ(" ").ʈ(Đ.Ǥ("I2")).ʈ(" >>"));}for(;ѻ<Ѽ.Count;ѻ++){if(!
Ɩ.Ȟ(30))return false;double ѳ=Ѽ[ѻ].Ə;if(ҏ&&ѳ>=Ѽ[ѻ].Ź)continue;int Y=Ѽ[ѻ].Ÿ;if(ҏ)Y=Ѽ[ѻ].Ź;string ȧ=k.Ƕ(Ѽ[ѻ].Ò,Ѽ[ѻ].Ñ);ҟ(ȧ,
ѳ,Y);}}return true;}List<ź>ѷ;int Ѷ=0;int ѵ=0;bool Ѵ(bool õ){if(!õ){Ѷ=0;ѵ=0;}if(ѵ==0){if((ѷ=ґ.ſ("Ingot",õ))==null)return
false;ѵ++;õ=false;}if(ѷ.Count>0){if(!Ҏ&&!õ){if(!k.ƻ)k.Ǒ();k.Ʀ(Ư.ŵ().ʈ("<< ").ʈ(Đ.Ǥ("I4")).ʈ(" ").ʈ(Đ.Ǥ("I2")).ʈ(" >>"));}for(
;Ѷ<ѷ.Count;Ѷ++){if(!Ɩ.Ȟ(40))return false;double ѳ=ѷ[Ѷ].Ə;if(ҏ&&ѳ>=ѷ[Ѷ].Ź)continue;int Y=ѷ[Ѷ].Ÿ;if(ҏ)Y=ѷ[Ѷ].Ź;string ȧ=k.Ƕ
(ѷ[Ѷ].Ò,ѷ[Ѷ].Ñ);if(ѷ[Ѷ].Ò!="Scrap"){double Ѿ=ґ.Ƃ(ѷ[Ѷ].Ò+" Ore",ѷ[Ѷ].Ò,"Ore").Ə;Ҟ(ȧ,ѳ,Ѿ,Y);}else ҟ(ȧ,ѳ,Y);}}return true;}δ
ķ=null;Ƙ ґ;List<ˆ>ˢ;bool Ґ,ͺ,ҏ,Ҏ,ҍ,Ҍ;int Ķ,Ĳ;string Ғ="";float ҋ=0;bool ҁ=true;void Ҁ(){if(k.Ƽ!=Ғ||ҋ!=k.ɠ){Ҡ.Clear();ҋ=k.
ɠ;}if(k.Ƽ!=Ғ){ҡ=k.ǡ(" / ",k.Ƽ);Ң=k.Ǽ(' ',k.Ƽ);Ғ=k.Ƽ;}ķ.ŵ();Ґ=Ɯ.ˡ.EndsWith("x")||Ɯ.ˡ.EndsWith("xs");ͺ=Ɯ.ˡ.EndsWith("s")||Ɯ
.ˡ.EndsWith("sx");ҏ=Ɯ.ˡ.StartsWith("missing");Ҏ=Ɯ.ˡ.Contains("list");Ҍ=Ɯ.ˡ.Contains("nb");ҍ=Ɯ.ˡ.Contains("nn");ґ.ŵ();ˢ=Ɯ.
ˢ;if(ˢ.Count==0)ˢ.Add(new ˆ("all"));}bool ѿ(bool õ){if(!õ)Ķ=0;for(;Ķ<ˢ.Count;Ķ++){ˆ Ĵ=ˢ[Ķ];Ĵ.ʚ();string Ñ=Ĵ.ˀ;if(!õ)Ĳ=0;
else õ=false;for(;Ĳ<Ĵ.ʜ.Count;Ĳ++){if(!Ɩ.Ȟ(30))return false;string[]Ĉ=Ĵ.ʜ[Ĳ].Split(':');double Ȇ;if(string.Compare(Ĉ[0],
"all",true)==0)Ĉ[0]="";int Ź=1;int Ÿ=-1;if(Ĉ.Length>1){if(Double.TryParse(Ĉ[1],out Ȇ)){if(ҏ)Ź=(int)Math.Ceiling(Ȇ);else Ÿ=(
int)Math.Ceiling(Ȇ);}}string ƣ=Ĉ[0];if(!string.IsNullOrEmpty(Ñ))ƣ+=' '+Ñ;ґ.Ƥ(ƣ,Ĵ.ˁ=="-",Ź,Ÿ);}}return true;}int Н=0;int τ=0
;int ҿ=0;List<MyInventoryItem>Ƈ=new List<MyInventoryItem>();bool Ҿ(bool õ){δ Ѐ=ķ;if(!õ)Н=0;for(;Н<Ѐ.λ.Count;Н++){if(!õ)τ=
0;for(;τ<Ѐ.λ[Н].InventoryCount;τ++){IMyInventory σ=Ѐ.λ[Н].GetInventory(τ);if(!õ){ҿ=0;Ƈ.Clear();σ.GetItems(Ƈ);}else õ=
false;for(;ҿ<Ƈ.Count;ҿ++){if(!Ɩ.Ȟ(40))return false;MyInventoryItem º=Ƈ[ҿ];string Ô=k.ǹ(º);string Ò,Ñ;k.Ƿ(Ô,out Ò,out Ñ);if(
string.Compare(Ñ,"ore",true)==0){if(ґ.ơ(Ò+" ingot",Ò,"Ingot")&&ґ.ơ(Ô,Ò,Ñ))continue;}else{if(ґ.ơ(Ô,Ò,Ñ))continue;}k.Ƿ(Ô,out Ò,
out Ñ);ź ŷ=ґ.Ƃ(Ô,Ò,Ñ);ŷ.Ə+=(double)º.Amount;}}}return true;}int ĝ=0;public override bool ƙ(bool õ){if(!õ){Ҁ();ĝ=0;}for(;ĝ<=
13;ĝ++){switch(ĝ){case 0:if(!ķ.ν(Ɯ.ˠ,õ))return false;break;case 1:if(!ѿ(õ))return false;if(Ґ)ĝ++;break;case 2:if(!ґ.Ż(õ))
return false;break;case 3:if(!Ҿ(õ))return false;break;case 4:if(!ѹ(õ,Ҏ,"Ore",Đ.Ǥ("I3")))return false;break;case 5:if(ͺ){if(!ѹ(
õ,Ҏ,"Ingot",Đ.Ǥ("I4")))return false;}else{if(!Ѵ(õ))return false;}break;case 6:if(!ѹ(õ,Ҏ,"Component",Đ.Ǥ("I5")))return
false;break;case 7:if(!ѹ(õ,Ҏ,"OxygenContainerObject",Đ.Ǥ("I6")))return false;break;case 8:if(!ѹ(õ,true,"GasContainerObject",
""))return false;break;case 9:if(!ѹ(õ,Ҏ,"AmmoMagazine",Đ.Ǥ("I7")))return false;break;case 10:if(!ѹ(õ,Ҏ,"PhysicalGunObject"
,Đ.Ǥ("I8")))return false;break;case 11:if(!ѹ(õ,true,"Datapad",""))return false;break;case 12:if(!ѹ(õ,true,
"ConsumableItem",""))return false;break;case 13:if(!ѹ(õ,true,"PhysicalObject",""))return false;break;}õ=false;}ҁ=false;return true;}}
class ҽ:Ɲ{public ҽ(){ɐ=2;ɔ="CmdAmount";}δ ķ;public override void ɇ(){ķ=new δ(Ɩ,k.œ);}bool Ҽ;bool һ=false;int ĳ=0;int Ķ=0;int
Ĳ=0;public override bool ƙ(bool õ){if(!õ){Ҽ=!Ɯ.ˡ.EndsWith("x");һ=Ɯ.ˡ.EndsWith("bar");if(һ)Ҽ=true;if(Ɯ.ˢ.Count==0)Ɯ.ˢ.Add(
new ˆ("reactor,gatlingturret,missileturret,interiorturret,gatlinggun,launcherreload,launcher,oxygenerator"));Ķ=0;}for(;Ķ<Ɯ.
ˢ.Count;Ķ++){ˆ Ĵ=Ɯ.ˢ[Ķ];if(!õ){Ĵ.ʚ();ĳ=0;Ĳ=0;}for(;Ĳ<Ĵ.ʜ.Count;Ĳ++){if(ĳ==0){if(!õ){if(Ĵ.ʜ[Ĳ]=="")continue;ķ.ŵ();}string
ŀ=Ĵ.ʜ[Ĳ];if(!ķ.ϵ(ŀ,Ɯ.ˠ,õ))return false;ĳ++;õ=false;}if(!Ӄ(õ))return false;õ=false;ĳ=0;}}return true;}int Ӏ=0;int Ĭ=0;
double ŷ=0;double ӈ=0;double Ӈ=0;int ҿ=0;IMyTerminalBlock ӆ;IMyInventory Ӆ;List<MyInventoryItem>Ƈ=new List<MyInventoryItem>();
string ӄ="";bool Ӄ(bool õ){if(!õ){Ӏ=0;Ĭ=0;}for(;Ӏ<ķ.Ћ();Ӏ++){if(Ĭ==0){if(!Ɩ.Ȟ(50))return false;ӆ=ķ.λ[Ӏ];Ӆ=ӆ.GetInventory(0);if
(Ӆ==null)continue;Ĭ++;õ=false;}if(!õ){Ƈ.Clear();Ӆ.GetItems(Ƈ);ӄ=(Ƈ.Count>0?Ƈ[0].Type.ToString():"");ҿ=0;ŷ=0;ӈ=0;Ӈ=0;}for(
;ҿ<Ƈ.Count;ҿ++){if(!Ɩ.Ȟ(30))return false;MyInventoryItem º=Ƈ[ҿ];if(º.Type.ToString()!=ӄ)Ӈ+=(double)º.Amount;else ŷ+=(
double)º.Amount;}string Ӊ=Đ.Ǥ("A1");string Ŷ=ӆ.CustomName;if(ŷ>0&&(double)Ӆ.CurrentVolume>0){double ӂ=Ӈ*(double)Ӆ.
CurrentVolume/(ŷ+Ӈ);ӈ=Math.Floor(ŷ*((double)Ӆ.MaxVolume-ӂ)/((double)Ӆ.CurrentVolume-ӂ));Ӊ=k.ȉ(ŷ)+" / "+(Ӈ>0?"~":"")+k.ȉ(ӈ);}if(!һ||ӈ
<=0){Ŷ=k.ǟ(Ŷ,k.ɠ*0.8f);k.Ƅ(Ŷ);k.Ʒ(Ӊ);}if(Ҽ&&ӈ>0){double ʕ=100*ŷ/ӈ;k.Ʋ(ʕ);}Ĭ=0;õ=false;}return true;}}class Ӂ:Ɲ{δ ķ;public
Ӂ(){ɐ=2;ɔ="CmdMass";}public override void ɇ(){ķ=new δ(Ɩ,k.œ);}bool ͺ=false;bool Σ=false;int ĝ=0;public override bool ƙ(
bool õ){if(!õ){ķ.ŵ();ͺ=(Ɯ.ˡ[Ɯ.ˡ.Length-1]=='x');Σ=(Ɯ.ˡ[Ɯ.ˡ.Length-1]=='p');ĝ=0;}if(ĝ==0){if(!ķ.ν(Ɯ.ˠ,õ))return false;ĝ++;õ=
false;}double É=ķ.ό(õ);if(Double.IsNaN(É))return false;double ʖ=0;int Ѧ=Ɯ.ˢ.Count;if(Ѧ>0){double.TryParse(Ɯ.ˢ[0].Ĵ.Trim(),out
ʖ);if(Ѧ>1){string ѝ=Ɯ.ˢ[1].Ĵ.Trim();char ё=' ';if(ѝ.Length>0)ё=Char.ToLower(ѝ[0]);int ъ="kmgtpezy".IndexOf(ё);if(ъ>=0)ʖ*=
Math.Pow(1000.0,ъ);}ʖ*=1000.0;}k.Ƅ(Đ.Ǥ("M1")+" ");if(ʖ<=0){k.Ʒ(k.ȇ(É,false));return true;}double ʕ=É/ʖ*100;if(!ͺ&&!Σ){k.Ʒ(k.
ȇ(É)+" / "+k.ȇ(ʖ));k.Ƹ(ʕ,1.0f,k.ə);k.Ǒ(' '+k.Ȁ(ʕ)+"%");}else if(Σ){k.Ʒ(k.Ȁ(ʕ)+"%");k.Ʋ(ʕ);}else k.Ʒ(k.Ȁ(ʕ)+"%");return
true;}}class ҭ:Ɲ{ȵ ǅ;δ ķ;public ҭ(){ɐ=3;ɔ="CmdOxygen";}public override void ɇ(){ǅ=k.ǅ;ķ=new δ(Ɩ,k.œ);}int ĝ=0;int í=0;bool Ơ
=false;int Ҭ=0;double Ȯ=0;double ȷ=0;double Ʊ;public override bool ƙ(bool õ){if(!õ){ķ.ŵ();ĝ=0;í=0;Ʊ=0;}if(ĝ==0){if(!ķ.ϵ(
"airvent",Ɯ.ˠ,õ))return false;Ơ=(ķ.Ћ()>0);ĝ++;õ=false;}if(ĝ==1){for(;í<ķ.Ћ();í++){if(!Ɩ.Ȟ(8))return false;IMyAirVent ő=ķ.λ[í]as
IMyAirVent;Ʊ=Math.Max(ő.GetOxygenLevel()*100,0f);k.Ƅ(ő.CustomName);if(ő.CanPressurize)k.Ʒ(k.Ȁ(Ʊ)+"%");else k.Ʒ(Đ.Ǥ("O1"));k.Ʋ(Ʊ);}
ĝ++;õ=false;}if(ĝ==2){if(!õ)ķ.ŵ();if(!ķ.ϵ("oxyfarm",Ɯ.ˠ,õ))return false;Ҭ=ķ.Ћ();ĝ++;õ=false;}if(ĝ==3){if(Ҭ>0){if(!õ)í=0;
double ҫ=0;for(;í<Ҭ;í++){if(!Ɩ.Ȟ(4))return false;IMyOxygenFarm Ҫ=ķ.λ[í]as IMyOxygenFarm;ҫ+=Ҫ.GetOutput()*100;}Ʊ=ҫ/Ҭ;if(Ơ)k.Ǒ(
"");Ơ|=(Ҭ>0);k.Ƅ(Đ.Ǥ("O2"));k.Ʒ(k.Ȁ(Ʊ)+"%");k.Ʋ(Ʊ);}ĝ++;õ=false;}if(ĝ==4){if(!õ)ķ.ŵ();if(!ķ.ϵ("oxytank",Ɯ.ˠ,õ))return
false;Ҭ=ķ.Ћ();if(Ҭ==0){if(!Ơ)k.Ǒ(Đ.Ǥ("O3"));return true;}ĝ++;õ=false;}if(ĝ==5){if(!õ){Ȯ=0;ȷ=0;í=0;}if(!ǅ.ȹ(ķ.λ,"oxygen",ref ȷ
,ref Ȯ,õ))return false;if(Ȯ==0){if(!Ơ)k.Ǒ(Đ.Ǥ("O3"));return true;}Ʊ=ȷ/Ȯ*100;if(Ơ)k.Ǒ("");k.Ƅ(Đ.Ǥ("O4"));k.Ʒ(k.Ȁ(Ʊ)+"%");k
.Ʋ(Ʊ);ĝ++;}return true;}}class ҩ:Ɲ{public ҩ(){ɐ=1;ɔ="CmdPosition";}public override bool ƙ(bool õ){bool Ҩ=(Ɯ.ˡ=="posxyz");
bool ϥ=(Ɯ.ˡ=="posgps");IMyTerminalBlock ç=f.C.ç;if(Ɯ.ˠ!=""&&Ɯ.ˠ!="*"){ç=k.ǂ.GetBlockWithName(Ɯ.ˠ);if(ç==null){k.Ǒ("Pos: "+Đ.
Ǥ("P1")+": "+Ɯ.ˠ);return true;}}if(ϥ){Vector3D Ŭ=ç.GetPosition();k.Ǒ("GPS:"+Đ.Ǥ("P2")+":"+Ŭ.GetDim(0).ToString("F2")+":"+
Ŭ.GetDim(1).ToString("F2")+":"+Ŭ.GetDim(2).ToString("F2")+":");return true;}k.Ƅ(Đ.Ǥ("P2")+": ");if(!Ҩ){k.Ʒ(ç.GetPosition(
).ToString("F0"));return true;}k.Ǒ("");k.Ƅ(" X: ");k.Ʒ(ç.GetPosition().GetDim(0).ToString("F0"));k.Ƅ(" Y: ");k.Ʒ(ç.
GetPosition().GetDim(1).ToString("F0"));k.Ƅ(" Z: ");k.Ʒ(ç.GetPosition().GetDim(2).ToString("F0"));return true;}}class ҳ:Ɲ{public ҳ(
){ɐ=3;ɔ="CmdPower";}ȵ ǅ;δ ҹ;δ Ҹ;δ ҷ;δ Ц;δ Ҷ;δ ķ;public override void ɇ(){ҹ=new δ(Ɩ,k.œ);Ҹ=new δ(Ɩ,k.œ);ҷ=new δ(Ɩ,k.œ);Ц=
new δ(Ɩ,k.œ);Ҷ=new δ(Ɩ,k.œ);ķ=new δ(Ɩ,k.œ);ǅ=k.ǅ;}string С;bool ҵ;string Ф;string Һ;int Ҵ;int ĝ=0;public override bool ƙ(
bool õ){if(!õ){С=(Ɯ.ˡ.EndsWith("x")?"s":(Ɯ.ˡ.EndsWith("p")?"p":(Ɯ.ˡ.EndsWith("v")?"v":"n")));ҵ=(Ɯ.ˡ.StartsWith(
"powersummary"));Ф="a";Һ="";if(Ɯ.ˡ.Contains("stored"))Ф="s";else if(Ɯ.ˡ.Contains("in"))Ф="i";else if(Ɯ.ˡ.Contains("out"))Ф="o";ĝ=0;ҹ.ŵ
();Ҹ.ŵ();ҷ.ŵ();Ц.ŵ();Ҷ.ŵ();}if(Ф=="a"){if(ĝ==0){if(!ҹ.ϵ("reactor",Ɯ.ˠ,õ))return false;õ=false;ĝ++;}if(ĝ==1){if(!Ҹ.ϵ(
"hydrogenengine",Ɯ.ˠ,õ))return false;õ=false;ĝ++;}if(ĝ==2){if(!ҷ.ϵ("solarpanel",Ɯ.ˠ,õ))return false;õ=false;ĝ++;}if(ĝ==3){if(!Ҷ.ϵ(
"windturbine",Ɯ.ˠ,õ))return false;õ=false;ĝ++;}}else if(ĝ==0)ĝ=4;if(ĝ==4){if(!Ц.ϵ("battery",Ɯ.ˠ,õ))return false;õ=false;ĝ++;}int Ҳ=ҹ.
Ћ();int ұ=Ҹ.Ћ();int Ұ=ҷ.Ћ();int ү=Ц.Ћ();int Ү=Ҷ.Ћ();if(ĝ==5){Ҵ=0;if(Ҳ>0)Ҵ++;if(ұ>0)Ҵ++;if(Ұ>0)Ҵ++;if(Ү>0)Ҵ++;if(ү>0)Ҵ++;
if(Ҵ<1){k.Ǒ(Đ.Ǥ("P6"));return true;}if(Ɯ.ˢ.Count>0){if(Ɯ.ˢ[0].Ĵ.Length>0)Һ=Ɯ.ˢ[0].Ĵ;}ĝ++;õ=false;}if(Ф!="a"){if(!Ч(Ц,(Һ==
""?Đ.Ǥ("P7"):Һ),Ф,С,õ))return false;return true;}string Х=Đ.Ǥ("P8");if(!ҵ){if(ĝ==6){if(Ҳ>0)if(!а(ҹ,(Һ==""?Đ.Ǥ("P9"):Һ),С,õ
))return false;ĝ++;õ=false;}if(ĝ==7){if(ұ>0)if(!а(Ҹ,(Һ==""?Đ.Ǥ("P12"):Һ),С,õ))return false;ĝ++;õ=false;}if(ĝ==8){if(Ұ>0)
if(!а(ҷ,(Һ==""?Đ.Ǥ("P10"):Һ),С,õ))return false;ĝ++;õ=false;}if(ĝ==9){if(Ү>0)if(!а(Ҷ,(Һ==""?Đ.Ǥ("P13"):Һ),С,õ))return false
;ĝ++;õ=false;}if(ĝ==10){if(ү>0)if(!Ч(Ц,(Һ==""?Đ.Ǥ("P7"):Һ),Ф,С,õ))return false;ĝ++;õ=false;}}else{Х=Đ.Ǥ("P11");Ҵ=10;if(ĝ
==6)ĝ=11;}if(Ҵ==1)return true;if(!õ){ķ.ŵ();ķ.Ϲ(ҹ);ķ.Ϲ(Ҹ);ķ.Ϲ(ҷ);ķ.Ϲ(Ҷ);ķ.Ϲ(Ц);}if(!а(ķ,Х,С,õ))return false;return true;}
void П(double ʙ,double Ÿ){double О=(Ÿ>0?ʙ/Ÿ*100:0);switch(С){case"s":k.Ʒ(Ư.ŵ().ʈ(' ').ʈ(О.ToString("F1")).ʈ("%"));break;case
"v":k.Ʒ(Ư.ŵ().ʈ(k.ȉ(ʙ)).ʈ("W / ").ʈ(k.ȉ(Ÿ)).ʈ("W"));break;case"c":k.Ʒ(Ư.ŵ().ʈ(k.ȉ(ʙ)).ʈ("W"));break;case"p":k.Ʒ(Ư.ŵ().ʈ(' '
).ʈ(О.ToString("F1")).ʈ("%"));k.Ʋ(О);break;default:k.Ʒ(Ư.ŵ().ʈ(k.ȉ(ʙ)).ʈ("W / ").ʈ(k.ȉ(Ÿ)).ʈ("W"));k.Ƹ(О,1.0f,k.ə);k.Ʒ(Ư.
ŵ().ʈ(' ').ʈ(О.ToString("F1")).ʈ("%"));break;}}double Ѳ=0;double И=0,в=0;int б=0;bool а(δ Я,string Х,string ʓ,bool õ){if(
!õ){И=0;в=0;б=0;}if(б==0){if(!ǅ.ȿ(Я.λ,ǅ.ȴ,ref Ѳ,ref Ѳ,ref И,ref в,õ))return false;б++;õ=false;}if(!Ɩ.Ȟ(50))return false;
double О=(в>0?И/в*100:0);k.Ƅ(Х+": ");П(И*1000000,в*1000000);return true;}double Ю=0,Э=0,г=0,Ь=0;double Ъ=0,Щ=0;int Ш=0;ʌ Ư=new
ʌ(100);bool Ч(δ Ц,string Х,string Ф,string ʓ,bool õ){if(!õ){Ю=Э=0;г=Ь=0;Ъ=Щ=0;Ш=0;}if(Ш==0){if(!ǅ.Ȱ(Ц.λ,ref г,ref Ь,ref Ю
,ref Э,ref Ъ,ref Щ,õ))return false;г*=1000000;Ь*=1000000;Ю*=1000000;Э*=1000000;Ъ*=1000000;Щ*=1000000;Ш++;õ=false;}double
Ы=(Щ>0?Ъ/Щ*100:0);double д=(Э>0?Ю/Э*100:0);double п=(Ь>0?г/Ь*100:0);bool щ=Ф=="a";if(Ш==1){if(!Ɩ.Ȟ(50))return false;if(щ)
{if(ʓ!="p"){k.Ƅ(Ư.ŵ().ʈ(Х).ʈ(": "));k.Ʒ(Ư.ŵ().ʈ("(IN ").ʈ(k.ȉ(г)).ʈ("W / OUT ").ʈ(k.ȉ(Ю)).ʈ("W)"));}else k.Ǒ(Ư.ŵ().ʈ(Х).ʈ
(": "));k.Ƅ(Ư.ŵ().ʈ("  ").ʈ(Đ.Ǥ("P3")).ʈ(": "));}else k.Ƅ(Ư.ŵ().ʈ(Х).ʈ(": "));if(щ||Ф=="s")switch(ʓ){case"s":k.Ʒ(Ư.ŵ().ʈ(
' ').ʈ(Ы.ToString("F1")).ʈ("%"));break;case"v":k.Ʒ(Ư.ŵ().ʈ(k.ȉ(Ъ)).ʈ("Wh / ").ʈ(k.ȉ(Щ)).ʈ("Wh"));break;case"p":k.Ʒ(Ư.ŵ().ʈ(
' ').ʈ(Ы.ToString("F1")).ʈ("%"));k.Ʋ(Ы);break;default:k.Ʒ(Ư.ŵ().ʈ(k.ȉ(Ъ)).ʈ("Wh / ").ʈ(k.ȉ(Щ)).ʈ("Wh"));k.Ƹ(Ы,1.0f,k.ə);k.Ʒ
(Ư.ŵ().ʈ(' ').ʈ(Ы.ToString("F1")).ʈ("%"));break;}if(Ф=="s")return true;Ш++;õ=false;}if(Ш==2){if(!Ɩ.Ȟ(50))return false;if(
щ)k.Ƅ(Ư.ŵ().ʈ("  ").ʈ(Đ.Ǥ("P4")).ʈ(": "));if(щ||Ф=="o")switch(ʓ){case"s":k.Ʒ(Ư.ŵ().ʈ(' ').ʈ(д.ToString("F1")).ʈ("%"));
break;case"v":k.Ʒ(Ư.ŵ().ʈ(k.ȉ(Ю)).ʈ("W / ").ʈ(k.ȉ(Э)).ʈ("W"));break;case"p":k.Ʒ(Ư.ŵ().ʈ(' ').ʈ(д.ToString("F1")).ʈ("%"));k.Ʋ(
д);break;default:k.Ʒ(Ư.ŵ().ʈ(k.ȉ(Ю)).ʈ("W / ").ʈ(k.ȉ(Э)).ʈ("W"));k.Ƹ(д,1.0f,k.ə);k.Ʒ(Ư.ŵ().ʈ(' ').ʈ(д.ToString("F1")).ʈ(
"%"));break;}if(Ф=="o")return true;Ш++;õ=false;}if(!Ɩ.Ȟ(50))return false;if(щ)k.Ƅ(Ư.ŵ().ʈ("  ").ʈ(Đ.Ǥ("P5")).ʈ(": "));if(щ
||Ф=="i")switch(ʓ){case"s":k.Ʒ(Ư.ŵ().ʈ(' ').ʈ(п.ToString("F1")).ʈ("%"));break;case"v":k.Ʒ(Ư.ŵ().ʈ(k.ȉ(г)).ʈ("W / ").ʈ(k.ȉ(
Ь)).ʈ("W"));break;case"p":k.Ʒ(Ư.ŵ().ʈ(' ').ʈ(п.ToString("F1")).ʈ("%"));k.Ʋ(п);break;default:k.Ʒ(Ư.ŵ().ʈ(k.ȉ(г)).ʈ("W / ")
.ʈ(k.ȉ(Ь)).ʈ("W"));k.Ƹ(п,1.0f,k.ə);k.Ʒ(Ư.ŵ().ʈ(' ').ʈ(п.ToString("F1")).ʈ("%"));break;}return true;}}class ч:Ɲ{public ч()
{ɐ=7;ɔ="CmdPowerTime";}class ц{public TimeSpan Č=new TimeSpan(-1);public double Б=-1;public double х=0;}ц ф=new ц();δ у;δ
т;public override void ɇ(){у=new δ(Ɩ,k.œ);т=new δ(Ɩ,k.œ);}int с=0;double ш=0;double р=0,о=0;double н=0,м=0,л=0;double к=0
,й=0;int и=0;private bool з(string ˠ,out TimeSpan ж,out double е,bool õ){MyResourceSourceComponent Ȼ;
MyResourceSinkComponent Ȥ;double Л=ɑ;ц К=ф;ж=К.Č;е=К.Б;if(!õ){у.ŵ();т.ŵ();К.Б=0;с=0;ш=0;р=о=0;н=0;м=л=0;к=й=0;и=0;}if(с==0){if(!у.ϵ("reactor",ˠ
,õ))return false;õ=false;с++;}if(с==1){for(;и<у.λ.Count;и++){if(!Ɩ.Ȟ(6))return false;IMyReactor ç=у.λ[и]as IMyReactor;if(
ç==null||!ç.IsWorking)continue;if(ç.Components.TryGet<MyResourceSourceComponent>(out Ȼ)){р+=Ȼ.CurrentOutputByType(k.ǅ.ȴ);
о+=Ȼ.MaxOutputByType(k.ǅ.ȴ);}ш+=(double)ç.GetInventory(0).CurrentMass;}õ=false;с++;}if(с==2){if(!т.ϵ("battery",ˠ,õ))
return false;õ=false;с++;}if(с==3){if(!õ)и=0;for(;и<т.λ.Count;и++){if(!Ɩ.Ȟ(15))return false;IMyBatteryBlock ç=т.λ[и]as
IMyBatteryBlock;if(ç==null||!ç.IsWorking)continue;if(ç.Components.TryGet<MyResourceSourceComponent>(out Ȼ)){м=Ȼ.CurrentOutputByType(k.ǅ
.ȴ);л=Ȼ.MaxOutputByType(k.ǅ.ȴ);}if(ç.Components.TryGet<MyResourceSinkComponent>(out Ȥ)){м-=Ȥ.CurrentInputByType(k.ǅ.ȴ);}
double Й=(м<0?(ç.MaxStoredPower-ç.CurrentStoredPower)/(-м/3600):0);if(Й>К.Б)К.Б=Й;if(ç.ChargeMode==ChargeMode.Recharge)
continue;к+=м;й+=л;н+=ç.CurrentStoredPower;}õ=false;с++;}double И=р+к;if(И<=0)К.Č=TimeSpan.FromSeconds(-1);else{double З=К.Č.
TotalSeconds;double Ж;double Е=(К.х-ш)/Л;if(р<=0)Е=Math.Min(И,о)/3600000;double Д=0;if(й>0)Д=Math.Min(И,й)/3600;if(Е<=0&&Д<=0)Ж=-1;
else if(Е<=0)Ж=н/Д;else if(Д<=0)Ж=ш/Е;else{double Г=Д;double В=(р<=0?И/3600:Е*И/р);Ж=н/Г+ш/В;}if(З<=0||Ж<0)З=Ж;else З=(З+Ж)/
2;try{К.Č=TimeSpan.FromSeconds(З);}catch{К.Č=TimeSpan.FromSeconds(-1);}}К.х=ш;е=К.Б;ж=К.Č;return true;}int ĝ=0;bool Λ=
false;bool ͺ=false;bool Σ=false;double Б=0;TimeSpan Ȗ;int А=0,Џ=0,Ў=0;int ǳ=0;int Ѝ=0;public override bool ƙ(bool õ){if(!õ){Λ
=Ɯ.ˡ.EndsWith("bar");ͺ=(Ɯ.ˡ[Ɯ.ˡ.Length-1]=='x');Σ=(Ɯ.ˡ[Ɯ.ˡ.Length-1]=='p');ĝ=0;А=Џ=Ў=ǳ=0;Ѝ=0;Б=0;}if(ĝ==0){if(Ɯ.ˢ.Count>0
){for(;Ѝ<Ɯ.ˢ.Count;Ѝ++){if(!Ɩ.Ȟ(100))return false;Ɯ.ˢ[Ѝ].ʚ();if(Ɯ.ˢ[Ѝ].ʜ.Count<=0)continue;string Ĵ=Ɯ.ˢ[Ѝ].ʜ[0];int.
TryParse(Ĵ,out ǳ);if(Ѝ==0)А=ǳ;else if(Ѝ==1)Џ=ǳ;else if(Ѝ==2)Ў=ǳ;}}ĝ++;õ=false;}if(ĝ==1){if(!з(Ɯ.ˠ,out Ȗ,out Б,õ))return false;ĝ
++;õ=false;}if(!Ɩ.Ȟ(30))return false;double Č=0;TimeSpan У;try{У=new TimeSpan(А,Џ,Ў);}catch{У=TimeSpan.FromSeconds(-1);}
string Ĺ;if(Ȗ.TotalSeconds>0||Б<=0){if(!Λ)k.Ƅ(Đ.Ǥ("PT1")+" ");Ĺ=k.ǅ.ȗ(Ȗ);Č=Ȗ.TotalSeconds;}else{if(!Λ)k.Ƅ(Đ.Ǥ("PT2")+" ");Ĺ=k.
ǅ.ȗ(TimeSpan.FromSeconds(Б));if(У.TotalSeconds>=Б)Č=У.TotalSeconds-Б;else Č=0;}if(У.Ticks<=0){k.Ʒ(Ĺ);return true;}double
ʕ=Č/У.TotalSeconds*100;if(ʕ>100)ʕ=100;if(Λ){k.Ʋ(ʕ);return true;}if(!ͺ&&!Σ){k.Ʒ(Ĺ);k.Ƹ(ʕ,1.0f,k.ə);k.Ǒ(' '+ʕ.ToString(
"0.0")+"%");}else if(Σ){k.Ʒ(ʕ.ToString("0.0")+"%");k.Ʋ(ʕ);}else k.Ʒ(ʕ.ToString("0.0")+"%");return true;}}class Т:Ɲ{public Т()
{ɐ=7;ɔ="CmdPowerUsed";}ȵ ǅ;δ ķ;public override void ɇ(){ķ=new δ(Ɩ,k.œ);ǅ=k.ǅ;}string С;string Р;string Ϟ;void П(double ʙ,
double Ÿ){double О=(Ÿ>0?ʙ/Ÿ*100:0);switch(С){case"s":k.Ʒ(О.ToString("0.0")+"%",1.0f);break;case"v":k.Ʒ(k.ȉ(ʙ)+"W / "+k.ȉ(Ÿ)+
"W",1.0f);break;case"c":k.Ʒ(k.ȉ(ʙ)+"W",1.0f);break;case"p":k.Ʒ(О.ToString("0.0")+"%",1.0f);k.Ʋ(О);break;default:k.Ʒ(k.ȉ(ʙ)+
"W / "+k.ȉ(Ÿ)+"W");k.Ƹ(О,1.0f,k.ə);k.Ʒ(' '+О.ToString("0.0")+"%");break;}}double ȶ=0,Ⱦ=0;int Н=0;int ĝ=0;ѣ М=new ѣ();public
override bool ƙ(bool õ){if(!õ){С=(Ɯ.ˡ.EndsWith("x")?"s":(Ɯ.ˡ.EndsWith("usedp")||Ɯ.ˡ.EndsWith("topp")?"p":(Ɯ.ˡ.EndsWith("v")?"v":
(Ɯ.ˡ.EndsWith("c")?"c":"n"))));Р=(Ɯ.ˡ.Contains("top")?"top":"");Ϟ=(Ɯ.ˢ.Count>0?Ɯ.ˢ[0].Ĵ:Đ.Ǥ("PU1"));ȶ=Ⱦ=0;ĝ=0;Н=0;ķ.ŵ();М
.v();}if(ĝ==0){if(!ķ.ν(Ɯ.ˠ,õ))return false;õ=false;ĝ++;}MyResourceSinkComponent Ȥ;MyResourceSourceComponent Ȼ;switch(Р){
case"top":if(ĝ==1){for(;Н<ķ.λ.Count;Н++){if(!Ɩ.Ȟ(20))return false;IMyTerminalBlock ç=ķ.λ[Н];if(ç.Components.TryGet<
MyResourceSinkComponent>(out Ȥ)){ListReader<MyDefinitionId>Ș=Ȥ.AcceptedResources;if(Ș.IndexOf(ǅ.ȴ)<0)continue;ȶ=Ȥ.CurrentInputByType(ǅ.ȴ)*
1000000;}else continue;М.Ã(ȶ,ç);}õ=false;ĝ++;}if(М.µ()<=0){k.Ǒ("PowerUsedTop: "+Đ.Ǥ("D2"));return true;}int ľ=10;if(Ɯ.ˢ.Count>0
)if(!int.TryParse(Ϟ,out ľ)){ľ=10;}if(ľ>М.µ())ľ=М.µ();if(ĝ==2){if(!õ){Н=М.µ()-1;М.o();}for(;Н>=М.µ()-ľ;Н--){if(!Ɩ.Ȟ(30))
return false;IMyTerminalBlock ç=М.w(Н);string Ŷ=k.ǟ(ç.CustomName,k.ɠ*0.4f);if(ç.Components.TryGet<MyResourceSinkComponent>(out
Ȥ)){ȶ=Ȥ.CurrentInputByType(ǅ.ȴ)*1000000;Ⱦ=Ȥ.MaxRequiredInputByType(ǅ.ȴ)*1000000;}k.Ƅ(Ŷ+" ");П(ȶ,Ⱦ);}}break;default:for(;Н
<ķ.λ.Count;Н++){if(!Ɩ.Ȟ(10))return false;double Ѥ;IMyTerminalBlock ç=ķ.λ[Н];if(ç.Components.TryGet<
MyResourceSinkComponent>(out Ȥ)){ListReader<MyDefinitionId>Ș=Ȥ.AcceptedResources;if(Ș.IndexOf(ǅ.ȴ)<0)continue;Ѥ=Ȥ.CurrentInputByType(ǅ.ȴ);Ⱦ+=Ȥ.
MaxRequiredInputByType(ǅ.ȴ);}else continue;if(ç.Components.TryGet<MyResourceSourceComponent>(out Ȼ)&&(ç as IMyBatteryBlock!=null)){Ѥ-=Ȼ.
CurrentOutputByType(ǅ.ȴ);if(Ѥ<=0)continue;}ȶ+=Ѥ;}k.Ƅ(Ϟ);П(ȶ*1000000,Ⱦ*1000000);break;}return true;}public class ѣ{List<KeyValuePair<double,
IMyTerminalBlock>>Ѣ=new List<KeyValuePair<double,IMyTerminalBlock>>();public void Ã(double ѡ,IMyTerminalBlock ç){Ѣ.Add(new KeyValuePair<
double,IMyTerminalBlock>(ѡ,ç));}public int µ(){return Ѣ.Count;}public IMyTerminalBlock w(int Â){return Ѣ[Â].Value;}public void
v(){Ѣ.Clear();}public void o(){Ѣ.Sort((Ё,џ)=>(Ё.Key.CompareTo(џ.Key)));}}}class ў:Ɲ{δ ķ;public ў(){ɐ=1;ɔ="CmdProp";}
public override void ɇ(){ķ=new δ(Ɩ,k.œ);}int ĝ=0;int Н=0;bool Ѡ=false;string ѥ=null;string ѩ=null;string ѱ=null;string ѯ=null;
public override bool ƙ(bool õ){if(!õ){Ѡ=Ɯ.ˡ.StartsWith("props");ѥ=ѩ=ѱ=ѯ=null;Н=0;ĝ=0;}if(Ɯ.ˢ.Count<1){k.Ǒ(Ɯ.ˡ+": "+
"Missing property name.");return true;}if(ĝ==0){if(!õ)ķ.ŵ();if(!ķ.ν(Ɯ.ˠ,õ))return false;Ѯ();ĝ++;õ=false;}if(ĝ==1){int ľ=ķ.Ћ();if(ľ==0){k.Ǒ(Ɯ.ˡ+
": "+"No blocks found.");return true;}for(;Н<ľ;Н++){if(!Ɩ.Ȟ(50))return false;IMyTerminalBlock ç=ķ.λ[Н];if(ç.GetProperty(ѥ)!=
null){if(ѩ==null){string Ϟ=k.ǟ(ç.CustomName,k.ɠ*0.7f);k.Ƅ(Ϟ);}else k.Ƅ(ѩ);k.Ʒ(ѭ(ç,ѥ,ѱ,ѯ));if(!Ѡ)return true;}}}return true;}
void Ѯ(){ѥ=Ɯ.ˢ[0].Ĵ;if(Ɯ.ˢ.Count>1){if(!Ѡ)ѩ=Ɯ.ˢ[1].Ĵ;else ѱ=Ɯ.ˢ[1].Ĵ;if(Ɯ.ˢ.Count>2){if(!Ѡ)ѱ=Ɯ.ˢ[2].Ĵ;else ѯ=Ɯ.ˢ[2].Ĵ;if(Ɯ.ˢ
.Count>3&&!Ѡ)ѯ=Ɯ.ˢ[3].Ĵ;}}}string ѭ(IMyTerminalBlock ç,string Ѭ,string ѫ=null,string Ѱ=null){return(ç.GetValue<bool>(Ѭ)?(
ѫ!=null?ѫ:Đ.Ǥ("W9")):(Ѱ!=null?Ѱ:Đ.Ǥ("W1")));}}class Ѫ:Ɲ{public Ѫ(){ɐ=5;ɔ="CmdShipCtrl";}δ ķ;public override void ɇ(){ķ=
new δ(Ɩ,k.œ);}public override bool ƙ(bool õ){if(!õ)ķ.ŵ();if(!ķ.ϵ("shipctrl",Ɯ.ˠ,õ))return false;if(ķ.Ћ()<=0){if(Ɯ.ˠ!=""&&Ɯ.
ˠ!="*")k.Ǒ(Ɯ.ˡ+": "+Đ.Ǥ("SC1")+" ("+Ɯ.ˠ+")");else k.Ǒ(Ɯ.ˡ+": "+Đ.Ǥ("SC1"));return true;}if(Ɯ.ˡ.StartsWith("damp")){bool Ў
=(ķ.λ[0]as IMyShipController).DampenersOverride;k.Ƅ(Đ.Ǥ("SCD"));k.Ʒ(Ў?"ON":"OFF");}else{bool Ў=(ķ.λ[0]as
IMyShipController).IsUnderControl;k.Ƅ(Đ.Ǥ("SCO"));k.Ʒ(Ў?"YES":"NO");}return true;}}class Ѩ:Ɲ{public Ѩ(){ɐ=1;ɔ="CmdShipMass";}public
override bool ƙ(bool õ){bool ѧ=Ɯ.ˡ.EndsWith("base");double ʖ=0;if(Ɯ.ˠ!="")double.TryParse(Ɯ.ˠ.Trim(),out ʖ);int Ѧ=Ɯ.ˢ.Count;if(Ѧ
>0){string ѝ=Ɯ.ˢ[0].Ĵ.Trim();char ё=' ';if(ѝ.Length>0)ё=Char.ToLower(ѝ[0]);int ъ="kmgtpezy".IndexOf(ё);if(ъ>=0)ʖ*=Math.
Pow(1000.0,ъ);}double ɺ=(ѧ?k.Ǆ.ɹ:k.Ǆ.ʄ);if(!ѧ)k.Ƅ(Đ.Ǥ("SM1")+" ");else k.Ƅ(Đ.Ǥ("SM2")+" ");k.Ʒ(k.ȇ(ɺ,true,'k')+" ");if(ʖ>0)
k.Ʋ(ɺ/ʖ*100);return true;}}class ѓ:Ɲ{public ѓ(){ɐ=0.5;ɔ="CmdSpeed";}public override bool ƙ(bool õ){double ʖ=0;double ђ=1;
string ё="m/s";if(Ɯ.ˡ.Contains("kmh")){ђ=3.6;ё="km/h";}else if(Ɯ.ˡ.Contains("mph")){ђ=2.23694;ё="mph";}if(Ɯ.ˠ!="")double.
TryParse(Ɯ.ˠ.Trim(),out ʖ);k.Ƅ(Đ.Ǥ("S1")+" ");k.Ʒ((k.Ǆ.ʂ*ђ).ToString("F1")+" "+ё+" ");if(ʖ>0)k.Ʋ(k.Ǆ.ʂ/ʖ*100);return true;}}
class ѐ:Ɲ{public ѐ(){ɐ=1;ɔ="CmdStopTask";}public override bool ƙ(bool õ){double є=0;if(Ɯ.ˡ.Contains("best"))є=k.Ǆ.ʂ/k.Ǆ.ɾ;
else є=k.Ǆ.ʂ/k.Ǆ.ɻ;double я=k.Ǆ.ʂ/2*є;if(Ɯ.ˡ.Contains("time")){k.Ƅ(Đ.Ǥ("ST"));if(double.IsNaN(є)){k.Ʒ("N/A");return true;}
string Ĺ="";try{TimeSpan Ε=TimeSpan.FromSeconds(є);if((int)Ε.TotalDays>0)Ĺ=" > 24h";else{if(Ε.Hours>0)Ĺ=Ε.Hours+"h ";if(Ε.
Minutes>0||Ĺ!="")Ĺ+=Ε.Minutes+"m ";Ĺ+=Ε.Seconds+"s";}}catch{Ĺ="N/A";}k.Ʒ(Ĺ);return true;}k.Ƅ(Đ.Ǥ("SD"));if(!double.IsNaN(я)&&!
double.IsInfinity(я))k.Ʒ(k.ȉ(я)+"m ");else k.Ʒ("N/A");return true;}}class э:Ɲ{ȵ ǅ;δ ķ;public э(){ɐ=2;ɔ="CmdTanks";}public
override void ɇ(){ǅ=k.ǅ;ķ=new δ(Ɩ,k.œ);}int ĝ=0;char С='n';string ь;double ы=0;double ю=0;double Ʊ;bool Λ=false;public override
bool ƙ(bool õ){List<ˆ>ˢ=Ɯ.ˢ;if(ˢ.Count==0){k.Ǒ(Đ.Ǥ("T4"));return true;}if(!õ){С=(Ɯ.ˡ.EndsWith("x")?'s':(Ɯ.ˡ.EndsWith("p")?
'p':(Ɯ.ˡ.EndsWith("v")?'v':'n')));Λ=Ɯ.ˡ.EndsWith("bar");ĝ=0;if(ь==null){ь=ˢ[0].Ĵ.Trim();ь=char.ToUpper(ь[0])+ь.Substring(1)
.ToLower();}ķ.ŵ();ы=0;ю=0;}if(ĝ==0){if(!ķ.ϵ("oxytank",Ɯ.ˠ,õ))return false;õ=false;ĝ++;}if(ĝ==1){if(!ķ.ϵ("hydrogenengine",
Ɯ.ˠ,õ))return false;õ=false;ĝ++;}if(ĝ==2){if(!ǅ.ȹ(ķ.λ,ь,ref ы,ref ю,õ))return false;õ=false;ĝ++;}if(ю==0){k.Ǒ(String.
Format(Đ.Ǥ("T5"),ь));return true;}Ʊ=ы/ю*100;if(Λ){k.Ʋ(Ʊ);return true;}k.Ƅ(ь);switch(С){case's':k.Ʒ(' '+k.Ȁ(Ʊ)+"%");break;case
'v':k.Ʒ(k.ȉ(ы)+"L / "+k.ȉ(ю)+"L");break;case'p':k.Ʒ(' '+k.Ȁ(Ʊ)+"%");k.Ʋ(Ʊ);break;default:k.Ʒ(k.ȉ(ы)+"L / "+k.ȉ(ю)+"L");k.Ƹ(
Ʊ,1.0f,k.ə);k.Ʒ(' '+Ʊ.ToString("0.0")+"%");break;}return true;}}class ќ{ɯ k=null;public string H="Debug";public float ћ=
1.0f;public List<ʌ>ǌ=new List<ʌ>();public int ŗ=0;public float њ=0;public ќ(ɯ Ý){k=Ý;ǌ.Add(new ʌ());}public void љ(string Ĺ)
{ǌ[ŗ].ʈ(Ĺ);}public void љ(ʌ Ʈ){ǌ[ŗ].ʈ(Ʈ);}public void ј(){ǌ.Add(new ʌ());ŗ++;њ=0;}public void ј(string ǒ){ǌ[ŗ].ʈ(ǒ);ј();}
public void ї(List<ʌ>і){if(ǌ[ŗ].ʊ==0)ǌ.RemoveAt(ŗ);else ŗ++;ǌ.AddList(і);ŗ+=і.Count-1;ј();}public List<ʌ>ť(){if(ǌ[ŗ].ʊ==0)
return ǌ.GetRange(0,ŗ);else return ǌ;}public void ѕ(string ǎ,string N=""){string[]ǌ=ǎ.Split('\n');for(int D=0;D<ǌ.Length;D++)ј
(N+ǌ[D]);}public void ý(){ǌ.Clear();ј();ŗ=0;}public int ŧ(){return ŗ+(ǌ[ŗ].ʊ>0?1:0);}public string Ŧ(){return String.Join
("\n",ǌ);}public void ť(List<ʌ>Ť,int ţ,int Ţ){int š=ţ+Ţ;int Š=ŧ();if(š>Š)š=Š;for(int D=ţ;D<š;D++)Ť.Add(ǌ[D]);}}class ş{ɯ
k=null;public float Ş=1.0f;public int Ŝ=17;public int ś=0;int Ś=1;int ř=1;public List<ќ>Ř=new List<ќ>();public int ŗ=0;
public ş(ɯ Ý){k=Ý;}public void Ŗ(int ľ){ř=ľ;}public void ŝ(){Ŝ=(int)Math.Floor(ɯ.ɬ*Ş*ř/ɯ.ɪ);}public void Ũ(ќ Ĺ){Ř.Add(Ĺ);}
public void ŵ(){Ř.Clear();}public int ŧ(){int ľ=0;foreach(var Ĺ in Ř){ľ+=Ĺ.ŧ();}return ľ;}ʌ ų=new ʌ(256);public ʌ Ŧ(){ų.ŵ();
int ľ=Ř.Count;for(int D=0;D<ľ-1;D++){ų.ʈ(Ř[D].Ŧ());ų.ʈ("\n");}if(ľ>0)ų.ʈ(Ř[ľ-1].Ŧ());return ų;}List<ʌ>Ų=new List<ʌ>(20);
public ʌ ű(int Ű=0){ų.ŵ();Ų.Clear();if(ř<=0)return ų;int Ŵ=Ř.Count;int ů=0;int Ů=(Ŝ/ř);int ŭ=(Ű*Ů);int Ŭ=ś+ŭ;int ū=Ŭ+Ů;bool Ū=
false;for(int D=0;D<Ŵ;D++){ќ Ĺ=Ř[D];int Š=Ĺ.ŧ();int ũ=ů;ů+=Š;if(!Ū&&ů>Ŭ){int ţ=Ŭ-ũ;if(ů>=ū){Ĺ.ť(Ų,ţ,ū-ũ-ţ);break;}Ū=true;Ĺ.ť(
Ų,ţ,Š);continue;}if(Ū){if(ů>=ū){Ĺ.ť(Ų,0,ū-ũ);break;}Ĺ.ť(Ų,0,Š);}}int ľ=Ų.Count;for(int D=0;D<ľ-1;D++){ų.ʈ(Ų[D]);ų.ʈ("\n")
;}if(ľ>0)ų.ʈ(Ų[ľ-1]);return ų;}public bool Ł(int ľ=-1){if(ľ<=0)ľ=k.ɨ;if(ś-ľ<=0){ś=0;return true;}ś-=ľ;return false;}
public bool Ŀ(int ľ=-1){if(ľ<=0)ľ=k.ɨ;int Ľ=ŧ();if(ś+ľ+Ŝ>=Ľ){ś=Math.Max(Ľ-Ŝ,0);return true;}ś+=ľ;return false;}public int ļ=0;
public void Ļ(){if(ļ>0){ļ--;return;}if(ŧ()<=Ŝ){ś=0;Ś=1;return;}if(Ś>0){if(Ŀ()){Ś=-1;ļ=2;}}else{if(Ł()){Ś=1;ļ=2;}}}}class ĺ:Ɲ{
public ĺ(){ɐ=1;ɔ="CmdTextLCD";}public override bool ƙ(bool õ){string Ĺ="";if(Ɯ.ˠ!=""&&Ɯ.ˠ!="*"){IMyTextPanel ÿ=k.ǂ.
GetBlockWithName(Ɯ.ˠ)as IMyTextPanel;if(ÿ==null){k.Ǒ("TextLCD: "+Đ.Ǥ("T1")+Ɯ.ˠ);return true;}Ĺ=ÿ.GetText();}else{k.Ǒ("TextLCD:"+Đ.Ǥ("T2"
));return true;}if(Ĺ.Length==0)return true;k.ǐ(Ĺ);return true;}}class ĸ:Ɲ{public ĸ(){ɐ=5;ɔ="CmdWorking";}δ ķ;public
override void ɇ(){ķ=new δ(Ɩ,k.œ);}int ĝ=0;int Ķ=0;bool ĵ;public override bool ƙ(bool õ){if(!õ){ĝ=0;ĵ=(Ɯ.ˡ=="workingx");Ķ=0;}if(Ɯ
.ˢ.Count==0){if(ĝ==0){if(!õ)ķ.ŵ();if(!ķ.ν(Ɯ.ˠ,õ))return false;ĝ++;õ=false;}if(!ņ(ķ,ĵ,"",õ))return false;return true;}for(
;Ķ<Ɯ.ˢ.Count;Ķ++){ˆ Ĵ=Ɯ.ˢ[Ķ];if(!õ)Ĵ.ʚ();if(!ł(Ĵ,õ))return false;õ=false;}return true;}int ĳ=0;int Ĳ=0;string[]ı;string ŀ
;string İ;bool ł(ˆ Ĵ,bool õ){if(!õ){ĳ=0;Ĳ=0;}for(;Ĳ<Ĵ.ʜ.Count;Ĳ++){if(ĳ==0){if(!õ){if(string.IsNullOrEmpty(Ĵ.ʜ[Ĳ]))
continue;ķ.ŵ();ı=Ĵ.ʜ[Ĳ].Split(':');ŀ=ı[0];İ=(ı.Length>1?ı[1]:"");}if(!string.IsNullOrEmpty(ŀ)){if(!ķ.ϵ(ŀ,Ɯ.ˠ,õ))return false;}
else{if(!ķ.ν(Ɯ.ˠ,õ))return false;}ĳ++;õ=false;}if(!ņ(ķ,ĵ,İ,õ))return false;ĳ=0;õ=false;}return true;}string Ŕ(
IMyTerminalBlock ç){Љ œ=k.œ;if(!ç.IsWorking)return Đ.Ǥ("W1");IMyProductionBlock Œ=ç as IMyProductionBlock;if(Œ!=null)if(Œ.IsProducing)
return Đ.Ǥ("W2");else return Đ.Ǥ("W3");IMyAirVent ő=ç as IMyAirVent;if(ő!=null){if(ő.CanPressurize)return(ő.GetOxygenLevel()*
100).ToString("F1")+"%";else return Đ.Ǥ("W4");}IMyGasTank Ő=ç as IMyGasTank;if(Ő!=null)return(Ő.FilledRatio*100).ToString(
"F1")+"%";IMyBatteryBlock ŏ=ç as IMyBatteryBlock;if(ŏ!=null)return œ.ϟ(ŏ);IMyJumpDrive Ŏ=ç as IMyJumpDrive;if(Ŏ!=null)return
œ.ϛ(Ŏ).ToString("0.0")+"%";IMyLandingGear ō=ç as IMyLandingGear;if(ō!=null){switch((int)ō.LockMode){case 0:return Đ.Ǥ(
"W8");case 1:return Đ.Ǥ("W10");case 2:return Đ.Ǥ("W7");}}IMyDoor ŕ=ç as IMyDoor;if(ŕ!=null){if(ŕ.Status==DoorStatus.Open)
return Đ.Ǥ("W5");return Đ.Ǥ("W6");}IMyShipConnector Ō=ç as IMyShipConnector;if(Ō!=null){if(Ō.Status==MyShipConnectorStatus.
Unconnected)return Đ.Ǥ("W8");if(Ō.Status==MyShipConnectorStatus.Connected)return Đ.Ǥ("W7");else return Đ.Ǥ("W10");}IMyLaserAntenna
ŋ=ç as IMyLaserAntenna;if(ŋ!=null)return œ.Ϥ(ŋ);IMyRadioAntenna Ŋ=ç as IMyRadioAntenna;if(Ŋ!=null)return k.ȉ(Ŋ.Radius)+
"m";IMyBeacon ŉ=ç as IMyBeacon;if(ŉ!=null)return k.ȉ(ŉ.Radius)+"m";IMyThrust ň=ç as IMyThrust;if(ň!=null&&ň.ThrustOverride>
0)return k.ȉ(ň.ThrustOverride)+"N";return Đ.Ǥ("W9");}int Ň=0;bool ņ(δ Ă,bool Ņ,string ń,bool õ){if(!õ)Ň=0;for(;Ň<Ă.Ћ();Ň
++){if(!Ɩ.Ȟ(20))return false;IMyTerminalBlock ç=Ă.λ[Ň];string Ń=(Ņ?(ç.IsWorking?Đ.Ǥ("W9"):Đ.Ǥ("W1")):Ŕ(ç));if(!string.
IsNullOrEmpty(ń)&&String.Compare(Ń,ń,true)!=0)continue;if(Ņ)Ń=Ŕ(ç);string Ŷ=ç.CustomName;Ŷ=k.ǟ(Ŷ,k.ɠ*0.7f);k.Ƅ(Ŷ);k.Ʒ(Ń);}return true
;}}class Ɲ:ɕ{public ќ Ĺ=null;protected ˣ Ɯ;protected ɯ k;protected ę f;protected Ǖ Đ;public Ɲ(){ɐ=3600;ɔ="CommandTask";}
public void ƛ(ę e,ˣ ƚ){f=e;k=f.k;Ɯ=ƚ;Đ=k.Đ;}public virtual bool ƙ(bool õ){k.Ǒ(Đ.Ǥ("UC")+": '"+Ɯ.ˑ+"'");return true;}public
override bool Ɏ(bool õ){Ĺ=k.Ǐ(Ĺ,f.C);if(!õ)k.ý();return ƙ(õ);}}class Ƙ{Dictionary<string,string>Ɨ=new Dictionary<string,string>(
StringComparer.InvariantCultureIgnoreCase){{"ingot","ingot"},{"ore","ore"},{"component","component"},{"tool","physicalgunobject"},{
"ammo","ammomagazine"},{"oxygen","oxygencontainerobject"},{"gas","gascontainerobject"}};ȕ Ɩ;ɯ k;Ƌ ƕ;Ƌ Ɣ;Ƌ Ɠ;ƈ ƒ;bool Ƒ;public
Ƌ Ɛ;public Ƙ(ȕ ƞ,ɯ Ý,int I=20){ƕ=new Ƌ();Ɣ=new Ƌ();Ɠ=new Ƌ();Ƒ=false;Ɛ=new Ƌ();Ɩ=ƞ;k=Ý;ƒ=k.ƒ;}public void ŵ(){Ɠ.v();Ɣ.v()
;ƕ.v();Ƒ=false;Ɛ.v();}public void Ƥ(string ƣ,bool Ɖ=false,int Ź=1,int Ÿ=-1){if(string.IsNullOrEmpty(ƣ)){Ƒ=true;return;}
string[]Ƣ=ƣ.Split(' ');string Ñ="";ź Ž=new ź(Ɖ,Ź,Ÿ);if(Ƣ.Length==2){if(!Ɨ.TryGetValue(Ƣ[1],out Ñ))Ñ=Ƣ[1];}string Ò=Ƣ[0];if(Ɨ.
TryGetValue(Ò,out Ž.Ñ)){Ɣ.Ã(Ž.Ñ,Ž);return;}k.ǵ(ref Ò,ref Ñ);if(string.IsNullOrEmpty(Ñ)){Ž.Ò=Ò;ƕ.Ã(Ž.Ò,Ž);return;}Ž.Ò=Ò;Ž.Ñ=Ñ;Ɠ.Ã(Ò+
' '+Ñ,Ž);}public ź ƥ(string Ô,string Ò,string Ñ){ź Ž;Ž=Ɠ.ª(Ô);if(Ž!=null)return Ž;Ž=ƕ.ª(Ò);if(Ž!=null)return Ž;Ž=Ɣ.ª(Ñ);if(
Ž!=null)return Ž;return null;}public bool ơ(string Ô,string Ò,string Ñ){ź Ž;bool Ơ=false;Ž=Ɣ.ª(Ñ);if(Ž!=null){if(Ž.Ɖ)
return true;Ơ=true;}Ž=ƕ.ª(Ò);if(Ž!=null){if(Ž.Ɖ)return true;Ơ=true;}Ž=Ɠ.ª(Ô);if(Ž!=null){if(Ž.Ɖ)return true;Ơ=true;}return!(Ƒ
||Ơ);}public ź Ɵ(string Ô,string Ò,string Ñ){ź ŷ=new ź();ź Ž=ƥ(Ô,Ò,Ñ);if(Ž!=null){ŷ.Ź=Ž.Ź;ŷ.Ÿ=Ž.Ÿ;}ŷ.Ò=Ò;ŷ.Ñ=Ñ;Ɛ.Ã(Ô,ŷ);
return ŷ;}public ź Ƃ(string Ô,string Ò,string Ñ){ź ŷ=Ɛ.ª(Ô);if(ŷ==null)ŷ=Ɵ(Ô,Ò,Ñ);return ŷ;}int Ɓ=0;List<ź>ƀ;public List<ź>ſ(
string Ñ,bool õ,Func<ź,bool>ž=null){if(!õ){ƀ=new List<ź>();Ɓ=0;}for(;Ɓ<Ɛ.µ();Ɓ++){if(!Ɩ.Ȟ(5))return null;ź Ž=Ɛ.w(Ɓ);if(ơ(Ž.Ò+
' '+Ž.Ñ,Ž.Ò,Ž.Ñ))continue;if((string.Compare(Ž.Ñ,Ñ,true)==0)&&(ž==null||ž(Ž)))ƀ.Add(Ž);}return ƀ;}int ż=0;public bool Ż(
bool õ){if(!õ){ż=0;}for(;ż<ƒ.X.Count;ż++){if(!Ɩ.Ȟ(10))return false;Ð º=ƒ.Ƈ[ƒ.X[ż]];if(!º.É)continue;string Ô=º.Î+' '+º.Í;if(
ơ(Ô,º.Î,º.Í))continue;ź ŷ=Ƃ(Ô,º.Î,º.Í);if(ŷ.Ÿ==-1)ŷ.Ÿ=º.Ì;}return true;}}class ź{public int Ź;public int Ÿ;public string
Ò="";public string Ñ="";public bool Ɖ;public double Ə;public ź(bool Ǝ=false,int ƍ=1,int ƌ=-1){Ź=ƍ;Ɖ=Ǝ;Ÿ=ƌ;}}class Ƌ{
Dictionary<string,ź>Ɗ=new Dictionary<string,ź>(StringComparer.InvariantCultureIgnoreCase);List<string>X=new List<string>();public
void Ã(string z,ź º){if(!Ɗ.ContainsKey(z)){X.Add(z);Ɗ.Add(z,º);}}public int µ(){return Ɗ.Count;}public ź ª(string z){if(Ɗ.
ContainsKey(z))return Ɗ[z];return null;}public ź w(int Â){return Ɗ[X[Â]];}public void v(){X.Clear();Ɗ.Clear();}public void o(){X.
Sort();}}class ƈ{public Dictionary<string,Ð>Ƈ=new Dictionary<string,Ð>(StringComparer.InvariantCultureIgnoreCase);Dictionary
<string,Ð>Ɔ=new Dictionary<string,Ð>(StringComparer.InvariantCultureIgnoreCase);public List<string>X=new List<string>();
public Dictionary<string,Ð>ƅ=new Dictionary<string,Ð>(StringComparer.InvariantCultureIgnoreCase);public void Ƅ(string Ò,string
Ñ,int Y,string Ö,string Õ,bool É){if(Ñ=="Ammo")Ñ="AmmoMagazine";else if(Ñ=="Tool")Ñ="PhysicalGunObject";string Ô=Ò+' '+Ñ;
Ð º=new Ð(Ò,Ñ,Y,Ö,Õ,É);Ƈ.Add(Ô,º);if(!Ɔ.ContainsKey(Ò))Ɔ.Add(Ò,º);if(Õ!="")ƅ.Add(Õ,º);X.Add(Ô);}public Ð Ó(string Ò="",
string Ñ=""){if(Ƈ.ContainsKey(Ò+" "+Ñ))return Ƈ[Ò+" "+Ñ];if(string.IsNullOrEmpty(Ñ)){Ð º=null;Ɔ.TryGetValue(Ò,out º);return º;
}if(string.IsNullOrEmpty(Ò))for(int D=0;D<Ƈ.Count;D++){Ð º=Ƈ[X[D]];if(string.Compare(Ñ,º.Í,true)==0)return º;}return null
;}}class Ð{public string Î;public string Í;public int Ì;public string Ë;public string Ê;public bool É;public Ð(string È,
string Ç,int Æ=0,string Ï="",string Ø="",bool ß=true){Î=È;Í=Ç;Ì=Æ;Ë=Ï;Ê=Ø;É=ß;}}class ë{ɯ k=null;public B é=new B();public ş è
;public IMyTerminalBlock ç;public IMyTextSurface æ;public int å=0;public int ä=0;public string ã="";public string â="";
public bool á=true;public IMyTextSurface ê=>(Þ?æ:ç as IMyTextSurface);public int à=>(Þ?(k.ǃ(ç)?0:1):é.µ());public bool Þ=false
;public ë(ɯ Ý,string Ü){k=Ý;â=Ü;}public ë(ɯ Ý,string Ü,IMyTerminalBlock Û,IMyTextSurface E,int Ú){k=Ý;â=Ü;ç=Û;æ=E;å=Ú;Þ=
true;}public bool Ù(){return è.ŧ()>è.Ŝ||è.ś!=0;}float Ä=1.0f;bool W=false;public float r(){if(W)return Ä;W=true;if(ç.
BlockDefinition.SubtypeId.Contains("PanelWide")){if(ê.SurfaceSize.X<ê.SurfaceSize.Y)Ä=2.0f;}return Ä;}float U=1.0f;bool S=false;public
float R(){if(S)return U;S=true;if(ç.BlockDefinition.SubtypeId.Contains("PanelWide")){if(ê.SurfaceSize.X<ê.SurfaceSize.Y)U=
2.0f;}return U;}bool Q=false;public void P(){if(Q)return;if(!Þ){é.o();ç=é.w(0);}int O=ç.CustomName.IndexOf("!MARGIN:");if(O<
0||O+8>=ç.CustomName.Length){ä=1;ã=" ";}else{string N=ç.CustomName.Substring(O+8);int L=N.IndexOf(" ");if(L>=0)N=N.
Substring(0,L);if(!int.TryParse(N,out ä))ä=1;ã=new String(' ',ä);}if(ç.CustomName.Contains("!NOSCROLL"))á=false;else á=true;Q=
true;}public void K(ş J=null){if(è==null||ç==null)return;if(J==null)J=è;if(!Þ){IMyTextSurface E=ç as IMyTextSurface;if(E!=
null){float I=E.FontSize;string H=E.Font;for(int D=0;D<é.µ();D++){IMyTextSurface C=é.w(D)as IMyTextSurface;if(C==null)
continue;C.Alignment=VRage.Game.GUI.TextPanel.TextAlignment.LEFT;C.FontSize=I;C.Font=H;string G=J.ű(D).Ɇ();if(!k.ǆ.
SKIP_CONTENT_TYPE)C.ContentType=VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;C.WriteText(G);}}}else{æ.Alignment=VRage.Game.GUI.
TextPanel.TextAlignment.LEFT;if(!k.ǆ.SKIP_CONTENT_TYPE)æ.ContentType=VRage.Game.GUI.TextPanel.ContentType.TEXT_AND_IMAGE;æ.
WriteText(J.ű().Ɇ());}Q=false;}public void F(){if(ç==null)return;if(Þ){æ.WriteText("");return;}IMyTextSurface E=ç as
IMyTextSurface;if(E==null)return;for(int D=0;D<é.µ();D++){IMyTextSurface C=é.w(D)as IMyTextSurface;if(C==null)continue;C.WriteText("")
;}}}class B{Dictionary<string,IMyTerminalBlock>V=new Dictionary<string,IMyTerminalBlock>();Dictionary<IMyTerminalBlock,
string>A=new Dictionary<IMyTerminalBlock,string>();List<string>X=new List<string>();public void Ã(string z,IMyTerminalBlock º)
{if(!X.Contains(z)){X.Add(z);V.Add(z,º);A.Add(º,z);}}public void Á(string z){if(X.Contains(z)){X.Remove(z);A.Remove(V[z])
;V.Remove(z);}}public void À(IMyTerminalBlock º){if(A.ContainsKey(º)){X.Remove(A[º]);V.Remove(A[º]);A.Remove(º);}}public
int µ(){return V.Count;}public IMyTerminalBlock ª(string z){if(X.Contains(z))return V[z];return null;}public
IMyTerminalBlock w(int Â){return V[X[Â]];}public void v(){X.Clear();V.Clear();A.Clear();}public void o(){X.Sort();}}class n:ɕ{public ɯ k
;public ë j;ę f;public n(ę e){f=e;k=f.k;j=f.C;ɐ=0.5;ɔ="PanelDisplay";}double b=0;public void a(){b=0;}int Z=0;int Å=0;
bool ì=true;double Ċ=double.MaxValue;int ĝ=0;public override bool Ɏ(bool õ){Ɲ ě;if(!õ&&(f.Ĕ==false||f.ĕ==null||f.ĕ.Count<=0)
)return true;if(f.Ĝ.Ġ>3)return Ɋ(0);if(!õ){Å=0;ì=false;Ċ=double.MaxValue;ĝ=0;}if(ĝ==0){while(Å<f.ĕ.Count){if(!Ɩ.Ȟ(5))
return false;if(f.Ė.TryGetValue(f.ĕ[Å],out ě)){if(!ě.ɍ)return Ɋ(ě.ɓ-Ɩ.ȑ+0.001);if(ě.ɒ>b)ì=true;if(ě.ɓ<Ċ)Ċ=ě.ɓ;}Å++;}ĝ++;õ=
false;}double Ě=Ċ-Ɩ.ȑ+0.001;if(!ì&&!j.Ù())return Ɋ(Ě);k.Ǔ(j.è,j);if(ì){if(!õ){b=Ɩ.ȑ;j.è.ŵ();Z=0;}while(Z<f.ĕ.Count){if(!Ɩ.Ȟ(7
))return false;if(!f.Ė.TryGetValue(f.ĕ[Z],out ě)){j.è.Ř.Add(k.Ǐ(null,j));k.ý();k.Ǒ("ERR: No cmd task ("+f.ĕ[Z]+")");Z++;
continue;}j.è.Ũ(ě.Ĺ);Z++;}}k.Ǵ(j);f.Ĝ.Ġ++;if(ɐ<Ě&&!j.Ù())return Ɋ(Ě);return true;}}class ę:ɕ{public ɯ k;public ë C;public n Ę=
null;string ė="N/A";public Dictionary<string,Ɲ>Ė=new Dictionary<string,Ɲ>();public List<string>ĕ=null;public Ģ Ĝ;public bool
Ĕ{get{return Ĝ.Ć;}}public ę(Ģ Ē,ë đ){ɐ=5;C=đ;Ĝ=Ē;k=Ē.k;ɔ="PanelProcess";}Ǖ Đ;public override void ɇ(){Đ=k.Đ;}ˣ ď=null;Ɲ Ď
(string č,bool õ){if(!õ)ď=new ˣ(Ɩ);if(!ď.ʚ(č,õ))return null;Ɲ Č=ď.ˍ();Č.ƛ(this,ď);Ɩ.ȋ(Č,0);return Č;}string ċ="";void ē()
{try{ċ=C.ç.Ǫ(C.å,k.ɩ);}catch{ċ="";return;}ċ=ċ?.Replace("\\\n","");}int Z=0;int Ħ=0;List<string>į=null;HashSet<string>ĭ=
new HashSet<string>();int Ĭ=0;bool ī(bool õ){if(!õ){char[]Ī={';','\n'};string ĩ=ċ.Replace("\\;","\f");if(ĩ.StartsWith("@"))
{int Ĩ=ĩ.IndexOf("\n");if(Ĩ<0){ĩ="";}else{ĩ=ĩ.Substring(Ĩ+1);}}į=new List<string>(ĩ.Split(Ī,StringSplitOptions.
RemoveEmptyEntries));ĭ.Clear();Z=0;Ħ=0;Ĭ=0;}while(Z<į.Count){if(!Ɩ.Ȟ(500))return false;if(į[Z].StartsWith("//")){į.RemoveAt(Z);continue;}į
[Z]=į[Z].Replace('\f',';');if(!Ė.ContainsKey(į[Z])){if(Ĭ!=1)õ=false;Ĭ=1;Ɲ ě=Ď(į[Z],õ);if(ě==null)return false;õ=false;Ė.
Add(į[Z],ě);Ĭ=0;}if(!ĭ.Contains(į[Z]))ĭ.Add(į[Z]);Z++;}if(ĕ!=null){Ɲ Č;while(Ħ<ĕ.Count){if(!Ɩ.Ȟ(7))return false;if(!ĭ.
Contains(ĕ[Ħ]))if(Ė.TryGetValue(ĕ[Ħ],out Č)){Č.Ɉ();Ė.Remove(ĕ[Ħ]);}Ħ++;}}ĕ=į;return true;}public override void ɣ(){if(ĕ!=null){Ɲ
Č;for(int Į=0;Į<ĕ.Count;Į++){if(Ė.TryGetValue(ĕ[Į],out Č))Č.Ɉ();}ĕ=null;}if(Ę!=null){Ę.Ɉ();Ę=null;}else{}}ş ħ=null;string
ĥ="";bool Ĥ=false;public override bool Ɏ(bool õ){if(C.à<=0){Ɉ();return true;}if(!õ){C.è=k.Ǔ(C.è,C);ħ=k.Ǔ(ħ,C);ē();if(ċ==
null){if(C.Þ){Ĝ.ò(C.æ,C.ç as IMyTextPanel);}else{Ɉ();}return true;}if(C.ç.CustomName!=ĥ){Ĥ=true;}else{Ĥ=false;}ĥ=C.ç.
CustomName;}if(ċ!=ė){if(!ī(õ))return false;if(ċ==""){ė="";if(Ĝ.Ć){if(ħ.Ř.Count<=0)ħ.Ř.Add(k.Ǐ(null,C));else k.Ǐ(ħ.Ř[0],C);k.ý();k.
Ǒ(Đ.Ǥ("H1"));bool ģ=C.á;C.á=false;k.Ǵ(C,ħ);C.á=ģ;return true;}return this.Ɋ(2);}Ĥ=true;}ė=ċ;if(Ę!=null&&Ĥ){Ɩ.Ȫ(Ę);Ę.a();Ɩ
.ȋ(Ę,0);}else if(Ę==null){Ę=new n(this);Ɩ.ȋ(Ę,0);}return true;}}class Ģ:ɕ{const string ġ="T:!LCD!";public int Ġ=0;public
ɯ k;public ƃ é=new ƃ();δ ğ;δ Ğ;Dictionary<ë,ę>ĉ=new Dictionary<ë,ę>();public Dictionary<IMyTextSurface,ë>û=new Dictionary
<IMyTextSurface,ë>();public bool Ć=false;Ϛ ù=null;public Ģ(ɯ Ý){ɐ=5;k=Ý;ɔ="ProcessPanels";}public override void ɇ(){ğ=new
δ(Ɩ,k.œ);Ğ=new δ(Ɩ,k.œ);ù=new Ϛ(k,this);}int ø=0;bool ö(bool õ){if(!õ)ø=0;if(ø==0){if(!ğ.ν(k.ɩ,õ))return false;ø++;õ=
false;}if(ø==1){if(k.ɩ=="T:[LCD]"&&ġ!="")if(!ğ.ν(ġ,õ))return false;ø++;õ=false;}return true;}string ô(IMyTerminalBlock ç){int
ó=ç.CustomName.IndexOf("!LINK:");if(ó>=0&&ç.CustomName.Length>ó+6){return ç.CustomName.Substring(ó+6)+' '+ç.Position.
ToString();}return ç.EntityId.ToString();}public void ò(IMyTextSurface E,IMyTextPanel C){ë j;if(E==null)return;if(!û.TryGetValue
(E,out j))return;if(C!=null){j.é.À(C);}û.Remove(E);if(j.à<=0||j.Þ){ę ñ;if(ĉ.TryGetValue(j,out ñ)){é.À(j.â);ĉ.Remove(j);ñ.
Ɉ();}}}void ð(IMyTerminalBlock ç){IMyTextSurfaceProvider ï=ç as IMyTextSurfaceProvider;IMyTextSurface E=ç as
IMyTextSurface;if(E!=null){ò(E,ç as IMyTextPanel);return;}if(ï==null)return;for(int D=0;D<ï.SurfaceCount;D++){E=ï.GetSurface(D);ò(E,
null);}}string Ü;string î;bool ú;int í=0;int ü=0;public override bool Ɏ(bool õ){if(!õ){ğ.ŵ();í=0;ü=0;}if(!ö(õ))return false;
while(í<ğ.Ћ()){if(!Ɩ.Ȟ(20))return false;IMyTerminalBlock ç=(ğ.λ[í]as IMyTerminalBlock);if(ç==null||!ç.IsWorking){ğ.λ.RemoveAt
(í);continue;}IMyTextSurfaceProvider ï=ç as IMyTextSurfaceProvider;IMyTextSurface E=ç as IMyTextSurface;IMyTextPanel C=ç
as IMyTextPanel;ë j;Ü=ô(ç);string[]Ĉ=Ü.Split(' ');î=Ĉ[0];ú=Ĉ.Length>1;if(C!=null){if(û.ContainsKey(E)){j=û[E];if(j.â==Ü+
"@0"||(ú&&j.â==î)){í++;continue;}ð(ç);}if(!ú){j=new ë(k,Ü+"@0",ç,E,0);ę ñ=new ę(this,j);Ɩ.ȋ(ñ,0);ĉ.Add(j,ñ);é.Ã(j.â,j);û.Add
(E,j);í++;continue;}j=é.ª(î);if(j==null){j=new ë(k,î);é.Ã(î,j);ę ñ=new ę(this,j);Ɩ.ȋ(ñ,0);ĉ.Add(j,ñ);}j.é.Ã(Ü,ç);û.Add(E,
j);}else{if(ï==null){í++;continue;}for(int D=0;D<ï.SurfaceCount;D++){E=ï.GetSurface(D);if(û.ContainsKey(E)){j=û[E];if(j.â
==Ü+'@'+D.ToString()){continue;}ò(E,null);}if(ç.Ǫ(D,k.ɩ)==null)continue;j=new ë(k,Ü+"@"+D.ToString(),ç,E,D);ę ñ=new ę(this
,j);Ɩ.ȋ(ñ,0);ĉ.Add(j,ñ);é.Ã(j.â,j);û.Add(E,j);}}í++;}while(ü<Ğ.Ћ()){if(!Ɩ.Ȟ(300))return false;IMyTerminalBlock ç=Ğ.λ[ü];
if(ç==null)continue;if(!ğ.λ.Contains(ç)){ð(ç);}ü++;}Ğ.ŵ();Ğ.Ϲ(ğ);if(!ù.ɏ&&ù.Ϭ())Ɩ.ȋ(ù,0);return true;}public bool ć(string
ą){if(string.Compare(ą,"clear",true)==0){ù.ϱ();if(!ù.ɏ)Ɩ.ȋ(ù,0);return true;}if(string.Compare(ą,"boot",true)==0){ù.Ϧ=0;
if(!ù.ɏ)Ɩ.ȋ(ù,0);return true;}if(ą.ǰ("scroll")){θ Ą=new θ(k,this,ą);Ɩ.ȋ(Ą,0);return true;}if(string.Compare(ą,"props",true
)==0){Љ ă=k.œ;List<IMyTerminalBlock>Ă=new List<IMyTerminalBlock>();List<ITerminalAction>ā=new List<ITerminalAction>();
List<ITerminalProperty>Ā=new List<ITerminalProperty>();IMyTextPanel ÿ=Ɩ.ǆ.GridTerminalSystem.GetBlockWithName("DEBUG")as
IMyTextPanel;if(ÿ==null){return true;}ÿ.WriteText("Properties: ");foreach(var º in ă.І){ÿ.WriteText(º.Key+" =============="+"\n",
true);º.Value(Ă,null);if(Ă.Count<=0){ÿ.WriteText("No blocks\n",true);continue;}Ă[0].GetProperties(Ā,(j)=>{return j.Id!=
"Name"&&j.Id!="OnOff"&&!j.Id.StartsWith("Show");});foreach(var þ in Ā){ÿ.WriteText("P "+þ.Id+" "+þ.TypeName+"\n",true);}Ā.
Clear();Ă.Clear();}}return false;}}class ƃ{Dictionary<string,ë>Ɗ=new Dictionary<string,ë>();List<string>X=new List<string>();
public void Ã(string z,ë º){if(!Ɗ.ContainsKey(z)){X.Add(z);Ɗ.Add(z,º);}}public int µ(){return Ɗ.Count;}public ë ª(string z){if
(Ɗ.ContainsKey(z))return Ɗ[z];return null;}public ë w(int Â){return Ɗ[X[Â]];}public void À(string z){Ɗ.Remove(z);X.Remove
(z);}public void v(){X.Clear();Ɗ.Clear();}public void o(){X.Sort();}}class ȵ{ȕ Ɩ;ɯ k;public MyDefinitionId ȴ=new
MyDefinitionId(typeof(VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_GasProperties),"Electricity");public MyDefinitionId ȳ=new
MyDefinitionId(typeof(VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_GasProperties),"Oxygen");public MyDefinitionId Ȳ=new
MyDefinitionId(typeof(VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_GasProperties),"Hydrogen");public ȵ(ȕ ƞ,ɯ Ý){Ɩ=ƞ;k=Ý;}int
ȱ=0;public bool Ȱ(List<IMyTerminalBlock>Ă,ref double ȶ,ref double Ⱦ,ref double Ƚ,ref double ȼ,ref double Ƀ,ref double ɂ,
bool õ){if(!õ)ȱ=0;MyResourceSinkComponent Ȥ;MyResourceSourceComponent Ȼ;for(;ȱ<Ă.Count;ȱ++){if(!Ɩ.Ȟ(8))return false;if(Ă[ȱ].
Components.TryGet<MyResourceSinkComponent>(out Ȥ)){ȶ+=Ȥ.CurrentInputByType(ȴ);Ⱦ+=Ȥ.MaxRequiredInputByType(ȴ);}if(Ă[ȱ].Components.
TryGet<MyResourceSourceComponent>(out Ȼ)){Ƚ+=Ȼ.CurrentOutputByType(ȴ);ȼ+=Ȼ.MaxOutputByType(ȴ);}IMyBatteryBlock Ɂ=(Ă[ȱ]as
IMyBatteryBlock);Ƀ+=Ɂ.CurrentStoredPower;ɂ+=Ɂ.MaxStoredPower;}return true;}int ɀ=0;public bool ȿ(List<IMyTerminalBlock>Ă,MyDefinitionId
Ʉ,ref double ȶ,ref double Ⱦ,ref double Ƚ,ref double ȼ,bool õ){if(!õ)ɀ=0;MyResourceSinkComponent Ȥ;
MyResourceSourceComponent Ȼ;for(;ɀ<Ă.Count;ɀ++){if(!Ɩ.Ȟ(6))return false;if(Ă[ɀ].Components.TryGet<MyResourceSinkComponent>(out Ȥ)){ȶ+=Ȥ.
CurrentInputByType(Ʉ);Ⱦ+=Ȥ.MaxRequiredInputByType(Ʉ);}if(Ă[ɀ].Components.TryGet<MyResourceSourceComponent>(out Ȼ)){Ƚ+=Ȼ.
CurrentOutputByType(Ʉ);ȼ+=Ȼ.MaxOutputByType(Ʉ);}}return true;}int Ⱥ=0;public bool ȹ(List<IMyTerminalBlock>Ă,string ȸ,ref double ȷ,ref
double Ȯ,bool õ){if(!õ){Ⱥ=0;Ȯ=0;ȷ=0;}MyResourceSinkComponent Ȥ;for(;Ⱥ<Ă.Count;Ⱥ++){if(!Ɩ.Ȟ(30))return false;IMyGasTank Ő=Ă[Ⱥ]
as IMyGasTank;if(Ő==null)continue;double ș=0;if(Ő.Components.TryGet<MyResourceSinkComponent>(out Ȥ)){ListReader<
MyDefinitionId>Ș=Ȥ.AcceptedResources;int D=0;for(;D<Ș.Count;D++){if(string.Compare(Ș[D].SubtypeId.ToString(),ȸ,true)==0){ș=Ő.Capacity;
Ȯ+=ș;ȷ+=ș*Ő.FilledRatio;break;}}}}return true;}public string ȗ(TimeSpan Ȗ){string Ĺ="";if(Ȗ.Ticks<=0)return"-";if((int)Ȗ.
TotalDays>0)Ĺ+=(long)Ȗ.TotalDays+" "+k.Đ.Ǥ("C5")+" ";if(Ȗ.Hours>0||Ĺ!="")Ĺ+=Ȗ.Hours+"h ";if(Ȗ.Minutes>0||Ĺ!="")Ĺ+=Ȗ.Minutes+"m ";
return Ĺ+Ȗ.Seconds+"s";}}class ȕ{public const double Ȕ=0.05;public const int ȓ=1000;public const int Ȓ=10000;public double ȑ{
get{return ȏ;}}int Ȑ=ȓ;double ȏ=0;List<ɕ>Ȏ=new List<ɕ>(100);public MyGridProgram ǆ;public bool ȍ=false;int Ȍ=0;public ȕ(
MyGridProgram ƿ,int ƾ=1,bool Ț=false){ǆ=ƿ;Ȍ=ƾ;ȍ=Ț;}public void ȋ(ɕ ñ,double ȭ,bool ȫ=false){ñ.ɏ=true;ñ.ɋ(this);if(ȫ){ñ.ɓ=ȑ;Ȏ.Insert(0
,ñ);return;}if(ȭ<=0)ȭ=0.001;ñ.ɓ=ȑ+ȭ;for(int D=0;D<Ȏ.Count;D++){if(Ȏ[D].ɓ>ñ.ɓ){Ȏ.Insert(D,ñ);return;}if(ñ.ɓ-Ȏ[D].ɓ<Ȕ)ñ.ɓ=Ȏ
[D].ɓ+Ȕ;}Ȏ.Add(ñ);}public void Ȫ(ɕ ñ){if(Ȏ.Contains(ñ)){Ȏ.Remove(ñ);ñ.ɏ=false;}}public void Ȩ(ʌ ȩ,int Ȧ=1){if(Ȍ==Ȧ)ǆ.Echo
(ȩ.Ɇ());}public void Ȩ(string ȧ,int Ȧ=1){if(Ȍ==Ȧ)ǆ.Echo(ȧ);}const double Ȭ=(16.66666666/16);double ȥ=0;public void ȣ(){ȥ
+=ǆ.Runtime.TimeSinceLastRun.TotalSeconds*Ȭ;}ʌ Ư=new ʌ();public void Ȣ(){double ȡ=ǆ.Runtime.TimeSinceLastRun.TotalSeconds*
Ȭ+ȥ;ȥ=0;ȏ+=ȡ;Ȑ=(int)Math.Min((ȡ*60)*ȓ/(ȍ?5:1),Ȓ-1000);while(Ȏ.Count>=1){ɕ ñ=Ȏ[0];if(Ȑ-ǆ.Runtime.CurrentInstructionCount<=
0)break;if(ñ.ɓ>ȏ){int Ƞ=(int)(60*(ñ.ɓ-ȏ));if(Ƞ>=100){ǆ.Runtime.UpdateFrequency=UpdateFrequency.Update100;}else{if(Ƞ>=10||
ȍ)ǆ.Runtime.UpdateFrequency=UpdateFrequency.Update10;else ǆ.Runtime.UpdateFrequency=UpdateFrequency.Update1;}break;}Ȏ.
Remove(ñ);if(!ñ.ɉ())break;}}public int ȟ(){return(Ȓ-ǆ.Runtime.CurrentInstructionCount);}public bool Ȟ(int ȝ){return((Ȑ-ǆ.
Runtime.CurrentInstructionCount)>=ȝ);}public void Ȝ(){Ȩ(Ư.ŵ().ʈ("Remaining Instr: ").ʈ(ȟ()));}}class ȯ:ɕ{MyShipVelocities ț;
public Vector3D Ʌ{get{return ț.LinearVelocity;}}public Vector3D ʅ{get{return ț.AngularVelocity;}}double ʃ=0;public double ʂ{
get{if(ɳ!=null)return ɳ.GetShipSpeed();else return ʃ;}}double ʁ=0;public double ʀ{get{return ʁ;}}double ɿ=0;public double ɾ
{get{return ɿ;}}double ɽ=0;double ɼ=0;public double ɻ{get{return ɽ;}}MyShipMass ɺ;public double ʄ{get{return ɺ.TotalMass;
}}public double ɹ{get{return ɺ.BaseMass;}}double ɷ=double.NaN;public double ɶ{get{return ɷ;}}double ɵ=double.NaN;public
double ɴ{get{return ɵ;}}IMyShipController ɳ=null;IMySlimBlock ɲ=null;public IMyShipController ɱ{get{return ɳ;}}Vector3D ɰ;
public ȯ(ȕ ƞ){ɔ="ShipMgr";Ɩ=ƞ;ɰ=Ɩ.ǆ.Me.GetPosition();ɐ=0.5;}List<IMyTerminalBlock>ɸ=new List<IMyTerminalBlock>();int ʆ=0;
public override bool Ɏ(bool õ){if(!õ){ɸ.Clear();Ɩ.ǆ.GridTerminalSystem.GetBlocksOfType<IMyShipController>(ɸ);ʆ=0;if(ɳ!=null&&ɳ
.CubeGrid.GetCubeBlock(ɳ.Position)!=ɲ)ɳ=null;}if(ɸ.Count>0){for(;ʆ<ɸ.Count;ʆ++){if(!Ɩ.Ȟ(20))return false;
IMyShipController ʏ=ɸ[ʆ]as IMyShipController;if(ʏ.IsMainCockpit||ʏ.IsUnderControl){ɳ=ʏ;ɲ=ʏ.CubeGrid.GetCubeBlock(ʏ.Position);if(ʏ.
IsMainCockpit){ʆ=ɸ.Count;break;}}}if(ɳ==null){ɳ=ɸ[0]as IMyShipController;ɲ=ɳ.CubeGrid.GetCubeBlock(ɳ.Position);}ɺ=ɳ.CalculateShipMass
();if(!ɳ.TryGetPlanetElevation(MyPlanetElevation.Sealevel,out ɷ))ɷ=double.NaN;if(!ɳ.TryGetPlanetElevation(
MyPlanetElevation.Surface,out ɵ))ɵ=double.NaN;ț=ɳ.GetShipVelocities();}double ʍ=ʃ;ʃ=Ʌ.Length();ʁ=(ʃ-ʍ)/ɑ;if(-ʁ>ɿ)ɿ=-ʁ;if(-ʁ>ɽ){ɽ=-ʁ;ɼ=Ɩ.ȑ
;}if(Ɩ.ȑ-ɼ>5&&-ʁ>0.1)ɽ-=(ɽ+ʁ)*0.3f;return true;}}class ʌ{public StringBuilder Ư;public ʌ(int ʋ=0){Ư=new StringBuilder(ʋ);
}public int ʊ{get{return Ư.Length;}}public ʌ ŵ(){Ư.Clear();return this;}public ʌ ʈ(string ĩ){Ư.Append(ĩ);return this;}
public ʌ ʈ(double ʎ){Ư.Append(ʎ);return this;}public ʌ ʈ(char ǳ){Ư.Append(ǳ);return this;}public ʌ ʈ(ʌ ʉ){Ư.Append(ʉ.Ư);return
this;}public ʌ ʈ(string ĩ,int Ȃ,int ɖ){Ư.Append(ĩ,Ȃ,ɖ);return this;}public ʌ ʈ(char ǳ,int Ţ){Ư.Append(ǳ,Ţ);return this;}
public ʌ ʇ(int Ȃ,int ɖ){Ư.Remove(Ȃ,ɖ);return this;}public string Ɇ(){return Ư.ToString();}public string Ɇ(int Ȃ,int ɖ){return
Ư.ToString(Ȃ,ɖ);}public char this[int z]{get{return Ư[z];}}}class ɕ{public string ɔ="MMTask";public double ɓ=0;public
double ɒ=0;public double ɑ=0;public double ɐ=-1;double ɗ=-1;public bool ɏ=false;public bool ɍ=false;double Ɍ=0;protected ȕ Ɩ;
public void ɋ(ȕ ƞ){Ɩ=ƞ;if(Ɩ.ȍ){if(ɗ==-1){ɗ=ɐ;ɐ*=2;}else{ɐ=ɗ*2;}}else{if(ɗ!=-1){ɐ=ɗ;ɗ=-1;}}}protected bool Ɋ(double ȭ){Ɍ=Math.
Max(ȭ,0.0001);return true;}public bool ɉ(){if(ɒ>0){ɑ=Ɩ.ȑ-ɒ;Ɩ.Ȩ((ɍ?"Running":"Resuming")+" task: "+ɔ);ɍ=Ɏ(!ɍ);}else{ɑ=0;Ɩ.Ȩ(
"Init task: "+ɔ);ɇ();Ɩ.Ȩ("Running..");ɍ=Ɏ(false);if(!ɍ)ɒ=0.001;}if(ɍ){ɒ=Ɩ.ȑ;if((ɐ>=0||Ɍ>0)&&ɏ)Ɩ.ȋ(this,(Ɍ>0?Ɍ:ɐ));else{ɏ=false;ɒ=0;}}
else{if(ɏ)Ɩ.ȋ(this,0,true);}Ɩ.Ȩ("Task "+(ɍ?"":"NOT ")+"finished. "+(ɏ?(Ɍ>0?"Postponed by "+Ɍ.ToString("F1")+"s":
"Scheduled after "+ɐ.ToString("F1")+"s"):"Stopped."));Ɍ=0;return ɍ;}public void Ɉ(){Ɩ.Ȫ(this);ɣ();ɏ=false;ɍ=false;ɒ=0;}public virtual void
ɇ(){}public virtual bool Ɏ(bool õ){return true;}public virtual void ɣ(){}}class ɯ{public const float ɮ=512;public const
float ɭ=ɮ/0.7783784f;public const float ɬ=ɮ/0.7783784f;public const float ɫ=ɭ;public const float ɪ=37;public string ɩ=
"T:[LCD]";public int ɨ=1;public bool ɧ=true;public List<string>ɦ=null;public bool ɥ=true;public int Ȍ=0;public float ɤ=1.0f;
public float ɢ=1.0f;public float ɡ{get{return ɫ*ƽ.ћ;}}public float ɠ{get{return(float)ɡ-2*ɘ[Ƽ]*ä;}}string ɟ;string ɞ;float ɝ=-
1;Dictionary<string,float>ɜ=new Dictionary<string,float>(2);Dictionary<string,float>ɛ=new Dictionary<string,float>(2);
Dictionary<string,float>ɚ=new Dictionary<string,float>(2);public float ə{get{return ɚ[Ƽ];}}Dictionary<string,float>ɘ=new
Dictionary<string,float>(2);Dictionary<string,float>Ȋ=new Dictionary<string,float>(2);Dictionary<string,float>ǋ=new Dictionary<
string,float>(2);int ä=0;string ã="";Dictionary<string,char>Ǌ=new Dictionary<string,char>(2);Dictionary<string,char>ǉ=new
Dictionary<string,char>(2);Dictionary<string,char>ǈ=new Dictionary<string,char>(2);Dictionary<string,char>Ǉ=new Dictionary<string,
char>(2);public ȕ Ɩ;public Program ǆ;public ȵ ǅ;public Љ œ;public ȯ Ǆ;public ƈ ƒ;public Ǖ Đ;public IMyGridTerminalSystem ǂ{
get{return ǆ.GridTerminalSystem;}}public IMyProgrammableBlock ǁ{get{return ǆ.Me;}}public Action<string>ǀ{get{return ǆ.Echo;
}}public ɯ(Program ƿ,int ƾ,ȕ ƞ){Ɩ=ƞ;Ȍ=ƾ;ǆ=ƿ;Đ=new Ǖ();ǅ=new ȵ(ƞ,this);œ=new Љ(ƞ,this);œ.Ѕ();Ǆ=new ȯ(Ɩ);Ɩ.ȋ(Ǆ,0);}ќ ƽ=null
;public string Ƽ{get{return ƽ.H;}}public bool ƻ{get{return(ƽ.ŧ()==0);}}public bool ǃ(IMyTerminalBlock ç){if(ç==null||ç.
WorldMatrix==MatrixD.Identity)return true;return ǂ.GetBlockWithId(ç.EntityId)==null;}public ќ Ǐ(ќ ǔ,ë j){j.P();IMyTextSurface E=j.ê
;if(ǔ==null)ǔ=new ќ(this);ǔ.H=E.Font;if(!ɘ.ContainsKey(ǔ.H))ǔ.H=ɟ;ǔ.ћ=(E.SurfaceSize.X/E.TextureSize.X)*(E.TextureSize.X/
E.TextureSize.Y)*ɤ/E.FontSize*(100f-E.TextPadding*2)/100*j.R();ã=j.ã;ä=j.ä;ƽ=ǔ;return ǔ;}public ş Ǔ(ş è,ë j){j.P();
IMyTextSurface E=j.ê;if(è==null)è=new ş(this);è.Ŗ(j.à);è.Ş=j.r()*(E.SurfaceSize.Y/E.TextureSize.Y)*ɢ/E.FontSize*(100f-E.TextPadding*2)
/100;è.ŝ();ã=j.ã;ä=j.ä;return è;}public void Ǒ(){ƽ.ј();}public void Ǒ(ʌ ǒ){if(ƽ.њ<=0)ƽ.љ(ã);ƽ.љ(ǒ);ƽ.ј();}public void Ǒ(
string ǒ){if(ƽ.њ<=0)ƽ.љ(ã);ƽ.ј(ǒ);}public void ǐ(string ǎ){ƽ.ѕ(ǎ,ã);}public void Ǎ(List<ʌ>ǌ){ƽ.ї(ǌ);}public void Ƅ(ʌ Ʈ,bool ƶ=
true){if(ƽ.њ<=0)ƽ.љ(ã);ƽ.љ(Ʈ);if(ƶ)ƽ.њ+=ǡ(Ʈ,ƽ.H);}public void Ƅ(string Ĺ,bool ƶ=true){if(ƽ.њ<=0)ƽ.љ(ã);ƽ.љ(Ĺ);if(ƶ)ƽ.њ+=ǡ(Ĺ,
ƽ.H);}public void Ʒ(ʌ Ʈ,float Ƭ=1.0f,float ƫ=0f){ƭ(Ʈ,Ƭ,ƫ);ƽ.ј();}public void Ʒ(string Ĺ,float Ƭ=1.0f,float ƫ=0f){ƭ(Ĺ,Ƭ,ƫ)
;ƽ.ј();}ʌ Ư=new ʌ();public void ƭ(ʌ Ʈ,float Ƭ=1.0f,float ƫ=0f){float ƪ=ǡ(Ʈ,ƽ.H);float Ʃ=Ƭ*ɫ*ƽ.ћ-ƽ.њ-ƫ;if(ä>0)Ʃ-=2*ɘ[ƽ.H]*
ä;if(Ʃ<ƪ){ƽ.љ(Ʈ);ƽ.њ+=ƪ;return;}Ʃ-=ƪ;int ƨ=(int)Math.Floor(Ʃ/ɘ[ƽ.H]);float Ƨ=ƨ*ɘ[ƽ.H];Ư.ŵ().ʈ(' ',ƨ).ʈ(Ʈ);ƽ.љ(Ư);ƽ.њ+=Ƨ+ƪ
;}public void ƭ(string Ĺ,float Ƭ=1.0f,float ƫ=0f){float ƪ=ǡ(Ĺ,ƽ.H);float Ʃ=Ƭ*ɫ*ƽ.ћ-ƽ.њ-ƫ;if(ä>0)Ʃ-=2*ɘ[ƽ.H]*ä;if(Ʃ<ƪ){ƽ.љ
(Ĺ);ƽ.њ+=ƪ;return;}Ʃ-=ƪ;int ƨ=(int)Math.Floor(Ʃ/ɘ[ƽ.H]);float Ƨ=ƨ*ɘ[ƽ.H];Ư.ŵ().ʈ(' ',ƨ).ʈ(Ĺ);ƽ.љ(Ư);ƽ.њ+=Ƨ+ƪ;}public void
Ʀ(ʌ Ʈ){ƺ(Ʈ);ƽ.ј();}public void Ʀ(string Ĺ){ƺ(Ĺ);ƽ.ј();}public void ƺ(ʌ Ʈ){float ƪ=ǡ(Ʈ,ƽ.H);float ƹ=ɫ/2*ƽ.ћ-ƽ.њ;if(ƹ<ƪ/2){
ƽ.љ(Ʈ);ƽ.њ+=ƪ;return;}ƹ-=ƪ/2;int ƨ=(int)Math.Round(ƹ/ɘ[ƽ.H],MidpointRounding.AwayFromZero);float Ƨ=ƨ*ɘ[ƽ.H];Ư.ŵ().ʈ(' ',ƨ
).ʈ(Ʈ);ƽ.љ(Ư);ƽ.њ+=Ƨ+ƪ;}public void ƺ(string Ĺ){float ƪ=ǡ(Ĺ,ƽ.H);float ƹ=ɫ/2*ƽ.ћ-ƽ.њ;if(ƹ<ƪ/2){ƽ.љ(Ĺ);ƽ.њ+=ƪ;return;}ƹ-=ƪ
/2;int ƨ=(int)Math.Round(ƹ/ɘ[ƽ.H],MidpointRounding.AwayFromZero);float Ƨ=ƨ*ɘ[ƽ.H];Ư.ŵ().ʈ(' ',ƨ).ʈ(Ĺ);ƽ.љ(Ư);ƽ.њ+=Ƨ+ƪ;}
public void Ƹ(double Ʊ,float ư=1.0f,float ƫ=0f,bool ƶ=true){if(ä>0)ƫ+=2*ä*ɘ[ƽ.H];float Ƶ=ɫ*ư*ƽ.ћ-ƽ.њ-ƫ;if(Double.IsNaN(Ʊ))Ʊ=0;
int ƴ=(int)(Ƶ/Ȋ[ƽ.H])-2;if(ƴ<=0)ƴ=2;int Ƴ=Math.Min((int)(Ʊ*ƴ)/100,ƴ);if(Ƴ<0)Ƴ=0;if(ƽ.њ<=0)ƽ.љ(ã);Ư.ŵ().ʈ(Ǌ[ƽ.H]).ʈ(Ǉ[ƽ.H],Ƴ
).ʈ(ǈ[ƽ.H],ƴ-Ƴ).ʈ(ǉ[ƽ.H]);ƽ.љ(Ư);if(ƶ)ƽ.њ+=Ȋ[ƽ.H]*ƴ+2*ǋ[ƽ.H];}public void Ʋ(double Ʊ,float ư=1.0f,float ƫ=0f){Ƹ(Ʊ,ư,ƫ,
false);ƽ.ј();}public void ý(){ƽ.ý();}public void Ǵ(ë C,ş J=null){C.K(J);if(C.á)C.è.Ļ();}public void ǻ(string Ǻ,string Ĺ){
IMyTextPanel C=ǆ.GridTerminalSystem.GetBlockWithName(Ǻ)as IMyTextPanel;if(C==null)return;C.WriteText(Ĺ+"\n",true);}public string ǹ(
MyInventoryItem º){string Ǹ=º.Type.TypeId.ToString();Ǹ=Ǹ.Substring(Ǹ.LastIndexOf('_')+1);return º.Type.SubtypeId+" "+Ǹ;}public void Ƿ(
string Ô,out string Ò,out string Ñ){int ţ=Ô.LastIndexOf(' ');if(ţ>=0){Ò=Ô.Substring(0,ţ);Ñ=Ô.Substring(ţ+1);return;}Ò=Ô;Ñ="";}
public string Ƕ(string Ô){string Ò,Ñ;Ƿ(Ô,out Ò,out Ñ);return Ƕ(Ò,Ñ);}public string Ƕ(string Ò,string Ñ){Ð º=ƒ.Ó(Ò,Ñ);if(º!=
null){if(º.Ë.Length>0)return º.Ë;return º.Î;}return System.Text.RegularExpressions.Regex.Replace(Ò,"([a-z])([A-Z])","$1 $2")
;}public void ǵ(ref string Ò,ref string Ñ){Ð º;if(ƒ.ƅ.TryGetValue(Ò,out º)){Ò=º.Î;Ñ=º.Í;return;}º=ƒ.Ó(Ò,Ñ);if(º!=null){Ò=
º.Î;if((string.Compare(Ñ,"Ore",true)==0)||(string.Compare(Ñ,"Ingot",true)==0))return;Ñ=º.Í;}}public string ȉ(double Ȇ,
bool ȅ=true,char Ȉ=' '){if(!ȅ)return Ȇ.ToString("#,###,###,###,###,###,###,###,###,###");string Ȅ=" kMGTPEZY";double ȃ=Ȇ;int
Ȃ=Ȅ.IndexOf(Ȉ);var ȁ=(Ȃ<0?0:Ȃ);while(ȃ>=1000&&ȁ+1<Ȅ.Length){ȃ/=1000;ȁ++;}Ư.ŵ().ʈ(Math.Round(ȃ,1,MidpointRounding.
AwayFromZero));if(ȁ>0)Ư.ʈ(" ").ʈ(Ȅ[ȁ]);return Ư.Ɇ();}public string ȇ(double Ȇ,bool ȅ=true,char Ȉ=' '){if(!ȅ)return Ȇ.ToString(
"#,###,###,###,###,###,###,###,###,###");string Ȅ=" ktkMGTPEZY";double ȃ=Ȇ;int Ȃ=Ȅ.IndexOf(Ȉ);var ȁ=(Ȃ<0?0:Ȃ);while(ȃ>=1000&&ȁ+1<Ȅ.Length){ȃ/=1000;ȁ++;}Ư.ŵ().ʈ
(Math.Round(ȃ,1,MidpointRounding.AwayFromZero));if(ȁ==1)Ư.ʈ(" kg");else if(ȁ==2)Ư.ʈ(" t");else if(ȁ>2)Ư.ʈ(" ").ʈ(Ȅ[ȁ]).ʈ(
"t");return Ư.Ɇ();}public string Ȁ(double Ʊ){return(Math.Floor(Ʊ*10)/10).ToString("F1");}Dictionary<char,float>ǿ=new
Dictionary<char,float>();void Ǿ(string ǽ,float I){I+=1;for(int D=0;D<ǽ.Length;D++){if(I>ɜ[ɟ])ɜ[ɟ]=I;ǿ.Add(ǽ[D],I);}}public float Ǽ
(char ǳ,string H){float Ƶ;if(H==ɞ||!ǿ.TryGetValue(ǳ,out Ƶ))return ɜ[H];return Ƶ;}public float ǡ(ʌ Ǣ,string H){if(H==ɞ)
return Ǣ.ʊ*ɜ[H];float Ǡ=0;for(int D=0;D<Ǣ.ʊ;D++)Ǡ+=Ǽ(Ǣ[D],H);return Ǡ;}public float ǡ(string ĩ,string H){if(H==ɞ)return ĩ.
Length*ɜ[H];float Ǡ=0;for(int D=0;D<ĩ.Length;D++)Ǡ+=Ǽ(ĩ[D],H);return Ǡ;}public string ǟ(string Ĺ,float Ǟ){if(Ǟ/ɜ[ƽ.H]>=Ĺ.
Length)return Ĺ;float ǝ=ǡ(Ĺ,ƽ.H);if(ǝ<=Ǟ)return Ĺ;float ǜ=ǝ/Ĺ.Length;Ǟ-=ɛ[ƽ.H];int Ǜ=(int)Math.Max(Ǟ/ǜ,1);if(Ǜ<Ĺ.Length/2){Ư.ŵ
().ʈ(Ĺ,0,Ǜ);ǝ=ǡ(Ư,ƽ.H);}else{Ư.ŵ().ʈ(Ĺ);Ǜ=Ĺ.Length;}while(ǝ>Ǟ&&Ǜ>1){Ǜ--;ǝ-=Ǽ(Ĺ[Ǜ],ƽ.H);}if(Ư.ʊ>Ǜ)Ư.ʇ(Ǜ,Ư.ʊ-Ǜ);return Ư.ʈ(
"..").Ɇ();}void ǚ(string Ǚ){ɟ=Ǚ;Ǌ[ɟ]=MMStyle.BAR_START;ǉ[ɟ]=MMStyle.BAR_END;ǈ[ɟ]=MMStyle.BAR_EMPTY;Ǉ[ɟ]=MMStyle.BAR_FILL;ɜ[ɟ
]=0f;}void ǘ(string Ǘ,float ǖ){ɞ=Ǘ;ɝ=ǖ;ɜ[ɞ]=ɝ+1;ɛ[ɞ]=2*(ɝ+1);Ǌ[ɞ]=MMStyle.BAR_MONO_START;ǉ[ɞ]=MMStyle.BAR_MONO_END;ǈ[ɞ]=
MMStyle.BAR_MONO_EMPTY;Ǉ[ɞ]=MMStyle.BAR_MONO_FILL;ɘ[ɞ]=Ǽ(' ',ɞ);Ȋ[ɞ]=Ǽ(ǈ[ɞ],ɞ);ǋ[ɞ]=Ǽ(Ǌ[ɞ],ɞ);ɚ[ɞ]=ǡ(" 100.0%",ɞ);}public void
ǣ(){if(ǿ.Count>0)return;
// Monospace font name, width of single character
// Change this if you want to use different (modded) monospace font
ǘ("Monospace", 24f);

// Classic/Debug font name (uses widths of characters below)
// Change this if you want to use different font name (non-monospace)
ǚ("Debug");
// Font characters width (font "aw" values here)
Ǿ("3FKTabdeghknopqsuy£µÝàáâãäåèéêëðñòóôõöøùúûüýþÿāăąďđēĕėęěĝğġģĥħĶķńņňŉōŏőśŝşšŢŤŦũūŭůűųŶŷŸșȚЎЗКЛбдекруцяёђћўџ", 17f);
Ǿ("ABDNOQRSÀÁÂÃÄÅÐÑÒÓÔÕÖØĂĄĎĐŃŅŇŌŎŐŔŖŘŚŜŞŠȘЅЊЖф□", 21f);
Ǿ("#0245689CXZ¤¥ÇßĆĈĊČŹŻŽƒЁЌАБВДИЙПРСТУХЬ€", 19f);
Ǿ("￥$&GHPUVY§ÙÚÛÜÞĀĜĞĠĢĤĦŨŪŬŮŰŲОФЦЪЯжы†‡", 20f);
Ǿ("！ !I`ijl ¡¨¯´¸ÌÍÎÏìíîïĨĩĪīĮįİıĵĺļľłˆˇ˘˙˚˛˜˝ІЇії‹›∙", 8f);
Ǿ("？7?Jcz¢¿çćĉċčĴźżžЃЈЧавийнопсъьѓѕќ", 16f);
Ǿ("（）：《》，。、；【】(),.1:;[]ft{}·ţťŧț", 9f);
Ǿ("+<=>E^~¬±¶ÈÉÊË×÷ĒĔĖĘĚЄЏЕНЭ−", 18f);
Ǿ("L_vx«»ĹĻĽĿŁГгзлхчҐ–•", 15f);
Ǿ("\"-rª­ºŀŕŗř", 10f);
Ǿ("WÆŒŴ—…‰", 31f);
Ǿ("'|¦ˉ‘’‚", 6f);
Ǿ("@©®мшњ", 25f);
Ǿ("mw¼ŵЮщ", 27f);
Ǿ("/ĳтэє", 14f);
Ǿ("\\°“”„", 12f);
Ǿ("*²³¹", 11f);
Ǿ("¾æœЉ", 28f);
Ǿ("%ĲЫ", 24f);
Ǿ("MМШ", 26f);
Ǿ("½Щ", 29f);
Ǿ("ю", 23f);
Ǿ("ј", 7f);
Ǿ("љ", 22f);
Ǿ("ґ", 13f);
Ǿ("™", 30f);
// End of font characters width
        ɘ[ɟ]=Ǽ(' ',ɟ);Ȋ[ɟ]=Ǽ(ǈ[ɟ],ɟ);ǋ[ɟ]=Ǽ(Ǌ[ɟ],ɟ);ɚ[ɟ]=ǡ(" 100.0%",ɟ);ɛ[ɟ]=Ǽ('.',ɟ)*2;}}class Ǖ{public string Ǥ(string
ǲ){return TT[ǲ];}
readonly Dictionary<string, string> TT = new Dictionary<string, string>
{
// Linhas para Tradução
// id da mensagem e Texto
{ "AC1", "Aceleração:" },
// quantidade
{ "A1", "VAZIO" },
{ "ALT1", "Altitude:"},
{ "ALT2", "Chão:"},
{ "B1", "Inicializando..." },
{ "C1", "numero:" },
{ "C2", "Carga Usada:" },
{ "C3", "Invalid countdown format, use:" },
{ "C4", "EXPIROU" },
{ "C5", "Dias" },
// customdata
{ "CD1", "Bloco não Encontrado: " },
{ "CD2", "Faltando nome do Bloco" },
{ "D1", "Você precisa definir um nome." },
{ "D2", "Nenhum Bloco Encontrado." },
{ "D3", "Nenhum Bloco Danificado Encontrado." },
{ "DO1", "Nenhum Conector Encontrado" }, // NEW
{ "DTU", "Formato de GPS Incorreto" },
{ "GA", "Artif."}, // (not more than 5 characters)
{ "GN", "Natur."}, // (not more than 5 characters)
{ "GT", "Total"}, // (not more than 5 characters)
{ "G1", "Gravidade Total:"},
{ "G2", "Gravidade Natural"},
{ "G3", "Gravidade Artificial"},
{ "GNC", "Sem Cabine!"},
{ "H1", "Escreva seus Comandos na aba 'CustomData' Desse Painel." },
// inventario
{ "I1", "Minério" },
{ "I2", "Armazenados" },
{ "I3", "Minérios" },
{ "I4", "Lingotes" },
{ "I5", "Componentes" },
{ "I6", "Gás" },
{ "I7", "Munição" },
{ "I8", "Ferramentas" },
{ "M1", "Massa da Carga:" },
// oxigênio
{ "O1", "Vazando" },
{ "O2", "Fazenda de Oxigênio" },
{ "O3", "Nenhum Bloco Relacionado a Oxigênio Encontrado" },
{ "O4", "Tanque de Oxigênio" },
// posição
{ "P1", "Bloco não Encontrado" },
{ "P2", "Localização" },
// Energia
{ "P3", "Armazenado" },
{ "P4", "Saída" },
{ "P5", "Entrada" },
{ "P6", "Nenhuma fonte de Energia Encontrada" },
{ "P7", "Baterias" },
{ "P8", "Saída Total" },
{ "P9", "Reatores" },
{ "P10", "Solar" },
{ "P11", "Energia" },
{ "P12", "Motores" }, // NEW!
{ "P13", "Turbinas" }, // NEW!
{ "PT1", "Tempo de Energia:" },
{ "PT2", "Tempo de Recarga:" },
{ "PU1", "Força usada:" },
{ "S1", "Velocidade:" },
{ "SM1", "Massa da Nave:" },
{ "SM2", "Tara da Nave:" },
{ "SD", "Distancia de Parada:" },
{ "ST", "Tempo de Parada:" },
// texto
{ "T1", "LCD base não Encontrado: " },
{ "T2", "Faltando o nome do LCD base" },
// tanques
{ "T4", "Faltando o nome do Tanque. ex: 'Tanks * Hydrogen'" },
{ "T5", "Nenhum Tanque de {0} Encontrado." }, // {0} is tank type
{ "UC", "Comando Desconhecido" },
// occupied & dampeners
{ "SC1", "Não foi possivel encontrar um bloco de controle." },
{ "SCD", "Compensadores: " },
{ "SCO", "Ocupado: " },
// trabalho
{ "W1", "OFF" },
{ "W2", "TRABALHANDO" },
{ "W3", "EM ESPERA" },
{ "W4", "VAZAMENTO" },
{ "W5", "ABERTO" },
{ "W6", "FECHADO" },
{ "W7", "TRANCADO" },
{ "W8", "DESTRANCADO" },
{ "W9", "LIGADO" },
{ "W10", "PRONTO" }
};
}
}static class Ǳ{public static bool ǰ(this string ĩ,string Ǯ){return ĩ.StartsWith(Ǯ,StringComparison.
InvariantCultureIgnoreCase);}public static bool ǯ(this string ĩ,string Ǯ){if(ĩ==null)return false;return ĩ.IndexOf(Ǯ,StringComparison.
InvariantCultureIgnoreCase)>=0;}public static bool ǭ(this string ĩ,string Ǯ){return ĩ.EndsWith(Ǯ,StringComparison.InvariantCultureIgnoreCase);}}
static class Ǭ{public static string ǫ(this IMyTerminalBlock ç){int Ŭ=ç.CustomData.IndexOf("\n---\n");if(Ŭ<0){if(ç.CustomData.
StartsWith("---\n"))return ç.CustomData.Substring(4);return ç.CustomData;}return ç.CustomData.Substring(Ŭ+5);}public static string
Ǫ(this IMyTerminalBlock ç,int ţ,string ǩ){string Ǩ=ç.ǫ();string ǧ="@"+ţ.ToString()+" AutoLCD";string Ǧ='\n'+ǧ;int Ŭ=0;if(
!Ǩ.StartsWith(ǧ,StringComparison.InvariantCultureIgnoreCase)){Ŭ=Ǩ.IndexOf(Ǧ,StringComparison.InvariantCultureIgnoreCase);
}if(Ŭ<0){if(ţ==0){if(Ǩ.Length==0)return"";if(Ǩ[0]=='@')return null;Ŭ=Ǩ.IndexOf("\n@");if(Ŭ<0)return Ǩ;return Ǩ.Substring(
0,Ŭ);}else return null;}int ǥ=Ǩ.IndexOf("\n@",Ŭ+1);if(ǥ<0){if(Ŭ==0)return Ǩ;return Ǩ.Substring(Ŭ+1);}if(Ŭ==0)return Ǩ.
Substring(0,ǥ);return Ǩ.Substring(Ŭ+1,ǥ-Ŭ);}