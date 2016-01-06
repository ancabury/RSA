using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RSA {
    public class BigIntegerRandom {
        public static BigInteger getRandom(int size, Random rnd) {
            string s = string.Empty;
            for (int i = 0; i < size; i++)
                s = string.Concat(s, rnd.Next(10).ToString());
            return BigInteger.Parse(s);
        }
    }
}
