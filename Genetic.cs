using static System.BitConverter;

public class Genetic
{
    int iteration = 0;
    int maxIterations;
    Dataset data;
    byte agentsCount;
    byte evolvePopulationCount;
    double targetFitnessDelta;
    float mutateChance;
    float inverseChance;
    byte genesCount = 6;
    List<Agent> agents;
    Random rnd = new Random();

    public Genetic(Dataset data, byte agentsCount, byte evolvePopulationCount, 
    double targetFitnessDelta, int maxIterations, float mutateChance, float inverseChance)
    {
        if (evolvePopulationCount % 2 != 0)
            throw new ArgumentException("Число должно быть чётным.", nameof(evolvePopulationCount));
        this.evolvePopulationCount = evolvePopulationCount;
        this.data = data;
        this.agentsCount = agentsCount;
        this.targetFitnessDelta = targetFitnessDelta;
        this.maxIterations = maxIterations;
        this.mutateChance = mutateChance;
        this.inverseChance = inverseChance;

        agents = new List<Agent>(agentsCount);

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
        bool IsEvolutionComplete() //переписать
        {
            for (int i = 0; i < agentsCount; i++)
                for (int j = 0; j < data.rows; j++)
                    if (Math.Abs(Fitness(Function(data.coefficients[j], agents[i].genes), data.desiredValues[j]) - agents[i].fitness) < targetFitnessDelta) return false;
            return true;
        }

        while (true)
        {
            if (IsEvolutionComplete()) return;
            if (iteration > maxIterations) return;
            iteration++;
            FitnessAgents();
            SortAgents();
            EvolveAgents();
        }
    }

    void SortAgents() => //сортирует агентов по общему фитнесу
        agents = agents.OrderByDescending(agent => agent.fitness).ToList();

    void FitnessAgents()
    {
        for (int i = 0; i < agentsCount; i++)
            for (int j = 0; j < data.rows; j++)
                agents[i].fitness += Fitness(Function(data.coefficients[j], agents[i].genes), data.desiredValues[j]);
    }

    void EvolveAgents() //проверить и доделать
    {
        var newPopulation = new List<Agent>(agentsCount);
        for (int i = 0; i < evolvePopulationCount; i+=2)
        {
            for (int k = 0; k < (agentsCount/evolvePopulationCount)*2; k++)
            {
                Agent crossover = new Agent(genesCount);
                for (int j = 0; j < genesCount; j++)
                {
                    var gene = Crossover(agents[i].genes[j], agents[i+1].genes[j]);
                    Mutate(ref gene);
                    Inverse(ref gene);
                    crossover.genes[j] = gene;
                }
                newPopulation.Add(crossover);
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

    void Mutate(ref double x) //мутация: генерация случайной величины
    {
        if (rnd.NextDouble() > mutateChance) return;
        int bitToFlip = rnd.Next(64);
        ulong mask = (byte)(1 << bitToFlip);
        x = UInt64BitsToDouble(DoubleToUInt64Bits(x) ^ mask);
    }

    void Inverse(ref double x) //инверсия: поиск в окрестностях точки
    {
        if (rnd.NextDouble() > inverseChance) return;
        var targetBit = rnd.Next(1, 63);
        x = UInt64BitsToDouble((DoubleToUInt64Bits(x) << targetBit) | (DoubleToUInt64Bits(x) >> 64 - targetBit));
    }

    double Crossover(double x, double y) //кроссовер
    {
        var targetBit = rnd.Next(1, 63);
        return rnd.Next(0, 1) > 0.5?
            UInt64BitsToDouble((DoubleToUInt64Bits(x) >> 64 - targetBit << 64 - targetBit) | (DoubleToUInt64Bits(y) << targetBit >> targetBit)) : 
            UInt64BitsToDouble((DoubleToUInt64Bits(x) << targetBit >> targetBit) | (DoubleToUInt64Bits(y) >> 64 - targetBit << 64 - targetBit));
    }
}