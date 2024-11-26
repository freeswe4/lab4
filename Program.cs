namespace lab4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random(); //ДСЧ для создания случайных значений

            int maxLen = (int)System.Array.MaxLength; //максимальная длина массива
            int minLen = 1; //минимальная длина массива
            int len = 0; //длина массива
            int k = 0; //номер для различных действий с массивом

            int[] arr = new int[0]; //основной массив для работы
            int[] temp = new int[0]; //вспомогательный массив

            int answer; //ответ для выбора задачи
            int chooseArr; //ответ для выбора способа создания массива

            bool isCorrect; //флаг для проверки корректности данных
            bool isSortArr = false; //флаг отсортирован массив или нет

            int j = 0; //вспомогательный счётчик для циклов

            do
            {
                Console.WriteLine("Выберите команду");
                Console.WriteLine("1. Создать массив");
                Console.WriteLine("2. Распечатать массив");
                Console.WriteLine("3. Удалить элемент из массива по номеру");
                Console.WriteLine("4. Добавить элемент в массив по номеру");
                Console.WriteLine("5. Переставить элементы: чётные - в начале, нечётные - в конце");
                Console.WriteLine("6. Найти элемент, равный среднему арифметическому всех элементов массива(округлённому)");
                Console.WriteLine("7. Отсортировать элементы методом простого включения");
                Console.WriteLine("8. Найти, введённый с клавиатуры, элемент");
                Console.WriteLine("9. Выход");

                do
                {
                    isCorrect = Int32.TryParse(Console.ReadLine(), out answer);
                    if (answer < 1 || answer > 9)
                    {
                        isCorrect = false;
                        Console.WriteLine("Ошибка ввода. Выберите номер команды из списка выше");
                    }                        
                } while (!isCorrect);

                switch (answer)
                {
                    case 1: //создание массива
                        Console.WriteLine("Введите количество элементов массива");
                        do
                        {
                            isCorrect = Int32.TryParse(Console.ReadLine(), out len);
                            if (len < minLen || len > maxLen)
                            {
                                isCorrect = false;
                                Console.WriteLine($"Ошибка ввода. Введите количество элементов массива от 1 до максимально допустимой длины массива ({System.Array.MaxLength})");
                            }                               
                        } while (!isCorrect);

                        arr = new int[len];

                        Console.WriteLine("Как вы хотите создать массив?\n1. С помощью датчика случайных чисел\n2. Самостоятельный ввод  с клавиатуры");
                        do
                        {
                            isCorrect = Int32.TryParse(Console.ReadLine(), out chooseArr);
                            if (chooseArr != 1 && chooseArr != 2)
                            {
                                isCorrect = false;
                                Console.WriteLine("Ошибка ввода. Введите номер способа создания массива из списка выше");
                            }    
                        } while (!isCorrect);

                        switch (chooseArr)
                        {
                            case 1:
                                arr = CreateArr1(arr, len, rnd);
                                isSortArr = false;
                                break;

                            case 2:
                                arr = CreateArr2(arr, len, isCorrect);
                                isSortArr = false;
                                break;
                        }
                        break;

                    case 2: //печать массива
                        TypeArr(arr);
                        break;

                    case 3: //удаление элемента из массива по номеру
                        arr = DeleteElem(arr, temp, k, isCorrect);
                        break;

                    case 4: //добавление элемента в массив
                        arr = AddElem(arr, temp, k, isCorrect, rnd, maxLen);
                        break;

                    case 5: //перестановка чётных элементов в начало, нечётных - в конец
                        arr = PermutElem(arr, temp, j, isCorrect);
                        break;

                    case 6: //поиск элемента, равного среднему арифметическому всех элементов массива
                        FindElemSimp(arr, isCorrect);
                        break;

                    case 7: //сортировка элементов простым включением
                        arr = SortElem(arr, temp, j);
                        if (arr.Length != 0)
                            isSortArr = true;
                        break;

                    case 8: //поиск заданного пользователем элемента (бинарный поиск) 
                        FindElemBin(arr, isCorrect, isSortArr);
                        break;
                }
            } while (answer != 9);
        }

        #region 1 class
        static int[] CreateArr1(int[] arr, int len, Random rnd)
        {
            for (int i = 0; i < len; i++)
                arr[i] = rnd.Next(-1000, 1000); //генерация элемента в отрезке от -1000 до 1000
            Console.WriteLine("Датчик случайных числе сгенерировал элементы. Массив создан\n");
            return arr;
        }
        #endregion

        #region 2 class
        static int[] CreateArr2(int[] arr, int len, bool isCorrect)
        {
            int num; //элемент создающегося массива
            for (int i = 0; i < len; i++)
            {
                Console.WriteLine($"Введите целое число - элемент массива\nВведено {i} элементов");
                do
                {
                    isCorrect = Int32.TryParse(Console.ReadLine(), out num); //проверка элемента создющегося массива
                    if (!isCorrect)
                        Console.WriteLine("Ошибка ввода. Введите целое число - элемент массива");
                } while (!isCorrect);
                arr[i] = num; //присваивание элемента создающегося массива
            }
            Console.WriteLine("Массив создан\n");
            return arr;
        }
        #endregion

        #region 3 class
        static void TypeArr(int[] arr)
        {
            if (arr.Length == 0)
                Console.WriteLine("Массив пустой. Создайте сначала массив\n"); //если массив до этого не создавался
            else
            {
                for (int i = 0; i < arr.Length; i++)
                    Console.Write(arr[i] + " ");
                Console.WriteLine("\nМассив распечатан\n");
            }
        }
        #endregion

        #region 4 class
        static int[] DeleteElem(int[] arr, int[] temp, int k, bool isCorrect)
        {
            if (arr.Length == 0)
                Console.WriteLine("Массив пустой. Создайте сначала массив\n"); //если массив пустой
            else
            {
                Console.WriteLine($"Введите номер элмента в массиве, который нужно удалить (в пределах от 1 до длины массива)\nP.s.Длина вашего массива равна {arr.Length})");
                do
                {
                    isCorrect = Int32.TryParse(Console.ReadLine(), out k); //ввод и проверка номера массива для удаления
                    if (0 >= k || k > arr.Length)
                    {
                        isCorrect = false;
                        Console.WriteLine("Ошибка ввода. Введите целое число от 1 до длины массива");
                    }
                } while (!isCorrect);

                temp = new int[arr.Length - 1]; //формирование вспомогательного массива и длиной на 1 меньше, чем основной

                for (int i = 0; i < arr.Length; i++)
                {
                    if (i < k - 1) //если ещё не прошли нужный элемент (формирование и печать)
                    {
                        temp[i] = arr[i];
                        Console.Write(temp[i] + " ");
                    }
                    if (i > k - 1)//если прошли нужный элемент (формирование и печать)
                    {
                        temp[i - 1] = arr[i];
                        Console.Write(temp[i - 1] + " ");
                    }
                }
                if (temp.Length == 0)
                    Console.WriteLine("Массив стал пустым");
                Console.WriteLine("\nЗадача выполнена\n");
            }
            return temp;
        }
        #endregion

        #region 5 class
        static int[] AddElem(int[] arr, int[] temp, int k, bool isCorrect, Random rnd, int maxLen)
        {
            if (arr.Length == maxLen)
            {
                Console.WriteLine("Массив полностью заполнен, вы больше не можете добавить в него элементы\nУдалите элементы, чтобы добавить другие в массив");
                return arr;
            }
            else
            {
                if (arr.Length == 0)
                {
                    arr = new int[arr.Length + 1];
                }
                int num, addAnswer; //элемент для добавления, вариант создания элемента
                Console.WriteLine("Как вы хотите сформировать элемент, который нужно добавить?\n1. С помощью ДСЧ\n2. Самостоятельный ввод с клавиатуры");
                do
                {
                    isCorrect = int.TryParse(Console.ReadLine(), out addAnswer); //проверка ввода номера команды
                    if (addAnswer != 1 && addAnswer != 2)
                    {
                        isCorrect = false;
                        Console.WriteLine("Ошибка ввода. Введите номер одной из команд приведённых выше");
                    }
                } while (!isCorrect);

                Console.WriteLine($"Введите номер, на какую позицию нужно добавить элемент, от 1 до длины массива + 1\nP.s. Длина вашего массива + 1 равна {arr.Length + 1})");
                do
                {
                    isCorrect = Int32.TryParse(Console.ReadLine(), out k); //проверка ввода номера массива
                    if (k < 1 || k > arr.Length + 1)
                    {
                        isCorrect = false;
                        Console.WriteLine("Ошибка ввода. Введите целое число от 1 до длины массива");
                    }
                } while (!isCorrect);

                temp = new int[arr.Length + 1]; //вспомогательный массив, увеличенный на единицу для нового элемента

                switch (addAnswer)
                {
                    case 1:
                        num = rnd.Next(-100, 100); //генерация элемента для добавления
                        for (int i = 0; i < temp.Length; i++)
                        {
                            if (i == k - 1) //если i = номеру, который  нужно добавить
                            {
                                temp[i] = num; //добавляем элемент
                                Console.Write(temp[i] + " ");
                            }
                            else
                            {
                                if (i < k - 1) //если до нужного номера ещё не дошли
                                {
                                    temp[i] = arr[i];
                                    Console.Write(temp[i] + " ");
                                }
                                else //если нужный номер уже пройден
                                {
                                    temp[i] = arr[i - 1];
                                    Console.Write(temp[i] + " ");
                                }
                            }
                        }
                        Console.WriteLine("\nЗадача выполнена\n");
                        break;

                    case 2:
                        Console.WriteLine("Введите число, которое нужно добавить");
                        do
                        {
                            isCorrect = Int32.TryParse(Console.ReadLine(), out num); //проверка ввода элемента
                            if (!isCorrect)
                                Console.WriteLine("Ошибка ввода. Введите целое число");
                        } while (!isCorrect);
                        for (int i = 0; i < temp.Length; i++)
                        {
                            if (i == k - 1) //если дошли до нужного номера
                            {
                                temp[i] = num;
                                Console.Write(temp[i] + " ");
                            }
                            else
                            {
                                if (i < k - 1) //если не дошли до нужного номера
                                {
                                    temp[i] = arr[i];
                                    Console.Write(temp[i] + " ");
                                }
                                else //если уже перешли нужный номер
                                {
                                    temp[i] = arr[i - 1];
                                    Console.Write(temp[i] + " ");
                                }
                            }
                        }
                        Console.WriteLine("\nЗадача выполнена\n");
                        break;
                }
                return temp;
            }
        }
        #endregion

        #region 6 class
        static int[] PermutElem(int[] arr, int[] temp, int j, bool isCorrect)
        {
            if (arr.Length == 0)
            {
                Console.WriteLine("Массив пустой. Создайте сначала массив");
            }
            else
            {
                int h = arr.Length - 1;
                temp = new int[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    if (Math.Abs(arr[i]) % 2 == 0)
                    {
                        temp[j] = arr[i];
                        j++;
                    }
                    else
                    {
                        temp[h] = arr[i];
                        h--;
                    }
                }
                foreach (int item in temp)
                    Console.Write(item + " ");
                Console.WriteLine("\nЗадача выполнена");
            }
            int chooseReturn;
            Console.WriteLine("Что вы хотите оставить?\n1. Первончальный массив\n2. Массив с перемещением чётных и нечётных");
            do
            {
                isCorrect = Int32.TryParse(Console.ReadLine(), out chooseReturn);
                if (!isCorrect || (chooseReturn != 1 && chooseReturn != 2))
                    Console.WriteLine("Ошибка ввода. Введите номер одной из команд выше");
            } while (!isCorrect);
            switch (chooseReturn)
            {
                case 1:
                    return arr;
                default:
                    return temp;
            }
                
        }
        #endregion

        #region 7 class
        static void FindElemSimp(int[] arr, bool isCorrect)
        {
            if (arr.Length == 0)
                Console.WriteLine("Массив пустой. Создайте сначала массив");
            else
            {
                int sumArr = 0, midArithmetic, count = 0;
                isCorrect = false;
                foreach (int item in arr)
                    sumArr += item;
                midArithmetic = sumArr / arr.Length; //округлённое значение среднего арифметического
                Console.WriteLine($"Среднее арифметическое всех элементов массива равно {midArithmetic}");
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i] == midArithmetic)
                    {
                        isCorrect = true;
                        count++;
                        Console.WriteLine($"Позиция элемента, который равен среднему арифметическому всех элементов массива, равна {i + 1}\nПотребовалось {count} сравнений для нахождения");
                        break;
                    }
                    count++;
                }
                if (!isCorrect)
                    Console.WriteLine($"Такого элемента в массиве нет\nПотреблвалось {count} сравнений");
                Console.WriteLine("Задача выполнена\n");
            }
        }
        #endregion

        #region 8 class
        static int[] SortElem(int[] arr, int[] temp, int j)
        {
            if (arr.Length == 0)
                Console.WriteLine("Массив пустой. создайте сначала массив");
            else
            {
                int elem;
                temp = new int[arr.Length];
                for (int i = 1; i < arr.Length; i++)
                {
                    elem = arr[i];
                    j = i - 1;
                    while (j >= 0 && elem < arr[j])
                    {
                        arr[j + 1] = arr[j];
                        j--;
                    }
                    arr[j + 1] = elem;
                }
                Console.WriteLine("Отсортированный массив:");
                foreach (int item in arr)
                    Console.Write(item + " ");
                Console.WriteLine();
            }
            return arr;
        }
        #endregion

        #region 9 class
        static void FindElemBin(int[] arr, bool isCorrect, bool isSortArr)
        {
            if (!isSortArr)
            {
                Console.WriteLine("Массив не отсортирован. Выберите сначала 7 команду и отсортируйте массив");
            }
            else
            {
                int num, left = 0, right = arr.Length, middle, count = 0;
                Console.WriteLine("Введите элемент, который нужно попытаться найти в массиве");
                do
                {
                    isCorrect = Int32.TryParse(Console.ReadLine(), out num);
                    if (!isCorrect)
                        Console.WriteLine("Ошибка ввода. Введите целое число");
                } while (!isCorrect);
                isCorrect = false;
                do
                {
                    middle = (left + right) / 2;
                    if (arr[middle] == num)
                    {
                        left = middle;
                        isCorrect = true;
                    }
                    if  (num > arr[middle])
                        left = middle + 1;
                    else
                        right = middle;
                    count++;
                    if (isCorrect)
                        break;
                } while (left != right);
                if (num == arr[left])
                    Console.WriteLine($"Элемент нашёлся, он находится на позиции {left + 1}\nДля поиска потребовалось {count} сравнений");
                else
                {
                    Console.WriteLine($"Введённый элемент не нашёлся\nПотребовалось {count} сравнений");
                }
            }
            
        }
        #endregion
    }
} 