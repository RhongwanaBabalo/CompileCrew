using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace CompileCrew1
{
    public partial class Register : Page
    {
        private string connectionString =
            System.Configuration.ConfigurationManager.ConnectionStrings["CompileCrew2"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            // Validate empty fields
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                lblMessage.CssClass = "block text-center text-red-600 mt-4";
                lblMessage.Text = "⚠ Please fill in all required fields.";
                return;
            }

            // Validate passwords
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                lblMessage.CssClass = "block text-center text-red-600 mt-4";
                lblMessage.Text = "❌ Passwords do not match.";
                return;
            }

            // Validate terms
            if (!chkAgreeTerms.Checked)
            {
                lblMessage.CssClass = "block text-center text-red-600 mt-4";
                lblMessage.Text = "❌ You must agree to the terms.";
                return;
            }

            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string email = txtEmail.Text.Trim().ToLower();
            string phone = txtPhone.Text.Trim();
            string role = ddlRole.SelectedValue;
            string passwordHash = HashPassword(txtPassword.Text);

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Check if email already exists
                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Email=@Email", con);
                    checkCmd.Parameters.AddWithValue("@Email", email);
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        lblMessage.CssClass = "block text-center text-red-600 mt-4";
                        lblMessage.Text = "❌ Email already registered.";
                        return;
                    }

                    // Insert new user
                    SqlCommand cmd = new SqlCommand(
                        @"INSERT INTO Users (FirstName, LastName, Email, Phone, Role, PasswordHash, CreatedAt)
                          VALUES (@FirstName, @LastName, @Email, @Phone, @Role, @PasswordHash, @CreatedAt)", con);

                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Role", role);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        lblMessage.CssClass = "block text-center text-green-600 mt-4 font-semibold";
                        lblMessage.Text = "✅ Registration successful! Redirecting to login...";

                        // Redirect after 2 seconds
                        ClientScript.RegisterStartupScript(this.GetType(), "redirect",
                            "setTimeout(function(){ window.location='Login.aspx'; }, 2000);", true);
                    }
                    else
                    {
                        lblMessage.CssClass = "block text-center text-red-600 mt-4";
                        lblMessage.Text = "❌ Registration failed. Please try again.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.CssClass = "block text-center text-red-600 mt-4";
                lblMessage.Text = "❌ Error: " + ex.Message;
            }
        }

        // Hash password with SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
