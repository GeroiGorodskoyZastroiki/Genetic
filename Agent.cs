public class Agent
{
    public double[] genes; //свободные члены уравнения
    public double fitness; //общий фитнесс

    public Agent(int genesCount)
    {
        genes = new double[genesCount];
    }
}