using De02.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace De02
{
    public partial class frmSanPham : Form
    {
        private QLSPContextDB db = new QLSPContextDB();
        public frmSanPham()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            var products = db.Sanphams.Select(p => new
            {
                p.MaSP,
                p.TenSP,
                p.NgayNhap,
                LoaiSP = p.LoaiSP.TenLoai
            }).ToList();

            dtgSanPham.Rows.Clear();

            foreach (var item in products)
            {
                dtgSanPham.Rows.Add(
                    item.MaSP,
                    item.TenSP,
                    item.NgayNhap?.ToString("dd/MM/yyyy"),
                    item.LoaiSP
                );
            }

            cboLoaiSP.DataSource = db.LoaiSPs.ToList();
            cboLoaiSP.DisplayMember = "TenLoai";
            cboLoaiSP.ValueMember = "MaLoai";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var newProduct = new Sanpham
            {
                MaSP = txtMaSP.Text,
                TenSP = txtTenSP.Text,
                NgayNhap = dtNgaynhap.Value,
                MaLoai = cboLoaiSP.SelectedValue.ToString()
            };

            db.Sanphams.Add(newProduct);
            db.SaveChanges();
            LoadData();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            var product = db.Sanphams.Find(txtMaSP.Text);
            if (product != null)
            {
                product.TenSP = txtTenSP.Text;
                product.NgayNhap = dtNgaynhap.Value;
                product.MaLoai = cboLoaiSP.SelectedValue.ToString();
                db.SaveChanges();
                LoadData();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Ask for confirmation before deleting the product
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?",
                                                "Xác nhận xóa",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                var product = db.Sanphams.Find(txtMaSP.Text);
                if (product != null)
                {
                    db.Sanphams.Remove(product);
                    db.SaveChanges();
                    LoadData(); // Reload data to refresh the grid
                    MessageBox.Show("Sản phẩm đã được xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Sản phẩm không tồn tại hoặc không thể xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Do nothing if the user selects 'No'
                MessageBox.Show("Hành động xóa đã bị hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            // Ask for confirmation before closing the application
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn thoát ứng dụng?",
                                                "Xác nhận thoát",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                this.Close(); // Close the application
            }
            else
            {
                // Do nothing if the user selects 'No'
                MessageBox.Show("Hành động thoát đã bị hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void ClearForm()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            dtNgaynhap.Value = DateTime.Now;
            cboLoaiSP.SelectedIndex = 0;
        }

        private void dtgSanPham_MouseClick(object sender, MouseEventArgs e)
        {
            if (dtgSanPham.SelectedRows.Count > 0)
            {
                var selectedRow = dtgSanPham.SelectedRows[0];
                txtMaSP.Text = selectedRow.Cells["MaSP"].Value?.ToString();
                txtTenSP.Text = selectedRow.Cells["TenSP"].Value?.ToString();

                if (DateTime.TryParse(selectedRow.Cells["NgayNhap"].Value?.ToString(), out DateTime ngayNhap))
                {
                    dtNgaynhap.Value = ngayNhap;
                }
                else
                {
                    dtNgaynhap.Value = DateTime.Now;
                }

                cboLoaiSP.Text = selectedRow.Cells["LoaiSP"].Value?.ToString();
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            var keyword = txtTimKiem.Text?.ToLower() ?? string.Empty;

            var products = db.Sanphams
                .Where(p => p.LoaiSP.TenLoai.ToLower().Contains(keyword)) // Filtering by product category name
                .Select(p => new
                {
                    p.MaSP,
                    p.TenSP,
                    p.NgayNhap,
                    LoaiSP = p.LoaiSP.TenLoai
                }).ToList();

            dtgSanPham.Rows.Clear();

            foreach (var item in products)
            {
                dtgSanPham.Rows.Add(
                    item.MaSP,
                    item.TenSP,
                    item.NgayNhap?.ToString("dd/MM/yyyy"),
                    item.LoaiSP
                );
            }
        }
    }
}
        //private void frmSanPham_Load(object sender, EventArgs e)
        //{
        //    LoadLoaiSPToComboBox();       // Load danh sách loại sản phẩm vào ComboBox
        //    LoadSanPhamToDataGridView();  // Load danh sách sản phẩm vào DataGridView
        //    SetControlState(false);       // Vô hiệu hóa các nút điều chỉnh ban đầu
        //}
        //private void LoadLoaiSPToComboBox()
        //{
        //    // Lấy danh sách loại sản phẩm từ database
        //    var loaiSPs = db.LoaiSPs.ToList();

        //    // Gán danh sách vào ComboBox
        //    cboLoaiSP.DataSource = loaiSPs;
        //    cboLoaiSP.DisplayMember = "TenLoai"; // Hiển thị tên loại sản phẩm
        //    cboLoaiSP.ValueMember = "MaLoai";    // Giá trị tương ứng là mã loại sản phẩm

        //    // Đảm bảo ComboBox không chọn mục nào ban đầu
        //    cboLoaiSP.SelectedIndex = -1;
        //}
        //private void LoadSanPhamToDataGridView()
        //{
        //    try
        //    {
        //        var sanPhams = db.Sanphams
        //            .Join(db.LoaiSPs,
        //                  sp => sp.MaLoai,
        //                  loai => loai.MaLoai,
        //                  (sp, loai) => new
        //                  {
        //                      sp.MaSP,
        //                      sp.TenSP,
        //                      TenLoai = loai.TenLoai ?? "Không xác định"
        //                  })
        //            .ToList();

        //        dtgSanPham.DataSource = sanPhams;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //private void ClearControls()
        //{
        //    txtMaSP.Text = "";
        //    txtTenSP.Text = "";
        //    dtNgaynhap.Text = "";
        //    cboLoaiSP.SelectedIndex = 0;
        //}

        //private void dtgSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    try
        //    {
        //        if (e.RowIndex >= 0 && dtgSanPham.Rows[e.RowIndex].Cells["MaSP"].Value != null)
        //        {
        //            DataGridViewRow row = dtgSanPham.Rows[e.RowIndex];

        //            txtMaSP.Text = row.Cells["MaSP"].Value?.ToString() ?? "";
        //            txtTenSP.Text = row.Cells["TenSP"].Value?.ToString() ?? "";
        //            dtNgaynhap.Text = row.Cells["NgayNhap"].Value != null
        //                ? Convert.ToDateTime(row.Cells["NgayNhap"].Value).ToString("yyyy-MM-dd")
        //                : DateTime.Now.ToString("yyyy-MM-dd");
        //            cboLoaiSP.SelectedValue = row.Cells["MaLoai"].Value?.ToString() ?? "";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Lỗi khi chọn sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //private void btnThem_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtMaSP.Text) || string.IsNullOrWhiteSpace(txtTenSP.Text))
        //    {
        //        MessageBox.Show("Vui lòng nhập đầy đủ thông tin sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    var newSanPham = new Sanpham
        //    {
        //        MaSP = txtMaSP.Text,
        //        TenSP = txtTenSP.Text,
        //        MaLoai = cboLoaiSP.SelectedValue?.ToString()
        //    };

        //    db.Sanphams.Add(newSanPham);
        //    db.SaveChanges();
        //    LoadSanPhamToDataGridView(); // Cập nhật lại DataGridView
        //    MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    ClearInput();
        //}

        //private void dtgSanPham_SelectionChanged(object sender, EventArgs e)
        //{
        //    if (dtgSanPham.CurrentRow != null)
        //    {
        //        var selectedRow = dtgSanPham.CurrentRow;
        //        txtMaSP.Text = selectedRow.Cells["MaSP"].Value?.ToString();
        //        txtTenSP.Text = selectedRow.Cells["TenSP"].Value?.ToString();
        //        cboLoaiSP.Text = selectedRow.Cells["TenLoai"].Value?.ToString();
        //        SetControlState(true); // Kích hoạt các nút chỉnh sửa, xóa
        //    }
        //}

        //private void btnSua_Click(object sender, EventArgs e)
        //{
        //    var selectedSanPham = db.Sanphams.Find(txtMaSP.Text);
        //    if (selectedSanPham != null)
        //    {
        //        selectedSanPham.TenSP = txtTenSP.Text;
        //        selectedSanPham.MaLoai = cboLoaiSP.SelectedValue?.ToString();
        //        db.SaveChanges();
        //        LoadSanPhamToDataGridView(); // Cập nhật lại DataGridView
        //        MessageBox.Show("Sửa thông tin sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Không tìm thấy sản phẩm để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //}

        //private void btnXoa_Click(object sender, EventArgs e)
        //{
        //    var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //    if (confirmResult == DialogResult.Yes)
        //    {
        //        var selectedSanPham = db.Sanphams.Find(txtMaSP.Text);
        //        if (selectedSanPham != null)
        //        {
        //            db.Sanphams.Remove(selectedSanPham);
        //            db.SaveChanges();
        //            LoadSanPhamToDataGridView(); // Cập nhật lại DataGridView
        //            MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            ClearInput();
        //        }
        //        else
        //        {
        //            MessageBox.Show("Không tìm thấy sản phẩm để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        }
        //    }
        //}

        //private void btnTimKiem_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var keyword = txtTimKiem.Text?.ToLower() ?? string.Empty;

        //        // Tìm sản phẩm dựa trên từ khóa
        //        var sanPhams = db.Sanphams
        //            .Where(sp => sp.TenSP != null && sp.TenSP.ToLower().Contains(keyword))
        //            .Join(db.LoaiSPs,
        //                  sp => sp.MaLoai,
        //                  loai => loai.MaLoai,
        //                  (sp, loai) => new
        //                  {
        //                      sp.MaSP,
        //                      sp.TenSP,
        //                      TenLoai = loai.TenLoai ?? "Không xác định"
        //                  })
        //            .ToList();

        //        // Gán kết quả vào DataGridView
        //        dtgSanPham.DataSource = sanPhams;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //private void btnThoat_Click(object sender, EventArgs e)
        //{
        //    var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn đóng form?", "Xác nhận đóng", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //    if (confirmResult == DialogResult.Yes)
        //    {
        //        this.Close();
        //    }
        //}
        //private void ClearInput()
        //{
        //    txtMaSP.Text = "";
        //    txtTenSP.Text = "";
        //    cboLoaiSP.SelectedIndex = -1;
        //    SetControlState(false);
        //}

        //private void SetControlState(bool editing)
        //{
        //    btnThem.Enabled = !editing;
        //    btnSua.Enabled = editing;
        //    btnXoa.Enabled = editing;
        //}
   
