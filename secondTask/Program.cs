using System.IO;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace secondTask
{
    internal class Program
    {
        /// <summary>
        /// Отвечает за наполнение нового видоизмененного массива с помощью полученного массива из файла (B)
        /// </summary>
        /// <param name="B">Старый массив</param>
        /// <param name="C">Новый массив</param>
        public static void Slice(ref double[][] B, ref double[][] C)
        {
            int k = 0, m = 0;
            for (int i=0; i<B.Length; i++)
            {
                C[i] = new double[B[i].Length - 1];
                m = 0;
                for (int j = 0; j < B[i].Length; j++)
                {
                    if (i == j) // условие для сдвига
                    {
                        continue;
                    }
                    C[k][m] = B[i][j];
                    m++;
                }
                k++;
            }
        }

        /// <summary>
        /// Чтение файла с ловлей ошибок связанных с содержимом файла. Если появляется такая ошибка, то возвращает false, за счет чего в Main вызываем заново.
        /// Возвращаем его перезапись в массив B. Вызов метода для формирования нового массива по созданию и вызов функции по выводу массивов на консоль.
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public static bool Reading(string directory)
        {

            string path = directory + Path.DirectorySeparatorChar + Console.ReadLine() + ".txt"; // построение пути к файлу
            using (StreamReader sr = new StreamReader(path))
            {
                string line = sr.ReadLine(); // считывание первой строки
                int N; // первое число файла (размерность массива)
                int i = 0, j = 0; // индексы для занесения данных в массив
                if (line == null || line.Length == 0) // ловля одной из ошибок
                {
                    Console.WriteLine("Файл пустой, повторите попытку:");
                    return false;
                }
                //проверка данных в файле на корректность 
                if (!int.TryParse(line, out N))
                {
                    Console.WriteLine("Нарушен формат данных в файле. Пожалуйста, выберите другой:");
                    return false;
                }
                double[][] B = new double[N][]; // создание массива для переноса данных из файла
                line = sr.ReadLine();
                while (line != null) // цикл для считывания всех строк
                {
                    string[] elements = line.Split("  ", StringSplitOptions.RemoveEmptyEntries); // разделение строки по элементам
                    B[i] = new double[elements.Length];
                    foreach (string element in elements) // цикл добавления элементов в массив
                    {
                        double massiv_element;
                        if (!double.TryParse(element, out massiv_element)) // проверка данных на корректность
                        {
                            Console.WriteLine("Нарушен формат данных в файле. Пожалуйста, выберите другой:");
                            return false;
                        }
                        B[i][j] = massiv_element;
                        j++;
                    }
                    line = sr.ReadLine();
                    i++;
                    j = 0;
                    N--; // уменьшая N, мы потом сможем проверить сопадает ли оно с истинным размером массива (должно стать равно 0 в таком случае)
                }
                if (N!=0)
                {
                    Console.WriteLine("Истинный размер массива и первое значение файла не совпадают. Пожалуйста, выберите другой:");
                    return false;
                }
                double[][] C = new double[B.Length][];
                Slice(ref B, ref C);
                Print(B, C);  
            }
            return true;
        }

        /// <summary>
        /// Вывод двух полученных массивов
        /// </summary>
        /// <param name="B">Старый массив</param>
        /// <param name="C">Новый массив</param>
        public static void Print(double[][] B, double[][] C)
        {
            for (int i = 0; i < B.Length; i++)
            {
                for (int j = 0; j < B[i].Length; j++)
                {
                    Console.Write($"{B[i][j]:f3} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            for (int i = 0; i < C.Length; i++)
            {
                for (int j = 0; j < C[i].Length; j++)
                {
                    Console.Write($"{C[i][j]:f3} ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Основной метод с вызывами всех функций и проверки данных с помощью try catch.
        /// </summary>
        static void Main()
        {
            do {
                Console.Clear();
                Console.WriteLine("Введите название файла, который хотите открыть:");
                string directory = System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString()).ToString()).ToString(); // поиск дериктории с файлами из прошлой задачи
                while (true)
                {
                    try // ловля исключений
                    {
                        if (Reading(directory)==false)
                        {
                            continue;
                        }
                        break;
                    }
                    catch (FileNotFoundException ex) // исключение отсутствие файла
                    {
                        Console.WriteLine("Такой файл не существует, повторите попытку:");
                        continue;
                    }
                    catch (IOException ex) // поимка исключения при вводе некорректных символов
                    {
                        Console.WriteLine("Введено некорректное название файла. Повторите попытку:");
                        continue;
                    }
                    catch (Exception ex) // поимка остальных
                    {
                        Console.WriteLine("Возникла непредвиденная ошибка, повторите попытку:");
                        continue;
                    }
                }
                Console.WriteLine("Если вы хотите выйти из программы, то нажмите ESC, в ином случае программа повторится...");
            } while (Console.ReadKey().Key != ConsoleKey.Escape); // продолжение программы по желанию пользователя
        }
    }
}