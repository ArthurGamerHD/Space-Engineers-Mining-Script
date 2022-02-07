//Antenna Message Sender & Receiver v2.02 by Kezeslabas                               Updated up until Space Engineers v1.193.1                     
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//   This is a script that is capable of sending and receiving messages to/from other grids remotely, by using Antennas.
//   You can send a message to only one Receiver Block, but you can receive a message from multiple Sender Blocks.
//   On the Receiving Side, the script can write out the messages to LCDs, or can Trigger/Start Timer Blocks.
//   It's ideal to use it to open Doors remotely.

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Hotfix (v2.02):        - Script got updated to Space Engineers v1.193.1
//                               - Adding an Antenna to the script is no longer reqired in the sending side
//                                     - The grid should have an Antenna still to send a message
//                                     - The script notifies the user if there is no antenna on the grid, or if it's not able to send a message.
//                                     - You can still add an Antenna to the script with the /Transmitter/ tag, but it does not do anything with it.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Bugfix (v2.01):        - Fixed a bug that always made the Tags sent with a message to lower case.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Change Log (v2.0): - MAJOR UPDATE! The script can now both send and receive messages!
//                                     - The whole code has been rewritten!
//                                     - Changes to LCDs:
//                                         - The default LCD is this block's LCD, where only the newest message is shown
//                                         - Other LCDs will be used only if a Tag is sent alongisde a message
//                                             - This makes it possible to use any number of LCDs for different tasks
//                                             - You can use any number of LCDs for the same task
//                                         - You can now use other LCD Surfaces as well, like Cockpits.
//                                         - Color coding added to all LCDs
//                                     - Changes to Timer Blocks:
//                                         - Now, Timers will be used only if a Tag is sent alongside a message
//                                             - This makes it possible to use any number of Timers for different tasks
//                                             - You can use any number of Timers for the same task
//                                         - Now, if the message is a number, then the Timer(s) will be Started with that much delay
//                                         - Otherwise the Timer will be Triggered instead
//                                     - The Custom Data is now used to Customize the script
//                                         - The "get address" command is now obsolote, the address is now written to the Custom Data automatically
//                                         - The receiver's address can be added to the script, which is needed to send a message, 
//                                            in the Custom Data as well.
//                                     - For more details, check the description below.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////
// Components:
//                         - Antenna ( 1 )

//                         - Timer Block (0 or more)
//                         - LCD/Text Panels (0 or more)
//                         - Other LCD Blocks, like Cockpits (0 or more)

///////////////////////////////////////////
//Quick Setup
//         1.) How to Send a message
//         2.) How to Setup the Receiving
//         3.) Notes

//////////////////////////////////////////
//         1.) How to Send a message

//             - To send a message, you will need an Antenna attached to the script.
//                     - Add the "/Transmitter/" tag to the name of the Antenna that you want to use, 
//                       then run the script with the "refresh" argument.
//                     - Multiple Sender scripts can use the same Antenna

//             - You will also need the address of a Receiver.
//                     - Place a Programmable Block to the Receiver Grid, load this script to it, then click Check Code.
//                     - After that you can find the address in that block's Custom Data.
//                     - Copy that address to this block's Custom Data to the Receiver's Address, 
//                       then run this script with the "refresh" argument.

//             - Run this script with the right argument:
//                     - First part: send
//                     - Second part: tag
//                     - Third part: message
//
//                     - Use a ";" between the parts.
//                     - Examples: 
//                         send;/Status/;What's up?    <- The Receiver Side will searches blocks with the /Status/ tag in their names
//                                                                             In case of found Timer(s) it Triggers them (since the message is not a number)
//                                                                             In case of found LCDs it writes out "What's up?" on them
//
//                         send;/Hangar Door 01/;3      <- The Receiver Side will searches blocks with the /Hangar Door 01/ tag in their names
//                                                                            In case of found Timer(s) it Starts them with a 3 second delay
//                                                                            In case of found LCDs it writes out "3" on them

