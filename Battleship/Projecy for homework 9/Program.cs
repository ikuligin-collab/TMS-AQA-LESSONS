using System;
using System.Collections.Generic;


namespace TaskSolution
{
    
    // Интерфейс скидки
    public interface IDiscountable
    {
        void ApplyDiscount(decimal percentage);
    }

    // Базовый класс Товар
    public class Product : IDiscountable
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public virtual void ApplyDiscount(decimal percentage)
        {
            Price -= Price * (percentage / 100);
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Товар: {Name} | Цена: {Price:C}");
        }
    }

    // Класс Книга (наследник Product)
    public class Book : Product
    {
        public string Author { get; set; }

        public Book(string name, decimal price, string author) : base(name, price)
        {
            Author = author;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Книга: '{Name}' ({Author}) | Цена: {Price:C}");
        }
    }

    // Класс Телефон (наследник Product)
    public class Phone : Product
    {
        public string Brand { get; set; }

        public Phone(string name, decimal price, string brand) : base(name, price)
        {
            Brand = brand;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Телефон: {Brand} {Name} | Цена: {Price:C}");
        }
    }
    
    // 2. ГЛАВНЫЙ КЛАСС ПРОГРАММЫ
    internal class Program
    {
       
        //Вывод списка всех товаров
        public static void PrintAllProducts(List<Product> products)
        {
            Console.WriteLine("--- Список всех товаров ---");
            foreach (var product in products)
            {
                product.DisplayInfo();
            }
            Console.WriteLine();
        }

        //Применение скидки к объектам, поддерживающим IDiscountable
        public static void ApplyDiscountToAll(List<IDiscountable> discountables, decimal discountPercent)
        {
            Console.WriteLine($"--- Применение скидки {discountPercent}% ---");
            foreach (var item in discountables)
            {
                item.ApplyDiscount(discountPercent);
            }
            Console.WriteLine("Скидка успешно применена!\n");
        }

        //Расчет общей стоимости всех товаров
        public static decimal CalculateTotalPrice(List<Product> products)
        {
            decimal total = 0;
            foreach (var product in products)
            {
                total += product.Price;
            }
            return total;
        }
        
        //Вывод одного значения
        static void PrintValue<T>(T value)
        {
            Console.WriteLine($"Значение: {value}");
        }

        // Вывод всех элементов списка
        static void PrintList<T>(List<T> value)
        {
            Console.WriteLine($"Список {typeof(T).Name}:)"); // 
            foreach (var item in value) // перебор элементов
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(); //отступ в консоли
        }
        
        // Получение последнего элемента списка
        static T GetLast<T>(List<T> items)
        {
            if (items == null || items.Count == 0)
                throw new InvalidOperationException("Список пуст!");

            return items[items.Count - 1]; // У последнего элемента индекс на 1 больше, чем счётчик
        }
        
        // Получение элемента по индексу
        static T GetByIndex<T>(List<T> items, int index)
        {
            if (index < 0 || index >= items.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне границ списка.");

            return items[index];
        }
        
         // Вывод списка повторяющихся элементов
        static List<T> Repeat<T>(T value, int count)
        {
            List<T> result = new List<T>();
            for (int i = 0; i < count; i++)
            {
                result.Add(value);
            }
            return result;
        }
        
        // Копирование списка
        static List<T> Copy<T>(List<T> items)
        {
            return new List<T>(items);
        }
        
        //Объеденение 2х списков 
        static List<T> Merge<T>(List<T> first, List<T> second)
        {
            List<T> result = new List<T>(first);
            result.AddRange(second);
            return result;
        }
        
        // Переворот списка static List<T> Reverse<T>(List<T> items)
        static List<T> Reverse<T>(List<T> items)
        {
            List<T> result = new List<T>(items);
            result.Reverse(); // Переворачивает копию
            return result;
        }
        
        // Вывод прервых элементов
        static List<T> Take<T>(List<T> items, int count)
        {
            List<T> result = new List<T>();
            int limit = Math.Min(count, items.Count); // Чтобы не выйти за пределы массива
            for (int i = 0; i < limit; i++)
            {
                result.Add(items[i]);
            }
            return result;
        }
        
        // Main метьд
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // 1. Подготавливаем необходимые данные
            Book book1 = new Book("Идиот", 800, "Ф. Достоевский");
            Book book2 = new Book("Чистый код", 1500, "Р. Мартин");
            Phone phone1 = new Phone("iPhone 15", 90000, "Apple");
            Phone phone2 = new Phone("Galaxy S24", 80000, "Samsung");

            // Коллекция всех товаров
            List<Product> cart = new List<Product> { book1, book2, phone1, phone2 };

            // Коллекция предметов со скидкой (через интерфейс IDiscountable)
            List<IDiscountable> discountItems = new List<IDiscountable> { book1, phone1 };

            
            // 2. Вызываем КАЖДЫЙ созданный метод и выводим результаты

            // Вызов метода 1
            PrintAllProducts(cart);

            // Вызов метода 2
            ApplyDiscountToAll(discountItems, 10); // Скидка 10% на book1 и phone1

            // Повторный вызов метода 1, чтобы показать изменение цен после скидки
            PrintAllProducts(cart);

            // Вызов метода 3 + вывод результата
            decimal totalPrice = CalculateTotalPrice(cart);
            Console.WriteLine($"Общая стоимость товаров в корзине: {totalPrice:C}");

            Console.ReadKey();
        }
    }
}