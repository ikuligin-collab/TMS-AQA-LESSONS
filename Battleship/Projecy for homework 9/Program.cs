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
        public override string ToString()
        {
            return $"{Name} ({Price} руб.)"; 
            // Названия полей укажите те, что используются у вас в классе
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
        public static void PrintAllProducts (List<Product> products)
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
            Console.WriteLine($"Список {typeof(T).Name}"); // 
            foreach (var item in value) // перебор элементов
            {
                Console.WriteLine(item);
            }

            Console.WriteLine(); //отступ в консоли
        }

        //Получение первого элемента
        static T GetFirst<T>(List<T> items)
        {
            if (items == null || items.Count == 0)
                throw new InvalidOperationException("Список пуст!");

            return items[0];
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

        // Поиск самого дорогого товара
        static T GetMostExpensive<T>(List<T> items) where T : Product
        {
            if (items == null || items.Count == 0)
                throw new InvalidOperationException("Список пуст");
            T mostExpensive = items[0];
            foreach (var item in items)
            {
                if (item.Price > mostExpensive.Price)
                {
                    mostExpensive = item;
                }
            }

            return mostExpensive;
        }
        
        //Товары дешевле указанной суммы
        static List<T> GetProductCheaperThan<T>(List<T> products, decimal maximumPrice) where T : Product
        {
            if (products == null )
                throw new InvalidOperationException("Список пуст");
            List<T> result = new List<T>();
            foreach (var rpoduct in products)
            {
                if (rpoduct.Price <= maximumPrice)
                {
                    result.Add(rpoduct);
                }
                
            }
            return result;
        }


    // Main метьд
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // 1. Подготавливаем необходимые данные
            Book book1 = new Book("Мастер и Маргарита", 800, "М. Булгаков");
            Book book2 = new Book("1984", 1500, "Д. Оруэлл");
            Phone phone1 = new Phone("iPhone 15 pro Maх", 99000, "Apple");
            Phone phone2 = new Phone("Galaxy S24", 80000, "Samsung");

            // Коллекция всех товаров
            List<Product> cart = new List<Product> { book1, book2, phone1, phone2 };

            // Коллекция предметов со скидкой (через интерфейс IDiscountable)
            List<IDiscountable> discountItems = new List<IDiscountable> { book1, phone1 };

            
            // Вызов каждого метода и вывод результата

            // Вызов метода "вывод всех продуктов"
            PrintAllProducts(cart);

            // Вызов метода "2"
            ApplyDiscountToAll(discountItems, 10); // Скидка 10% на book1 и phone1

            // Повторный вызов метода 1, чтобы показать изменение цен после скидки
            PrintAllProducts(cart);

            // Вызов метода 3 + вывод результата
            decimal totalPrice = CalculateTotalPrice(cart);
            Console.WriteLine($"Общая стоимость товаров в корзине: {totalPrice:C}");
           
            //-------
            // Проверка дженерик методов
            //-------
            Console.WriteLine("=== Проверка Generic-методов ===\n");
            
            // 1. PrintValue (проверяем число, строку, bool)
            Console.WriteLine("1. PrintValue:");
            PrintValue(cart[0].Price);// проверка на число
            PrintValue($"{cart[0].Name}");// проверка на строку
            PrintValue(true);
            Console.WriteLine("-------------");
            
            
            // 2. PrintList (проверяем со числами и строками)
            
            // Создаем пустой список для названий
            List<string> productNames = new List<string>();
            //Заполняем его 
            foreach (var product in cart)
            {
                productNames.Add(product.Name); 
            }
            // Создаем пустой список для цен
            List<decimal> productPrices = new List<decimal>();
            //Заполняем его 
            foreach (var product in cart)
            {
                productPrices.Add(product.Price); 
            }
            Console.WriteLine("2. PrintList:");
            PrintList(productNames);// Выводим строки
            PrintList(productPrices); // выводим инты
            Console.WriteLine("-------------");
            
            // 3. GetFirst
            Console.WriteLine("3. GetFirst:");
            Console.WriteLine($"Цена первого товара: {GetFirst(cart).Price}");
            Console.WriteLine($"Наименование первого товара: {GetFirst(cart).Name}");
            Console.WriteLine("-------------");

            // 4. GetLast
            Console.WriteLine("4. GetLast:");
            Console.WriteLine($"Наименование последнего товара: {GetLast(cart).Name}");
            Console.WriteLine($"Цена последнего товара: {GetLast(cart).Price}");
            Console.WriteLine("-------------");
            
            //5. GetByIndex
            Console.WriteLine("5. GetByIndex:");
            Console.WriteLine($"Товар под индексом 2: {GetByIndex(cart, 2)}\n");
            Console.WriteLine("-------------");

            // 6. Repeat
            Console.WriteLine("6. Repeat:");
            List<Product> repeatedCarts = Repeat(cart[2], 2); // создаю список из повторяющегося второго объекта
            PrintList(repeatedCarts);//Вывод нового списка
            Console.WriteLine("-------------");

            //7. Copy
            Console.WriteLine("7. Copy:");
            List<Product> cartCopy = Copy(cart); // Создаем копию списка товаров
            PrintList(cartCopy);
            Console.WriteLine("-------------");

            // 8. Merge
            Console.WriteLine("8. Merge:");
            // Создаем второй список товаров для объединения
            List<Product> additionalProducts = new List<Product>
            {
                new Book("Преступление и наказание", 700, "Ф. Достоевский"),
                new Phone("Pixel 8", 65000, "Google")
            };
            List<Product> mergedCart = Merge(cart, additionalProducts); // Объединяем корзину с новым списком
            PrintList(mergedCart);
            Console.WriteLine("-------------");

            // 9. Reverse
            Console.WriteLine("9. Reverse:");
            List<Product> reversedCart = Reverse(cart); // Разворачиваем список товаров
            PrintList(reversedCart);
            Console.WriteLine("-------------");

            // 10. Take
            Console.WriteLine("10. Take:");
            List<Product> takenProducts = Take(cart, 2); // Берем первые 2 товара из корзины
            PrintList(takenProducts);
            Console.WriteLine("-------------");
            
            //Поиск самого дорогого товара
            Console.WriteLine("MostExpensive:");
            Product mostExpensive = GetMostExpensive(cart);
            Console.WriteLine($"Самый дорогой товар {mostExpensive.Name}");
            Console.WriteLine("-------------");  
            Console.ReadKey();
        }
    }
}