//////////////////////////////////////////
//         2.) How to Setup the Receiving

//             - Load this script to a Programmable Block and click Check Code.

//             - Run the script with the "start" argument to start listening to new messages
//                     - You can stop the listening with the "stop" argument

//             - How to use Timers:
//                     - Add a tag of your choosing to it's name, that you will send alongise a message with the send command.
//                     - That's it.
//                             - The script will looks for this block if the right message is received.
//                             - The script then saves these block(s) to it's memory, and tries to call them when the right message arrives again
//                                     - This is to improve prefromance.
//                                     - The script recognizes that if the block is missing and tries to locate it again, instead of crashing.

//             - How to use LCD/Text Panels:
//                     - Add a tag of your choosing to it's name, that you will send alongise a message with the send command.
//                     - That's it.
//                             - The script handles them the same as with Timers.

//             - How to use Other LCD Blocks, like Cockpits:
//                     - Add a tag of your choosing to it's name, that you will send alongise a message with the send command.
//                     - Open it's Custom Data, then write this line to it:
//                             - @Number Tag
//                                     - Number: The number of the LCD you want to use on that block
//                                     - Tag: The same tag that you added to the block's name
//
//                             - Example: @0 /Status/          <- Will uses the 0. LCD of that block
//                     - That's it.
//                             - The script handles these blocks the same as with Timers.

//             - How to remove a block from the scripts memory:
//                     - Run the script with the "refresh" argument
//                     - That's it.
//                             - This will deletes every block from the script's memory.
//                             - If a block was working before and you haven't removed your chosen tag from it's name, 
//                               then it will work the same as before.

//////////////////////////////////////////
//         3.) Notes

//         - A message can only be received, if the Sender Side's Antenna has enough range to reach an Antenna in the Receiver End.
//         - You don't need to add an Antenna to the script, if you only want to Receive messages, but oyu need an Antenna on the grid.

//         - You can open a Hangar Door remotely, if you set up Sender and a Receiver with a Timer Block on the Receiver End.
//            Set up the Timer to open/colse the Hangar Door, then add a Tag to it's name, then by sending a Message to the Receiver
//            with the Tag that you gave to the Timer, you can trigger the Timer and open/close the Hangar Door.

//         - You can change the /Transmitter/ tag that you should add to the name of an Antenna
//           in the Custom Data of the Programmable Block. After changing it you can apply it with the "refresh" command.

//         - If you mess up something in the Custom Data, or you delete it's first line, then the script will resets it.

//         - You can use multiple screens on a Cockpit, if you add multiple different tags to it's name 
//           and you write multiple lines to it's Custom Data, that tells which LCD belongs to which Tag.

//         - The script works alongside MMaster's Auto LCD script in the same cockpit.

//         - The script is able to receive the progression data of my Automatic Mining Platform script.
//            Link: https://steamcommunity.com/sharedfiles/filedetails/?id=1695500366
 
//         - The script changes the color of the text of LCDs it uses but doesn't changes anything else, 
//           so you can costumize them as you please.

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//DO NOT MODIFY ANYTHING BELOW THIS
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// Storage variables
string sender="";
bool run=false;

string main_tag="/Transmitter/";
long receiver_address=0;

public class TEXTS
{
   public string indicator="";
   bool indi=true;
   public string status="Waiting for commands...\n";
   public string msg;

   public void update_indicator()
   {
       if(indi)indicator="[-/-/-/] ";
       else indicator="[/-/-/-] ";
       indi=!indi;
   }
}

int msg_count=0;

Dictionary<string,Color> Colors = new Dictionary<string,Color>()
{
   {"New Message ",Color.DodgerBlue},
   {"Listening Started!\n",Color.LimeGreen},
   {"Listening...\n",Color.Lime},
   {"Waiting for commands...\n",Color.White},
   {"Listening Stoped!\n",Color.Orange},
   {"Message Sent! ",Color.Cyan},
};


IMyUnicastListener listener;
MyIGCMessage data = new MyIGCMessage();

