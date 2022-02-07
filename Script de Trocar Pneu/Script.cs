List<IMyMotorSuspension> rodas = new List<IMyMotorSuspension>();
public Program()
{
    Runtime.UpdateFrequency = UpdateFrequency.Update1;
}

public void Main(string Teste)
{
    GridTerminalSystem.GetBlocksOfType(rodas);
    foreach (IMyMotorSuspension roda1 in rodas)
    {
        IMyMotorSuspension roda = (IMyMotorSuspension)roda1;
        if (roda.PendingAttachment) roda.Detach();
        roda.Attach();
    }
}
