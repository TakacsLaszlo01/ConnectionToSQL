namespace ConnectionToSQL;
using MySql.Data.MySqlClient;
using System.Text;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        MySqlConnection connection = CreateConnection("localhost", "shopping", "root");
        try
        {
            connection.Open();
            MySqlDataReader productsReader = CreateReader(connection,
                "SELECT id, name, price FROM products ORDER BY price DESC");

            while (productsReader.Read())
            {
                string id = productsReader.GetString(0);
                string name = productsReader.GetString(1);
                double price = productsReader.GetDouble(2);
                Console.WriteLine($"{id}: {name} - €{price}");
            }    
        }
        finally { connection.Close(); }
    }
    private static MySqlConnection CreateConnection(string address,
                string database, string user, string password = "")
    {
        string connectionString = $"Data Source={address};Initial Catalog={database};User ID={user};Password={password};SslMode=none";
        return new MySqlConnection(connectionString);
    }
    private static MySqlDataReader CreateReader(MySqlConnection connection, string query)
    {
        MySqlCommand command = connection.CreateCommand();
        command.CommandText = query;
        return command.ExecuteReader();
    }
}
public static class MySqlExtends
{
    public static MySqlCommand CreateCommand(this MySqlConnection connection, string commandText)
    {
        MySqlCommand command = connection.CreateCommand();
        command.CommandText = commandText;
        return command;
    }
}