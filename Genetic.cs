using System.Collections.Generic;
using System;
using System.Linq;
using System.Net.Mail;

public class Genetic
{
    int iteration = 1;
    int maxIterations;
    Excel data;
    byte agentsCount;
    double targetFitness;
    byte genesCount = 6;
    byte[][] agents;
    double[] currentFitness; //вычисляется путём складывания всех фитнесов для каждой строчки (чем больше тем лучше)
    Random rnd = new Random();

    public Genetic(Excel data, byte agentsCount, double targetFitness, int maxIterations)
    {
        this.data = data;
        this.agentsCount = agentsCount;
        this.targetFitness = targetFitness;
        this.maxIterations = maxIterations;

        agents = new byte[agentsCount][];
        currentFitness = new double[agentsCount];

        GenerateAgents();
        Evolve();
    }

    void GenerateAgents() //генерируем случайные гены
    {
        for (int i = 0; i < agentsCount; i++) 
        {
            byte[] buffer = new byte[agentsCount];
            rnd.NextBytes(buffer);
            agents[i] = buffer;
        }
    }

    void Evolve()
    {
        bool IsEvolutionComplete()
        {
            for (int i = 0; i < agentsCount; i++)
                for (int j = 0; j < data.rows; j++)
                    if (Fitness(Function(data.coefficients[j], agents[i]), data.desiredValues[j]) < targetFitness) return false;
            return true;
        }

        while (true)
        {
            if (IsEvolutionComplete()) return;
            if (iteration > maxIterations) return;
            iteration++;
            SortAgents();
            CrossoverAgents();
        }
    }

    void SortAgents()
    {
        FitnessAgents();
        var sortedData = agents
            .Zip(currentFitness, (agent, fitness) => new { Agent = agent, Fitness = fitness })
            .OrderByDescending(data => data.Fitness)
            .ToArray();

        agents = sortedData.Select(data => data.Agent).ToArray();
        currentFitness = sortedData.Select(data => data.Fitness).ToArray();
    }

    void FitnessAgents()
    {
        for (int i = 0; i < agentsCount; i++)
        {
            currentFitness[i] = 0;
            for (int j = 0; j < data.rows; j++)
                currentFitness[i] += Fitness(Function(data.coefficients[j], agents[i]), data.desiredValues[j]);
        }
    }


    void CrossoverAgents() //тут могут быть разные стратегии
    {
        for (int i = 0; i < agentsCount/2; i++)
        {
            for (int j = 0; j < genesCount; j++)
            {
                var crossover = Crossover(agents[i][j], agents[i+1][j]);
                agents[i][j] = Mutation(crossover[0]);
                agents[i+1][j] = Mutation(crossover[1]);
                crossover = Crossover(agents[i][j], agents[i+1][j]);
                agents[i+(agentsCount/2)-1][j] = Mutation(crossover[0]);
                agents[i+(agentsCount/2)][j] = Mutation(crossover[1]);
            }
        }
    }

    short Function(List<byte> coefficients, byte[] genes) => (short)
        (genes[0]*coefficients[0]+
        genes[1]*coefficients[1]+
        genes[2]*coefficients[2]+
        genes[3]*coefficients[0]*coefficients[0]+
        genes[4]*coefficients[1]*coefficients[1]+
        genes[5]*coefficients[2]*coefficients[2]);

    double Fitness(short y, short desired)
    {
        var error = Math.Abs(desired-y);
        var errorNorm = 1/Math.Pow(1.02, error); //(1+Math.E*(-2*error));
        return errorNorm;
    }

    byte Mutation(byte x) // мутация: генерация случайной величины
    {
        int bitToFlip = rnd.Next(8);
        byte mask = (byte)(1 << bitToFlip);
        if (rnd.NextDouble() > 0.9) x = (byte)(x ^ mask);
        return x;
    }

    // double Inversion(double x, double eps) // инверсия: поиск в окрестностях точки
    // {
    //     int sign = 0;//static
    //     sign++;
    //     sign %= 2;
    //     if (sign == 0) return x - eps;
    //     else return x + eps;
    // }

    byte[] Crossover(byte n, byte m) // кроссовер
    {
        var targetBit = rnd.Next(1,8);
        string bitMaskString = "";
        for (int i = 0; i < 8; i++)
            bitMaskString += i < targetBit ? "1" : "0";
        byte bitMask = Convert.ToByte(bitMaskString, 2);
        return new byte[]{(byte)((n & bitMask) | (m & ~bitMask)), (byte)((n & ~bitMask) | (m & bitMask))};
    }
}