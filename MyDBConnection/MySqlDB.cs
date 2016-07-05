using MySql.Data.MySqlClient;
using System;

namespace MyDBConnection
{
    public class MySqlDB
    {
        private MySqlConnection _conn;
        private string _server;
        private string _database;
        private string _uid;
        private string _password;

        public MySqlDB()
        {
            Initialize();
        }

        private void Initialize()
        {
            _server = "localhost";
            _database = "dbname";
            _uid = "username";
            _password = "password";
            string connectionString;
            connectionString = "SERVER=" + _server + ";" + "DATABASE=" +
            _database + ";" + "UID=" + _uid + ";" + "PASSWORD=" + _password + ";";

            _conn = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                _conn.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to the database server. Contact the database administrator.");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username or password, please try again.");
                        break;
                }

                return false;
            }
        }

        public bool Select(string selection)
        {
            string querystring = "SELECT " + selection + " FROM Customers";
            try
            {
                MySqlCommand cmd = new MySqlCommand(querystring, _conn);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    Console.WriteLine(dataReader.GetString("row1"));
                }

                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error:" + ex.Message);
                Console.WriteLine("Please check input, or contact the database administrator.");
                return false;
            }
        }
    }
}