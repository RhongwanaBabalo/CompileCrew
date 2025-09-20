using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace CompileCrew1
{
    public partial class Login : System.Web.UI.Page
    {
        private string connectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e) { }

        // Login
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = ddlRole.SelectedValue;

            // ✅ Validate fields
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                lblMessage.Text = "❌ Please fill in all fields.";
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, FirstName, LastName, PasswordHash, Role FROM Users WHERE Email=@Email AND Role=@Role";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Role", role);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    string storedHash = dr["PasswordHash"].ToString();

                    if (VerifyPassword(password, storedHash))
                    {
                        // Store session details
                        Session["Id"] = dr["Id"].ToString();
                        Session["UserEmail"] = email;
                        Session["UserRole"] = role;
                        Session["UserName"] = dr["FirstName"].ToString() + " " + dr["LastName"].ToString();

                        Response.Redirect("~/Dashboard.aspx");
                    }
                    else
                    {
                        lblMessage.Text = "❌ Incorrect password.";
                    }
                }
                else
                {
                    lblMessage.Text = "❌ User not found or role mismatch.";
                }
            }
        }

        // Verify password hash
        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] enteredBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));
                string enteredHash = BitConverter.ToString(enteredBytes).Replace("-", "").ToLower();
                return enteredHash == storedHash;
            }
        }

        // Forgot Password
        protected void btnForgot_Click(object sender, EventArgs e)
        {
            string email = txtForgotEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                lblMessage.Text = "❌ Please enter your registered email.";
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT Id FROM Users WHERE Email=@Email";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", email);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    // Generate a temporary password
                    string tempPassword = GenerateTempPassword();
                    string hashedPassword = HashPassword(tempPassword);

                    // Update user password
                    string updateQuery = "UPDATE Users SET PasswordHash=@PasswordHash WHERE Email=@Email";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                    updateCmd.Parameters.AddWithValue("@Email", email);
                    updateCmd.ExecuteNonQuery();

                    lblMessage.CssClass = "block text-center text-green-600 mt-4";
                    lblMessage.Text = $"✅ Temporary password: {tempPassword} (Use this to login and change password)";
                }
                else
                {
                    lblMessage.Text = "❌ Email not registered.";
                }
            }
        }

        // Generate random temporary password
        private string GenerateTempPassword(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder sb = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] buffer = new byte[4];
                for (int i = 0; i < length; i++)
                {
                    rng.GetBytes(buffer);
                    uint num = BitConverter.ToUInt32(buffer, 0);
                    sb.Append(chars[(int)(num % (uint)chars.Length)]);
                }
            }
            return sb.ToString();
        }

        // Hash function
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}
