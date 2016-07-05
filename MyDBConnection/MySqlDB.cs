using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

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

        // Initializes the member variables, and
        // combines them into a string of server credentials.
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

        // Opens a connection to the MySql server, and
        // reports errors to the user.
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

        // Closes the connection to the MySql server,
        // and reports errors to the user.
        private bool CloseConnection()
        {
            try
            {
                _conn.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        // Returns the count of elements in a given column.
        // @param column - Name of the column to be counted.
        public int Count(string column, string tableName)
        {
            string query = String.Format("SELECT Count({0}) FROM {1}", column, tableName);
            int Count = -1;

            // Opens a connection to the server.
            if (this.OpenConnection() == true)
            {
                // Creates a command from the query string, and binds it to the server connection.
                MySqlCommand cmd = new MySqlCommand(query, _conn);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                // Ends the connection to the server.
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }

        // Selects data from a MySql database and
        // writes the data to Console in string format.
        // -----------------------------------------------
        // @param selection - Inputs the name of the column(s) to be read.
        public List<string> Select(string tableName, params string[] selections)
        {
            string query = "SELECT ";

            // Appends each addition selection to the query string.
            for (int i = 0; i < selections.Length; i++)
            {
                query += selections[i] + ", ";
            }
            query = query.Remove(query.Length - 2, 2);
            query += " FROM " + tableName;

            // Opens a connection to server.
            if (this.OpenConnection() == true)
            {
                // Creates a command from the query string, and binds it to the server connection.
                MySqlCommand command = new MySqlCommand(query, _conn);
                // Initializes the DataReader.
                MySqlDataReader dataReader = command.ExecuteReader();

                List<string> dataList = new List<string>();

                // Reads data from the server, and stores it in a list.
                while (dataReader.Read())
                {
                    for (int i = 0; i < selections.Length; i++)
                    {
                        dataList.Add(selections[i]);
                    }
                    dataList.Add(dataReader.GetString("row1"));
                }

                // Closes the DataReader, and ends the server connection.
                dataReader.Close();
                this.CloseConnection();

                return dataList;
            }
            else
            {
                return null;
            }
        }
    }
}