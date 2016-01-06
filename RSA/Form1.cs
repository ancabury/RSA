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
        Tuple<BigInteger, BigInteger> publicKey;
        BigInteger privateKey;
        public Form1() {
            InitializeComponent();
            p = new PrimeNumber(random);
            q = new PrimeNumber(random);

            tbxMessage.Text = "p = " + p.ToString();
            tbxEncryption.Text = "q = " + q.ToString();
            generateKeys();
        }

        private void generateKeys() {
            BigInteger n = p.prime * q.prime;
            BigInteger phi_n = (p.prime - 1) * (q.prime - 1);
            BigInteger e = selectE(phi_n);
            publicKey = new Tuple<BigInteger, BigInteger>(n, e);
            privateKey = modular_pow(e, -1, phi_n);

            tbxMessage.Text += "    e = " + e.ToString() + "    privateKey =" + privateKey;
            tbxEncryption.Text += "    phi(n) = " + phi_n.ToString() + "     publicKey = " + publicKey;
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

        BigInteger modular_pow(BigInteger value, int exponent, BigInteger modulus) {
            BigInteger result = 1;
            while (exponent > 0) {
                if (exponent % 2 == 1)
                    result = (result * value) % modulus;
                exponent = exponent >> 1;
                value = (value * value) % modulus;
            }
            return result;
        }
    }
}