IMyTextSurface screen;
IMyTextSurface mylcd;
IMyTextSurfaceProvider lcd_block;

List<IMyTerminalBlock> blocks;

public class Argument_Decode
{
   public string mode="";
   public string block="";
   public string msg="";
   
   public Argument_Decode(string arg)
   {
       int count=0;
       for(int i=0;i<arg.Length;i++)
       {
           if(arg[i]==';')count++;
       }
       if(count==0)
       {
           mode=arg;
           mode=mode.ToLower();
       }
       else if(count==1)
       {
           mode=arg.Split(';')[0];
           mode=mode.ToLower();
           block=arg.Split(';')[1];
       }
       else if(count==2)
       {
           mode=arg.Split(';')[0];
           mode=mode.ToLower();
           block=arg.Split(';')[1];
           msg=arg.Split(';')[2];
       }
   }
}

TEXTS text = new TEXTS();
Argument_Decode arg;

IMyRadioAntenna antenna;

Dictionary<string,List<IMyTextPanel>> LCDS = new Dictionary<string,List<IMyTextPanel>>();
Dictionary<string,List<IMyTimerBlock>> TIMERS = new Dictionary<string,List<IMyTimerBlock>>();

public class SURFACEPAIRS
{
   public IMyTerminalBlock block;
   public IMyTextSurface surface;

   public SURFACEPAIRS(IMyTerminalBlock b,IMyTextSurface t)
   {
       block=b;
       surface=t;
   }
}

Dictionary<string,List<SURFACEPAIRS>> SURFACES = new Dictionary<string,List<SURFACEPAIRS>>();

public Program() 
{
    load_data();
    refresh_components();
    if(run)
    {
        Runtime.UpdateFrequency = UpdateFrequency.Update100;
    }
    if(!get_configuration())set_configuration(); 
} 
 
public void Save() 
{
    save_data(); 
} 
 
public void Main(string argument, UpdateType updateSource) 
{
   text.update_indicator();
   if((updateSource & UpdateType.Update100)!=0)
   {
        if(!get_messages())
        {
           text.status="Listening...\n";

           mylcd.WriteText(text.indicator+text.status+text.msg);
           mylcd.FontColor=Colors[text.status];
           Echo(text.indicator+text.status);
        }
        else
        {
           mylcd.WriteText(text.indicator+text.status+"["+msg_count+"]\n"+text.msg);
           mylcd.FontColor=Colors[text.status];
           Echo(text.indicator+text.status+"["+msg_count+"]");
        }
   }
   else
   {
       arg = new Argument_Decode(argument);

       switch(arg.mode)
       {
           case "send":
           {
               Echo("[Command: Send]");
                
                   string antennaStatus=antennaCheck();
                   if(antennaStatus=="")
                   {
                       send_message(arg.block,arg.msg);
                       text.status="Message Sent! ";

                       mylcd.WriteText(text.indicator+text.status+"["+arg.block+"]");
                       mylcd.FontColor=Colors[text.status];
                       Echo(text.indicator+text.status+"["+arg.block+"]");
                   }
                   else
                   {
                       Echo(antennaStatus);
                   }
               break;
           }
           case "refresh":
           {
               Echo("[Command: Refresh]");
               refresh_components();
               if(!get_configuration())set_configuration();
               break;
           }
           case "start":
           {
               Echo("[Command: Start]");
               Runtime.UpdateFrequency = UpdateFrequency.Update100;
               run=true;
               text.status="Listening Started!\n";
               mylcd.WriteText(text.indicator+text.status);
               mylcd.FontColor=Colors[text.status];
               Echo(text.indicator+text.status);
               break;
           }
           case "stop":
           {
                Echo("[Command: Stop]");
                Runtime.UpdateFrequency = UpdateFrequency.None;
                if(run)
                {
                    run=false;
                }
                text.status="Listening Stoped!\n";
                mylcd.WriteText(text.indicator+text.status);
                mylcd.FontColor=Colors[text.status];
                Echo(text.indicator+text.status);
                break;
           }
           default:
           {
               Echo("Command doesn't found!");
               break;
           }
       }
   } 
}

