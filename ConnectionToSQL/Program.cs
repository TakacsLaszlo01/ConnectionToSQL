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

        //create command with query
        //execute nonquery
        
        try
        {
            connection.Open();
            MySqlCommand command = connection.CreateCommand(
                "INSERT INTO products (id, name, price) VALUES (@id, @name, @price)" );

            string id = Input("Kérem adjon meg egy ID-t: ");
            command.Parameters.AddWithValue("@id", id);

            string name = Input("Kérem adja meg a termék nevét: ");
            command.Parameters.AddWithValue("@name", name);

            double price;
            while (!double.TryParse(Input("Kérem adja meg a termék árát: €"), out price))
                Console.WriteLine("Számot adjon meg értéknek!");
            command.Parameters.AddWithValue("@price", price);

            command.ExecuteNonQuery();
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
        MySqlCommand command = connection.CreateCommand(query);
        return command.ExecuteReader();
    }
    private int ExecuteNonQuery()
    {

        return 1;
    }
    private static string Input(string text)
    {
        Console.Write(text);
        return Console.ReadLine();
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