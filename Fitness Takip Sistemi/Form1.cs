using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp1 // <-- BURAYI KENDİ PROJE ADINLA DEĞİŞTİR
{
    // --- 1. SPORCU SINIFI ---
    public class Sporcu
    {
        public int sporcu_id { get; set; }
        public string ad { get; set; }
        public double kilo { get; set; }
        public double boy { get; set; }
        public List<string> kilo_gecmisi = new List<string>();

        // İSTEDİĞİN METOD: Sadece değişkeni değiştirmez, geçmişe not düşer.
        public void ilerleme_kaydet(double yeniKilo, double yeniBoy)
        {
            this.kilo = yeniKilo;
            this.boy = yeniBoy;
            kilo_gecmisi.Add($"{DateTime.Now.ToShortDateString()} - Kilo: {yeniKilo}kg, Boy: {yeniBoy}cm");
        }
    }

    // --- 2. ANTRENMAN SINIFI ---
    public class Antrenman
    {
        public int antrenman_id { get; set; }
        public string tur { get; set; }
        public int sure { get; set; }
    }

    // --- 3. TAKİP SINIFI ---
    public class Takip
    {
        [DisplayName("Tarih")]
        public string tarih { get; set; }

        [DisplayName("Yapılan İşlem")]
        public string aciklama { get; set; }

        [DisplayName("Yakılan Kalori")]
        public int kalori { get; set; }
    }

    public partial class Form1 : Form
    {
        Sporcu sporcum;
        List<Takip> takipLoglari = new List<Takip>();
        List<Antrenman> antrenmanTipleri = new List<Antrenman>();

        // UI Elemanları
        TabControl sekmeler;
        TabPage sekmeSporcu, sekmeAntrenman, sekmeGecmis;
        DataGridView dgvGecmis;
        ListBox lstKiloGecmisi;
        Label lblProfil;
        TextBox txtAd, txtKilo, txtBoy, txtSure, txtKalori;
        ComboBox cmbAntrenmanlar;

        public Form1()
        {
            this.Text = "Fitness Management System - 2300005412 Fırat Diricanlı";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            VerileriIlklendir();
            ArayuzuKur();
        }

        private void VerileriIlklendir()
        {
            // Sporcu Nesnesi Oluşturma
            sporcum = new Sporcu { sporcu_id = 1, ad = "Fırat Diricanlı", kilo = 80, boy = 180 };

            // Antrenman Tipleri (Şablonlar)
            antrenmanTipleri.Add(new Antrenman { antrenman_id = 101, tur = "Kardiyo (Koşu)", sure = 30 });
            antrenmanTipleri.Add(new Antrenman { antrenman_id = 102, tur = "Ağırlık Antrenmanı", sure = 60 });
            antrenmanTipleri.Add(new Antrenman { antrenman_id = 103, tur = "Crossfit", sure = 45 });
            antrenmanTipleri.Add(new Antrenman { antrenman_id = 104, tur = "Yüzme", sure = 40 });
        }

        private void ArayuzuKur()
        {
            sekmeler = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 12, FontStyle.Bold) };

            sekmeSporcu = new TabPage("🏃 Sporcu & İlerleme");
            sekmeAntrenman = new TabPage("🏋️ Antrenman Takip");
            sekmeGecmis = new TabPage("📊 Genel Geçmiş");

            // --- SPORCU SEKMESİ (İlerleme Kaydet Burada) ---
            lblProfil = new Label { Location = new Point(30, 30), AutoSize = true, Font = new Font("Segoe UI", 14), ForeColor = Color.DarkSlateBlue };

            Label l1 = new Label { Text = "Güncel Kilo:", Location = new Point(30, 180), AutoSize = true };
            txtKilo = new TextBox { Location = new Point(150, 178), Width = 100 };

            Label l2 = new Label { Text = "Güncel Boy:", Location = new Point(30, 220), AutoSize = true };
            txtBoy = new TextBox { Location = new Point(150, 218), Width = 100 };

            Button btnIlerleme = new Button
            {
                Text = "İLERLEME KAYDET",
                Location = new Point(30, 270),
                Size = new Size(220, 50),
                BackColor = Color.Indigo,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnIlerleme.Click += (s, e) => {
                if (double.TryParse(txtKilo.Text, out double k) && double.TryParse(txtBoy.Text, out double b))
                {
                    sporcum.ilerleme_kaydet(k, b); // Nesne metodunu çağırıyoruz
                    ProfilGuncelle();
                    MessageBox.Show("İlerleme başarıyla nesneye kaydedildi!");
                }
            };

            lstKiloGecmisi = new ListBox { Location = new Point(300, 60), Size = new Size(350, 260), Font = new Font("Consolas", 10) };
            Label lGecmis = new Label { Text = "Vücut Ölçüleri Değişim Logu:", Location = new Point(300, 30), AutoSize = true };

            sekmeSporcu.Controls.AddRange(new Control[] { lblProfil, l1, txtKilo, l2, txtBoy, btnIlerleme, lstKiloGecmisi, lGecmis });

            // --- ANTRENMAN SEKMESİ ---
            Label l3 = new Label { Text = "Antrenman Seç:", Location = new Point(30, 40), AutoSize = true };
            cmbAntrenmanlar = new ComboBox { Location = new Point(170, 38), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            foreach (var item in antrenmanTipleri) cmbAntrenmanlar.Items.Add(item.tur);

            Label l4 = new Label { Text = "Yakılan Kalori:", Location = new Point(30, 90), AutoSize = true };
            txtKalori = new TextBox { Location = new Point(170, 88), Width = 100 };

            Button btnTakipEkle = new Button
            {
                Text = "GÜNLÜĞE EKLE",
                Location = new Point(30, 140),
                Size = new Size(390, 50),
                BackColor = Color.SeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnTakipEkle.Click += (s, e) => {
                if (cmbAntrenmanlar.SelectedItem != null && int.TryParse(txtKalori.Text, out int cal))
                {
                    takipLoglari.Add(new Takip
                    {
                        tarih = DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                        aciklama = cmbAntrenmanlar.SelectedItem.ToString() + " yapıldı.",
                        kalori = cal
                    });
                    TabloyuGuncelle();
                    MessageBox.Show("Antrenman günlüğe işlendi.");
                }
            };
            sekmeAntrenman.Controls.AddRange(new Control[] { l3, cmbAntrenmanlar, l4, txtKalori, btnTakipEkle });

            // --- GEÇMİŞ SEKMESİ ---
            dgvGecmis = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false
            };
            sekmeGecmis.Controls.Add(dgvGecmis);

            sekmeler.TabPages.AddRange(new TabPage[] { sekmeSporcu, sekmeAntrenman, sekmeGecmis });
            this.Controls.Add(sekmeler);

            ProfilGuncelle();
        }

        private void ProfilGuncelle()
        {
            double vki = sporcum.kilo / ((sporcum.boy / 100) * (sporcum.boy / 100));
            lblProfil.Text = $"PRO SPORCU KARTI\n----------------------\n" +
                             $"ID: {sporcum.sporcu_id}\n" +
                             $"İsim: {sporcum.ad}\n" +
                             $"Kilo: {sporcum.kilo} kg\n" +
                             $"Boy: {sporcum.boy} cm\n" +
                             $"VKI: {vki:N2}";

            lstKiloGecmisi.Items.Clear();
            foreach (var log in sporcum.kilo_gecmisi) lstKiloGecmisi.Items.Add(log);
        }

        private void TabloyuGuncelle()
        {
            dgvGecmis.DataSource = null;
            dgvGecmis.DataSource = takipLoglari.ToList();
        }
    }
}