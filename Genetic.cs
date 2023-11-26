using static System.BitConverter;

public class Genetic
{
    int iteration = 0;
    int maxIterations;
    Dataset data;
    byte agentsCount;
    byte evolvePopulationCount;
    double targetFitnessDelta;
    float mutatePos;
    float inversePos;
    byte genesCount = 6;
    Agent[] agents;
    Random rnd = new Random();

    public Genetic(Dataset data, byte agentsCount, byte evolvePopulationCount, 
    double targetFitnessDelta, int maxIterations, float mutatePos, float inversePos)
    {
        this.data = data;
        this.agentsCount = agentsCount;
        this.targetFitnessDelta = targetFitnessDelta;
        this.maxIterations = maxIterations;
        this.mutatePos = mutatePos;
        this.inversePos = inversePos;

        agents = new Agent[agentsCount];

        GenerateAgents();
        Evolve();
    }

    void GenerateAgents() //генерируем случайные гены
    {
        for (int i = 0; i < agentsCount; i++) 
        {
            for (int j = 0; j < genesCount; j++)
            {
                var value = rnd.NextDouble();
                agents[i] = new Agent(genesCount);
                agents[i].genes[j] = value;
            }
        }
    }

    void Evolve()
    {
        bool IsEvolutionComplete()
        {
            for (int i = 0; i < agentsCount; i++)
                for (int j = 0; j < data.rows; j++)
                    if (Fitness(Function(data.coefficients[j], agents[i].genes), data.desiredValues[j]) < targetFitnessDelta) return false;
            return true;
        }

        while (true)
        {
            if (IsEvolutionComplete()) return;
            if (iteration > maxIterations) return;
            iteration++;
            SortAgents();
            EvolveAgents();
        }
    }

    void SortAgents() //сортирует агентов по общему фитнесу
    {
        FitnessAgents();
        //
    }

    void FitnessAgents()
    {
        for (int i = 0; i < agentsCount; i++)
        {
            for (int j = 0; j < data.rows; j++)
                agents[i].fitness += Fitness(Function(data.coefficients[j], agents[i].genes), data.desiredValues[j]);
        }
    }

    void EvolveAgents() //сделать адаптивным, а не только под половину
    {
        for (int i = 0; i < evolvePopulationCount; i+=2)
        {
            for (int j = 0; j < genesCount; j++)
            {
                var crossover = Crossover(agents[i].genes[j], agents[i+1].genes[j]);
                agents[i].genes[j] = Mutate(ref crossover[0]);
                agents[i+1].genes[j] = Mutate(ref crossover[1]);
                crossover = Crossover(agents[i].genes[j], agents[i+1].genes[j]);
                agents[i+(agentsCount/2)-1].genes[j] = Mutate(crossover[0]);
                agents[i+(agentsCount/2)].genes[j] = Mutate(crossover[1]);
            }
        }
    }

    double Function(List<byte> coefficients, double[] genes) =>
        (genes[0]*coefficients[0]+
        genes[1]*coefficients[1]+
        genes[2]*coefficients[2]+
        genes[3]*coefficients[0]*coefficients[0]+
        genes[4]*coefficients[1]*coefficients[1]+
        genes[5]*coefficients[2]*coefficients[2]);

    double Fitness(double y, short desired)
    {
        var error = Math.Abs(desired-y);
        var errorNorm = 1/(1+Math.E*(-2*error));
        return 1-errorNorm;
    }

    void Mutate(ref double x) // мутация: генерация случайной величины
    {
        int bitToFlip = rnd.Next(64);
        ulong mask = (byte)(1 << bitToFlip);
        if (rnd.NextDouble() > mutatePos) x = UInt64BitsToDouble(DoubleToUInt64Bits(x) ^ mask);
    }

    // void Inverse(ref double x) // инверсия: поиск в окрестностях точки
    // {
    //     var targetBit = rnd.Next(64);
    //     if (rnd.NextDouble() > inversePos) x = BitConverter.UInt64BitsToDouble(BitConverter.DoubleToUInt64Bits(x) << targetBit);
    // }

    double[] Crossover(double n, double m) // кроссовер
    {
        var targetBit = rnd.Next(0,64);
        string bitMaskString = "";
        for (int i = 0; i < 64; i++)
            bitMaskString += i < targetBit ? "1" : "0";
        var bitMask = Convert.ToByte(bitMaskString, 2);
        return new double[]{
            UInt64BitsToDouble((DoubleToUInt64Bits(n) & bitMask) | (DoubleToUInt64Bits(m) & (ulong)~bitMask)), 
            UInt64BitsToDouble((DoubleToUInt64Bits(n) & (ulong)~bitMask) | (DoubleToUInt64Bits(m) & bitMask))};
    }
}