public bool get_messages()
{
   msg_count=0;
   while(listener.HasPendingMessage)
   {
       msg_count++;
       data = listener.AcceptMessage();
       sender=data.Tag;
       text.msg=data.Data.ToString();
       refresh_components(sender);

       int i;
       if(LCDS.ContainsKey(sender))
       {
           
           text.status="New Message "; 
           for(i=0;i<LCDS[sender].Count;i++)
           {
               screen=LCDS[sender][i] as IMyTextSurface;
               screen.WriteText(text.indicator+text.status+"["+sender+"]\n"+text.msg);
               screen.FontColor=Colors[text.status];
           }
       }
       if(SURFACES.ContainsKey(sender))
       {
           text.status="New Message "; 

           for(i=0;i<SURFACES[sender].Count;i++)
           {
               screen=SURFACES[sender][i].surface;
               screen.WriteText(text.indicator+text.status+"["+sender+"]\n"+text.msg);
               screen.FontColor=Colors[text.status];
           }
       }
       if(TIMERS.ContainsKey(sender))
       {
           float n=0;
           if(Single.TryParse(data.Data.ToString(), out n))
           {
               if(n>0)
               {
                   for(i=0;i<TIMERS[sender].Count;i++)
                   {
                       TIMERS[sender][i].TriggerDelay=n;
                       TIMERS[sender][i].StartCountdown();
                   }
               }
               else
               {
                   for(i=0;i<TIMERS[sender].Count;i++)
                   {
                       TIMERS[sender][i].Trigger();
                   }
               }
           }
           else 
           {
               for(i=0;i<TIMERS[sender].Count;i++)
               {
                   TIMERS[sender][i].Trigger();
               }
           }
       }
   }
   if(msg_count>0)
   {
       text.status="New Message ";
       return true;
   }
   else return false;
}

public void send_message(string block,string msg="")
{
    //if(antenna.AttachedProgrammableBlock!=Me.EntityId)antenna.AttachedProgrammableBlock=Me.EntityId;
    //No need for it since Space Engineers v1.193.1 update
    IGC.SendUnicastMessage(receiver_address,block,msg);
}


public string antennaCheck()
{
    bool antenna_working=false;
    GridTerminalSystem.GetBlocksOfType<IMyRadioAntenna>(blocks);
    for(int i=0;i<blocks.Count;i++)
    {
        antenna=blocks[i] as IMyRadioAntenna;
        if(antenna.Enabled)
        {
            if(antenna.IsBroadcasting)
            {
                antenna_working=true;
                break;
            }
        }
    }
    if(antenna_working)return "";
    else return "No enabled or broadcasting antenna!";
}



