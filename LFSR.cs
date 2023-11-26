using System;
using System.Numerics;
using System.Threading;

public class Lfsr
{
    private int state, polynomial, n;

    //inicjalizacja
    public Lfsr(int seed, int polynomial)
    {
        this.state = seed;
        this.polynomial = polynomial;
        this.n = GetBitLength(polynomial);
    }
    //metoda zwracająca pojedynczy bit z lsfr
    public int GetKeyBit()
    {
        //kopia wartości wielomianu i stanu, ponieważ będą przesuwane aby odczytać najmłodszy bit
        var polycp = polynomial;
        var statecp = state;
        int newBit = 0b0, firstBit = state & (1);
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
        return firstBit;
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
    static void Main()
    {
        int seed = 0b0010, polynomial = 0b1001;
        Lfsr lfsr = new Lfsr(seed, polynomial);

        while (true)
        {
            Console.Write(lfsr.GetKeyBit());
            Thread.Sleep(200);
        }
    }
}
