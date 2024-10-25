using System.Data.SqlClient;

internal static class Program
{
    static async Task Main(string[] args)
    {
        string connectionString = "Server=MOYA-PRELEST;Database=lab6;Trusted_Connection=True";
        SqlCommand command = new SqlCommand();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();
            command.Connection = connection;
            bool flag = true;
            string? name, date, term;
            
            while (flag)
            {
                Console.WriteLine("Выберите, что хотите сделать: ");
                Console.WriteLine("[1] - добавить");
                Console.WriteLine("[2] - вывести");
                Console.WriteLine("[3] - изменить");
                Console.WriteLine("[4] - удалить");
                Console.WriteLine("[5] - выйти");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Введите фио ");
                        name = Console.ReadLine();
                        Console.WriteLine("Введите дату рождения ");
                        date = Console.ReadLine();
                        Console.WriteLine("Введите срок действия ");
                        term = Console.ReadLine();
                        command.CommandText = $"insert into Insurance(fio, birthDate, term) values('{name}','{date}','{term}')";
                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine("Запись успешно добавлена");
                        break;
                    case 2:
                        command.CommandText = "select * from Insurance";
                        using(SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("Список записей пуст");
                            }
                            else
                            {
                                Console.WriteLine("Список объектов:");

                                while (await reader.ReadAsync()) // построчно считываем данные
                                {
                                    object ID = reader.GetValue(0);
                                    object fio = reader.GetValue(1);
                                    object birthDate = reader.GetValue(2);
                                    object t = reader.GetValue(3);

                                    Console.WriteLine($"{ID} \t{fio} \t{birthDate} \t{t}");
                                }
                            }
                        }
                        break;
                    case 3:
                        Console.WriteLine("Введите id записи, которую хотите изменить:  ");
                        int id = Convert.ToInt32(Console.ReadLine());
                        bool isExist=false;
                        command.CommandText = $"select * from Insurance where Id = {id}";
                        using(SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("Записи с таким id нет");

                            }
                            else
                            {
                                isExist = true;
                            }
                        }

                        if (isExist)
                        {
                            Console.WriteLine("Введите новое фио");
                            name = Console.ReadLine();
                            command.CommandText = $"update Insurance set fio='{name}' where Id={id}";

                            await command.ExecuteNonQueryAsync();
                            Console.WriteLine("Запись успешно изменена");
                        }
                        

                        break;
                    case 4:
                        Console.WriteLine("Введите id записи, которую хотите удалить:  ");
                        int id1 = Convert.ToInt32(Console.ReadLine());
                        bool isExist1 = false;
                        command.CommandText = $"select * from Insurance where Id = {id1}";
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("Записи с таким id нет");

                            }
                            else
                            {
                                isExist1 = true;
                                
                            }
                        }
                        if (isExist1)
                        {
                            command.CommandText = $"delete from Insurance where Id = {id1}";
                            await command.ExecuteNonQueryAsync();
                            Console.WriteLine("Запись успешно удалена");
                        }
                        break;
                    case 5:
                        flag = false;
                        break;
                    default:
                        Console.WriteLine("Введено неверное значение");
                        break;
                }
            }
        }
    }
}