List<IMyLightingBlock> lights = new List<IMyLightingBlock>();
int StartR = 255, StartG = 255, StartB = 255, anim, R, G, B, set, time;
bool HighPerformance, FadeIn, Auto = true, Random = false;
Color MyColor;

public Program()
{
    Color MyColor = new Color(StartR, StartG, StartB);
    GridTerminalSystem.GetBlocksOfType(lights);
    R = 255;
    G = 0;
    B = 0;
    anim = 0;
    time = 0;
    if (HighPerformance) { Runtime.UpdateFrequency = UpdateFrequency.Update1; set = 1; }
    else
    {
        Runtime.UpdateFrequency = UpdateFrequency.Update10; set = 10;
    }

}



public void Save()

{

}
public Color FadeLight()
{
    Color Sync;
    Sync = new Color(R, G, B);
    return Sync;
}

public void refresh()
{
    if (FadeIn | Auto)
    {
        switch (anim)
        {
            case 0: if (R < 255) R += set; else { anim = 1; G = 0; } break;
            case 1: if (B > 0) B -= set; else { anim = 2; } break;
            case 2: if (G < 255) G += set; else { anim = 3; B = 0; } break;
            case 3: if (R > 0) R -= set; else { anim = 4; } break;
            case 4: if (B < 255) B += set; else { anim = 5; R = 0; } break;
            case 5: if (G > 0) G -= set; else { anim = 0; } break;
        }
    }
    return;
}


public void Main(string argument, UpdateType updateSource)

{

    if (time == 100)
    {
        time = 0;
        GridTerminalSystem.GetBlocksOfType(lights);
    }

    Echo((time).ToString());
    time += 1;


    Random rnd = new Random();
    refresh();

    foreach (IMyLightingBlock light in lights)
    {
        if (light.IsWorking)
        {
            Color SetColor;
            if (argument != null)
            {
                switch ((argument.ToUpper()))

                {
                    case "RED": manual(); MyColor = new Color(255, 0, 0); break;
                    case "ORANGE": manual(); MyColor = new Color(255, 127, 0); break;
                    case "YELLOW": manual(); MyColor = new Color(255, 255, 0); break;
                    case "GREEN": manual(); MyColor = new Color(0, 255, 0); break;
                    case "AQUA": manual(); MyColor = new Color(0, 127, 255); break;
                    case "CYAN": manual(); MyColor = new Color(0, 255, 255); break;
                    case "BLUE": manual(); MyColor = new Color(0, 0, 255); break;
                    case "PINK": manual(); MyColor = new Color(255, 0, 255); break;
                    case "PURPLE": manual(); MyColor = new Color(127, 0, 255); break;
                    case "WHITE": manual(); MyColor = new Color(255, 255, 255); break;
                    case "RANDOM": Random = true; FadeIn = false; Auto = false; break;
                    case "DISABLE": manual(); MyColor = new Color(0, 0, 0); break;
                    case "DEFAULT": manual(); MyColor = new Color(StartR, StartG, StartB); break;
                    case "FADE": Random = false; FadeIn = true; Auto = false; break;
                    case "AUTO": if (Auto) { Auto = false; } else { Auto = true; }; break;
                }
            }
            if (Random) { MyColor = new Color(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255)); }
            light.Color = MyColor;
            if (Auto)
            {
                switch ((light.CustomData.ToUpper()))
                {
                    case "RED": SetColor = new Color(255, 0, 0); break;
                    case "ORANGE": SetColor = new Color(255, 127, 0); break;
                    case "YELLOW": SetColor = new Color(255, 255, 0); break;
                    case "GREEN": SetColor = new Color(0, 255, 0); break;
                    case "AQUA": SetColor = new Color(0, 127, 255); break;
                    case "CYAN": SetColor = new Color(0, 255, 255); break;
                    case "BLUE": SetColor = new Color(0, 0, 255); break;
                    case "PINK": SetColor = new Color(255, 0, 255); break;
                    case "PURPLE": SetColor = new Color(127, 0, 255); break;
                    case "WHITE": SetColor = new Color(255, 255, 255); break;
                    case "RANDOM": SetColor = new Color(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255)); break;
                    case "DEFAULT": SetColor = new Color(StartR, StartG, StartB); break;
                    case "FADE": SetColor = FadeLight(); break;
                    default: SetColor = FadeLight(); break;
                }
                light.Color = SetColor;
            }
        }
        if (FadeIn) { light.Color = FadeLight(); }
    }
}
void manual()
{
    FadeIn = false;
    Auto = false;
    Random = false;
}
