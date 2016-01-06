using System;
using System.Numerics;

namespace RSA {
    public class PrimeNumber {
        public BigInteger prime { get; set; }
        private const int SIZE = 5;
        Random rnd;
        public PrimeNumber(Random rnd) {
            this.rnd = rnd;
            generateNumber();
        }

        private void generateNumber() {
            BigInteger nr;

            do {
                nr = BigIntegerRandom.getRandom(SIZE, rnd);
            } while (!bigIntIsPrime(nr));
            prime = nr;
        }

        private bool bigIntIsPrime(BigInteger n) {
            bool isPrime = true;
            int k = 3;

            BigInteger previousN = n - 1, t = 0;
            int s = 1;
            while (t % 2 == 0) {
                t = previousN / BigInteger.Pow(2, s);
                s++;
            }

            Random rand = new Random();
            for (int i = 1; i <= k; i++) {
                BigInteger b = rand.Next(1, (int)previousN);
                BigInteger r = BigInteger.ModPow(b, t, n);
                if (r != 1 && r != previousN) {
                    int j = 1;
                    while (j <= s - 1 && r != previousN) {
                        r = BigInteger.ModPow(r, 2, n);
                        if (r == 1)
                            isPrime = false;
                        j++;
                    }
                    if (r != previousN)
                        isPrime = false;
                }
            }

            return isPrime;
        }

        public override string ToString() {
            return prime.ToString();
        }
    }
}
