using System.Windows.Forms;
using System.Numerics;
using System;

namespace RSA {
    public partial class Form1 : Form {
        string alphabet = "_abcdefghijklmnopqrstuvwxyz";
        PrimeNumber p;
        PrimeNumber q;
        Random random = new Random();
        public Form1() {
            InitializeComponent();
            p = new PrimeNumber(random);
            q = new PrimeNumber(random);

            tbxMessage.Text = p.ToString();
            tbxEncryption.Text = q.ToString();
        }
    }
}
