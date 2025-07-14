using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> _availableJsonFiles = new();
        private string _selectedJsonPath;

        public Form1()
        {
            InitializeComponent();

            // JSON dosyalar�n� bul ve ComboBox'a ekle
            LoadAppSettingsFiles();

            // Event'leri ba�la
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            button1.Click += button1_Click;
        }

        private void LoadAppSettingsFiles()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;

            // "Business" klas�r�ne ula�ana kadar yukar� ��k
            while (dir != null && !Directory.Exists(Path.Combine(dir, "Business")))
            {
                dir = Directory.GetParent(dir)?.FullName;
                if (dir == null)
                {
                    MessageBox.Show("Business klas�r� bulunamad�.");
                    return;
                }
            }

            var businessPath = Path.Combine(dir, "Business");

            // Dinamik olarak t�m appsettings*.json dosyalar�n� al�yoruz
            var files = Directory.GetFiles(businessPath, "appsettings*.json");

            comboBox1.Items.Clear();
            _availableJsonFiles.Clear();

            foreach (var filePath in files)
            {
                string fileName = Path.GetFileName(filePath);
                _availableJsonFiles[fileName] = filePath;
                comboBox1.Items.Add(fileName);
            }

            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
            else
                MessageBox.Show("Hi�bir appsettings dosyas� bulunamad�.");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFile = comboBox1.SelectedItem.ToString();

            if (!_availableJsonFiles.ContainsKey(selectedFile))
                return;

            _selectedJsonPath = _availableJsonFiles[selectedFile];

            // ConnectionString�leri oku
            LoadConnectionStrings();
        }

        private void LoadConnectionStrings()
        {
            if (!File.Exists(_selectedJsonPath)) return;

            string json = File.ReadAllText(_selectedJsonPath);
            JObject jObject = JObject.Parse(json);
            var connStrings = jObject["ConnectionStrings"]?.ToObject<Dictionary<string, string>>();

            if (connStrings != null && connStrings.Count > 0)
            {
                textBox1.Text = connStrings.First().Value;
            }
            else
            {
                textBox1.Text = string.Empty;
                MessageBox.Show("ConnectionStrings b�l�m� bulunamad� veya bo�.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_selectedJsonPath)) return;

            string json = File.ReadAllText(_selectedJsonPath);
            JObject jObject = JObject.Parse(json);
            var connStrings = jObject["ConnectionStrings"]?.ToObject<Dictionary<string, string>>();

            if (connStrings == null || connStrings.Count == 0)
            {
                MessageBox.Show("ConnectionStrings b�l�m� yok.");
                return;
            }

            string firstKey = connStrings.Keys.First();
            string newValue = textBox1.Text.Trim();

            if (!string.IsNullOrWhiteSpace(newValue))
            {
                jObject["ConnectionStrings"][firstKey] = newValue;
                File.WriteAllText(_selectedJsonPath, jObject.ToString());
                MessageBox.Show($"{firstKey} connection string g�ncellendi.");
            }
            else
            {
                MessageBox.Show("Connection string bo� olamaz. ");
            }
        }
    }
}
