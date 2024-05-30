using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace CTDL_CuoiKyGiaoDienAA
{
    public partial class Form1 : Form
    {
        public struct Word
        {
            public string WordName;
            public string Type;
            public string Meaning;
            public string Example;
        }
        public class DictNode
        {
            public Word Word;
            public DictNode Next;
        }
        public class HashTable
        {
            public int M;
            public DictNode[] heads;

            public HashTable()      
            {
                M = 100;
                heads = new DictNode[M];
                Init(heads);
            }

            public void Init(DictNode[] heads)
            {
                for (int i = 0; i < heads.Length; i++)
                {
                    heads[i] = null;
                }
            }

            public int hashFunc(Word w)
            {
                return hash1(w.WordName);
            }

            public int hash1(string en)
            {
                // Tính mã băm dựa trên mã ASCII chia cho tổng số phần tử của mảng băm
                int h = 0;
                for (int i = 0; i < en.Length; i++)
                {
                    h += ((int)char.ToLower(en[i])) * (i + 1);
                }

                return h % M;
            }
            public DictNode createWord(Word w)
            {
                DictNode word = new DictNode();
                word.Word = w;
                word.Next = null;
                return word;
            }
            public void addWord(DictNode[] heads, Word w)
            {
                int h = hashFunc(w);
                DictNode r = heads[h];
                DictNode p = createWord(w);
                if (r == null)
                {
                    heads[h] = p;
                }
                else
                {
                    while (r.Next != null)
                    {
                        r = r.Next;
                    }

                    r.Next = p;
                }
            }
        }
        public Form1()
        {
            InitializeComponent();
        }
        HashTable hash = new HashTable();   //Khởi tạo hashtable hash
        private void btnThem_Click(object sender, EventArgs e)
        {
            //Khởi tạo Word w
            Word w = new Word();
            w.WordName = txtWord.Text.Trim();       //Gán nội dung của textbox txtWord cho WordName
            w.Type = txtType.Text.Trim();           //Gán nội dung của textbox txtType cho Type
            w.Meaning = txtMeaning.Text.Trim();      //Gán nội dung của textbox txtMeaning cho Meaning
            w.Example = txtExample.Text.Trim();       //Gán nội dung của textbox txtExample cho Example


            hash.addWord(hash.heads, w);        // Sử dụng phương thức addWWord để thêm từ vào hash.

            // Xoá trắng các textbox sau khi đã nhập xong từ
            txtWord.ResetText();
            txtType.ResetText();
            txtMeaning.ResetText();
            txtExample.ResetText();
            // Đưa trỏ chuột về lại textbox txtWord
            txtWord.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();     //Thoát chương trình 
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            lblKQ.Text = " ";   //Xoá trắng label lblKQ sau mỗi lần nhấn
            // Duyệt qua tất cả các phần tử của hashtable
            for (int i = 0; i < hash.M; i++)
            {
                // phần tử nào có từ thì in lần lượt từ, loại từ, nghĩa, ví dụ của từ
                if (hash.heads[i] != null)
                {
                    DictNode p = hash.heads[i];
                    while (p != null)
                    {
                        lblKQ.Text = lblKQ.Text + "=============================" + "\n";
                        lblKQ.Text = lblKQ.Text + p.Word.WordName + "\n";
                        lblKQ.Text = lblKQ.Text + p.Word.Type + "\n";
                        lblKQ.Text = lblKQ.Text + p.Word.Meaning + "\n";
                        lblKQ.Text = lblKQ.Text + p.Word.Example + "\n";
                        p = p.Next;
                    }
                }
            }

        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            Word w = new Word();
            w.WordName = txtWord.Text.Trim();       //Gán nội dung của textbox txtWord cho WordName
            w.Type = txtType.Text.Trim();           //Gán nội dung của textbox txtType cho Type
            w.Meaning = txtMeaning.Text.Trim();      //Gán nội dung của textbox txtMeaning cho Meaning
            w.Example = txtExample.Text.Trim();       //Gán nội dung của textbox txtExample cho Example



            int h = hash.hashFunc(w);            //Lấy mã hashFunc của từ cần cập nhập
            DictNode r = hash.heads[h];
            if (r != null)
            {
                int t = hash.hashFunc(r.Word);
                do
                {
                    t = hash.hashFunc(r.Word);
                    if (t == h)
                    {
                        r.Word = w;
                        break;
                    }

                    r = r.Next;
                } while (r != null);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string en = txtWord.Text.Trim();        //Gán từ cần xoá được nhập vào từ textbox txtWord cho biến en. 
            int h = hash.hash1(en);                 // Lấy mã hash của từ en lưu vào biến h
            DictNode r = hash.heads[h];
            DictNode prev = null;
            while (r != null)
            {
                if (r.Word.WordName.CompareTo(en) == 0)
                {
                    if (prev == null)        //Xóa nếu en là đầu danh sách
                    {
                        hash.heads[h] = r.Next;
                    }
                    else                      //Xóa ở vị trí bất kì của en 
                    {
                        prev.Next = r.Next;
                    }
                    r.Next = null;
                    r = r.Next;
                    break;
                }
                prev = r;
                r = r.Next;
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string en = txtTim.Text.Trim();      //Gán từ cần xoá được nhập vào từ textbox txtWord cho biến en. 

            // Lấy mã hash của từ en lưu vào biến h
            int h = hash.hash1(en);
            DictNode r = hash.heads[h];
            while (r != null)
            {
                // Tìm kiếm theo thuật toán tìm kiếm tuần tự (Linear Search)
                int h1 = hash.hash1(r.Word.WordName);
                if (h == h1)  
                {
                    lblKQ.Text = r.Word.WordName + "\n" + r.Word.Type + "\n" + r.Word.Meaning + "\n" + r.Word.Example;
                    break;
                }
                r = r.Next;
            }
        }

        private void btnDoc_Click(object sender, EventArgs e)
        {
            int i = 0, k = 1, z = 2, j = 3;
            // đọc tất cả các hàng của file 'dicts.txt' vào chuỗi wordData
            string[] wordData = File.ReadAllLines("dicts.txt");

            while (z <= wordData.Length)
            {
                Word data = new Word
                {
                    WordName = wordData[i],
                    Type = wordData[k],
                    Meaning = wordData[z],
                    Example = wordData[j]
                };
                i = i + 4;
                k = k + 4;
                z = z + 4;
                j = j + 4;

                hash.addWord(hash.heads, data);
            }
            // Thông báo khi đã đọc xong file
            lblKQ.Text = "File da duoc doc xong";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("tudienAnhAnh.txt", FileMode.Create, FileAccess.ReadWrite);
            StreamWriter swrite = new StreamWriter(fs);
            // Duyệt lần lượt các phần tử của hashtable nếu phần tử nào có từ thì lưu từ, loại từ, nghĩa, ví dụ của từ vào file.
            for (int i = 0; i < hash.M; i++)
            {
                if (hash.heads[i] != null)
                {
                    DictNode p = hash.heads[i];
                    while (p != null)
                    {
                        swrite.WriteLine(p.Word.WordName);
                        swrite.WriteLine(p.Word.Type);
                        swrite.WriteLine(p.Word.Meaning);
                        swrite.WriteLine(p.Word.Example);
                        p = p.Next;
                    }
                }
            }
            swrite.Flush();
            fs.Close();     // Đóng file sau khi đã ghi xong.
            //Thông Báo sau khi đã lưu xong vào file
            lblKQ.Text = "TuDienDaDuocLuuVaoFile";
        }
    }
}
