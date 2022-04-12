using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using Microsoft.Win32;

namespace Dokumentooborot.Windows
{
    /// <summary>
    /// Логика взаимодействия для Window8.xaml
    /// </summary>
    public partial class wBackUp : Window
    {
        public wBackUp()
        {
            InitializeComponent();
        }

        private static void CreateBackup(string connectionString, string databaseName, string backupFilePath)
        {
            var backupCommand = "BACKUP DATABASE @BD TO DISK = @backupFilePath";
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(backupCommand, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@BD", databaseName);
                    cmd.Parameters.AddWithValue("@backupFilePath", backupFilePath);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["BackupBDEntities"];
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                DefaultExt = "bak",
                InitialDirectory = "C:\\Windows\\temp\\"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                var backUpPath = saveFileDialog.FileName;
                txtWay_backup.Text = backUpPath;

                CreateBackup(connectionString.ConnectionString, "BD", backUpPath);
                MessageBox.Show("успешно");
            }
            else
            {
                MessageBox.Show("отменено");
            }
        }

        private static void RestoreBackup(string connectionString, string databaseName, string backupFilePath)
        {
            try
            {
                MainWindow.db.Database.Connection.Close();
                wAdmin.db.Database.Connection.Close();
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var sql = @"
                                declare @database varchar(max) = '[' + @databaseName + ']'
                                EXEC('ALTER DATABASE ' + @database + ' SET SINGLE_USER WITH ROLLBACK IMMEDIATE')";
                    using (var command = new SqlCommand(sql, conn))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@databaseName", databaseName);

                        command.ExecuteNonQuery();
                    }


                    var restoreCommand = "RESTORE DATABASE @BD FROM DISK = @backupFilePath  WITH REPLACE";
                    using (var cmd = new SqlCommand(restoreCommand, conn))
                    {
                        cmd.Parameters.AddWithValue("@BD", databaseName);
                        cmd.Parameters.AddWithValue("@backupFilePath", backupFilePath);
                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // set database to multi user
                var sql = @"
                                declare @database varchar(max) = '[' + @databaseName + ']'
                                EXEC('ALTER DATABASE ' + @database + ' SET MULTI_USER')";
                using (var command = new SqlCommand(sql, conn))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@databaseName", databaseName);

                    command.ExecuteNonQuery();
                }
                conn.Close();
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["BackupBDEntities"];
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                DefaultExt = "bak",
                InitialDirectory = "C:\\Windows\\temp\\"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var backUpPath = openFileDialog.FileName;

                txtWay_restore.Text = backUpPath;

                RestoreBackup(connectionString.ConnectionString, "BD", backUpPath);
                MessageBox.Show("успешно");
            }
            else
            {
                MessageBox.Show("отменено");
            }
        }
        /*
        public void RestoreDatabase(string databaseName, string localDatabasePath = null)
        {
            // use the default sql server base path from appsettings.json if localDatabasePath is null
            if (localDatabasePath == null)
            {
                localDatabasePath = Path.Combine(options.Value.SqlServerBasePath, "Backup", $"{databaseName}.bak");
            }
            // otherwise check if it ends with .bak
            else if (!localDatabasePath.EndsWith(".bak"))
            {
                throw new ArgumentException("localDatabasePath must end with .bak.");
            }

            // get file list data
            var fileList = GetDatabaseFileList(localDatabasePath);

            RestoreDatabase(localDatabasePath, fileList.DataName, fileList.LogName);

            DatabaseFileList GetDatabaseFileList(string localDatabasePath)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    var sql = @"RESTORE FILELISTONLY FROM DISK = @localDatabasePath";
                    connection.Open();

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@localDatabasePath", localDatabasePath);

                        using (var reader = command.ExecuteReader())
                        {
                            var result = new DatabaseFileList();
                            while (reader.Read())
                            {
                                var type = reader["Type"].ToString();
                                switch (type)
                                {
                                    case "D":
                                        result.DataName = reader["LogicalName"].ToString();
                                        break;
                                    case "L":
                                        result.LogName = reader["LogicalName"].ToString();
                                        break;
                                }
                            }

                            return result;
                        }
                    }
                }
            }

            void RestoreDatabase(string localDatabasePath, string fileListDataName, string fileListLogName)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        // set database to single user
                        var sql = @"
                                declare @database varchar(max) = '[' + @databaseName + ']'
                                EXEC('ALTER DATABASE ' + @database + ' SET SINGLE_USER WITH ROLLBACK IMMEDIATE')";
                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandType = CommandType.Text;
                            command.Parameters.AddWithValue("@databaseName", databaseName);

                            command.ExecuteNonQuery();
                        }

                        // execute the database restore
                        var dataPath = Path.Combine(options.Value.SqlServerBasePath, "DATA");
                        var fileListDataPath = Path.Combine(dataPath, $"{fileListDataName}.mdf");
                        var fileListLogPath = Path.Combine(dataPath, $"{fileListLogName}.ldf");

                        sql = @"
                                        RESTORE DATABASE @databaseName 
                                        FROM DISK = @localDatabasePath 
                                        WITH REPLACE,
                                        MOVE @fileListDataName to @fileListDataPath,
                                        MOVE @fileListLogName to @fileListLogPath";

                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandTimeout = 7200;
                            command.CommandType = CommandType.Text;
                            command.Parameters.AddWithValue("@databaseName", fileListDataName);
                            command.Parameters.AddWithValue("@localDatabasePath", localDatabasePath);
                            command.Parameters.AddWithValue("@fileListDataName", fileListDataName);
                            command.Parameters.AddWithValue("@fileListDataPath", fileListDataPath);
                            command.Parameters.AddWithValue("@fileListLogName", fileListLogName);
                            command.Parameters.AddWithValue("@fileListLogPath", fileListLogPath);

                            command.ExecuteNonQuery();
                        }

                        // set database to multi user
                        sql = @"
                                declare @database varchar(max) = '[' + @databaseName + ']'
                                EXEC('ALTER DATABASE ' + @database + ' SET MULTI_USER')";
                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandType = CommandType.Text;
                            command.Parameters.AddWithValue("@databaseName", databaseName);

                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }*/
    }
}