public void refresh_components(string tag="")
{
   if(tag=="")
   {
       LCDS.Clear();
       TIMERS.Clear();
       SURFACES.Clear();

       listener  = IGC.UnicastListener;

       mylcd = Me.GetSurface(0);
       if(mylcd.ContentType!=ContentType.TEXT_AND_IMAGE)
       {
            mylcd.ContentType=ContentType.TEXT_AND_IMAGE;
       }
       mylcd.FontSize=1.2F;
       mylcd.Alignment=TextAlignment.LEFT;

       blocks = new List<IMyTerminalBlock>();
   
       GridTerminalSystem.GetBlocksOfType<IMyRadioAntenna>(blocks);
       for(int i=0;i<blocks.Count;i++)
       {
           if(blocks[i].CustomName.Contains(main_tag))
           {
               antenna=blocks[i] as IMyRadioAntenna;
               Echo("Antenna found!");
               break;
           }
       }
   }
   else
   {
       int i;
       bool found;
       blocks.Clear();
       if(LCDS.ContainsKey(tag))
       {
           found=true;
           for(i=0;i<LCDS[tag].Count;i++)
           {
               if(!component_ready(LCDS[tag][i]))
               {
                   found=false;
                   break;
               }
           }
           if(!found)
           {
               LCDS[tag].Clear();
               GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(blocks);
               for(i=0;i<blocks.Count;i++)
               {
                   if(blocks[i].CustomName.Contains(tag))
                   {
                       LCDS[tag].Add((IMyTextPanel)blocks[i]);
                       screen=blocks[i] as IMyTextSurface;
                       if(screen.ContentType!=ContentType.TEXT_AND_IMAGE)
                       {
                           screen.ContentType=ContentType.TEXT_AND_IMAGE;
                       }
                   }
               }
               if(LCDS[tag].Count==0)LCDS.Remove(tag);
           }
       }
       else 
       {
           LCDS.Add(tag,new List<IMyTextPanel>());
 
           GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(blocks);
           for(i=0;i<blocks.Count;i++)
           {
               if(blocks[i].CustomName.Contains(tag))
               {
                   LCDS[tag].Add(blocks[i] as IMyTextPanel);
                   screen=blocks[i] as IMyTextSurface;
                   if(screen.ContentType!=ContentType.TEXT_AND_IMAGE)
                   {
                       screen.ContentType=ContentType.TEXT_AND_IMAGE;
                   }
               }
           }
           if(LCDS[tag].Count==0)LCDS.Remove(tag);
       }
       if(TIMERS.ContainsKey(tag))
       {  
           found=true;
           for(i=0;i<TIMERS[tag].Count;i++)
           {
               if(!component_ready(TIMERS[tag][i]))
               {
                   found=false;
                   break;
               }
           }
           if(!found)
           {
               TIMERS[tag].Clear();
               GridTerminalSystem.GetBlocksOfType<IMyTimerBlock>(blocks);
               for(i=0;i<blocks.Count;i++)
               {
                   if(blocks[i].CustomName.Contains(tag))TIMERS[tag].Add((IMyTimerBlock)blocks[i]);
               }
               if(TIMERS[tag].Count==0)TIMERS.Remove(tag);
           }    
       }
       else
       {
           TIMERS.Add(tag,new List<IMyTimerBlock>());
 
           GridTerminalSystem.GetBlocksOfType<IMyTimerBlock>(blocks);
           for(i=0;i<blocks.Count;i++)
           {
               if(blocks[i].CustomName.Contains(tag))TIMERS[tag].Add((IMyTimerBlock)blocks[i]);
           }
           if(TIMERS[tag].Count==0)TIMERS.Remove(tag);
       }
       if(SURFACES.ContainsKey(tag))
       {
           found=true;
           for(i=0;i<SURFACES[tag].Count;i++)
           {
               if(!component_ready(SURFACES[tag][i].block))
               {
                   found=false;
                   break;
               }
           }
           if(!found)
           {
               SURFACES[tag].Clear();
               GridTerminalSystem.GetBlocksOfType<IMyTextSurfaceProvider>(blocks);
               for(i=0;i<blocks.Count;i++)
               {
                   if(blocks[i].EntityId!=Me.EntityId && !(blocks[i] is IMyTextPanel) && blocks[i].CustomName.Contains(tag))
                   {
                       string[] data=blocks[i].CustomData.Split('\n');
                       string subs="";
                       foreach(string s in data)
                       {
                           if(s.StartsWith("@"))
                           {
                               subs=s.Substring(1);
                               if(subs.Contains(tag))
                               {
                                   subs=subs.Replace(tag,"");
                                   int n=0;
                                   if(Int32.TryParse(subs, out n))
                                   {
                                       lcd_block=blocks[i] as IMyTextSurfaceProvider;
                                       if(lcd_block.SurfaceCount>=n)
                                       {
                                           screen=lcd_block.GetSurface(n);
                                           if(screen.ContentType!=ContentType.TEXT_AND_IMAGE)
                                           {
                                               screen.ContentType=ContentType.TEXT_AND_IMAGE;
                                           }
                                           SURFACES[tag].Add(new SURFACEPAIRS(blocks[i],screen));
                                           break;
                                       }
                                   }
                               }
                           }
                       }
                   }
               }
               if(SURFACES[tag].Count==0)SURFACES.Remove(tag);
           }    
       }
       else
       {
           SURFACES.Add(tag,new List<SURFACEPAIRS>());
           GridTerminalSystem.GetBlocksOfType<IMyTextSurfaceProvider>(blocks);
           for(i=0;i<blocks.Count;i++)
           {
               if(blocks[i].EntityId!=Me.EntityId && !(blocks[i] is IMyTextPanel) && blocks[i].CustomName.Contains(tag))
               {
                   string[] data=blocks[i].CustomData.Split('\n');
                   string subs="";
                   foreach(string s in data)
                   {
                       if(s.StartsWith("@"))
                       {
                           subs=s.Substring(1);
                           if(subs.Contains(tag))
                           {
                               subs=subs.Replace(tag,"");
                               int n=0;
                               if(Int32.TryParse(subs, out n))
                               {
                                   lcd_block=blocks[i] as IMyTextSurfaceProvider;
                                   if(lcd_block.SurfaceCount>=n)
                                   {
                                       screen=lcd_block.GetSurface(n);
                                       if(screen.ContentType!=ContentType.TEXT_AND_IMAGE)
                                       {
                                           screen.ContentType=ContentType.TEXT_AND_IMAGE;
                                       }
                                       SURFACES[tag].Add(new SURFACEPAIRS(blocks[i],screen));
                                       break;
                                   }
                               }
                           }
                       }
                   }
               }
           }
           if(SURFACES[tag].Count==0)SURFACES.Remove(tag);
       }
   }
}

