using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace luckydraw_v2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        string[] ketqua_luop1 = File.ReadAllLines(@"ketqua/ket-qua-luot-1.csv");
        string[] ketqua_luop2 = File.ReadAllLines(@"ketqua/ket-qua-luot-2.csv");
        string[] ketqua_luop3 = File.ReadAllLines(@"ketqua/ket-qua-luot-3.csv");
        string[] ketqua_luop4 = File.ReadAllLines(@"ketqua/ket-qua-luot-4.csv");
        string[] ketqua_luop5 = File.ReadAllLines(@"ketqua/ket-qua-luot-5.csv");
        string[] ketqua_luop6 = File.ReadAllLines(@"ketqua/ket-qua-luot-6.csv");
        string[] ketqua_luop7 = File.ReadAllLines(@"ketqua/ket-qua-luot-7.csv");

        string show_kq = "";

        List<giaithuong> danhsach_giaithuong = new List<giaithuong>();

        private void Form2_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.TopMost = true;
            this.Bounds = Screen.PrimaryScreen.Bounds; // Đảm bảo form lấp đầy màn hình chính

            // Tạo một danh sách giá trị để gán vào ComboBox
            string[] danhSachGiaTri = new string[]
            {
                "Lượt quay 1",
                "Lượt quay 2",
                "Lượt quay 3",
                "Lượt quay 4",
                "Lượt quay 5(Giải ba)",
                "Lượt quay 6(Giải nhì)",
                "Lượt quay 7(Giải nhất)"
            };

            // Gán danh sách giá trị vào ComboBox
            comboBox_show_ketqua.Items.AddRange(danhSachGiaTri);

            can_giua_label_trong_form(label_chucmung.Width, label_chucmung, 287);

            timer_nhapnhay.Start();



            // Tạo đối tượng FileInfo
            var file1 = new System.IO.FileInfo(@"data/giaithuong.xlsx");

            // Tạo đối tượng ExcelPackage
            using (var package = new ExcelPackage(file1))
            {
                // Lấy sheet đầu tiên trong file
                var worksheet = package.Workbook.Worksheets[1];

                // Lấy số dòng và cột của sheet
                int rows = worksheet.Dimension.End.Row;
                int cols = worksheet.Dimension.End.Column;

                // Duyệt từng ô trong sheet
                for (int r = 2; r <= rows; r++)
                {
                    danhsach_giaithuong.Add(new giaithuong()
                    {
                        luotquay = worksheet.Cells[r, 1].Value.ToString(),
                        quatang = worksheet.Cells[r, 2].Value.ToString(),
                        soluong = worksheet.Cells[r, 3].Value.ToString(),
                        thanhtien = worksheet.Cells[r, 4].Value.ToString()
                    });
                }

            }

            comboBox_show_ketqua.SelectedIndex = 0;

        }

        private void can_giua_label_trong_form(int labelWidth, System.Windows.Forms.Label label_, int ylabel)
        {
            int formWidth = this.Width;

            int xlabel = (formWidth - labelWidth) / 2;

            label_.Location = new Point(xlabel, ylabel);
        }

        private void comboBox_show_ketqua_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index_select = comboBox_show_ketqua.SelectedIndex;

            label_show_ketqua_form2.Text = File.ReadAllText(string.Format(@"ketqua/ket-qua-luot-{0}.csv", (index_select + 1)));
            can_giua_label_trong_form(label_show_ketqua_form2.Width, label_show_ketqua_form2, 370);

            string quatang = "";
            foreach (giaithuong gt in danhsach_giaithuong)
            {
                if (Int32.Parse(gt.luotquay) == (index_select + 1))
                {
                    string quatang_ = gt.soluong + " " + gt.quatang + " trị giá " + string.Format("{0:N0} đồng", decimal.Parse(gt.thanhtien));
                    quatang = quatang + quatang_ + "\n";
                }
            }

            label_chucmung.Text = comboBox_show_ketqua.SelectedItem.ToString();
            can_giua_label_trong_form(label_chucmung.Size.Width, label_chucmung, 314);

            label_quatang_form2.Text = quatang;
            can_giua_label_trong_form(label_quatang_form2.Size.Width, label_quatang_form2, 562);
        }

        int nn = 0;
        private void timer_nhapnhay_Tick(object sender, EventArgs e)
        {
            if (nn % 2 == 0)
            {
                label_chucmung.ForeColor = Color.White;
                label_show_ketqua_form2.ForeColor = Color.White;
            }
            else
            {
                label_chucmung.ForeColor = Color.Navy;
                label_show_ketqua_form2.ForeColor = Color.Navy;
            }
            nn++;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
