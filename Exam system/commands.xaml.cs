using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Exam_system
{
    public partial class commands : Window
    {
        private string connectionString = "Data Source=(local);Initial Catalog=Exam_system;Integrated Security=True;";

        public commands()
        {
            InitializeComponent();
        }

       private void btnGetInstructors_Click(object sender, RoutedEventArgs e)
{
    try
    {
        if (!int.TryParse(txtParameter.Text, out int intakeId))
        {
            MessageBox.Show("Please enter a valid integer parameter.", "Error");
            return;
        }

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("ShowInstructorAllIntake", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", intakeId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    listBoxCommands.Items.Clear();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                string instructorName = reader["Ins_Name"].ToString();
                                string courseName = reader["Crs_Name"].ToString();
                                string year = reader["Year"].ToString();

                                // Create an object to hold the data for the list box
                                listBoxCommands.Items.Add($"Instructor: {instructorName}, Course: {courseName}, Year: {year}");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error processing row: {ex.Message}", "Error");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No rows returned by the stored procedure", "Information");
                    }
                }
            }
        }

    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error executing stored procedure: {ex.Message}", "Error");
    }
}

private void btnCommand1_Click(object sender, RoutedEventArgs e)
{
    try
    {
        string courseName = txtParameter.Text.Trim(); 

        if (string.IsNullOrEmpty(courseName))
        {
            MessageBox.Show("Please enter a valid course name.", "Error");
            return;
        }

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("ShowQuestionsInCourse", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Crs_name", courseName);

                SqlDataReader reader = command.ExecuteReader();

                listBoxCommands.Items.Clear();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            // Adjust these based on your actual column names in the result set
                            int questionId = Convert.ToInt32(reader["Question_Id"]);
                            string questionType = reader["Q_Type"].ToString();
                            string questionText = reader["Question_Text"].ToString();
                            string correctAnswer = reader["Correct_Answer"].ToString();

                            listBoxCommands.Items.Add($"ID: {questionId}, Type: {questionType}, Text: {questionText}, Answer: {correctAnswer}");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error processing row: {ex.Message}", "Error");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No rows returned by the stored procedure", "Information");
                }
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error executing stored procedure: {ex.Message}", "Error");
    }
}


        private void btnBackToLogin_Click(object sender, RoutedEventArgs e)
        {
            Mainscrean loginWindow = new Mainscrean();
            loginWindow.Show();
            Close();
        }

    }
}
