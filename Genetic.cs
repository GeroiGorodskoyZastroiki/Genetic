using System.Collections.Generic;
using System;

public class Genetic
{
    Excel data;
    byte agents;
    byte targetFitness;
    int iter = 0;
    byte[][6] genes; //population byte[agents][6]
    byte[] currentFitness;
    Random rnd = new Random();

    Genetic(Excel data, byte agents, byte targetFitness)
    {
        this.data = data;
        this.agents = agents;
        this.targetFitness = targetFitness;

        genes = new byte[agents][6];
        currentFitness[agents];

        GenerateGenes();
        //Evolve();
    }

    void GenerateGenes() //генерируем случайные гены
    {
        for (int i = 0; i < agents; i++) 
        {
            byte[6] buffer;
            rnd.NextBytes(buffer);
            genes[i] = buffer;
        }
    }

    void Evolve()
    {
        //и тут большой луп
        while (true)
        {
            for (int i; i < data.rows; i++)
            {
                Fitness(Function(), data.desired[i]);
            }
            iter++;
            Mutation();
            Sort(genes);
            Crossover(genes);
        }
    }

    short Function(byte a, byte b, byte c, byte d, byte e, byte f, byte x1, byte x2, byte x3) => 
        a*x1+b*x2+c*x3+d*x1*x1+e*x2*x2+f*x3*x3;

    short Fitness(short y, short desired)
    {
        error = abs(desired-y);
        errorNorm = 1/(1+Exp(-2*error));
        return 1-errorNorm;
    }

    double Mutation(double x0, double x1)  // мутация: генерация случайной величины
    {
        
    }

    double Inversion(double x, double eps)  // инверсия: поиск в окрестностях точки
    {
        int sign = 0;//static
        sign++;
        sign %= 2;
        if (sign == 0) return x - eps;
        else return x + eps;
    }

    void Crossover()  // кроссовер: среднее арифметическое
    {
        //определять на сколько сдвигать
        //сдвигать одно влево, другое в право на кол-во знаков
    }

    void Sort(double *x, double *y)  // сортировка
    {
        for (int i = 0; i < 100; i++)
            for (int j = i + 1; j < 100; j++) 
            if (fabs(y[j]) < fabs(y[i])) {
                double temp = y[i];
                y[i] = y[j];
                y[j] = temp;
                temp = x[i];
                x[i] = x[j];
                x[j] = temp;
            }
    }
}

