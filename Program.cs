using System;
using System.Text;
using System.Text.RegularExpressions;

namespace RegexTextAnalyzer
{
    class Program
    {
        unsafe static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Работа с регулярными выражениями и указателями ===\n");

            // ========== ЧАСТЬ 1: Регулярные выражения ==========
            string text = "Бык тупогуб, тупогубенький бычок, у быка губа бела была тупа. " +
                          "Мама мыла раму. Привет мир! Это тестовый текст. " +
                          "123-456-7890 - номер телефона. " +
                          "Москва, Санкт-Петербург, Новосибирск. " +
                          "Регулярные выражения в C# очень полезны.";

            Console.WriteLine("Исходный текст:");
            Console.WriteLine(text);
            Console.WriteLine();

            CountCharactersAndWords(text);
            LinesStartingWith(text, "Бык");
            LinesEndingWith(text, ".");
            ReplaceTextExample(text);

            // ========== ЧАСТЬ 2: Указатели (небезопасный код) ==========
            Console.WriteLine("\n=== РАБОТА С УКАЗАТЕЛЯМИ ===");
            Console.WriteLine("(Небезопасный код - unsafe)\n");

            // Объявляем переменные
            int value1 = 100;
            int value2 = 200;
            int value3 = 300;

            // Объявляем указатели
            int* ptr1;
            int* ptr2;
            int* ptr3;

            // Получаем адреса переменных
            ptr1 = &value1;
            ptr2 = &value2;
            ptr3 = &value3;

            // Выводим адреса и значения ДО изменения
            Console.WriteLine("=== ДО ИЗМЕНЕНИЯ ===");
            Console.WriteLine($"Переменная value1 = {value1}, Адрес: {(ulong)ptr1:X}");
            Console.WriteLine($"Переменная value2 = {value2}, Адрес: {(ulong)ptr2:X}");
            Console.WriteLine($"Переменная value3 = {value3}, Адрес: {(ulong)ptr3:X}");

            // Изменяем переменные через указатели
            *ptr1 = 999;
            *ptr2 = 888;
            *ptr3 = 777;

            // Выводим значения ПОСЛЕ изменения
            Console.WriteLine("\n=== ПОСЛЕ ИЗМЕНЕНИЯ ЧЕРЕЗ УКАЗАТЕЛИ ===");
            Console.WriteLine($"Переменная value1 = {value1} (было 100)");
            Console.WriteLine($"Переменная value2 = {value2} (было 200)");
            Console.WriteLine($"Переменная value3 = {value3} (было 300)");

            // Работа с указателем на указатель
            Console.WriteLine("\n=== УКАЗАТЕЛЬ НА УКАЗАТЕЛЬ ===");
            int* ptrMain = &value1;
            int** ptrToPtr = &ptrMain;

            Console.WriteLine($"Адрес указателя ptrMain: {(ulong)ptrToPtr:X}");
            Console.WriteLine($"Значение по адресу ptrMain (адрес value1): {(ulong)*ptrToPtr:X}");
            Console.WriteLine($"Значение value1 через двойной указатель: {**ptrToPtr}");

            // Изменяем через двойной указатель
            **ptrToPtr = 555;
            Console.WriteLine($"\nПосле изменения через двойной указатель:");
            Console.WriteLine($"value1 = {value1}");

            // Арифметика указателей
            Console.WriteLine("\n=== АРИФМЕТИКА УКАЗАТЕЛЕЙ ===");
            int[] numbers = { 10, 20, 30, 40, 50 };

            fixed (int* arrPtr = numbers)
            {
                Console.WriteLine("Массив numbers: [10, 20, 30, 40, 50]");
                Console.WriteLine($"Адрес первого элемента: {(ulong)arrPtr:X}");

                for (int i = 0; i < numbers.Length; i++)
                {
                    Console.WriteLine($"Элемент [{i}] = {*(arrPtr + i)}, Адрес: {(ulong)(arrPtr + i):X}");
                }
            }

            Console.WriteLine("\nНажмите любую клавишу для завершения...");
            Console.ReadKey();
        }

        static void CountCharactersAndWords(string text)
        {
            Console.WriteLine("=== 2. Подсчёт символов и слов ===");

            Regex lettersRegex = new Regex(@"[A-Za-zА-Яа-я0-9]");
            int letterCount = lettersRegex.Matches(text).Count;
            Console.WriteLine($"Количество букв и цифр: {letterCount}");

            Console.WriteLine($"Общее количество символов: {text.Length}");

            Regex wordRegex = new Regex(@"\b\w+\b");
            int wordCount = wordRegex.Matches(text).Count;
            Console.WriteLine($"Количество слов: {wordCount}");

            Regex phraseRegex = new Regex(@"тупогуб\w*");
            int phraseCount = phraseRegex.Matches(text).Count;
            Console.WriteLine($"Количество слов с корнем 'тупогуб': {phraseCount}");

            Console.WriteLine();
        }

        static void LinesStartingWith(string text, string startWord)
        {
            Console.WriteLine($"=== 3. Строки, начинающиеся с '{startWord}' ===");

            string[] lines = text.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            Regex regex = new Regex($"^{startWord}", RegexOptions.IgnoreCase);
            int found = 0;

            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                if (regex.IsMatch(trimmed))
                {
                    Console.WriteLine(trimmed);
                    found++;
                }
            }

            if (found == 0)
                Console.WriteLine("Не найдено.");
            else
                Console.WriteLine($"Найдено строк: {found}");

            Console.WriteLine();
        }

        static void LinesEndingWith(string text, string endSymbol)
        {
            Console.WriteLine($"=== 4. Строки, оканчивающиеся на '{endSymbol}' ===");

            string[] lines = text.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            string escaped = Regex.Escape(endSymbol);
            Regex regex = new Regex($"{escaped}$");
            int found = 0;

            foreach (string line in lines)
            {
                string trimmed = line.Trim() + ".";
                if (regex.IsMatch(trimmed))
                {
                    Console.WriteLine(trimmed);
                    found++;
                }
            }

            if (found == 0)
                Console.WriteLine("Не найдено.");
            else
                Console.WriteLine($"Найдено строк: {found}");

            Console.WriteLine();
        }

        static void ReplaceTextExample(string text)
        {
            Console.WriteLine("=== 5. Замена части текста ===");

            Regex spaceRegex = new Regex(@"\s+");
            string result1 = spaceRegex.Replace(text, " ");
            Console.WriteLine("После замены нескольких пробелов на один:");
            Console.WriteLine(result1);
            Console.WriteLine();

            Regex digitRegex = new Regex(@"\d");
            string result2 = digitRegex.Replace(text, "#");
            Console.WriteLine("После замены цифр на #:");
            Console.WriteLine(result2);
            Console.WriteLine();

            Regex wordRegex = new Regex(@"тупогуб\w*");
            string result3 = wordRegex.Replace(text, "****");
            Console.WriteLine("После замены 'тупогуб' на '****':");
            Console.WriteLine(result3);
        }
    }
}