// Console.WriteLine(Convert.ToString(((long)((DoubleToUInt64Bits(x) >> 64 - targetBit << 64 - targetBit) | (DoubleToUInt64Bits(y) << targetBit >> targetBit))), 2).PadLeft(64, '0'));
// Console.WriteLine(Convert.ToString(((long)((DoubleToUInt64Bits(x) << targetBit >> targetBit) | (DoubleToUInt64Bits(y) >> 64 - targetBit << 64 - targetBit))), 2).PadLeft(64, '0'));

new Genetic(new Dataset(), 20, 4, 0.01, 200, 0.1f, 0.1f);

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
