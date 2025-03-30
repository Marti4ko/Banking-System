using System;
using System.IO;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Добре дошли в BularByte Online Banking!");
        Console.Write("Влизане или регистриране? (L/R): ");
        string check_name = "";
        int passcode = -1;
        string symbol = Console.ReadLine();
        if (symbol == "L" || symbol == "l")
        {
            Login.LoginMethod(ref check_name, ref passcode);
        }
        else if (symbol.ToLower() == "r")
        {
            Register.RegisterMethod();
        }
        else
        {
            Console.WriteLine("Невалиден символ!");
            Console.ReadKey();
            return;
        }
    }
    public class Register
    {
        public static void RegisterMethod()
        {
            Console.Write("Създайте своето потребителско име: ");
            string username = Console.ReadLine();
            if (File.Exists($"e:/data_base/bank_accounts/{username}.txt"))
            {
                Console.WriteLine("Такова потребителско име вече съществува! ");
                return;
            }
            else
            {
                Console.Write("Създайте своята парола: ");
                int password = int.Parse(Console.ReadLine());
                using (StreamWriter w = new StreamWriter($"e:/data_base/bank_accounts/{username}.txt"))
                {
                    w.WriteLine(username);
                    w.WriteLine(password);
                    w.WriteLine(0);
                }
                Console.WriteLine("Регистрирахте се успешно! :D");
                return;
            }
        }
    }
    public class Login
    {
        public static void LoginMethod(ref string check_name, ref int passcode)
        {
            Console.Write("Въведете своето потребителско име: ");
            check_name = Console.ReadLine();
            if (File.Exists($"e:/data_base/bank_accounts/{check_name}.txt"))
            {
                Console.Write("Въведете своята парола: ");
                passcode = int.Parse(Console.ReadLine());
                string[] info = File.ReadAllLines($"e:/data_base/bank_accounts/{check_name}.txt");
                int buffer = int.Parse(info[1]);
                if (buffer == passcode)
                {
                    Console.WriteLine("Паролата е правилна. Влязохте успешно! :D");
                    Console.Write("Какво ще правите? (Теглене/Внасяне/Преглед): ");
                    string action = Console.ReadLine();

                    if (action.ToLower() == "теглене")
                    {
                        Banking_Actions.Withdraw(ref check_name);
                    }
                    else if (action.ToLower() == "внасяне")
                    {
                        Banking_Actions.Importing(ref check_name);
                    }
                    else if (action.ToLower() == "преглед")
                    {
                        Banking_Actions.View(ref check_name);
                    }
                    else
                    {
                        Console.WriteLine("Невалидна операция!");
                    }
                }
                else
                {
                    Console.WriteLine("Паролата е неправилна!");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Невалидно потребителско име!");
                passcode = -1;
            }
        }
    }
    public class Banking_Actions
    {
        public static void Withdraw(ref string check_name)
        {
            Console.Write("Въведете сума, която искате да изтеглите: ");
            int num;
            if (!int.TryParse(Console.ReadLine(), out num))
            {
                Console.WriteLine("Моля, въведете валидна сума.");
                return;
            }
            string[] info = File.ReadAllLines($"e:/data_base/bank_accounts/{check_name}.txt");
            int sum = int.Parse(info[2]);
            if (sum <= 0)
            {
                Console.WriteLine("Банковата сметка е празна.");
                return;
            }
            else if (num > sum)
            {
                Console.WriteLine("Няма толкова налични пари в сметката.");
                return;
            }
            else if (num <= 0)
            {
                Console.WriteLine("Невалидна сума за теглене.");
                return;
            }
            else
            {
                sum -= num;
                info[2] = sum.ToString();
                File.WriteAllLines($"e:/data_base/bank_accounts/{check_name}.txt", info);
                Console.WriteLine($"Изтеглихте {num} лв. Балансът ви е {sum} лв.");
                return;
            }
        }
        public static void Importing(ref string check_name)
        {
            Console.Write("Въведете сума, която искате да внесете: ");
            int num;
            if (!int.TryParse(Console.ReadLine(), out num))
            {
                Console.WriteLine("Моля, въведете валидна сума.");
                return;
            }
            string[] info = File.ReadAllLines($"e:/data_base/bank_accounts/{check_name}.txt");
            int sum = int.Parse(info[2]);
            if (num <= 0)
            {
                Console.WriteLine("Невалидна сума за внасяне.");
                return;
            }
            else
            {
                sum += num;
                info[2] = sum.ToString();
                File.WriteAllLines($"e:/data_base/bank_accounts/{check_name}.txt", info);
                Console.WriteLine($"Внесохте {num} лв. Балансът ви е {sum} лв.");
                return;
            }
        }
        public static void View(ref string check_name)
        {
            string[] info = File.ReadAllLines($"e:/data_base/bank_accounts/{check_name}.txt");
            Console.WriteLine($"Балансът ви по сметката е {info[2]} лв.");
            return;
        }
    }
}
