using OfficeOpenXml;
using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace luckydraw_v2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        System.Media.SoundPlayer nhacdangquay = new System.Media.SoundPlayer("audio/dangquay.wav");
        System.Media.SoundPlayer nhacnen = new System.Media.SoundPlayer("audio/nhacnen.wav");
        System.Media.SoundPlayer check_music = new System.Media.SoundPlayer("audio/chec.wav");
        System.Media.SoundPlayer tada_music = new System.Media.SoundPlayer("audio/gametada.wav");
        System.Media.SoundPlayer chot_music = new System.Media.SoundPlayer("audio/chot.wav");

        List<nhanvien> danhsach_nhanvien = new List<nhanvien>();

        List<giaithuong> danhsach_giaithuong = new List<giaithuong>();

        List<int> danhsach_luotquay = new List<int>();

        List<string> ketqua_data = new List<string>();

        int y_lable_nguoitrunggiai = 390;

        int so_nguoi_con_lai = 0;
        int tongso_nhanvien = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile("image_sys/b2.png");

            DoubleBuffered = true;

            label_tentrunggiai_nhat.Hide();


            // Đặt căn giữa cho văn bản bên trong label
            //label_tentrunggiai_1.TextAlign = ContentAlignment.MiddleCenter;

            //label_tentrunggiai_1.Visible = false;
            btn_chot.Visible = false;
            btn_xannhan.Hide();
            //btn_showketqua.Hide();
            btn_quaylai.Hide();
            btn_tieptuc.Hide();

            btn_showketqua.Show();

            hide_btn_chot_day_so();
            hide_btn_xoa_va_quay_lai();

            label_showketqua_2mien.Visible = false;

            timer_quaydaoso.Start();

            //nhacnen.Load();
            //nhacnen.PlayLooping();

            DocFileExcel();

            tongso_nhanvien = danhsach_nhanvien.Count();
            cap_nhap_lai_so_nguoi_con_lai();

            for (int i = 1; i <= 7; i++)
            {
                int dem = 0;
                foreach (giaithuong gt in danhsach_giaithuong)
                {
                    if (Int32.Parse(gt.luotquay) == i)
                    {
                        dem++;
                    }
                }
                danhsach_luotquay.Add(dem);
            }

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
            comboBox_luotquay.Items.AddRange(danhSachGiaTri);
            comboBox_luotquay.SelectedIndex = 0;

            //comboBox_luotquay.Hide();

            //thread_quay_so();


        }

        private void DocFileExcel()
        {
            // Tạo đối tượng FileInfo
            var file = new System.IO.FileInfo(@"data/nhanvien.xlsx");

            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Tạo đối tượng ExcelPackage
            using (var package = new ExcelPackage(file))
            {
                // Lấy sheet đầu tiên trong file
                var worksheet = package.Workbook.Worksheets[1];

                // Lấy số dòng và cột của sheet
                int rows = worksheet.Dimension.End.Row;
                int cols = worksheet.Dimension.End.Column;

                // Duyệt từng ô trong shOfficeOpenXml.LicenseException: 'Please set the ExcelPackage.LicenseContext property. See https://epplussoftware.com/developers/licenseexception'eet
                for (int r = 2; r <= rows; r++)
                {
                    danhsach_nhanvien.Add(new nhanvien()
                    {
                        manhanvien = worksheet.Cells[r, 1].Value.ToString(),
                        ten = worksheet.Cells[r, 2].Value.ToString(),
                        vitri = worksheet.Cells[r, 3].Value.ToString(),
                        phongban = worksheet.Cells[r, 4].Value.ToString(),
                        nganhang = worksheet.Cells[r, 5].Value.ToString()
                    });
                }
            }

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
            int a = 9;
        }

        private void cap_nhap_lai_so_nguoi_con_lai()
        {
            label_so_nguoi_con_lai.Text = string.Format("Còn lại: {0}/{1} người", danhsach_nhanvien.Count(), tongso_nhanvien);
        }

        //hàm quay ramdom ra người trúng giải
        private int[] random_so_trung_giai(int so_luong_nguoi)
        {
            Random rd = new Random();

            //tạo mảng số quay ra ngẫu nhiên
            int[] danhsach_so_trung_giai = new int[so_luong_nguoi];
            //gán nó mặc định lại, vì mặc định nó bằng 0
            for (int i = 0; i < so_luong_nguoi; i++)
            {
                danhsach_so_trung_giai[i] = 999999;
            }

            //tổng số nhân viên
            int soluong_nhanvien = danhsach_nhanvien.Count;

            for (int i = 0; i < so_luong_nguoi; i++)
            {
                int random_so;

                //random số làm sao không mảng không có trùng nhau
                do
                {
                    random_so = rd.Next(0, soluong_nhanvien);
                } while (Array.Exists(danhsach_so_trung_giai, num => num == random_so));

                danhsach_so_trung_giai[i] = random_so;
            }

            return danhsach_so_trung_giai;
        }

        // số dăc biet
        int index_quaydao1 = 0;
        int index_quaydao2 = 1920;
        int index_quaydao3 = 0;
        int index_quaydao4 = 1920;
        int index_quaydao5 = 0;
        int index_quaydao6 = 1920;

        // day 2
        int index_quaydao_2_1 = 0;
        int index_quaydao_2_2 = 1920;
        int index_quaydao_2_3 = 0;
        int index_quaydao_2_4 = 1920;
        int index_quaydao_2_5 = 0;
        int index_quaydao_2_6 = 1920;

        // day 3
        int index_quaydao_3_1 = 0;
        int index_quaydao_3_2 = 1920;
        int index_quaydao_3_3 = 0;
        int index_quaydao_3_4 = 1920;
        int index_quaydao_3_5 = 0;
        int index_quaydao_3_6 = 1920;

        //day 4
        int index_quaydao_4_1 = 0;
        int index_quaydao_4_2 = 1920;
        int index_quaydao_4_3 = 0;
        int index_quaydao_4_4 = 1920;
        int index_quaydao_4_5 = 0;
        int index_quaydao_4_6 = 1920;

        int frame_quaydao = 6;

        private void quay_so_all()
        {
            Thread t2 = new Thread(() =>
            {
                while (true)
                {
                    //-----------------day so 1 dac biet
                    // quay dạo sô 1
                    if (index_quaydao1 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_so1(index_quaydao1);
                            index_quaydao1 = index_quaydao1 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao1 = 0;
                    }

                    // quay dạo sô 2
                    if (index_quaydao2 >= 0 && index_quaydao2 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_so2(index_quaydao2);
                            index_quaydao2 = index_quaydao2 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao2 = 1920;
                    }

                    // quay dạo sô 3
                    if (index_quaydao3 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_so3(index_quaydao3);
                            index_quaydao3 = index_quaydao3 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao3 = 0;
                    }

                    // quay dạo sô 4
                    if (index_quaydao4 >= 0 && index_quaydao4 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_so4(index_quaydao4);
                            index_quaydao4 = index_quaydao4 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao4 = 1920;
                    }

                    // quay dạo sô 5
                    if (index_quaydao5 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_so5(index_quaydao5);
                            index_quaydao5 = index_quaydao5 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao5 = 0;
                    }

                    // quay dạo sô 6
                    if (index_quaydao6 >= 0 && index_quaydao6 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_so6(index_quaydao6);
                            index_quaydao6 = index_quaydao6 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao6 = 1920;
                    }

                    //-----------------------day so 2
                    // quay dạo sô 1
                    if (index_quaydao_2_1 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_3_so1(index_quaydao_2_1);
                            index_quaydao_2_1 = index_quaydao_2_1 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_2_1 = 0;
                    }

                    // quay dạo sô 2
                    if (index_quaydao_2_2 >= 0 && index_quaydao_2_2 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_3_so2(index_quaydao_2_2);
                            index_quaydao_2_2 = index_quaydao_2_2 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_2_2 = 1920;
                    }

                    // quay dạo sô 3
                    if (index_quaydao_2_3 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_3_so3(index_quaydao_2_3);
                            index_quaydao_2_3 = index_quaydao_2_3 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_2_3 = 0;
                    }

                    // quay dạo sô 4
                    if (index_quaydao_2_4 >= 0 && index_quaydao_2_4 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_3_so4(index_quaydao_2_4);
                            index_quaydao_2_4 = index_quaydao_2_4 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_2_4 = 1920;
                    }

                    // quay dạo sô 5
                    if (index_quaydao_2_5 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_3_so5(index_quaydao_2_5);
                            index_quaydao_2_5 = index_quaydao_2_5 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_2_5 = 0;
                    }

                    // quay dạo sô 6
                    if (index_quaydao_2_6 >= 0 && index_quaydao_2_6 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_3_so6(index_quaydao_2_6);
                            index_quaydao_2_6 = index_quaydao_2_6 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_2_6 = 1920;
                    }

                    //-----------------------day so 3
                    // quay dạo sô 1
                    if (index_quaydao_3_1 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_2_so1(index_quaydao_3_1);
                            index_quaydao_3_1 = index_quaydao_3_1 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_3_1 = 0;
                    }

                    // quay dạo sô 2
                    if (index_quaydao_3_2 >= 0 && index_quaydao_3_2 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_2_so2(index_quaydao_3_2);
                            index_quaydao_3_2 = index_quaydao_3_2 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_3_2 = 1920;
                    }

                    // quay dạo sô 3
                    if (index_quaydao_3_3 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_2_so3(index_quaydao_3_3);
                            index_quaydao_3_3 = index_quaydao_3_3 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_3_3 = 0;
                    }

                    // quay dạo sô 4
                    if (index_quaydao_3_4 >= 0 && index_quaydao_3_4 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_2_so4(index_quaydao_3_4);
                            index_quaydao_3_4 = index_quaydao_3_4 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_3_4 = 1920;
                    }

                    // quay dạo sô 5
                    if (index_quaydao_3_5 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_2_so5(index_quaydao_3_5);
                            index_quaydao_3_5 = index_quaydao_3_5 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_3_5 = 0;
                    }

                    // quay dạo sô 6
                    if (index_quaydao_3_6 >= 0 && index_quaydao_3_6 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_2_so6(index_quaydao_3_6);
                            index_quaydao_3_6 = index_quaydao_3_6 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_3_6 = 1920;
                    }

                    //-----------------------day so 4
                    // quay dạo sô 1
                    if (index_quaydao_4_1 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_4_so1(index_quaydao_4_1);
                            index_quaydao_4_1 = index_quaydao_4_1 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_4_1 = 0;
                    }

                    // quay dạo sô 2
                    if (index_quaydao_4_2 >= 0 && index_quaydao_4_2 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_4_so2(index_quaydao_4_2);
                            index_quaydao_4_2 = index_quaydao_4_2 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_4_2 = 1920;
                    }

                    // quay dạo sô 3
                    if (index_quaydao_4_3 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_4_so3(index_quaydao_4_3);
                            index_quaydao_4_3 = index_quaydao_4_3 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_4_3 = 0;
                    }

                    // quay dạo sô 4
                    if (index_quaydao_4_4 >= 0 && index_quaydao_4_4 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_4_so4(index_quaydao_4_4);
                            index_quaydao_4_4 = index_quaydao_4_4 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_4_4 = 1920;
                    }

                    // quay dạo sô 5
                    if (index_quaydao_4_5 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_4_so5(index_quaydao_4_5);
                            index_quaydao_4_5 = index_quaydao_4_5 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_4_5 = 0;
                    }

                    // quay dạo sô 6
                    if (index_quaydao_4_6 >= 0 && index_quaydao_4_6 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_4_so6(index_quaydao_4_6);
                            index_quaydao_4_6 = index_quaydao_4_6 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_4_6 = 1920;
                    }
                }
            });
            t2.Start();
            t2.IsBackground = true;
        }

        private int chi_so_show_so(int so)
        {
            int chiso1;
            if (so == 9)
            {
                chiso1 = 1920;
            }
            else
            {
                chiso1 = 192 * (9 - so);
            }

            return chiso1;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isstop == true)
            {
                if (frame_quaydao >= 2 && frame_quaydao <= 192)
                {
                    frame_quaydao--;
                }

                if (frame_quaydao == 1)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_so1(chi_so_show_so(kq_so1));
                        show_so2(chi_so_show_so(kq_so2));
                        show_so3(chi_so_show_so(kq_so3));
                        show_so4(chi_so_show_so(kq_so4));
                        show_so5(chi_so_show_so(kq_so5));
                        show_so6(chi_so_show_so(kq_so6));

                        show_2_so1(chi_so_show_so(kq_2_so1));
                        show_2_so2(chi_so_show_so(kq_2_so2));
                        show_2_so3(chi_so_show_so(kq_2_so3));
                        show_2_so4(chi_so_show_so(kq_2_so4));
                        show_2_so5(chi_so_show_so(kq_2_so5));
                        show_2_so6(chi_so_show_so(kq_2_so6));

                        show_3_so1(chi_so_show_so(kq_3_so1));
                        show_3_so2(chi_so_show_so(kq_3_so2));
                        show_3_so3(chi_so_show_so(kq_3_so3));
                        show_3_so4(chi_so_show_so(kq_3_so4));
                        show_3_so5(chi_so_show_so(kq_3_so5));
                        show_3_so6(chi_so_show_so(kq_3_so6));

                        show_4_so1(chi_so_show_so(kq_4_so1));
                        show_4_so2(chi_so_show_so(kq_4_so2));
                        show_4_so3(chi_so_show_so(kq_4_so3));
                        show_4_so4(chi_so_show_so(kq_4_so4));
                        show_4_so5(chi_so_show_so(kq_4_so5));
                        show_4_so6(chi_so_show_so(kq_4_so6));

                        show_check = true;

                        timer_quaydaoso.Stop();
                    }));
                }
                else
                {
                    //-----------------day so 1 dac biet
                    // quay dạo sô 1
                    if (index_quaydao1 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_so1(index_quaydao1);
                            index_quaydao1 = index_quaydao1 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao1 = 0;
                    }

                    // quay dạo sô 2
                    if (index_quaydao2 >= 0 && index_quaydao2 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_so2(index_quaydao2);
                            index_quaydao2 = index_quaydao2 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao2 = 1920;
                    }

                    // quay dạo sô 3
                    if (index_quaydao3 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_so3(index_quaydao3);
                            index_quaydao3 = index_quaydao3 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao3 = 0;
                    }

                    // quay dạo sô 4
                    if (index_quaydao4 >= 0 && index_quaydao4 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_so4(index_quaydao4);
                            index_quaydao4 = index_quaydao4 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao4 = 1920;
                    }

                    // quay dạo sô 5
                    if (index_quaydao5 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_so5(index_quaydao5);
                            index_quaydao5 = index_quaydao5 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao5 = 0;
                    }

                    // quay dạo sô 6
                    if (index_quaydao6 >= 0 && index_quaydao6 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_so6(index_quaydao6);
                            index_quaydao6 = index_quaydao6 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao6 = 1920;
                    }

                    //-----------------------day so 2
                    // quay dạo sô 1
                    if (index_quaydao_2_1 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_3_so1(index_quaydao_2_1);
                            index_quaydao_2_1 = index_quaydao_2_1 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_2_1 = 0;
                    }

                    // quay dạo sô 2
                    if (index_quaydao_2_2 >= 0 && index_quaydao_2_2 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_3_so2(index_quaydao_2_2);
                            index_quaydao_2_2 = index_quaydao_2_2 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_2_2 = 1920;
                    }

                    // quay dạo sô 3
                    if (index_quaydao_2_3 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_3_so3(index_quaydao_2_3);
                            index_quaydao_2_3 = index_quaydao_2_3 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_2_3 = 0;
                    }

                    // quay dạo sô 4
                    if (index_quaydao_2_4 >= 0 && index_quaydao_2_4 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_3_so4(index_quaydao_2_4);
                            index_quaydao_2_4 = index_quaydao_2_4 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_2_4 = 1920;
                    }

                    // quay dạo sô 5
                    if (index_quaydao_2_5 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_3_so5(index_quaydao_2_5);
                            index_quaydao_2_5 = index_quaydao_2_5 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_2_5 = 0;
                    }

                    // quay dạo sô 6
                    if (index_quaydao_2_6 >= 0 && index_quaydao_2_6 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_3_so6(index_quaydao_2_6);
                            index_quaydao_2_6 = index_quaydao_2_6 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_2_6 = 1920;
                    }

                    //-----------------------day so 3
                    // quay dạo sô 1
                    if (index_quaydao_3_1 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_2_so1(index_quaydao_3_1);
                            index_quaydao_3_1 = index_quaydao_3_1 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_3_1 = 0;
                    }

                    // quay dạo sô 2
                    if (index_quaydao_3_2 >= 0 && index_quaydao_3_2 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_2_so2(index_quaydao_3_2);
                            index_quaydao_3_2 = index_quaydao_3_2 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_3_2 = 1920;
                    }

                    // quay dạo sô 3
                    if (index_quaydao_3_3 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_2_so3(index_quaydao_3_3);
                            index_quaydao_3_3 = index_quaydao_3_3 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_3_3 = 0;
                    }

                    // quay dạo sô 4
                    if (index_quaydao_3_4 >= 0 && index_quaydao_3_4 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_2_so4(index_quaydao_3_4);
                            index_quaydao_3_4 = index_quaydao_3_4 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_3_4 = 1920;
                    }

                    // quay dạo sô 5
                    if (index_quaydao_3_5 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_2_so5(index_quaydao_3_5);
                            index_quaydao_3_5 = index_quaydao_3_5 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_3_5 = 0;
                    }

                    // quay dạo sô 6
                    if (index_quaydao_3_6 >= 0 && index_quaydao_3_6 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_2_so6(index_quaydao_3_6);
                            index_quaydao_3_6 = index_quaydao_3_6 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_3_6 = 1920;
                    }

                    //-----------------------day so 4
                    // quay dạo sô 1
                    if (index_quaydao_4_1 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_4_so1(index_quaydao_4_1);
                            index_quaydao_4_1 = index_quaydao_4_1 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_4_1 = 0;
                    }

                    // quay dạo sô 2
                    if (index_quaydao_4_2 >= 0 && index_quaydao_4_2 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_4_so2(index_quaydao_4_2);
                            index_quaydao_4_2 = index_quaydao_4_2 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_4_2 = 1920;
                    }

                    // quay dạo sô 3
                    if (index_quaydao_4_3 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_4_so3(index_quaydao_4_3);
                            index_quaydao_4_3 = index_quaydao_4_3 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_4_3 = 0;
                    }

                    // quay dạo sô 4
                    if (index_quaydao_4_4 >= 0 && index_quaydao_4_4 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_4_so4(index_quaydao_4_4);
                            index_quaydao_4_4 = index_quaydao_4_4 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_4_4 = 1920;
                    }

                    // quay dạo sô 5
                    if (index_quaydao_4_5 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_4_so5(index_quaydao_4_5);
                            index_quaydao_4_5 = index_quaydao_4_5 + frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_4_5 = 0;
                    }

                    // quay dạo sô 6
                    if (index_quaydao_4_6 >= 0 && index_quaydao_4_6 <= 1920)
                    {
                        this.Invoke(new Action(() =>
                        {
                            show_4_so6(index_quaydao_4_6);
                            index_quaydao_4_6 = index_quaydao_4_6 - frame_quaydao;
                        }));
                    }
                    else
                    {
                        index_quaydao_4_6 = 1920;
                    }
                }
            }
            else
            {
                //-----------------day so 1 dac biet
                // quay dạo sô 1
                if (index_quaydao1 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_so1(index_quaydao1);
                        index_quaydao1 = index_quaydao1 + frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao1 = 0;
                }

                // quay dạo sô 2
                if (index_quaydao2 >= 0 && index_quaydao2 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_so2(index_quaydao2);
                        index_quaydao2 = index_quaydao2 - frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao2 = 1920;
                }

                // quay dạo sô 3
                if (index_quaydao3 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_so3(index_quaydao3);
                        index_quaydao3 = index_quaydao3 + frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao3 = 0;
                }

                // quay dạo sô 4
                if (index_quaydao4 >= 0 && index_quaydao4 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_so4(index_quaydao4);
                        index_quaydao4 = index_quaydao4 - frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao4 = 1920;
                }

                // quay dạo sô 5
                if (index_quaydao5 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_so5(index_quaydao5);
                        index_quaydao5 = index_quaydao5 + frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao5 = 0;
                }

                // quay dạo sô 6
                if (index_quaydao6 >= 0 && index_quaydao6 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_so6(index_quaydao6);
                        index_quaydao6 = index_quaydao6 - frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao6 = 1920;
                }

                //-----------------------day so 2
                // quay dạo sô 1
                if (index_quaydao_2_1 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_3_so1(index_quaydao_2_1);
                        index_quaydao_2_1 = index_quaydao_2_1 + frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao_2_1 = 0;
                }

                // quay dạo sô 2
                if (index_quaydao_2_2 >= 0 && index_quaydao_2_2 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_3_so2(index_quaydao_2_2);
                        index_quaydao_2_2 = index_quaydao_2_2 - frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao_2_2 = 1920;
                }

                // quay dạo sô 3
                if (index_quaydao_2_3 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_3_so3(index_quaydao_2_3);
                        index_quaydao_2_3 = index_quaydao_2_3 + frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao_2_3 = 0;
                }

                // quay dạo sô 4
                if (index_quaydao_2_4 >= 0 && index_quaydao_2_4 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_3_so4(index_quaydao_2_4);
                        index_quaydao_2_4 = index_quaydao_2_4 - frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao_2_4 = 1920;
                }

                // quay dạo sô 5
                if (index_quaydao_2_5 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_3_so5(index_quaydao_2_5);
                        index_quaydao_2_5 = index_quaydao_2_5 + frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao_2_5 = 0;
                }

                // quay dạo sô 6
                if (index_quaydao_2_6 >= 0 && index_quaydao_2_6 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_3_so6(index_quaydao_2_6);
                        index_quaydao_2_6 = index_quaydao_2_6 - frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao_2_6 = 1920;
                }

                //-----------------------day so 3
                // quay dạo sô 1
                if (index_quaydao_3_1 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_2_so1(index_quaydao_3_1);
                        index_quaydao_3_1 = index_quaydao_3_1 + frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao_3_1 = 0;
                }

                // quay dạo sô 2
                if (index_quaydao_3_2 >= 0 && index_quaydao_3_2 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_2_so2(index_quaydao_3_2);
                        index_quaydao_3_2 = index_quaydao_3_2 - frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao_3_2 = 1920;
                }

                // quay dạo sô 3
                if (index_quaydao_3_3 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_2_so3(index_quaydao_3_3);
                        index_quaydao_3_3 = index_quaydao_3_3 + frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao_3_3 = 0;
                }

                // quay dạo sô 4
                if (index_quaydao_3_4 >= 0 && index_quaydao_3_4 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_2_so4(index_quaydao_3_4);
                        index_quaydao_3_4 = index_quaydao_3_4 - frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao_3_4 = 1920;
                }

                // quay dạo sô 5
                if (index_quaydao_3_5 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_2_so5(index_quaydao_3_5);
                        index_quaydao_3_5 = index_quaydao_3_5 + frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao_3_5 = 0;
                }

                // quay dạo sô 6
                if (index_quaydao_3_6 >= 0 && index_quaydao_3_6 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_2_so6(index_quaydao_3_6);
                        index_quaydao_3_6 = index_quaydao_3_6 - frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao_3_6 = 1920;
                }

                //-----------------------day so 4
                // quay dạo sô 1
                if (index_quaydao_4_1 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_4_so1(index_quaydao_4_1);
                        index_quaydao_4_1 = index_quaydao_4_1 + frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao_4_1 = 0;
                }

                // quay dạo sô 2
                if (index_quaydao_4_2 >= 0 && index_quaydao_4_2 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_4_so2(index_quaydao_4_2);
                        index_quaydao_4_2 = index_quaydao_4_2 - frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao_4_2 = 1920;
                }

                // quay dạo sô 3
                if (index_quaydao_4_3 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_4_so3(index_quaydao_4_3);
                        index_quaydao_4_3 = index_quaydao_4_3 + frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao_4_3 = 0;
                }

                // quay dạo sô 4
                if (index_quaydao_4_4 >= 0 && index_quaydao_4_4 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_4_so4(index_quaydao_4_4);
                        index_quaydao_4_4 = index_quaydao_4_4 - frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao_4_4 = 1920;
                }

                // quay dạo sô 5
                if (index_quaydao_4_5 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_4_so5(index_quaydao_4_5);
                        index_quaydao_4_5 = index_quaydao_4_5 + frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao_4_5 = 0;
                }

                // quay dạo sô 6
                if (index_quaydao_4_6 >= 0 && index_quaydao_4_6 <= 1920)
                {
                    this.Invoke(new Action(() =>
                    {
                        show_4_so6(index_quaydao_4_6);
                        index_quaydao_4_6 = index_quaydao_4_6 - frame_quaydao;
                    }));
                }
                else
                {
                    index_quaydao_4_6 = 1920;
                }
            }
        }

        Boolean show_check = false;

        private void thread_show_danhsach_trunggiai()
        {
            Thread t1 = new Thread(() =>
            {
                while (true)
                {
                    if (show_check == true)
                    {
                        this.Invoke(new Action(() =>
                        {
                            if (danhsach_nguoi_trung_giai_hien_tai.Count() == 1)
                            {
                                label_tentrunggiai_nhat.Text = danhsach_nguoi_trung_giai_hien_tai[0];
                                label_tentrunggiai_nhat.Show();
                                can_giua_label_trong_form(label_tentrunggiai_nhat.Width, label_tentrunggiai_nhat, 420);
                                timer_ketqua.Start();

                                btn_tieptuc.Hide();
                            }
                            if (danhsach_nguoi_trung_giai_hien_tai.Count() == 2)
                            {
                                label_tentrunggiai_1.Text = danhsach_nguoi_trung_giai_hien_tai[0];
                                label_tentrunggiai_2.Text = danhsach_nguoi_trung_giai_hien_tai[1];
                            }
                            if (danhsach_nguoi_trung_giai_hien_tai.Count() == 3)
                            {
                                label_tentrunggiai_1.Text = danhsach_nguoi_trung_giai_hien_tai[0];
                                label_tentrunggiai_2.Text = danhsach_nguoi_trung_giai_hien_tai[1];
                                label_tentrunggiai_3.Text = danhsach_nguoi_trung_giai_hien_tai[2];
                            }
                            if (danhsach_nguoi_trung_giai_hien_tai.Count() == 4)
                            {
                                label_tentrunggiai_1.Text = danhsach_nguoi_trung_giai_hien_tai[0];
                                label_tentrunggiai_2.Text = danhsach_nguoi_trung_giai_hien_tai[1];
                                label_tentrunggiai_3.Text = danhsach_nguoi_trung_giai_hien_tai[2];
                                label_tentrunggiai_4.Text = danhsach_nguoi_trung_giai_hien_tai[3];
                            }

                            tada_music.Play();

                            btn_xannhan.Show();
                            btn_quaylai.Show();

                            show_btn_xoa_va_quay_lai();

                            timer_luckydraw.Stop();

                            timer_nhapnhay_ten.Start();

                            show_check = false;

                            isstop = false;
                        }));
                        return;
                    }
                }
            });
            t1.Start();
            t1.IsBackground = true;
        }

        // ---------------------dãy số 1
        private void show_so1(int id)
        {
            ////pictureBox1.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            //// Thay đổi hình ảnh trong PictureBox thông qua luồng chính
            //pictureBox1.Invoke(new Action(() =>
            //{
            //    pictureBox1.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            //}));

            pictureBox1.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
        }

        private void show_so2(int id)
        {
            pictureBox2.Invoke(new Action(() =>
            {
                pictureBox2.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        private void show_so3(int id)
        {
            pictureBox3.Invoke(new Action(() =>
            {
                pictureBox3.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        private void show_so4(int id)
        {
            pictureBox4.Invoke(new Action(() =>
            {
                pictureBox4.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        private void show_so5(int id)
        {
            pictureBox5.Invoke(new Action(() =>
            {
                pictureBox5.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        private void show_so6(int id)
        {
            pictureBox6.Invoke(new Action(() =>
            {
                pictureBox6.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        //----------------------dãy số 2
        private void show_2_so1(int id)
        {
            ////pictureBox1.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            //// Thay đổi hình ảnh trong PictureBox thông qua luồng chính
            //pictureBox1.Invoke(new Action(() =>
            //{
            //    pictureBox1.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            //}));

            pictureBox_2_so1.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
        }

        private void show_2_so2(int id)
        {
            pictureBox2.Invoke(new Action(() =>
            {
                pictureBox_2_so2.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        private void show_2_so3(int id)
        {
            pictureBox3.Invoke(new Action(() =>
            {
                pictureBox_2_so3.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        private void show_2_so4(int id)
        {
            pictureBox4.Invoke(new Action(() =>
            {
                pictureBox_2_so4.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        private void show_2_so5(int id)
        {
            pictureBox5.Invoke(new Action(() =>
            {
                pictureBox_2_so5.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        private void show_2_so6(int id)
        {
            pictureBox6.Invoke(new Action(() =>
            {
                pictureBox_2_so6.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        //----------------------dãy số 3
        private void show_3_so1(int id)
        {
            ////pictureBox1.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            //// Thay đổi hình ảnh trong PictureBox thông qua luồng chính
            //pictureBox1.Invoke(new Action(() =>
            //{
            //    pictureBox1.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            //}));

            pictureBox_3_so1.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
        }

        private void show_3_so2(int id)
        {
            pictureBox2.Invoke(new Action(() =>
            {
                pictureBox_3_so2.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        private void show_3_so3(int id)
        {
            pictureBox3.Invoke(new Action(() =>
            {
                pictureBox_3_so3.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        private void show_3_so4(int id)
        {
            pictureBox4.Invoke(new Action(() =>
            {
                pictureBox_3_so4.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        private void show_3_so5(int id)
        {
            pictureBox5.Invoke(new Action(() =>
            {
                pictureBox_3_so5.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        private void show_3_so6(int id)
        {
            pictureBox6.Invoke(new Action(() =>
            {
                pictureBox_3_so6.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        //----------------------dãy số 4
        private void show_4_so1(int id)
        {
            ////pictureBox1.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            //// Thay đổi hình ảnh trong PictureBox thông qua luồng chính
            //pictureBox1.Invoke(new Action(() =>
            //{
            //    pictureBox1.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            //}));

            pictureBox_4_so1.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
        }

        private void show_4_so2(int id)
        {
            pictureBox2.Invoke(new Action(() =>
            {
                pictureBox_4_so2.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        private void show_4_so3(int id)
        {
            pictureBox3.Invoke(new Action(() =>
            {
                pictureBox_4_so3.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        private void show_4_so4(int id)
        {
            pictureBox4.Invoke(new Action(() =>
            {
                pictureBox_4_so4.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        private void show_4_so5(int id)
        {
            pictureBox5.Invoke(new Action(() =>
            {
                pictureBox_4_so5.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        private void show_4_so6(int id)
        {
            pictureBox6.Invoke(new Action(() =>
            {
                pictureBox_4_so6.Image = (Bitmap)Image.FromFile(string.Format("number09/n1/{0}.png", id));
            }));
        }

        string nguoi_trung_giai_toan_cuc = "";

        //int[] chiso_trunggiai, List<string> danhsach_nguoi_trung_giai

        Random random = new Random();

        int frame = 90;

        Boolean isstop = false;
        int speed = 10;

        int check_tra_kq = 1;
        int check_ktra_kq_day2 = 1;
        int check_ktra_kq_day3 = 1;
        int check_ktra_kq_day4 = 1;
        int check_delay_1s = 0;

        // day 1
        int kq_so1;
        int kq_so2;
        int kq_so3;
        int kq_so4;
        int kq_so5;
        int kq_so6;

        // day 2
        int kq_2_so1;
        int kq_2_so2;
        int kq_2_so3;
        int kq_2_so4;
        int kq_2_so5;
        int kq_2_so6;

        // day 3
        int kq_3_so1;
        int kq_3_so2;
        int kq_3_so3;
        int kq_3_so4;
        int kq_3_so5;
        int kq_3_so6;

        // day 4
        int kq_4_so1;
        int kq_4_so2;
        int kq_4_so3;
        int kq_4_so4;
        int kq_4_so5;
        int kq_4_so6;

        private void thread_quay_so()
        {
            frame = 92;

            // day so 1 dac vbiet
            Thread t1 = new Thread(() =>
            {
                int index = 0;
                int stop = 0;
                int chiso = 999;

                while (true)
                {
                    if (index <= 1920 && frame > 0)
                    {
                        if (isstop == true)
                        {
                            int so = kq_so1;
                            if (so == 9)
                            {
                                chiso = 1920;
                            }
                            else
                            {
                                chiso = 192 * (9 - so);
                            }

                            if (stop < 150)
                            {
                                show_so1(index);
                                index = index + 30;
                                stop++;
                            }
                            else
                            {
                                index = 0;
                                do
                                {
                                    this.Invoke(new Action(() =>
                                    {

                                        if (index < (chiso - 64))
                                        {
                                            show_so1(index);
                                            index = index + 15;
                                        }
                                        else
                                        {
                                            show_so1(index);
                                            index = index + 1;
                                        }

                                    }));
                                    Thread.Sleep(10);
                                } while (index <= chiso);

                                check_tra_kq++;

                                return;
                            }

                        }
                        else
                        {
                            this.Invoke(new Action(() =>
                            {
                                show_so1(index);
                                index = index + frame;
                            }));
                        }
                    }
                    else
                    {
                        index = 0;
                    }
                    Thread.Sleep(speed);
                }
            });
            t1.Start();
            t1.IsBackground = true;

            Thread t2 = new Thread(() =>
            {
                int index = 0;
                int stop = 0;
                int chiso = 999;
                while (true)
                {
                    if (index <= 1920 && frame > 0)
                    {
                        if (isstop == true)
                        {
                            int so = kq_so2;
                            if (so == 9)
                            {
                                chiso = 1920;
                            }
                            else
                            {
                                chiso = 192 * (9 - so);
                            }

                            if (stop < 150)
                            {
                                show_so2(index);
                                index = index + 30;
                                stop++;
                            }
                            else
                            {
                                index = 0;
                                do
                                {
                                    this.Invoke(new Action(() =>
                                    {

                                        if (index < (chiso - 64))
                                        {
                                            show_so2(index);
                                            index = index + 15;
                                        }
                                        else
                                        {
                                            show_so2(index);
                                            index = index + 1;
                                        }

                                    }));
                                    Thread.Sleep(10);
                                } while (index <= chiso);

                                check_tra_kq++;
                                return;
                            }

                        }
                        else
                        {
                            this.Invoke(new Action(() =>
                            {
                                show_so2(index);
                                index = index + frame;
                            }));
                        }
                    }
                    else
                    {
                        index = 0;
                    }
                    Thread.Sleep(speed);
                }
            });
            t2.Start();
            t2.IsBackground = true;

            Thread t3 = new Thread(() =>
            {
                int index = 0;
                int stop = 0;
                int chiso = 999;
                while (true)
                {
                    if (index <= 1920 && frame > 0)
                    {
                        if (isstop == true)
                        {
                            int so = kq_so3;
                            if (so == 9)
                            {
                                chiso = 1920;
                            }
                            else
                            {
                                chiso = 192 * (9 - so);
                            }

                            if (stop < 150)
                            {
                                show_so3(index);
                                index = index + 30;
                                stop++;
                            }
                            else
                            {
                                index = 0;
                                do
                                {
                                    this.Invoke(new Action(() =>
                                    {

                                        if (index < (chiso - 64))
                                        {
                                            show_so3(index);
                                            index = index + 15;
                                        }
                                        else
                                        {
                                            show_so3(index);
                                            index = index + 1;
                                        }

                                    }));
                                    Thread.Sleep(10);
                                } while (index <= chiso);

                                check_tra_kq++;

                                return;
                            }

                        }
                        else
                        {
                            this.Invoke(new Action(() =>
                            {
                                show_so3(index);
                                index = index + frame;
                            }));
                        }
                    }
                    else
                    {
                        index = 0;
                    }
                    Thread.Sleep(speed);
                }
            });
            t3.Start();
            t3.IsBackground = true;

            Thread t4 = new Thread(() =>
            {
                int index = 0;
                int stop = 0;
                int chiso = 999;
                while (true)
                {
                    if (index <= 1920 && frame > 0)
                    {
                        if (isstop == true)
                        {
                            int so = kq_so4;
                            if (so == 9)
                            {
                                chiso = 1920;
                            }
                            else
                            {
                                chiso = 192 * (9 - so);
                            }

                            if (stop < 150)
                            {
                                show_so4(index);
                                index = index + 30;
                                stop++;
                            }
                            else
                            {
                                index = 0;
                                do
                                {
                                    this.Invoke(new Action(() =>
                                    {

                                        if (index < (chiso - 64))
                                        {
                                            show_so4(index);
                                            index = index + 15;
                                        }
                                        else
                                        {
                                            show_so4(index);
                                            index = index + 1;
                                        }

                                    }));
                                    Thread.Sleep(10);
                                } while (index <= chiso);

                                check_tra_kq++;

                                return;
                            }

                        }
                        else
                        {
                            this.Invoke(new Action(() =>
                            {
                                show_so4(index);
                                index = index + frame;
                            }));
                        }
                    }
                    else
                    {
                        index = 0;
                    }
                    Thread.Sleep(speed);
                }
            });
            t4.Start();
            t4.IsBackground = true;

            Thread t5 = new Thread(() =>
            {
                int index = 0;
                int stop = 0;
                int chiso = 999;
                while (true)
                {
                    if (index <= 1920 && frame > 0)
                    {
                        if (isstop == true)
                        {
                            int so = kq_so5;
                            if (so == 9)
                            {
                                chiso = 1920;
                            }
                            else
                            {
                                chiso = 192 * (9 - so);
                            }

                            if (stop < 150)
                            {
                                show_so5(index);
                                index = index + 30;
                                stop++;
                            }
                            else
                            {
                                index = 0;
                                do
                                {
                                    this.Invoke(new Action(() =>
                                    {

                                        if (index < (chiso - 64))
                                        {
                                            show_so5(index);
                                            index = index + 15;
                                        }
                                        else
                                        {
                                            show_so5(index);
                                            index = index + 1;
                                        }

                                    }));
                                    Thread.Sleep(10);
                                } while (index <= chiso);

                                check_tra_kq++;

                                return;
                            }

                        }
                        else
                        {
                            this.Invoke(new Action(() =>
                            {
                                show_so5(index);
                                index = index + frame;
                            }));
                        }
                    }
                    else
                    {
                        index = 0;
                    }
                    Thread.Sleep(speed);
                }
            });
            t5.Start();
            t5.IsBackground = true;

            Thread t6 = new Thread(() =>
            {
                int index = 0;
                int stop = 0;
                int chiso = 999;
                while (true)
                {
                    if (index <= 1920 && frame > 0)
                    {
                        if (isstop == true)
                        {
                            int so = kq_so6;
                            if (so == 9)
                            {
                                chiso = 1920;
                            }
                            else
                            {
                                chiso = 192 * (9 - so);
                            }

                            if (stop < 150)
                            {
                                show_so6(index);
                                index = index + 30;
                                stop++;
                            }
                            else
                            {
                                index = 0;
                                do
                                {
                                    this.Invoke(new Action(() =>
                                    {

                                        if (index < (chiso - 64))
                                        {
                                            show_so6(index);
                                            index = index + 15;
                                        }
                                        else
                                        {
                                            show_so6(index);
                                            index = index + 1;
                                        }

                                    }));
                                    Thread.Sleep(10);
                                } while (index <= chiso);

                                check_tra_kq++;

                                return;
                            }

                        }
                        else
                        {
                            this.Invoke(new Action(() =>
                            {
                                show_so6(index);
                                index = index + frame;
                            }));
                        }
                    }
                    else
                    {
                        index = 0;
                    }
                    Thread.Sleep(speed);
                }
            });
            t6.Start();
            t6.IsBackground = true;

            //----------------------------------------------------------

            //int songuoi_nhan = chiso_trunggiai.Length;
            int show_nguoi_so_may = 1;

            int kiem_tra_show_het_day = 0;

            //khi 6 số show ra kết quả thì show tên người trúng ra
            Thread check_show_du_6_so = new Thread(() =>
            {
                while (true)
                {

                    if (check_tra_kq == 6)
                    {
                        this.Invoke(new Action(() =>
                        {
                            label_tentrunggiai_1.Text = danhsach_nguoi_trung_giai_hien_tai[0];

                            label_tentrunggiai_1.Visible = true;

                            tada_music.Play();

                            timer_luckydraw.Stop();

                            check_tra_kq = 1;

                            kiem_tra_show_het_day++;
                            //isstop = false;
                        }));
                    }

                    if (check_ktra_kq_day2 == 6 && kiem_tra_show_het_day == 1)
                    {
                        //Thread.Sleep(1000);
                        this.Invoke(new Action(() =>
                        {
                            label_tentrunggiai_2.Text = danhsach_nguoi_trung_giai_hien_tai[1];

                            label_tentrunggiai_2.Visible = true;

                            tada_music.Play();

                            //check_ktra_kq_day2 = 1;

                            kiem_tra_show_het_day++;
                            //isstop = false;
                        }));
                    }

                    if (check_ktra_kq_day3 == 6 && kiem_tra_show_het_day == 2)
                    {
                        //Thread.Sleep(1000);
                        this.Invoke(new Action(() =>
                        {
                            label_tentrunggiai_3.Text = danhsach_nguoi_trung_giai_hien_tai[2];

                            label_tentrunggiai_3.Visible = true;

                            tada_music.Play();

                            check_tra_kq = 1;

                            kiem_tra_show_het_day++;
                            //isstop = false;
                        }));
                    }

                    if (check_ktra_kq_day2 == 6 && kiem_tra_show_het_day == 3)
                    {
                        this.Invoke(new Action(() =>
                        {
                            label_tentrunggiai_4.Text = danhsach_nguoi_trung_giai_hien_tai[3];

                            label_tentrunggiai_4.Visible = true;

                            tada_music.Play();

                            check_tra_kq = 1;

                            kiem_tra_show_het_day++;
                            //isstop = false;
                        }));
                    }

                    if (kiem_tra_show_het_day == 4)
                    {
                        this.Invoke(new Action(() =>
                        {
                            timer_nhapnhay_ten.Start();
                        }));
                        return;
                    }
                }
            });
            check_show_du_6_so.Start();
            check_show_du_6_so.IsBackground = true;

            ////khi show 6 số lên thì chờ 1 giây sau lại quay tiếp
            //Thread check_show_du_6_so_delay_1s = new Thread(() =>
            //{
            //    while (true)
            //    {

            //        if (check_tra_kq == 6 && check_delay_1s == 1)
            //        {
            //            this.Invoke(new Action(() =>
            //            {
            //                Thread.Sleep(1000);

            //                //thread_quay_so();

            //                nhacdangquay.Play();
            //                timer_luckydraw.Start();
            //            }));
            //            return;
            //        }
            //    }
            //});
            //check_show_du_6_so_delay_1s.Start();
            //check_show_du_6_so_delay_1s.IsBackground = true;
        }

        private void thread_quay_so_giai_nhat()
        {
            frame = 92;
            isstop = false;
            Thread t1 = new Thread(() =>
            {
                int index = 0;
                int stop = 0;
                int chiso = 999;

                while (true)
                {
                    if (index <= 1920 && frame > 0)
                    {
                        if (isstop == true)
                        {
                            int so = kq_so1;
                            if (so == 9)
                            {
                                chiso = 1920;
                            }
                            else
                            {
                                chiso = 192 * (9 - so);
                            }

                            if (stop < 150)
                            {
                                show_so1(index);
                                index = index + 30;
                                stop++;
                            }
                            else
                            {
                                index = 0;
                                do
                                {
                                    this.Invoke(new Action(() =>
                                    {

                                        if (index < (chiso - 64))
                                        {
                                            show_so1(index);
                                            index = index + 15;
                                        }
                                        else
                                        {
                                            show_so1(index);
                                            index = index + 1;
                                        }

                                    }));
                                    Thread.Sleep(10);
                                } while (index <= chiso);

                                check_tra_kq++;

                                return;
                            }

                        }
                        else
                        {
                            this.Invoke(new Action(() =>
                            {
                                show_so1(index);
                                index = index + frame;
                            }));
                        }
                    }
                    else
                    {
                        index = 0;
                    }
                    Thread.Sleep(speed);
                }
            });
            t1.Start();
            t1.IsBackground = true;

            Thread t2 = new Thread(() =>
            {
                int index = 0;
                int stop = 0;
                int chiso = 999;
                while (true)
                {
                    if (index <= 1920 && frame > 0)
                    {
                        if (isstop == true)
                        {
                            int so = kq_so2;
                            if (so == 9)
                            {
                                chiso = 1920;
                            }
                            else
                            {
                                chiso = 192 * (9 - so);
                            }

                            if (stop < 150)
                            {
                                show_so2(index);
                                index = index + 30;
                                stop++;
                            }
                            else
                            {
                                index = 0;
                                do
                                {
                                    this.Invoke(new Action(() =>
                                    {

                                        if (index < (chiso - 64))
                                        {
                                            show_so2(index);
                                            index = index + 15;
                                        }
                                        else
                                        {
                                            show_so2(index);
                                            index = index + 1;
                                        }

                                    }));
                                    Thread.Sleep(10);
                                } while (index <= chiso);
                                return;
                            }

                        }
                        else
                        {
                            this.Invoke(new Action(() =>
                            {
                                show_so2(index);
                                index = index + frame;
                            }));
                        }
                    }
                    else
                    {
                        index = 0;
                    }
                    Thread.Sleep(speed);
                }
            });
            t2.Start();
            t2.IsBackground = true;

            Thread t3 = new Thread(() =>
            {
                int index = 0;
                int stop = 0;
                int chiso = 999;
                while (true)
                {
                    if (index <= 1920 && frame > 0)
                    {
                        if (isstop == true)
                        {
                            int so = kq_so3;
                            if (so == 9)
                            {
                                chiso = 1920;
                            }
                            else
                            {
                                chiso = 192 * (9 - so);
                            }

                            if (stop < 150)
                            {
                                show_so3(index);
                                index = index + 30;
                                stop++;
                            }
                            else
                            {
                                index = 0;
                                do
                                {
                                    this.Invoke(new Action(() =>
                                    {

                                        if (index < (chiso - 64))
                                        {
                                            show_so3(index);
                                            index = index + 15;
                                        }
                                        else
                                        {
                                            show_so3(index);
                                            index = index + 1;
                                        }

                                    }));
                                    Thread.Sleep(10);
                                } while (index <= chiso);

                                check_tra_kq++;

                                return;
                            }

                        }
                        else
                        {
                            this.Invoke(new Action(() =>
                            {
                                show_so3(index);
                                index = index + frame;
                            }));
                        }
                    }
                    else
                    {
                        index = 0;
                    }
                    Thread.Sleep(speed);
                }
            });
            t3.Start();
            t3.IsBackground = true;

            Thread t4 = new Thread(() =>
            {
                int index = 0;
                int stop = 0;
                int chiso = 999;
                while (true)
                {
                    if (index <= 1920 && frame > 0)
                    {
                        if (isstop == true)
                        {
                            int so = kq_so4;
                            if (so == 9)
                            {
                                chiso = 1920;
                            }
                            else
                            {
                                chiso = 192 * (9 - so);
                            }

                            if (stop < 150)
                            {
                                show_so4(index);
                                index = index + 30;
                                stop++;
                            }
                            else
                            {
                                index = 0;
                                do
                                {
                                    this.Invoke(new Action(() =>
                                    {

                                        if (index < (chiso - 64))
                                        {
                                            show_so4(index);
                                            index = index + 15;
                                        }
                                        else
                                        {
                                            show_so4(index);
                                            index = index + 1;
                                        }

                                    }));
                                    Thread.Sleep(10);
                                } while (index <= chiso);

                                check_tra_kq++;

                                return;
                            }

                        }
                        else
                        {
                            this.Invoke(new Action(() =>
                            {
                                show_so4(index);
                                index = index + frame;
                            }));
                        }
                    }
                    else
                    {
                        index = 0;
                    }
                    Thread.Sleep(speed);
                }
            });
            t4.Start();
            t4.IsBackground = true;

            Thread t5 = new Thread(() =>
            {
                int index = 0;
                int stop = 0;
                int chiso = 999;
                while (true)
                {
                    if (index <= 1920 && frame > 0)
                    {
                        if (isstop == true)
                        {
                            int so = kq_so5;
                            if (so == 9)
                            {
                                chiso = 1920;
                            }
                            else
                            {
                                chiso = 192 * (9 - so);
                            }

                            if (stop < 150)
                            {
                                show_so5(index);
                                index = index + 30;
                                stop++;
                            }
                            else
                            {
                                index = 0;
                                do
                                {
                                    this.Invoke(new Action(() =>
                                    {

                                        if (index < (chiso - 64))
                                        {
                                            show_so5(index);
                                            index = index + 15;
                                        }
                                        else
                                        {
                                            show_so5(index);
                                            index = index + 1;
                                        }

                                    }));
                                    Thread.Sleep(10);
                                } while (index <= chiso);

                                check_tra_kq++;

                                return;
                            }

                        }
                        else
                        {
                            this.Invoke(new Action(() =>
                            {
                                show_so5(index);
                                index = index + frame;
                            }));
                        }
                    }
                    else
                    {
                        index = 0;
                    }
                    Thread.Sleep(speed);
                }
            });
            t5.Start();
            t5.IsBackground = true;

            Thread t6 = new Thread(() =>
            {
                int index = 0;
                int stop = 0;
                int chiso = 999;
                while (true)
                {
                    if (index <= 1920 && frame > 0)
                    {
                        if (isstop == true)
                        {
                            int so = kq_so6;
                            if (so == 9)
                            {
                                chiso = 1920;
                            }
                            else
                            {
                                chiso = 192 * (9 - so);
                            }

                            if (stop < 150)
                            {
                                show_so6(index);
                                index = index + 30;
                                stop++;
                            }
                            else
                            {
                                index = 0;
                                do
                                {
                                    this.Invoke(new Action(() =>
                                    {

                                        if (index < (chiso - 64))
                                        {
                                            show_so6(index);
                                            index = index + 15;
                                        }
                                        else
                                        {
                                            show_so6(index);
                                            index = index + 1;
                                        }

                                    }));
                                    Thread.Sleep(10);
                                } while (index <= chiso);

                                check_tra_kq++;

                                return;
                            }

                        }
                        else
                        {
                            this.Invoke(new Action(() =>
                            {
                                show_so6(index);
                                index = index + frame;
                            }));
                        }
                    }
                    else
                    {
                        index = 0;
                    }
                    Thread.Sleep(speed);
                }
            });
            t6.Start();
            t6.IsBackground = true;

            //khi 6 số show ra kết quả thì show tên người trúng ra
            Thread check_show_du_6_so = new Thread(() =>
            {
                while (true)
                {

                    if (check_tra_kq == 6)
                    {
                        this.Invoke(new Action(() =>
                        {
                            label_tentrunggiai_1.Text = nguoi_trung_giai_toan_cuc;

                            can_giua_label_trong_form(label_tentrunggiai_1.Width, label_tentrunggiai_1, y_lable_nguoitrunggiai);
                            label_tentrunggiai_1.Visible = true;
                            timer_nhapnhay_ten.Start();

                            pictureBox_giaithuong.Visible = true;

                            tada_music.Play();

                            timer_luckydraw.Stop();

                            check_tra_kq = 1;
                            check_delay_1s = 1;
                            isstop = false;
                        }));
                        return;
                    }
                }
            });
            check_show_du_6_so.Start();
            check_show_du_6_so.IsBackground = true;

            //khi show 6 số lên thì chờ 1 giây sau lại quay tiếp
            Thread check_show_du_6_so_delay_1s = new Thread(() =>
            {
                while (true)
                {

                    if (check_tra_kq == 6 && check_delay_1s == 1)
                    {
                        this.Invoke(new Action(() =>
                        {
                            Thread.Sleep(1000);

                            //thread_quay_so();

                            nhacdangquay.Play();
                            timer_luckydraw.Start();
                        }));
                        return;
                    }
                }
            });
            check_show_du_6_so_delay_1s.Start();
            check_show_du_6_so_delay_1s.IsBackground = true;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (!timer_quaydaoso.Enabled)
            {
                timer_quaydaoso.Start();
            }

            frame_quaydao = 96;

            timer_nhapnhay_ten.Stop();
            timer_luckydraw.Start();

            nhacnen.Stop();
            nhacdangquay.Load();
            nhacdangquay.PlayLooping();

            btn_quay.Visible = false;
            btn_chot.Visible = true;
        }

        List<string> danhsach_nguoi_trung_giai_hien_tai = new List<string>();

        private void can_giua_label_trong_form(int labelWidth, System.Windows.Forms.Label label_, int ylabel)
        {
            int formWidth = this.Width;

            int xlabel = (formWidth - labelWidth) / 2;

            label_.Location = new Point(xlabel, ylabel);
        }

        List<int> index_nguoitrunggiai = new List<int>();

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            btn_chot.Visible = false;

            nhacdangquay.Stop();
            chot_music.Play();

            int so_luong_nguoi_quay_tren_luot = danhsach_luotquay[index_luotquay];

            int[] random_ = random_so_trung_giai(so_luong_nguoi_quay_tren_luot);

            List<string> danhsach_nguoi_trung_giai = new List<string>();

            for (int i = 0; i < random_.Length; i++)
            {
                string bien_nguoi_trung_giai = danhsach_nhanvien[random_[i]].manhanvien + "-" + danhsach_nhanvien[random_[i]].ten + "-" + danhsach_nhanvien[random_[i]].vitri + "-" + danhsach_nhanvien[random_[i]].phongban;
                danhsach_nguoi_trung_giai.Add(bien_nguoi_trung_giai);

            }

            danhsach_nguoi_trung_giai_hien_tai = danhsach_nguoi_trung_giai;

            if (random_.Length == 4)
            {
                char[] number = danhsach_nhanvien[random_[0]].manhanvien.ToCharArray();

                kq_so1 = Convert.ToInt32(number[0].ToString());
                kq_so2 = Convert.ToInt32(number[1].ToString());
                kq_so3 = Convert.ToInt32(number[2].ToString());
                kq_so4 = Convert.ToInt32(number[3].ToString());
                kq_so5 = Convert.ToInt32(number[4].ToString());
                kq_so6 = Convert.ToInt32(number[5].ToString());

                char[] number2 = danhsach_nhanvien[random_[1]].manhanvien.ToCharArray();

                kq_2_so1 = Convert.ToInt32(number2[0].ToString());
                kq_2_so2 = Convert.ToInt32(number2[1].ToString());
                kq_2_so3 = Convert.ToInt32(number2[2].ToString());
                kq_2_so4 = Convert.ToInt32(number2[3].ToString());
                kq_2_so5 = Convert.ToInt32(number2[4].ToString());
                kq_2_so6 = Convert.ToInt32(number2[5].ToString());

                char[] number3 = danhsach_nhanvien[random_[2]].manhanvien.ToCharArray();

                kq_3_so1 = Convert.ToInt32(number3[0].ToString());
                kq_3_so2 = Convert.ToInt32(number3[1].ToString());
                kq_3_so3 = Convert.ToInt32(number3[2].ToString());
                kq_3_so4 = Convert.ToInt32(number3[3].ToString());
                kq_3_so5 = Convert.ToInt32(number3[4].ToString());
                kq_3_so6 = Convert.ToInt32(number3[5].ToString());

                char[] number4 = danhsach_nhanvien[random_[3]].manhanvien.ToCharArray();

                kq_4_so1 = Convert.ToInt32(number4[0].ToString());
                kq_4_so2 = Convert.ToInt32(number4[1].ToString());
                kq_4_so3 = Convert.ToInt32(number4[2].ToString());
                kq_4_so4 = Convert.ToInt32(number4[3].ToString());
                kq_4_so5 = Convert.ToInt32(number4[4].ToString());
                kq_4_so6 = Convert.ToInt32(number4[5].ToString());
            }

            if (random_.Length == 3)
            {
                char[] number = danhsach_nhanvien[random_[0]].manhanvien.ToCharArray();

                kq_so1 = Convert.ToInt32(number[0].ToString());
                kq_so2 = Convert.ToInt32(number[1].ToString());
                kq_so3 = Convert.ToInt32(number[2].ToString());
                kq_so4 = Convert.ToInt32(number[3].ToString());
                kq_so5 = Convert.ToInt32(number[4].ToString());
                kq_so6 = Convert.ToInt32(number[5].ToString());

                char[] number2 = danhsach_nhanvien[random_[1]].manhanvien.ToCharArray();

                kq_2_so1 = Convert.ToInt32(number2[0].ToString());
                kq_2_so2 = Convert.ToInt32(number2[1].ToString());
                kq_2_so3 = Convert.ToInt32(number2[2].ToString());
                kq_2_so4 = Convert.ToInt32(number2[3].ToString());
                kq_2_so5 = Convert.ToInt32(number2[4].ToString());
                kq_2_so6 = Convert.ToInt32(number2[5].ToString());

                char[] number3 = danhsach_nhanvien[random_[2]].manhanvien.ToCharArray();

                kq_3_so1 = Convert.ToInt32(number3[0].ToString());
                kq_3_so2 = Convert.ToInt32(number3[1].ToString());
                kq_3_so3 = Convert.ToInt32(number3[2].ToString());
                kq_3_so4 = Convert.ToInt32(number3[3].ToString());
                kq_3_so5 = Convert.ToInt32(number3[4].ToString());
                kq_3_so6 = Convert.ToInt32(number3[5].ToString());
            }

            if (random_.Length == 2)
            {
                char[] number = danhsach_nhanvien[random_[0]].manhanvien.ToCharArray();

                kq_so1 = Convert.ToInt32(number[0].ToString());
                kq_so2 = Convert.ToInt32(number[1].ToString());
                kq_so3 = Convert.ToInt32(number[2].ToString());
                kq_so4 = Convert.ToInt32(number[3].ToString());
                kq_so5 = Convert.ToInt32(number[4].ToString());
                kq_so6 = Convert.ToInt32(number[5].ToString());

                char[] number2 = danhsach_nhanvien[random_[1]].manhanvien.ToCharArray();

                kq_2_so1 = Convert.ToInt32(number2[0].ToString());
                kq_2_so2 = Convert.ToInt32(number2[1].ToString());
                kq_2_so3 = Convert.ToInt32(number2[2].ToString());
                kq_2_so4 = Convert.ToInt32(number2[3].ToString());
                kq_2_so5 = Convert.ToInt32(number2[4].ToString());
                kq_2_so6 = Convert.ToInt32(number2[5].ToString());
            }

            if (random_.Length == 1)
            {
                char[] number = danhsach_nhanvien[random_[0]].manhanvien.ToCharArray();

                kq_so1 = Convert.ToInt32(number[0].ToString());
                kq_so2 = Convert.ToInt32(number[1].ToString());
                kq_so3 = Convert.ToInt32(number[2].ToString());
                kq_so4 = Convert.ToInt32(number[3].ToString());
                kq_so5 = Convert.ToInt32(number[4].ToString());
                kq_so6 = Convert.ToInt32(number[5].ToString());
            }

            isstop = true;
            thread_show_danhsach_trunggiai();

            foreach (int i in random_)
            {
                index_nguoitrunggiai.Add(i);
            }

        }

        private void show_lan_luot_manv_trung_giai(int[] random_indexs, List<string> danhsach_nguoi_trung_giai)
        {
            //for(int i = 0; i < random_indexs.Length; i++)
            //{

            //}
            //string ketqua__ = danhsach_nhanvien[random_indexs[i]].manhanvien;
            //char[] number = ketqua__.ToCharArray();

            //kq_so1 = Convert.ToInt32(number[0].ToString());
            //kq_so2 = Convert.ToInt32(number[1].ToString());
            //kq_so3 = Convert.ToInt32(number[2].ToString());
            //kq_so4 = Convert.ToInt32(number[3].ToString());
            //kq_so5 = Convert.ToInt32(number[4].ToString());
            //kq_so6 = Convert.ToInt32(number[5].ToString());

            //isstop = true;
            //nguoi_trung_giai_toan_cuc = bien_nguoi_trung_giai;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("luckydraw_v2"))
            {
                process.Kill();
            }
        }

        int nhay = 0;
        private void timer_nhapnhay_ten_Tick(object sender, EventArgs e)
        {
            if (nhay % 2 == 0)
            {
                label_tentrunggiai_1.ForeColor = Color.Red;
                label_tentrunggiai_2.ForeColor = Color.Red;
                label_tentrunggiai_3.ForeColor = Color.Red;
                label_tentrunggiai_4.ForeColor = Color.Red;
            }
            else
            {
                label_tentrunggiai_1.ForeColor = Color.Navy;
                label_tentrunggiai_2.ForeColor = Color.Navy;
                label_tentrunggiai_3.ForeColor = Color.Navy;
                label_tentrunggiai_4.ForeColor = Color.Navy;
            }
            nhay++;
        }

        int n = 0;
        private void timer_luckydraw_Tick(object sender, EventArgs e)
        {
            if (n % 2 == 0)
            {
                pictureBox_luckydraw.Image = Image.FromFile("image_sys/luckydraw2.png");
            }
            else
            {
                pictureBox_luckydraw.Image = Image.FromFile("image_sys/luckydraw.png");
            }
            n++;
        }

        private void btn_quay_MouseEnter(object sender, EventArgs e)
        {
            btn_quay.Image = Image.FromFile("image/quay_hover.png");
        }

        private void btn_quay_MouseLeave(object sender, EventArgs e)
        {
            btn_quay.Image = Image.FromFile("image/quay.png");
        }

        private void btn_chot_MouseEnter(object sender, EventArgs e)
        {
            btn_chot.Image = Image.FromFile("image/chot_hover.png");
        }

        private void btn_chot_MouseLeave(object sender, EventArgs e)
        {
            btn_chot.Image = Image.FromFile("image/chot.png");
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) // Nhấn phím Escape để thoát chế độ toàn màn hình
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.TopMost = false;
            }

            if (e.KeyCode == Keys.F11)
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;

                this.TopMost = true;
                this.Bounds = Screen.PrimaryScreen.Bounds; // Đảm bảo form lấp đầy màn hình chính
            }

            if (e.KeyCode == Keys.F1)
            {
                btn_showketqua.Visible = true;

                comboBox_luotquay.Show();
            }

            if (e.KeyCode == Keys.F2)
            {
                btn_showketqua.Visible = false;
                comboBox_luotquay.Hide();
            }

            if (e.KeyCode == Keys.S)
            {
                btn_xannhan.Visible = true;
            }

            if (e.KeyCode == Keys.D)
            {
                btn_xannhan.Visible = false;
            }
        }

        private void btn_mienbac_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void btn_miennam_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void btn_showketqua_MouseClick(object sender, MouseEventArgs e)
        {
            Form2 f = new Form2();
            f.ShowDialog();
        }

        public void view_ketqua()
        {
            btn_quay.Visible = false;

            btn_showketqua.Visible = false;

            label_tentrunggiai_1.Visible = false;

            pictureBox_giaithuong.Visible = true;

            timer_quaydaoso.Start();
            timer_luckydraw.Start();
            timer_ketqua.Start();
        }

        private void GhiFile(List<string> list_data)
        {
            try
            {
                if (list_data[0] != "")
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(string.Format("ketqua/ket-qua-luot-{0}.csv", (comboBox_luotquay.SelectedIndex + 1).ToString())))
                    {
                        foreach (string d in list_data)
                        {
                            if (d != "")
                            {
                                sw.WriteLine(d);
                            }
                        }
                        sw.Close();
                    }
                }
            }
            catch (Exception) { }
        }

        private void btn_xannhan_MouseClick(object sender, MouseEventArgs e)
        {
            if (danhsach_nguoi_trung_giai_hien_tai.Count > 0)
            {
                GhiFile(danhsach_nguoi_trung_giai_hien_tai);
            }

            if (check_co_quay_lai_le_1_nguoi_khong == true)
            {
                int index_remove = 0;
                foreach (string manv_trunggiai in danhsach_nguoi_trung_giai_hien_tai)
                {
                    string manhanvien = manv_trunggiai.Split("-")[0];
                    danhsach_nhanvien.RemoveAll(nv => nv.manhanvien == manhanvien);
                }

                check_co_quay_lai_le_1_nguoi_khong = false;
            }
            else
            {
                //xóa nhưng người đã trúng giải khỏi danh sách
                foreach (int i in index_nguoitrunggiai)
                {
                    danhsach_nhanvien.RemoveAt(i);
                }
            }

            //xóa index người đã xóa tránh lần sau bị lặp lại
            index_nguoitrunggiai.Clear();

            danhsach_nguoi_trung_giai_hien_tai.Clear();

            cap_nhap_lai_so_nguoi_con_lai();

            btn_xannhan.Visible = false;
            btn_quay.Visible = true;


            btn_quaylai.Hide();
            btn_quay.Hide();

            btn_tieptuc.Show();

            if (check_quay_giai_nhat == true)
            {
                btn_tieptuc.Hide();
            }

            hide_btn_xoa_va_quay_lai();
        }

        private void label_showketqua_2mien_Click(object sender, EventArgs e)
        {

        }

        int nn = 0;
        private void timer_ketqua_Tick(object sender, EventArgs e)
        {
            if (nn % 2 == 0)
            {
                label_tentrunggiai_nhat.ForeColor = Color.White;
            }
            else
            {
                label_tentrunggiai_nhat.ForeColor = Color.Navy;
            }
            nn++;
        }

        int index_luotquay = 0;

        Boolean check_quay_giai_nhat = false;

        private void comboBox_luotquay_SelectedIndexChanged(global::System.Object sender, global::System.EventArgs e)
        {
            btn_quay.Show();

            label_luotquay.Text = comboBox_luotquay.SelectedItem.ToString();
            can_giua_label_trong_form(label_luotquay.Width, label_luotquay, 460);

            index_luotquay = comboBox_luotquay.SelectedIndex;

            string quatang = "";
            int soqua = 0;
            foreach (giaithuong gt in danhsach_giaithuong)
            {
                if (Int32.Parse(gt.luotquay) == (index_luotquay + 1))
                {
                    string quatang_ = gt.soluong + " " + gt.quatang + " trị giá " + string.Format("{0:N0} đồng", decimal.Parse(gt.thanhtien));
                    quatang = quatang + quatang_ + "\n";
                    soqua++;
                }
            }

            label_quatang.Text = quatang;
            can_giua_label_trong_form(label_quatang.Size.Width, label_quatang, 617);
            if (soqua == 1)
            {
                show_1_giai();
                check_quay_giai_nhat = true;

                this.BackgroundImage = Image.FromFile("image_sys/background.png");
            }
            if (soqua == 2)
            {
                show_2_giai();
                this.BackgroundImage = Image.FromFile("image_sys/b2.png");
            }
            if (soqua == 3)
            {
                show_3_giai();
                this.BackgroundImage = Image.FromFile("image_sys/b2.png");
            }
            if (soqua == 4)
            {
                show_4_giai();
                this.BackgroundImage = Image.FromFile("image_sys/b2.png");
            }

            if (!timer_quaydaoso.Enabled)
            {
                timer_quaydaoso.Start();
                frame_quaydao = 6;
            }

            dua_lable_ten_trung_giai_ve_mac_dinh();

            timer_nhapnhay_ten.Stop();
        }

        private void dua_lable_ten_trung_giai_ve_mac_dinh()
        {
            label_tentrunggiai_1.Text = "Mã nhân viên-Họ và tên-Vị trí công việc-Phòng ban";
            label_tentrunggiai_2.Text = "Mã nhân viên-Họ và tên-Vị trí công việc-Phòng ban";
            label_tentrunggiai_3.Text = "Mã nhân viên-Họ và tên-Vị trí công việc-Phòng ban";
            label_tentrunggiai_4.Text = "Mã nhân viên-Họ và tên-Vị trí công việc-Phòng ban";
        }

        private void show_1_giai()
        {
            // giai nhat 
            panel_day_so1.Hide();
            panel_day_so2.Hide();
            panel_day_so3.Hide();
            panel_day_so4.Hide();

            pictureBox_luckydraw.Location = new System.Drawing.Point(380, 100);

            // Di chuyển PictureBox và Label ra khỏi Panel và đưa vào Form chính
            this.Controls.Add(pictureBox1);
            this.Controls.Add(pictureBox2);
            this.Controls.Add(pictureBox3);
            this.Controls.Add(pictureBox4);
            this.Controls.Add(pictureBox5);
            this.Controls.Add(pictureBox6);

            pictureBox1.Size = new System.Drawing.Size(80, 96);
            pictureBox2.Size = new System.Drawing.Size(80, 96);
            pictureBox3.Size = new System.Drawing.Size(80, 96);
            pictureBox4.Size = new System.Drawing.Size(80, 96);
            pictureBox5.Size = new System.Drawing.Size(80, 96);
            pictureBox6.Size = new System.Drawing.Size(80, 96);

            pictureBox1.Location = new System.Drawing.Point(430, 205);
            pictureBox2.Location = new System.Drawing.Point(516, 205);
            pictureBox3.Location = new System.Drawing.Point(602, 205);
            pictureBox4.Location = new System.Drawing.Point(687, 205);
            pictureBox5.Location = new System.Drawing.Point(775, 205);
            pictureBox6.Location = new System.Drawing.Point(861, 205);
        }
        private void show_2_giai()
        {
            panel_day_so1.Show();
            panel_day_so2.Show();
            panel_day_so3.Hide();
            panel_day_so4.Hide();

            pictureBox_luckydraw.Location = new System.Drawing.Point(380, 11);
        }
        private void show_3_giai()
        {
            panel_day_so1.Show();
            panel_day_so2.Show();
            panel_day_so3.Show();
            panel_day_so4.Hide();

            pictureBox_luckydraw.Location = new System.Drawing.Point(380, 11);
        }
        private void show_4_giai()
        {
            panel_day_so1.Show();
            panel_day_so2.Show();
            panel_day_so3.Show();
            panel_day_so4.Show();

            pictureBox_luckydraw.Location = new System.Drawing.Point(380, 11);


            // Di chuyển PictureBox và Label ra khỏi Panel và đưa vào Form chính
            panel_day_so1.Controls.Add(pictureBox1);
            panel_day_so1.Controls.Add(pictureBox2);
            panel_day_so1.Controls.Add(pictureBox3);
            panel_day_so1.Controls.Add(pictureBox4);
            panel_day_so1.Controls.Add(pictureBox5);
            panel_day_so1.Controls.Add(pictureBox6);

            pictureBox1.Size = new System.Drawing.Size(50, 56);
            pictureBox2.Size = new System.Drawing.Size(50, 56);
            pictureBox3.Size = new System.Drawing.Size(50, 56);
            pictureBox4.Size = new System.Drawing.Size(50, 56);
            pictureBox5.Size = new System.Drawing.Size(50, 56);
            pictureBox6.Size = new System.Drawing.Size(50, 56);

            pictureBox1.Location = new System.Drawing.Point(24, 11);
            pictureBox2.Location = new System.Drawing.Point(80, 11);
            pictureBox3.Location = new System.Drawing.Point(136, 11);
            pictureBox4.Location = new System.Drawing.Point(192, 11);
            pictureBox5.Location = new System.Drawing.Point(248, 11);
            pictureBox6.Location = new System.Drawing.Point(305, 11);
        }

        private void btn_quaylai_Click(global::System.Object sender, global::System.EventArgs e)
        {
            if (!timer_quaydaoso.Enabled)
            {
                timer_quaydaoso.Start();
                frame_quaydao = 6;
            }

            danhsach_nguoi_trung_giai_hien_tai.Clear();
            index_nguoitrunggiai.Clear();

            dua_lable_ten_trung_giai_ve_mac_dinh();
            label_tentrunggiai_nhat.Hide();

            btn_quay.Show();
            btn_quaylai.Hide();
            btn_xannhan.Hide();
        }

        private void btn_tieptuc_MouseClick(global::System.Object sender, global::System.Windows.Forms.MouseEventArgs e)
        {
            int index_select = comboBox_luotquay.SelectedIndex + 1;
            if (index_select <= 6)
            {
                comboBox_luotquay.SelectedIndex = index_select;
            }

            btn_tieptuc.Hide();
            btn_quay.Show();
        }

        private void btn_tieptuc_Click(global::System.Object sender, global::System.EventArgs e)
        {

        }

        private void button1_Click(global::System.Object sender, global::System.EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            this.TopMost = true;
            this.Bounds = Screen.PrimaryScreen.Bounds; // Đảm bảo form lấp đầy màn hình chính

            comboBox_luotquay.Show();
        }

        private void hide_btn_chot_day_so()
        {
            btn_chot_day1.Hide();
            btn_chot_day2.Hide();
            btn_chot_day3.Hide();
            btn_chot_day4.Hide();
        }

        private void show_btn_xoa_va_quay_lai()
        {
            btn_xoa_quaylai_day1.Show();
            btn_xoa_quaylai_day2.Show();
            btn_xoa_quaylai_day3.Show();
            btn_xoa_quaylai_day4.Show();
        }

        private void hide_btn_xoa_va_quay_lai()
        {
            btn_xoa_quaylai_day1.Hide();
            btn_xoa_quaylai_day2.Hide();
            btn_xoa_quaylai_day3.Hide();
            btn_xoa_quaylai_day4.Hide();
        }

        int quay_day_nao = 9;

        Boolean check_co_quay_lai_le_1_nguoi_khong = false;

        private void xoa_nhan_vien_bang_ma_nv(string manhanvien_remove)
        {
            danhsach_nhanvien.RemoveAll(nv => nv.manhanvien == manhanvien_remove);
        }

        private void btn_xoa_quaylai_day1_Click(global::System.Object sender, global::System.EventArgs e)
        {
            quay_day_nao = 1; // tương duong chi quay day 1

            check_co_quay_lai_le_1_nguoi_khong = true;

            //danhsach_nhanvien.RemoveAt(index_nguoitrunggiai[0]);

            string ma_nv_xoa = danhsach_nguoi_trung_giai_hien_tai[0].Split("-")[0];
            xoa_nhan_vien_bang_ma_nv(ma_nv_xoa);

            if (!timer_quaylai_rieng_1.Enabled)
            {
                timer_quaylai_rieng_1.Start();
                frame_quaydao = 90;
            }

            nhacdangquay.Play();
            cap_nhap_lai_so_nguoi_con_lai();

            label_tentrunggiai_1.Text = "Mã nhân viên-Họ và tên-Vị trí công việc-Phòng ban";

            btn_chot_day1.Show();
            btn_xoa_quaylai_day1.Hide();
        }

        private void btn_xoa_quaylai_day2_Click(global::System.Object sender, global::System.EventArgs e)
        {
            quay_day_nao = 2; // tương duong chi quay day 1

            check_co_quay_lai_le_1_nguoi_khong = true;

            //danhsach_nhanvien.RemoveAt(index_nguoitrunggiai[1]);

            string ma_nv_xoa = danhsach_nguoi_trung_giai_hien_tai[1].Split("-")[0];
            xoa_nhan_vien_bang_ma_nv(ma_nv_xoa);

            if (!timer_quaylai_rieng_1.Enabled)
            {
                timer_quaylai_rieng_1.Start();
                frame_quaydao = 90;
            }
            nhacdangquay.Play();
            cap_nhap_lai_so_nguoi_con_lai();

            label_tentrunggiai_2.Text = "Mã nhân viên-Họ và tên-Vị trí công việc-Phòng ban";

            btn_chot_day2.Show();
            btn_xoa_quaylai_day2.Hide();
        }

        private void btn_xoa_quaylai_day3_Click(global::System.Object sender, global::System.EventArgs e)
        {
            quay_day_nao = 3; // tương duong chi quay day 1

            check_co_quay_lai_le_1_nguoi_khong = true;

            //danhsach_nhanvien.RemoveAt(index_nguoitrunggiai[2]);

            string ma_nv_xoa = danhsach_nguoi_trung_giai_hien_tai[2].Split("-")[0];
            xoa_nhan_vien_bang_ma_nv(ma_nv_xoa);

            if (!timer_quaylai_rieng_1.Enabled)
            {
                timer_quaylai_rieng_1.Start();
                frame_quaydao = 90;
            }
            nhacdangquay.Play();
            cap_nhap_lai_so_nguoi_con_lai();

            label_tentrunggiai_3.Text = "Mã nhân viên-Họ và tên-Vị trí công việc-Phòng ban";

            btn_chot_day3.Show();
            btn_xoa_quaylai_day3.Hide();
        }

        private void btn_xoa_quaylai_day4_Click(global::System.Object sender, global::System.EventArgs e)
        {
            quay_day_nao = 4; // tương duong chi quay day 1

            check_co_quay_lai_le_1_nguoi_khong = true;

            //danhsach_nhanvien.RemoveAt(index_nguoitrunggiai[3]);

            string ma_nv_xoa = danhsach_nguoi_trung_giai_hien_tai[3].Split("-")[0];
            xoa_nhan_vien_bang_ma_nv(ma_nv_xoa);

            if (!timer_quaylai_rieng_1.Enabled)
            {
                timer_quaylai_rieng_1.Start();
                frame_quaydao = 90;
            }
            nhacdangquay.Play();
            cap_nhap_lai_so_nguoi_con_lai();

            label_tentrunggiai_4.Text = "Mã nhân viên-Họ và tên-Vị trí công việc-Phòng ban";

            btn_chot_day4.Show();
            btn_xoa_quaylai_day4.Hide();
        }

        private void timer_quaylai_rieng_1_Tick(global::System.Object sender, global::System.EventArgs e)
        {
            if (isstop == true)
            {
                if (frame_quaydao >= 2 && frame_quaydao <= 192)
                {
                    frame_quaydao--;
                }

                if (frame_quaydao == 1)
                {
                    this.Invoke(new Action(() =>
                    {
                        if (quay_day_nao == 1)
                        {
                            show_so1(chi_so_show_so(kq_so1));
                            show_so2(chi_so_show_so(kq_so2));
                            show_so3(chi_so_show_so(kq_so3));
                            show_so4(chi_so_show_so(kq_so4));
                            show_so5(chi_so_show_so(kq_so5));
                            show_so6(chi_so_show_so(kq_so6));
                        }

                        if (quay_day_nao == 2)
                        {
                            show_2_so1(chi_so_show_so(kq_2_so1));
                            show_2_so2(chi_so_show_so(kq_2_so2));
                            show_2_so3(chi_so_show_so(kq_2_so3));
                            show_2_so4(chi_so_show_so(kq_2_so4));
                            show_2_so5(chi_so_show_so(kq_2_so5));
                            show_2_so6(chi_so_show_so(kq_2_so6));
                        }
                        if (quay_day_nao == 3)
                        {
                            show_3_so1(chi_so_show_so(kq_3_so1));
                            show_3_so2(chi_so_show_so(kq_3_so2));
                            show_3_so3(chi_so_show_so(kq_3_so3));
                            show_3_so4(chi_so_show_so(kq_3_so4));
                            show_3_so5(chi_so_show_so(kq_3_so5));
                            show_3_so6(chi_so_show_so(kq_3_so6));
                        }
                        if (quay_day_nao == 4)
                        {
                            show_4_so1(chi_so_show_so(kq_4_so1));
                            show_4_so2(chi_so_show_so(kq_4_so2));
                            show_4_so3(chi_so_show_so(kq_4_so3));
                            show_4_so4(chi_so_show_so(kq_4_so4));
                            show_4_so5(chi_so_show_so(kq_4_so5));
                            show_4_so6(chi_so_show_so(kq_4_so6));
                        }

                        show_check = true;

                        timer_quaylai_rieng_1.Stop();
                    }));
                }
                else
                {
                    if (quay_day_nao == 1)
                    {
                        quay_day_1();
                    }

                    if (quay_day_nao == 2)
                    {
                        quay_day_2();
                    }
                    if (quay_day_nao == 3)
                    {
                        quay_day_3();
                    }
                    if (quay_day_nao == 4)
                    {
                        quay_day_4();
                    }
                }
            }
            else
            {
                if (quay_day_nao == 1)
                {
                    quay_day_1();
                }

                if (quay_day_nao == 2)
                {
                    quay_day_2();
                }
                if (quay_day_nao == 3)
                {
                    quay_day_3();
                }
                if (quay_day_nao == 4)
                {
                    quay_day_4();
                }
            }
        }

        private void quay_day_1()
        {
            //-----------------day so 1 dac biet
            // quay dạo sô 1
            if (index_quaydao1 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_so1(index_quaydao1);
                    index_quaydao1 = index_quaydao1 + frame_quaydao;
                }));
            }
            else
            {
                index_quaydao1 = 0;
            }

            // quay dạo sô 2
            if (index_quaydao2 >= 0 && index_quaydao2 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_so2(index_quaydao2);
                    index_quaydao2 = index_quaydao2 - frame_quaydao;
                }));
            }
            else
            {
                index_quaydao2 = 1920;
            }

            // quay dạo sô 3
            if (index_quaydao3 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_so3(index_quaydao3);
                    index_quaydao3 = index_quaydao3 + frame_quaydao;
                }));
            }
            else
            {
                index_quaydao3 = 0;
            }

            // quay dạo sô 4
            if (index_quaydao4 >= 0 && index_quaydao4 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_so4(index_quaydao4);
                    index_quaydao4 = index_quaydao4 - frame_quaydao;
                }));
            }
            else
            {
                index_quaydao4 = 1920;
            }

            // quay dạo sô 5
            if (index_quaydao5 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_so5(index_quaydao5);
                    index_quaydao5 = index_quaydao5 + frame_quaydao;
                }));
            }
            else
            {
                index_quaydao5 = 0;
            }

            // quay dạo sô 6
            if (index_quaydao6 >= 0 && index_quaydao6 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_so6(index_quaydao6);
                    index_quaydao6 = index_quaydao6 - frame_quaydao;
                }));
            }
            else
            {
                index_quaydao6 = 1920;
            }
        }

        private void quay_day_3()
        {
            //-----------------------day so 2
            // quay dạo sô 1
            if (index_quaydao_2_1 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_3_so1(index_quaydao_2_1);
                    index_quaydao_2_1 = index_quaydao_2_1 + frame_quaydao;
                }));
            }
            else
            {
                index_quaydao_2_1 = 0;
            }

            // quay dạo sô 2
            if (index_quaydao_2_2 >= 0 && index_quaydao_2_2 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_3_so2(index_quaydao_2_2);
                    index_quaydao_2_2 = index_quaydao_2_2 - frame_quaydao;
                }));
            }
            else
            {
                index_quaydao_2_2 = 1920;
            }

            // quay dạo sô 3
            if (index_quaydao_2_3 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_3_so3(index_quaydao_2_3);
                    index_quaydao_2_3 = index_quaydao_2_3 + frame_quaydao;
                }));
            }
            else
            {
                index_quaydao_2_3 = 0;
            }

            // quay dạo sô 4
            if (index_quaydao_2_4 >= 0 && index_quaydao_2_4 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_3_so4(index_quaydao_2_4);
                    index_quaydao_2_4 = index_quaydao_2_4 - frame_quaydao;
                }));
            }
            else
            {
                index_quaydao_2_4 = 1920;
            }

            // quay dạo sô 5
            if (index_quaydao_2_5 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_3_so5(index_quaydao_2_5);
                    index_quaydao_2_5 = index_quaydao_2_5 + frame_quaydao;
                }));
            }
            else
            {
                index_quaydao_2_5 = 0;
            }

            // quay dạo sô 6
            if (index_quaydao_2_6 >= 0 && index_quaydao_2_6 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_3_so6(index_quaydao_2_6);
                    index_quaydao_2_6 = index_quaydao_2_6 - frame_quaydao;
                }));
            }
            else
            {
                index_quaydao_2_6 = 1920;
            }
        }


        private void quay_day_2()
        {
            //-----------------------day so 3
            // quay dạo sô 1
            if (index_quaydao_3_1 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_2_so1(index_quaydao_3_1);
                    index_quaydao_3_1 = index_quaydao_3_1 + frame_quaydao;
                }));
            }
            else
            {
                index_quaydao_3_1 = 0;
            }

            // quay dạo sô 2
            if (index_quaydao_3_2 >= 0 && index_quaydao_3_2 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_2_so2(index_quaydao_3_2);
                    index_quaydao_3_2 = index_quaydao_3_2 - frame_quaydao;
                }));
            }
            else
            {
                index_quaydao_3_2 = 1920;
            }

            // quay dạo sô 3
            if (index_quaydao_3_3 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_2_so3(index_quaydao_3_3);
                    index_quaydao_3_3 = index_quaydao_3_3 + frame_quaydao;
                }));
            }
            else
            {
                index_quaydao_3_3 = 0;
            }

            // quay dạo sô 4
            if (index_quaydao_3_4 >= 0 && index_quaydao_3_4 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_2_so4(index_quaydao_3_4);
                    index_quaydao_3_4 = index_quaydao_3_4 - frame_quaydao;
                }));
            }
            else
            {
                index_quaydao_3_4 = 1920;
            }

            // quay dạo sô 5
            if (index_quaydao_3_5 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_2_so5(index_quaydao_3_5);
                    index_quaydao_3_5 = index_quaydao_3_5 + frame_quaydao;
                }));
            }
            else
            {
                index_quaydao_3_5 = 0;
            }

            // quay dạo sô 6
            if (index_quaydao_3_6 >= 0 && index_quaydao_3_6 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_2_so6(index_quaydao_3_6);
                    index_quaydao_3_6 = index_quaydao_3_6 - frame_quaydao;
                }));
            }
            else
            {
                index_quaydao_3_6 = 1920;
            }
        }

        private void quay_day_4()
        {
            //-----------------------day so 4
            // quay dạo sô 1
            if (index_quaydao_4_1 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_4_so1(index_quaydao_4_1);
                    index_quaydao_4_1 = index_quaydao_4_1 + frame_quaydao;
                }));
            }
            else
            {
                index_quaydao_4_1 = 0;
            }

            // quay dạo sô 2
            if (index_quaydao_4_2 >= 0 && index_quaydao_4_2 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_4_so2(index_quaydao_4_2);
                    index_quaydao_4_2 = index_quaydao_4_2 - frame_quaydao;
                }));
            }
            else
            {
                index_quaydao_4_2 = 1920;
            }

            // quay dạo sô 3
            if (index_quaydao_4_3 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_4_so3(index_quaydao_4_3);
                    index_quaydao_4_3 = index_quaydao_4_3 + frame_quaydao;
                }));
            }
            else
            {
                index_quaydao_4_3 = 0;
            }

            // quay dạo sô 4
            if (index_quaydao_4_4 >= 0 && index_quaydao_4_4 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_4_so4(index_quaydao_4_4);
                    index_quaydao_4_4 = index_quaydao_4_4 - frame_quaydao;
                }));
            }
            else
            {
                index_quaydao_4_4 = 1920;
            }

            // quay dạo sô 5
            if (index_quaydao_4_5 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_4_so5(index_quaydao_4_5);
                    index_quaydao_4_5 = index_quaydao_4_5 + frame_quaydao;
                }));
            }
            else
            {
                index_quaydao_4_5 = 0;
            }

            // quay dạo sô 6
            if (index_quaydao_4_6 >= 0 && index_quaydao_4_6 <= 1920)
            {
                this.Invoke(new Action(() =>
                {
                    show_4_so6(index_quaydao_4_6);
                    index_quaydao_4_6 = index_quaydao_4_6 - frame_quaydao;
                }));
            }
            else
            {
                index_quaydao_4_6 = 1920;
            }
        }

        private void button2_Click(global::System.Object sender, global::System.EventArgs e)
        {
        }

        private void btn_chot_day1_Click(global::System.Object sender, global::System.EventArgs e)
        {
            int[] random_index = random_so_trung_giai(1);

            string ma_nhan_vien_sau_khi_quay_lai = danhsach_nhanvien[random_index[0]].manhanvien;

            //kiểm tra xem khi quay lại có vo tình trùng với các dãy số đã quay ra không, nếu trùng thì ramdom lại index khi nào không trùng thì thôi
            while (check_trung_nguoi_da_trung(ma_nhan_vien_sau_khi_quay_lai) == 1)
            {
                random_index = random_so_trung_giai(1);
                ma_nhan_vien_sau_khi_quay_lai = danhsach_nhanvien[random_index[0]].manhanvien;
            }

            char[] number = danhsach_nhanvien[random_index[0]].manhanvien.ToCharArray();

            kq_so1 = Convert.ToInt32(number[0].ToString());
            kq_so2 = Convert.ToInt32(number[1].ToString());
            kq_so3 = Convert.ToInt32(number[2].ToString());
            kq_so4 = Convert.ToInt32(number[3].ToString());
            kq_so5 = Convert.ToInt32(number[4].ToString());
            kq_so6 = Convert.ToInt32(number[5].ToString());

            List<string> danhsach_nguoi_trung_giai = new List<string>();

            string bien_nguoi_trung_giai = danhsach_nhanvien[random_index[0]].manhanvien + "-" + danhsach_nhanvien[random_index[0]].ten + "-" + danhsach_nhanvien[random_index[0]].vitri + "-" + danhsach_nhanvien[random_index[0]].phongban;

            danhsach_nguoi_trung_giai_hien_tai[0] = bien_nguoi_trung_giai;

            isstop = true;
            thread_show_danhsach_trunggiai();

            chot_music.Play();
            btn_chot_day1.Hide();
        }

        private void btn_chot_day2_Click(global::System.Object sender, global::System.EventArgs e)
        {
            int[] random_index = random_so_trung_giai(1);

            string ma_nhan_vien_sau_khi_quay_lai = danhsach_nhanvien[random_index[0]].manhanvien;

            //kiểm tra xem khi quay lại có vo tình trùng với các dãy số đã quay ra không, nếu trùng thì ramdom lại index khi nào không trùng thì thôi
            while (check_trung_nguoi_da_trung(ma_nhan_vien_sau_khi_quay_lai) == 1)
            {
                random_index = random_so_trung_giai(1);
                ma_nhan_vien_sau_khi_quay_lai = danhsach_nhanvien[random_index[0]].manhanvien;
            }

            char[] number = danhsach_nhanvien[random_index[0]].manhanvien.ToCharArray();

            kq_2_so1 = Convert.ToInt32(number[0].ToString());
            kq_2_so2 = Convert.ToInt32(number[1].ToString());
            kq_2_so3 = Convert.ToInt32(number[2].ToString());
            kq_2_so4 = Convert.ToInt32(number[3].ToString());
            kq_2_so5 = Convert.ToInt32(number[4].ToString());
            kq_2_so6 = Convert.ToInt32(number[5].ToString());

            List<string> danhsach_nguoi_trung_giai = new List<string>();

            string bien_nguoi_trung_giai = danhsach_nhanvien[random_index[0]].manhanvien + "-" + danhsach_nhanvien[random_index[0]].ten + "-" + danhsach_nhanvien[random_index[0]].vitri + "-" + danhsach_nhanvien[random_index[0]].phongban;

            danhsach_nguoi_trung_giai_hien_tai[1] = bien_nguoi_trung_giai;

            isstop = true;
            thread_show_danhsach_trunggiai();

            chot_music.Play();
            btn_chot_day2.Hide();
        }

        private void btn_chot_day3_Click(global::System.Object sender, global::System.EventArgs e)
        {
            int[] random_index = random_so_trung_giai(1);

            string ma_nhan_vien_sau_khi_quay_lai = danhsach_nhanvien[random_index[0]].manhanvien;

            //kiểm tra xem khi quay lại có vo tình trùng với các dãy số đã quay ra không, nếu trùng thì ramdom lại index khi nào không trùng thì thôi
            while (check_trung_nguoi_da_trung(ma_nhan_vien_sau_khi_quay_lai) == 1)
            {
                random_index = random_so_trung_giai(1);
                ma_nhan_vien_sau_khi_quay_lai = danhsach_nhanvien[random_index[0]].manhanvien;
            }

            char[] number = danhsach_nhanvien[random_index[0]].manhanvien.ToCharArray();

            kq_3_so1 = Convert.ToInt32(number[0].ToString());
            kq_3_so2 = Convert.ToInt32(number[1].ToString());
            kq_3_so3 = Convert.ToInt32(number[2].ToString());
            kq_3_so4 = Convert.ToInt32(number[3].ToString());
            kq_3_so5 = Convert.ToInt32(number[4].ToString());
            kq_3_so6 = Convert.ToInt32(number[5].ToString());

            List<string> danhsach_nguoi_trung_giai = new List<string>();

            string bien_nguoi_trung_giai = danhsach_nhanvien[random_index[0]].manhanvien + "-" + danhsach_nhanvien[random_index[0]].ten + "-" + danhsach_nhanvien[random_index[0]].vitri + "-" + danhsach_nhanvien[random_index[0]].phongban;

            danhsach_nguoi_trung_giai_hien_tai[2] = bien_nguoi_trung_giai;

            isstop = true;
            thread_show_danhsach_trunggiai();

            chot_music.Play();
            btn_chot_day3.Hide();
        }

        private void btn_chot_day4_Click(global::System.Object sender, global::System.EventArgs e)
        {
            int[] random_index = random_so_trung_giai(1);

            string ma_nhan_vien_sau_khi_quay_lai = danhsach_nhanvien[random_index[0]].manhanvien;

            //kiểm tra xem khi quay lại có vo tình trùng với các dãy số đã quay ra không, nếu trùng thì ramdom lại index khi nào không trùng thì thôi
            while (check_trung_nguoi_da_trung(ma_nhan_vien_sau_khi_quay_lai) == 1)
            {
                random_index = random_so_trung_giai(1);
                ma_nhan_vien_sau_khi_quay_lai = danhsach_nhanvien[random_index[0]].manhanvien;
            }

            char[] number = danhsach_nhanvien[random_index[0]].manhanvien.ToCharArray();

            kq_4_so1 = Convert.ToInt32(number[0].ToString());
            kq_4_so2 = Convert.ToInt32(number[1].ToString());
            kq_4_so3 = Convert.ToInt32(number[2].ToString());
            kq_4_so4 = Convert.ToInt32(number[3].ToString());
            kq_4_so5 = Convert.ToInt32(number[4].ToString());
            kq_4_so6 = Convert.ToInt32(number[5].ToString());

            List<string> danhsach_nguoi_trung_giai = new List<string>();

            string bien_nguoi_trung_giai = danhsach_nhanvien[random_index[0]].manhanvien + "-" + danhsach_nhanvien[random_index[0]].ten + "-" + danhsach_nhanvien[random_index[0]].vitri + "-" + danhsach_nhanvien[random_index[0]].phongban;

            danhsach_nguoi_trung_giai_hien_tai[3] = bien_nguoi_trung_giai;

            isstop = true;
            thread_show_danhsach_trunggiai();

            chot_music.Play();

            btn_chot_day4.Hide();
        }

        private int check_trung_nguoi_da_trung(string ma_nv_quay_lai)
        {
            foreach (string ds in danhsach_nguoi_trung_giai_hien_tai)
            {
                string manhanvien = ds.Split("-")[0];
                if (ma_nv_quay_lai.Equals(manhanvien))
                {
                    return 1;
                }
            }
            return 0;
        }

        private void button2_Click_1(global::System.Object sender, global::System.EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.TopMost = false;
        }
    }

}