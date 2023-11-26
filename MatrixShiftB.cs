using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
string Encrypt(string text, int[] key, int d)
{
    int length = text.Length;
    int numRows = (int)Math.Ceiling(length / 1.0 / d);
    char[,] matrix = new char[numRows, d];

    // Wstawienie znaków do tablicy
    int textIndex = 0;
    for (int row = 0; row < numRows; row++)
    {
        for (int col = 0; col < d; col++)
        {
            if (textIndex < length)
            {
                matrix[row, col] = text[textIndex++];
            }
            else
            {
                matrix[row, col] = '\x1B';
            }
        }
    }

    // Odczytanie znaków z tablicy w odpowiedniej kolejności
    string result = "";
    for (int col = 0; col < d; col++)
    {
        for (int row = 0; row < numRows; row++)
        {
            if (matrix[row, key[col] - 1] != '\x1B')
                result += matrix[row, key[col] - 1];
        }
    }

    return result;
}

string Decrypt(string text, int[] key, int d)
{
    int length = text.Length;
    int numRows = (int)Math.Ceiling(length / 1.0 / d);
    char[,] matrix = new char[numRows, d];

    // pusta tablica
    for (int row = 0; row < numRows; row++)
    {
        for (int col = 0; col < d; col++)
        {
            matrix[row, col] = '\x1B';
        }
    }

    // wypełnienie tablicy tekstem w odpowiedniej kolejności
    for (int i = 0; i < length; i++)
    {
        matrix[i / d, key[i % d] - 1] = text[i];
    }

    //odczytanie tekstu z tablicy
    string result = "";
    for (int row = 0; row < numRows; row++)
    {
        for (int col = 0; col < d; col++)
        {
            if (matrix[row, col] != '\x1B')
                result += matrix[row, col];
        }
    }

    return result;
}

int[] GetAlphabeticalOrder(string input)
{
    var list = new List<char>(input);
    var listSorted = new List<char>(input);
    listSorted.Sort();
    int[] result = new int[list.Count];
    for(int i=1; i<=list.Count; i++) {
        for(int j=0; j<list.Count; j++)
        {
            if (listSorted[i - 1] == list[j] && result[j] == 0)
            {
                result[j] = i;
                break;
            }
        }
    }
    return result;
}


char option = '0';
while (option != '\x1B') //klawisz esc wyłącza program
{
    Console.WriteLine(@"Kliknij żeby:
1 - zaszyfrować
2 - odszyfrować
3 - zaszyfrować i odszyfrować
Esc - wyjść z programu");
    option = Console.ReadKey().KeyChar;
    if (option == '1' || option == '2' || option == '3')
    {
        string text, keyText= "BCADA";
        int[] key = GetAlphabeticalOrder(keyText);
        int d = 5;
        Console.WriteLine("\n\nNapisz tekst: ");
        text = Console.ReadLine();
        Console.WriteLine("Klucz to BCADA");
        switch (option)
        {
            case '1':
                Console.WriteLine("Zaszyfrowany tekst: " + Encrypt(text, key, d));
                break;
            case '2':
                Console.WriteLine("Odszyfrowany tekst: " + Decrypt(text, key, d));
                break;
            case '3':
                Console.WriteLine("Zaszyfrowany tekst: " + Encrypt(text, key, d));
                Console.WriteLine("Odszyfrowany tekst: " + Decrypt(Encrypt(text, key, d), key, d));
                break;
        }
        Console.WriteLine();
    }
}