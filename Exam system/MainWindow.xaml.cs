using System.Data.SqlClient;
using System.Windows;

namespace Exam_system
{
    public partial class Mainscrean : Window
    {
        private string connectionString = "Data Source=(local);Initial Catalog=Exam_system;Integrated Security=True;";

        public Mainscrean(){
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Check credentials against the database
            bool authenticationSuccessful = AuthenticateUser(username, password);

            if (authenticationSuccessful)
            {
                // Open the commands window
                commands commandsWindow = new commands();
                commandsWindow.Show();

                // Close the current login window
                Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password", "Error");
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Use a parameterized query to avoid SQL injection
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        int count = (int)command.ExecuteScalar();

                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                    return false;
                }
            }
        }
    }
}
