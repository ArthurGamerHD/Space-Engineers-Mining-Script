public Program()
{
    Runtime.UpdateFrequency = UpdateFrequency.Update1;
}

public void Main(string Teste)
{
    List<IMyTerminalBlock> rodas = new List<IMyTerminalBlock>();
    GridTerminalSystem.SearchBlocksOfName("Wheel-Suspension", rodas);
    IMyMotorBase roda = (IMyMotorSuspension)rodas[0];
    if (roda.PendingAttachment) roda.Detach();
    roda.Attach();
}
