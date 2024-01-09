using System.Reflection.Metadata.Ecma335;

namespace firstTask
{
    internal class Program
    {
        /// <summary>
        /// Заполнение N, пока оно не будет заполнено корректно
        /// </summary>
        /// <param name="N">Количество строк массива</param>
        public static void CorrectDataForN(out int N)
        {
            Console.WriteLine("Введите N, оно должно быть натуральным и не больше 13: ");
            while (!(int.TryParse(Console.ReadLine(), out N) && (N > 0) && (N < 14)))
            {
                Console.WriteLine("Введены неверные данные, перечитайте условие на них и повторите ввод.");
            }
        }

        /// <summary>
        /// Заполнение M, пока оно не будет заполнено корректно
        /// </summary>
        /// <param name="M">Количество столбцов массива</param>
        public static void CorrectDataForM(out int M)
        {
            Console.WriteLine("Введите M, оно должно быть натуральным и не больше 17: ");
            while (!(int.TryParse(Console.ReadLine(), out M) && (M > 0) && (M < 18)))
            {
                Console.WriteLine("Введены неверные данные, перечитайте условие на них и повторите ввод.");
            }
        }

        /// <summary>
        /// Метод заполнения массива через функцию
        /// </summary>
        /// <param name="B">Массив для записи значений функции</param>
        /// <param name="N">Количество строк</param>
        /// <param name="M">Количество столбцов</param>
        public static void Filling(out double[][] B, int N, int M)
        {
            B = new double[N][];
            double n = 0;

            for (int j=0; j<N; j++)
            {
                B[j] = new double[M];
                for (int i = 0; i < M; i++)
                {
                    B[j][i] = Math.Pow((6.0 * n * n + 3 * n + 2) / (6 * n * n + 3 * n + 3), n); // присваивание элементу массива значение функции
                    n++;
                }
            }
        }

        /// <summary>
        /// Создание файла
        /// </summary>
        /// <param name="path"></param>
        /// <param name="M">Количество столбцов</param>
        /// <param name="N">Количество строк</param>
        /// <param name="A">Массив с данными</param>
        public static void CreatingFile(string directory, int M, int N, double[][] A)
        {
            while (true)
            {
                string name = Console.ReadLine();
                if (name.Length == 0) // Если пользователь не ввел имя, то запускаем цикл заново.
                {
                    Console.WriteLine("Вы не ввели название файла, пожалуйста, повторите ввод:");
                    continue;
                }
                string path = directory + Path.DirectorySeparatorChar + name + ".txt"; // создание пути для нового файла или поиска старого

                if (File.Exists(path)) // если уже существует такой файл, то очищаем его
                {
                    Console.WriteLine("Такой файл уже существует, поэтому данные будут перезаписаны.");
                    // очищение уже существующего файла от старых данных
                    FileStream fileStream = File.Open(path, FileMode.Open);
                    fileStream.SetLength(0);
                    fileStream.Close();
                }

                try
                {
                    // создание файла и заполнение его данными
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine($"{N}");
                        for (int i = 0; i < N; i++)
                        {
                            for (int j = 0; j < M; j++)
                            {
                                sw.Write($"{A[i][j]:f3}  ");
                            }
                            sw.WriteLine();
                        }
                    }
                    Console.WriteLine("Данные записаны успешно!");
                    break;
                }
                catch (IOException ex) // поимка одного из исключений
                {
                    Console.WriteLine("Введено некорректное название файла. Повторите попытку:");
                    continue;
                }
                catch (Exception ex) // поимка остальных
                {
                    Console.WriteLine("Возникла непредвиденная ошибка, пожалуйста, повторите попытку:");
                    continue;
                }
            }
        }

        /// <summary>
        /// Основной метод с вызывами всех функций и бесконечным циклом.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            do
            {
                Console.Clear();
                // ввод данных и проверка их на корректность
                int N, M;
                CorrectDataForN(out N);
                CorrectDataForM(out M);

                double[][] A; // создание массива
                Filling(out A, N, M); // заполнение массива с помощью метода
                string directory = System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString()).ToString()).ToString(); // поиск директории с папкой решения
                Console.WriteLine("Для создания файла задайте ему название: ");
                CreatingFile(directory, M, N, A);
                Console.WriteLine("Если вы хотите выйти из программы, то нажмите ESC, в ином случае программа повторится...");
            } while(Console.ReadKey().Key!= ConsoleKey.Escape);
        }
    }
}