namespace FitnessPlanner.Services;

public static class ConsoleInput
{
    public static string ReadRequired(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var value = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            Console.WriteLine("Значение не должно быть пустым.");
        }
    }

    public static string ReadOptional(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    public static int ReadInt(string prompt, int minimum, int maximum)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out var value) && value >= minimum && value <= maximum)
            {
                return value;
            }

            Console.WriteLine($"Введите целое число от {minimum} до {maximum}.");
        }
    }

    public static T ReadOption<T>(string prompt, IReadOnlyList<(T Value, string Label)> options)
    {
        Console.WriteLine(prompt);
        for (var index = 0; index < options.Count; index++)
        {
            Console.WriteLine($"{index + 1}. {options[index].Label}");
        }

        var choice = ReadInt("Ваш выбор: ", 1, options.Count);
        return options[choice - 1].Value;
    }
}
