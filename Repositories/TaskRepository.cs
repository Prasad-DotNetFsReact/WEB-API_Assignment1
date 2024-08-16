using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using Task_api.Models;
using Task = Task_api.Models.Task;


namespace Task_api.Repositories {
    public class TaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Create a new task
        public void AddTask(Task task)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Task (TaskName, Description, Duration, AssignedBy) VALUES (@TaskName, @Description, @Duration, @AssignedBy)", conn);
                cmd.Parameters.AddWithValue("@TaskName", task.TaskName);
                cmd.Parameters.AddWithValue("@Description", task.Description);
                cmd.Parameters.AddWithValue("@Duration", task.Duration);
                cmd.Parameters.AddWithValue("@AssignedBy", task.AssignedBy);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Get a task by ID
        public Task GetTask(int id)
        {
            Task task = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Task WHERE TaskID = @TaskID", conn);
                cmd.Parameters.AddWithValue("@TaskID", id);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    task = new Task
                    {
                        TaskID = (int)reader["TaskID"],
                        TaskName = reader["TaskName"].ToString(),
                        Description = reader["Description"].ToString(),
                        Duration = (int)reader["Duration"],
                        AssignedBy = reader["AssignedBy"].ToString()
                    };
                }
            }

            return task;
        }

        // Get all tasks
        public IEnumerable<Task> GetTasks()
        {
            var tasks = new List<Task>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Task", conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var task = new Task
                    {
                        TaskID = (int)reader["TaskID"],
                        TaskName = reader["TaskName"].ToString(),
                        Description = reader["Description"].ToString(),
                        Duration = (int)reader["Duration"],
                        AssignedBy = reader["AssignedBy"].ToString()
                    };
                    tasks.Add(task);
                }
            }

            return tasks;
        }

        // Update a task
        public void UpdateTask(Task task)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Task SET TaskName = @TaskName, Description = @Description, Duration = @Duration, AssignedBy = @AssignedBy WHERE TaskID = @TaskID", conn);
                cmd.Parameters.AddWithValue("@TaskID", task.TaskID);
                cmd.Parameters.AddWithValue("@TaskName", task.TaskName);
                cmd.Parameters.AddWithValue("@Description", task.Description);
                cmd.Parameters.AddWithValue("@Duration", task.Duration);
                cmd.Parameters.AddWithValue("@AssignedBy", task.AssignedBy);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Delete a task
        public void DeleteTask(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Task WHERE TaskID = @TaskID", conn);
                cmd.Parameters.AddWithValue("@TaskID", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

