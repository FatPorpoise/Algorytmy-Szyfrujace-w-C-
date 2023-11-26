using System.Text;
string Encrypt(string text, int n)
{
    int length = text.Length;
    int num = Math.Min(n, length);
    char[,] fence = new char[num, length];
    bool isGoingDown = false;
    int row = 0, col = 0;

    // pusta tablica
    for (int i = 0; i < num; i++)
    {
        for (int j = 0; j < length; j++)
        {
            fence[i, j] = '\x1B';
        }
    }

    // wypełnienie tablicy
    for (int i = 0; i < length; i++)
    {
        if (row == 0 || row == num - 1)
            isGoingDown = !isGoingDown;

        fence[row, col++] = text[i];

        if (isGoingDown)
            row++;
        else
            row--;
    }

    // przepisanie z tablicy do stringa
    string result = "";
    for (int i = 0; i < num; i++)
    {
        for (int j = 0; j < length; j++)
        {
            if (fence[i, j] != '\x1B')
                result += fence[i, j];
        }
    }

    return result;
}

string Decrypt(string text, int n)
{
    int length = text.Length;
    int num = Math.Min(n, length);
    char[,] fence = new char[num, length];
    bool isGoingDown = false;
    int row = 0, col = 0;

    // pusta tablica
    for (int i = 0; i < num; i++)
    {
        for (int j = 0; j < length; j++)
        {
            fence[i, j] = '\x1B';
        }
    }

    // wyznaczenie miejsc na znaki
    for (int i = 0; i < length; i++)
    {
        if (row == 0 || row == num - 1)
            isGoingDown = !isGoingDown;

        fence[row, col++] = '*';

        if (isGoingDown)
            row++;
        else
            row--;
    }

    // Wypełnienie tablicy znakami
    int index = 0;
    for (int i = 0; i < num; i++)
    {
        for (int j = 0; j < length; j++)
        {
            if (fence[i, j] == '*' && index < length)
                fence[i, j] = text[index++];
        }
    }

    // Odczytanie tekstu z tablicy
    string result = "";
    row = col = 0;
    isGoingDown = false;
    for (int i = 0; i < length; i++)
    {
        if (row == 0 || row == num - 1)
            isGoingDown = !isGoingDown;

        result += fence[row, col++];

        if (isGoingDown)
            row++;
        else
            row--;
    }

    return result;
}


char option = '0';
while (option!= '\x1B') //klawisz esc wyłącza program
{
    Console.WriteLine(@"Kliknij żeby:
1 - zaszyfrować
2 - odszyfrować
3 - zaszyfrować i odszyfrować
Esc - wyjść z programu");
    option = Console.ReadKey().KeyChar;
    if (option == '1' || option == '2' || option == '3')
    {
        Console.WriteLine("\n\nPodaj wartość n: ");
        int n;
        string text;
        try
        {
            n = int.Parse(Console.ReadLine());
            Console.WriteLine("\nNapisz tekst: ");
            text = Console.ReadLine();
            Console.WriteLine();
            switch (option)
            {
                case '1':
                    Console.WriteLine("Zaszyfrowany tekst: "+Encrypt(text,n));
                    break;
                case '2':
                    Console.WriteLine("Odszyfrowany tekst: " + Decrypt(text, n));
                    break;
                case '3':
                    Console.WriteLine("Zaszyfrowany tekst: " + Encrypt(text, n));
                    Console.WriteLine("Odszyfrowany tekst: " + Decrypt(Encrypt(text, n), n));
                    break;
            }
        }
        catch
        {
            option = '0';
        }
        Console.WriteLine();
    }
}
