new Genetic(new Excel(), 20, 0.5, 200);

// byte[][] genes = new byte[][]
// {
//     new byte[] { 1, 2, 3 },
//     new byte[] { 4, 5, 6 },
//     new byte[] { 7, 8, 9 }
// };

// short[] currentFitness = new short[] { 50, 30, 70 };

// // Сортировка массива genes по убыванию на основе currentFitness
// SortGenesByFitness(genes, currentFitness);

// // Вывод отсортированных данных
// Console.WriteLine("Отсортированный массив genes:");
// foreach (var gene in genes)
// {
//     Console.WriteLine($"[{string.Join(", ", gene)}]");
// }
// static void SortGenesByFitness(byte[][] genes, short[] currentFitness)
// {
//     // Используем метод Array.Sort с собственным компаратором
//     Array.Sort(genes, (gene1, gene2) =>
//     {
//         // Получаем индексы для сравнения
//         int index1 = Array.IndexOf(genes, gene1);
//         int index2 = Array.IndexOf(genes, gene2);

//         // Сравниваем значения currentFitness по индексам
//         return currentFitness[index2].CompareTo(currentFitness[index1]);
//     });
// }

// Создаём новую случайную популяцию из 20 агентов
// {
//     Если по фитнесу во всех строках выходит корректное значение, то заканчиваем работу алгоритма
//     Мутируем их
//     Проверяем какие из них лучше всего подходят по фитнесу и сортируем
//     Отбираем половину, скрещиваем
// }