public bool component_ready(IMyTerminalBlock comp)
{
   if(comp==null || comp.CubeGrid.GetCubeBlock(comp.Position)==null)return false;
   else return true;
}
 
public void save_data()
{
    Storage=sender+";"+text.msg+";"+text.status+";"+run;
}

public void load_data()
{
    string[] stored_data=Storage.Split(';');
    if(stored_data.Length==4)
    {
        sender=stored_data[0];
        text.msg=stored_data[1];
        text.status=stored_data[2];
        if(!Boolean.TryParse(stored_data[3], out run)){Echo("Converting |run| failed!");}

        Echo("Data Loaded!");
    }
    else
    {
        Echo("Load Failed!");
    } 
}

public bool get_configuration()
{
    if(Me.CustomData.StartsWith("@Configuration AM Sender/Receiver v2.0\n"))
    {
        string[] config=Me.CustomData.Split('|');
        if(config.Length==6)
        {
            bool result=true;
            if(!Int64.TryParse(config[2], out receiver_address)){Echo("Getting receiver_address failed!");result=false;}
            main_tag=config[4];

            if(result)
            {
                Echo("Configuration Done!");
                return true;
            }
            else
            {
                Echo("Configuration Error!");
                return false;
            }
        }
        else
        {
            Echo("Getting Configuration failed!");
            return false;
        }
    }
    else
    {
        Echo("Getting Configuration failed!");
        return false;
    }
}

public void set_configuration()
{
   
    Me.CustomData="@Configuration AM Sender/Receiver v2.0\n"+
                    "You can configure the script right below here,\n"+
                    "by changing the values between then | characters.\n\n"+

                    "The configuration will be loaded if you click Check Code\n"+
                    "in the Code Editor inside the Programmable Block,\n"+
                    "when the game Saves/Loads or when you use the\n"+
                    "Refresh command.\n\n"+

                    "This blocks address: "+Me.EntityId+"\n(Copy this to the other grid's Receiver's address)\n\n"+
                    "Receiver's address: |"+receiver_address+"|\n\n"+    //2 long
                    "Antenna Tag: |"+main_tag+"|\n\n"+   //4 string

                    "///////////////////////////////////////////\n";
                    
    Echo("Configuration Set to Custom Data!");
}