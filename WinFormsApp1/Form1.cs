using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private string appsettingsPath;
        private string aspNetCoreProjectName = "Business"; // ASP.NET Core proje ad�n�z

        public Form1()
        {
            InitializeComponent();
            // �al��ma dizininden ��z�m dizinine ula�
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // ��z�m dizinine ula�mak i�in 3 seviye yukar� ��k (bin\Debug\net8.0'dan)
            string solutionDirectory = Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName;
            // appsettings.json yolunu olu�tur
            appsettingsPath = Path.Combine(solutionDirectory, aspNetCoreProjectName, "appsettings.json");
            // Dosya yolunu do�rulamak i�in
            MessageBox.Show($"Appsettings Path: {appsettingsPath}");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadConnectionString();
        }

        private void LoadConnectionString()
        {
            try
            {
                if (!File.Exists(appsettingsPath))
                {
                    MessageBox.Show("appsettings.json dosyas� bulunamad�!",
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtConnectionString.Text = string.Empty;
                    return;
                }

                string jsonContent = File.ReadAllText(appsettingsPath);
                var jsonDocument = JsonDocument.Parse(jsonContent);
                var connectionString = jsonDocument.RootElement
                    .GetProperty("ConnectionStrings")
                    .GetProperty("DefaultConnection")
                    .GetString();

                txtConnectionString.Text = connectionString;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtConnectionString.Text = string.Empty;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string newConnectionString = txtConnectionString.Text;

                if (!IsValidConnectionString(newConnectionString))
                {
                    MessageBox.Show("Ge�ersiz ba�lant� dizesi format�! L�tfen do�ru formatta girin.",
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string jsonContent = File.ReadAllText(appsettingsPath);
                var jsonDocument = JsonDocument.Parse(jsonContent);
                var root = JsonSerializer.Deserialize<JsonElement>(jsonContent);

                using (var stream = new MemoryStream())
                {
                    using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true }))
                    {
                        writer.WriteStartObject();
                        foreach (var property in root.EnumerateObject())
                        {
                            if (property.Name == "ConnectionStrings")
                            {
                                writer.WritePropertyName("ConnectionStrings");
                                writer.WriteStartObject();
                                writer.WriteString("DefaultConnection", newConnectionString);
                                writer.WriteEndObject();
                            }
                            else
                            {
                                property.WriteTo(writer);
                            }
                        }
                        writer.WriteEndObject();
                    }

                    File.WriteAllBytes(appsettingsPath, stream.ToArray());
                }

                MessageBox.Show("Ba�lant� dizesi ba�ar�yla g�ncellendi!",
                    "Ba�ar�l�", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsValidConnectionString(string connectionString)
        {
            return connectionString.Contains("Host=") &&
                   connectionString.Contains("Port=") &&
                   connectionString.Contains("Database=") &&
                   connectionString.Contains("Username=") &&
                   connectionString.Contains("Password=");
        }
    }
}