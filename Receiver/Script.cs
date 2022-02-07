bool run = true;
IMyUnicastListener Rx;
MyIGCMessage data = new MyIGCMessage();
string Text;
string Data;
long Source = 0;
string Tag;
string LastText;
string LCDN = "CommsLCD";
public List<IMyTerminalBlock> LCDs = new List<IMyTerminalBlock>();



public bool IsLCD(IMyTerminalBlock block)
{
    IMyTextPanel block1;
    block1 = block as IMyTextPanel;
    if (block1 != null) return true;
    return false;
}



public Program()
{
    if (run)
    {
        Runtime.UpdateFrequency = UpdateFrequency.Update100;
        Me.CustomData = "ID: " + Me.EntityId;
    }
}


public void Main(string argument, UpdateType updateSource)
{
    GridTerminalSystem.SearchBlocksOfName(LCDN, LCDs, IsLCD);

    Rx = IGC.UnicastListener;
    Check();
    Echo(LastText);
}
public void Check()
{
    
    while (Rx.HasPendingMessage)
    {
        data = Rx.AcceptMessage();
        Tag = data.Tag;
        Source = data.Source;
        Text = data.Data.ToString();
        foreach (IMyTextPanel lcd in LCDs)
        {
            if (lcd.CustomName.Contains(Tag))
            {
                lcd.ContentType = ContentType.TEXT_AND_IMAGE; lcd.WriteText(Text);
            }
            LastText = (Text + " " + Tag + " ");

        }
    }
}
