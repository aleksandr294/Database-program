using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
namespace DataBase
{
    class Program
    {
        public List<Person> people = new List<Person>();
        static void Main(string[] args)
        {   
            Program prog = new Program();
            bool per = true;
            string choi;
            string constr = "server=localhost; user=root; database=magazin; password=root;";
            MySql.Data.MySqlClient.MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand com=new MySqlCommand();
            conn.Open();
            while(per)
            {
                Console.WriteLine("------------------------------");
                Console.WriteLine('\t' + "Меню");
                Console.WriteLine("Ввод" + '\n' + "Вывод" + '\n' + "Выход" + '\n');
                Console.WriteLine("------------------------------");
                Console.WriteLine("Введите команду");
                choi = Console.ReadLine();
                if (choi == "Ввод")
                    prog.Input(prog.people, conn, com);
                else if (choi == "Вывод")
                    prog.Show(conn, com);
                else if (choi == "Выход")
                    per = false;
                else
                    Console.WriteLine("Вы ввели не ту команду");
            }
            conn.Close();
            Console.ReadLine();
        }
        public void Input (List<Person> peop, MySqlConnection conn, MySqlCommand command)
        {
            int n;
            Console.WriteLine("Введите количество записей");
            n = Convert.ToInt32(Console.ReadLine());
            for(int i=0; i<n; i++)
            {
                Person pers = new Person();
                Console.WriteLine("Введите id");
                pers.id = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Введите имя человека");
                pers.fname = Console.ReadLine();
                Console.WriteLine("Введите фамилию человека");
                pers.lname = Console.ReadLine();
                Console.WriteLine("Введите номер телефона человека");
                pers.numbers = Console.ReadLine();
                Console.WriteLine("Введите возраст человека");
                pers.age = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Введите дату регистрации");
                pers.data_registr = Console.ReadLine();
                peop.Add(pers);
            }
            string sql;
            for (int i=0; i<peop.Count; i++)
            {
                sql= "INSERT INTO people (id, fname, lname, numbers, age, data_registr) VALUES ('" + peop[i].id + "','" + peop[i].fname + "','" + peop[i].lname + "', '" + peop[i].numbers + "', '" + peop[i].age + "', '" + peop[i].data_registr + "' )";
                command = new MySqlCommand(sql, conn);
                command.ExecuteNonQuery();
            }
        }
        public void Show (MySqlConnection conn, MySqlCommand command)
        {
            Console.WriteLine("Повний вигляд створеної бази даних");
            MySqlDataReader reader;
            string sql = "SELECT * FROM people";
            command = new MySqlCommand(sql, conn);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[0].ToString()+' '+ reader[1].ToString()+ ' ' + reader[2].ToString()+ ' ' + reader[3].ToString()+ ' ' + reader[4].ToString()+ ' ' + reader[5].ToString());
            }
            reader.Close();

            Console.WriteLine("Результат розрахунків середнього віку зареєстрований користувачів.");
            sql = "SELECT avg(age) FROM people";
            command = new MySqlCommand(sql, conn);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[0].ToString());
            }
            reader.Close();

            Console.WriteLine("Виведення ідентифікаторів користувачів та їх контактні дані і дату реєстрації");
            sql = "SELECT id, fname, lname, data_registr FROM people";
            command = new MySqlCommand(sql, conn);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[0].ToString()+' '+reader[1].ToString() + ' ' + reader[2].ToString() + ' ' + reader[3].ToString());
            }
            reader.Close();

            Console.WriteLine("Контактні дані користувачі зареєстрованих у зимовий період");
            sql = "SELECT * FROM people WHERE data_registr LIKE '%-12-%' OR MONTH(data_registr)<3";
            command = new MySqlCommand(sql, conn);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[0].ToString() + ' ' + reader[1].ToString() + ' ' + reader[2].ToString() + ' ' + reader[3].ToString() + ' ' + reader[4].ToString() + ' ' + reader[5].ToString());
            }
            reader.Close();

            Console.WriteLine("За відомими першими літерами прізвища – зв’язаних контактних даних");
            sql = "SELECT * FROM people WHERE lname LIKE 'P%' ";
            command = new MySqlCommand(sql, conn);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[0].ToString() + ' ' + reader[1].ToString() + ' ' + reader[2].ToString() + ' ' + reader[3].ToString() + ' ' + reader[4].ToString() + ' ' + reader[5].ToString());
            }
            reader.Close();

            Console.WriteLine("Людей зареєстрованих (що народились) заданого числа будь-якого місяця, будь-якого року.");
            sql = "SELECT * FROM people WHERE data_registr LIKE '%-%-24'";
            command = new MySqlCommand(sql, conn);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[0].ToString() + ' ' + reader[1].ToString() + ' ' + reader[2].ToString() + ' ' + reader[3].ToString() + ' ' + reader[4].ToString() + ' ' + reader[5].ToString());
            }
            reader.Close();

            Console.WriteLine("Анкети з пустими полями");
            sql = "SELECT * FROM people WHERE  lname='' OR fname='' OR numbers='' ";
            command = new MySqlCommand(sql, conn);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[0].ToString() + ' ' + reader[1].ToString() + ' ' + reader[2].ToString() + ' ' + reader[3].ToString() + ' ' + reader[4].ToString() + ' ' + reader[5].ToString());
            }
            reader.Close();
        }

       
    }

    class Person
    {
        public int id;
        public string fname="";
        public string lname="";
        public string numbers="";
        public int age;
        public string data_registr;
    }
}
