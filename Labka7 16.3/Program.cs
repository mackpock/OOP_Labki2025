using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Labka7_16._3
{
    internal class Program
    {


        struct Book
        {
            public string author;   // фамилия 
            public string title;    // название книги
            public int year;        // год издания
        }
        static void Main(string[] args)
        {
            const int COUNT = 10;
            Book[] books = new Book[COUNT];
            try
            {
                using (FileStream fs = new FileStream("books.txt", FileMode.Open))
                using (StreamReader reader = new StreamReader(fs))
                {
                    for (int i = 0; i < COUNT; i++)
                    {
                        string line = reader.ReadLine();
                        string[] parts = line.Split(';');
                        books[i].author = parts[0];
                        books[i].title = parts[1];
                        books[i].year = int.Parse(parts[2]);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Не удалось прочитать файл");
                return;
            }

            // а) Найти названия книг данного автора, изданных с 1960 г.,
            Console.Write("Введите фамилию автора: ");
            string targetAuthor = Console.ReadLine();

            Console.WriteLine($"\nКниги автора '{targetAuthor}', изданные с 1960 года:");
            bool A = false;
            for (int i = 0; i < books.Length; i++)
            {
                if (books[i].author == targetAuthor && books[i].year >= 1960)
                {
                    Console.WriteLine($"  - {books[i].title} ({books[i].year})");
                    A = true;
                }
            }
            if (!A)
            {
                Console.WriteLine("  Таких книг не найдено.");
            }

            // б) Определить, имеются ли книги с названием "Информатика" и если да, то сообщить фамилии авторов, год издания этих книг.*/
            Console.WriteLine("\nКниги с названием \"Информатика\":"); 
            bool B = false;
            for (int i = 0; i < books.Length; i++)
            {
                if (books[i].title == "Информатика")
                {
                    Console.WriteLine($"  Автор: {books[i].author}, год издания: {books[i].year}");
                    B = true;
                }
            }
            if (!B)
            {
                Console.WriteLine("  Книг с таким названием не найдено.");
            }

            Console.ReadLine();
        }
    }
}
