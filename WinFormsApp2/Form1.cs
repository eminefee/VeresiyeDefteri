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

            // JSON dosyalarýný bul ve ComboBox'a ekle
            LoadAppSettingsFiles();

            // Event'leri baðla
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            button1.Click += button1_Click;
        }

        private void LoadAppSettingsFiles()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;

            // "Business" klasörüne ulaþana kadar yukarý çýk
            while (dir != null && !Directory.Exists(Path.Combine(dir, "Business")))
            {
                dir = Directory.GetParent(dir)?.FullName;
                if (dir == null)
                {
                    MessageBox.Show("Business klasörü bulunamadý.");
                    return;
                }
            }

            var businessPath = Path.Combine(dir, "Business");

            // Dinamik olarak tüm appsettings*.json dosyalarýný alýyoruz
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
                MessageBox.Show("Hiçbir appsettings dosyasý bulunamadý.");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFile = comboBox1.SelectedItem.ToString();

            if (!_availableJsonFiles.ContainsKey(selectedFile))
                return;

            _selectedJsonPath = _availableJsonFiles[selectedFile];

            // ConnectionString’leri oku
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
                MessageBox.Show("ConnectionStrings bölümü bulunamadý veya boþ.");
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
                MessageBox.Show("ConnectionStrings bölümü yok.");
                return;
            }

            string firstKey = connStrings.Keys.First();
            string newValue = textBox1.Text.Trim();

            if (!string.IsNullOrWhiteSpace(newValue))
            {
                jObject["ConnectionStrings"][firstKey] = newValue;
                File.WriteAllText(_selectedJsonPath, jObject.ToString());
                MessageBox.Show($"{firstKey} connection string güncellendi.");
            }
            else
            {
                MessageBox.Show("Connection string boþ olamaz. ");
            }
        }
    }
}
