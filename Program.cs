﻿// ulong ulv = BitConverter.DoubleToUInt64Bits(vl);
// Console.WriteLine(BitConverter.UInt64BitsToDouble(ulv));

new Genetic(new Dataset(), 20, 4, 0.01, 200, 0.9f, 0.9f);

// Создаём новую случайную популяцию из 20 агентов
// {
//     Если по фитнесу во всех строках выходит корректное значение, то заканчиваем работу алгоритма
//     Мутируем их
//     Проверяем какие из них лучше всего подходят по фитнесу и сортируем
//     Отбираем половину, скрещиваем
// }

//некорректная фитнес функция
//плохость генетического алгоритма (так как кроссовер скорее всё портит)
//по каким принципап организовать размножение

//останавливать когда разница между прошлыми y очень мало отличается, а не когда разница с desired маленькая
//delphy geno.base
