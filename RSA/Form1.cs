using System.Windows.Forms;
using System.Numerics;
using System;
using System.Collections.Generic;

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
            generateKeys();
        }

        private void generateKeys() {
            BigInteger n = p.prime * q.prime;
            BigInteger fi_n = (p.prime - 1) * (q.prime - 1);
            BigInteger e = selectE(fi_n);
            tbxMessage.Text += "    " + e.ToString();
            tbxEncryption.Text += "    " + fi_n.ToString();
        }

        private BigInteger selectE(BigInteger fi) {
            BigInteger e;
            Random rnd = new Random();
            int size = fi.ToString().Length;

            do {
                e = BigIntegerRandom.getRandom(size, rnd);
            } while (BigInteger.GreatestCommonDivisor(e, fi) != 1);
            return e;
        }
    }
}
