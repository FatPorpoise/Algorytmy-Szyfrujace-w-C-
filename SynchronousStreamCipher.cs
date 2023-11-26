using System.Numerics;
using System;
using System.Threading;

public class LfsrCipher
{
    private int state, polynomial, n;

    //inicjalizacja
    public LfsrCipher(int seed, int polynomial)
    {
        this.state = seed;
        this.polynomial = polynomial;
        this.n = GetBitLength(polynomial);
    }
    //metoda zwracająca klucz o żądanej długości bitów
    public int GetKey(int length)
    {
        int key = 0;
        for (int i = 0; i < length; i++)
        {
            key = (key << 1) | GetKeyBit();
        }
        return key;
    }
    //metoda zwracająca pojedynczy bit z lsfr
    public int GetKeyBit()
    {
        //kopia wartości wielomianu i stanu, ponieważ będą przesuwane aby odczytać najmłodszy bit
        var polycp = polynomial;
        var statecp = state;
        int newBit = 0b0;
        //int firstBit = state & (1);
        //przejście przez wszystkie bity wielomianu, wykonanie xor na odpowiednich bitach stanu w celu uzyskania nowego najstarszego bitu
        while (polycp != 0)
        {
            if ((polycp & (1)) == 1)
            {
                newBit ^= (statecp & (1));
            }
            polycp >>= 1;
            statecp >>= 1;
        }
        //wstawienie wyliczonego bitu jako najstarszy bit i zwrócenie najmłodszego bitu oraz przesunięcie w lewo
        state = (state | (newBit << n)) >> 1;
        //return firstBit;
        return newBit;
    }
    //metoda licząca długość wielomianu w celu wstawienia nowo wyliczanych bitów w odpowiednie miejsce
    public static int GetBitLength(int number)
    {
        int bitLength = 0;

        while (number != 0)
        {
            number >>= 1;
            bitLength++;
        }

        return bitLength;
    }
}



class Program
{
    public static string IntToBinaryString(int number, int n)
    {
        string binary = Convert.ToString(number, 2);
        binary = binary.PadLeft(n, '0');
        return binary;
    }
    static void Main()
    {
        //int bits = 0b11101001110011001100110;
        int seed = 0b11111, polynomial = 0b11011, n = 16;
        LfsrCipher lfsrCipher = new LfsrCipher(seed, polynomial);

        //int key = lfsrCipher.GetKey(n);
        //int bitsCipher = bits ^ key;

        //Console.WriteLine("Ciąg bitów:\t"+IntToBinaryString(bits, n));
        //Console.WriteLine("Klucz:\t\t"+IntToBinaryString(key, n));
        //Console.WriteLine("Szyfr:\t\t" + IntToBinaryString(bitsCipher, n));

        //Szyfrowanie
        string input = "Hello, World!", inputBinary = "", resultBinary = "", keyBinary="";
        char[] charArray = input.ToCharArray();
        int key;
        // Perform bitwise XOR on each character
        for (int i = 0; i < charArray.Length; i++)
        {
            key = lfsrCipher.GetKey(n);
            keyBinary += IntToBinaryString(key, n);
            inputBinary+= IntToBinaryString(charArray[i], n);
            charArray[i] = (char)(charArray[i] ^ key);
            resultBinary += IntToBinaryString(charArray[i], n);
        }

        // Convert the modified character array back to a string
        string result = new string(charArray);

        Console.WriteLine("input:         " + input);
        Console.WriteLine("key:           " + keyBinary);
        Console.WriteLine("input binary:  " + inputBinary);
        Console.WriteLine("result binary: " + resultBinary);
        Console.WriteLine("result:        " + result);

        //Deszyfrowanie
        lfsrCipher = new LfsrCipher(seed, polynomial);
        Console.WriteLine();

        input = result; inputBinary = ""; resultBinary = ""; keyBinary = "";
        charArray = input.ToCharArray();
        // Perform bitwise XOR on each character
        for (int i = 0; i < charArray.Length; i++)
        {
            key = lfsrCipher.GetKey(n);
            keyBinary += IntToBinaryString(key, n);
            inputBinary += IntToBinaryString(charArray[i], n);
            charArray[i] = (char)(charArray[i] ^ key);
            resultBinary += IntToBinaryString(charArray[i], n);
        }

        // Convert the modified character array back to a string
        result = new string(charArray);

        Console.WriteLine("input:         " + input);
        Console.WriteLine("key:           " + keyBinary);
        Console.WriteLine("input binary:  " + inputBinary);
        Console.WriteLine("result binary: " + resultBinary);
        Console.WriteLine("result:        " + result);
    }
}