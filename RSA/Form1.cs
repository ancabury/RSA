using System.Windows.Forms;
using System.Numerics;
using System;
using System.Text;

namespace RSA {
    public partial class Form1 : Form {
        string alphabet = " abcdefghijklmnopqrstuvwxyz";
        PrimeNumber p;
        PrimeNumber q;
        Random random = new Random();
        Tuple<BigInteger, BigInteger> publicKey;
        BigInteger privateKey;
        const int K = 2;
        const int L = 3;
        public Form1() {
            InitializeComponent();
            p = new PrimeNumber(random);
            q = new PrimeNumber(random);
            generateKeys();
        }

        private void generateKeys() {
            BigInteger n = p.prime * q.prime;
            while(BigInteger.Pow(27, K) > n || BigInteger.Pow(27, L) < n) {
                p = new PrimeNumber(random);
                q = new PrimeNumber(random);
                n = p.prime * q.prime;
            }
            BigInteger phi_n = (p.prime - 1) * (q.prime - 1);
            BigInteger e = selectE(phi_n);
            publicKey = new Tuple<BigInteger, BigInteger>(n, e);
            privateKey = mul_inv(e, phi_n);
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
        
        BigInteger mul_inv(BigInteger e, BigInteger n) {
            BigInteger b0 = n, t, q;
            BigInteger x0 = 0, x1 = 1;
            if (n == 1)
                return 1;
            while (e > 1) {
                q = e / n;
                t = n; n = e % n; e = t;
                t = x0; x0 = x1 - q * x0; x1 = t;
            }
            if (x1 < 0)
                x1 += b0;
            return x1;
        }

        private void btnEncrypt_Click(object sender, EventArgs e) {
            if (tbxMessage.Text.ToString() == string.Empty)
                MessageBox.Show("Please insert a message", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else {
                string c;
                if((c = metaChars(tbxMessage.Text)) != null) {
                    MessageBox.Show("Unrecognized character: " + c, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string message = addSpaces(tbxMessage.Text);
                BigInteger[] numerical_eq = new BigInteger[message.Length / K];
                int index = 0;
                for(int i =0;i<message.Length;i = i + K) {
                    numerical_eq[index++] = SUM(message.Substring(i), K);
                }
                string cipher = getText(encrypt(numerical_eq), K);
                tbxEncryption.Text = cipher;
            }
        }

        private string metaChars(string msg) {
            foreach (char c in msg)
                if (!alphabet.Contains(c.ToString()))
                    return c.ToString();
            return null;
        }

        private string getText(BigInteger[] arr, int size) {
            StringBuilder text = new StringBuilder();
            foreach (BigInteger nr in arr) {
                BigInteger num = nr;
                for (int i = size; i >= 0; i--) {
                    BigInteger x = num / Convert.ToInt32(Math.Pow(27, i));
                    if (x == 0 && size == L) //decryption
                        continue;
                    num -= Convert.ToInt32(Math.Pow(27, i)) * x;
                    text.Append(alphabet[(int) x]);
                }
            }
            return text.ToString();
        }

        private BigInteger[] encrypt(BigInteger[] arr) {
            for (int i = 0; i < arr.Length; i++)
                arr[i] = BigInteger.ModPow(arr[i], publicKey.Item2, publicKey.Item1);
            return arr;
        }

        private BigInteger SUM(string msg, int size) {
            BigInteger sum = 0;
            for (int i = size - 1; i >= 0; i--)
                sum += alphabet.IndexOf(msg[size - i - 1]) * BigInteger.Pow(27, i);
            return sum;
        }

        private string addSpaces(string msg) {
            if (msg.Length % K == 0)
                return msg;
            else {
                StringBuilder str = new StringBuilder(msg);
                while(str.Length % K != 0) {
                    str.Append(" ");
                }
                return str.ToString();
            }
        }

        private void tbxMessage_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) 
                btnEncrypt_Click(sender, e);
        }

        private void btnDecrypt_Click(object sender, EventArgs e) {
            if (tbxEncryption.Text.ToString() == string.Empty)
                MessageBox.Show("Please insert a cipher", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else {
                string c;
                if ((c = metaChars(tbxEncryption.Text)) != null) {
                    MessageBox.Show("Unrecognized character: " + c, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                BigInteger[] numerical_eq = new BigInteger[tbxEncryption.Text.Length / L];
                int index = 0;
                for (int i = 0; i < tbxEncryption.Text.Length; i = i + L) {
                    numerical_eq[index++] = SUM(tbxEncryption.Text.Substring(i), L);
                }
                string message = getText(decrypt(numerical_eq), L);
                tbxMessage.Text = message;
            }
        }

        private BigInteger[] decrypt(BigInteger[] arr) {
            for (int i = 0; i < arr.Length; i++)
                arr[i] = BigInteger.ModPow(arr[i], privateKey, publicKey.Item1);
            return arr;
        }

        private void tbxEncryption_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter)
                btnDecrypt_Click(sender, e);
        }

        private void tbxEncryption_MouseClick(object sender, MouseEventArgs e) {
            tbxMessage.Clear();
        }

        private void tbxMessage_MouseClick(object sender, MouseEventArgs e) {
            tbxEncryption.Clear();
        }
    }
}
