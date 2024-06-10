namespace QuizMaster
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                await StartQuiz();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
            finally
            {
                Console.WriteLine($"Thank you for completing this Quiz");
            }
        }

        static async Task StartQuiz()
        {
            Console.WriteLine("Welcome to the Quiz! You have 10 seconds to answer each question. Let's get started!");

            int score = 0;

            List<string> questions = new List<string>
                {
                    "What is the capital of France?",
                    "What is the capital of Germany?",
                    "What is the capital of Italy?",
                    "What is the capital of Spain?",
                    "What is the capital of Portugal?",
                    "What is the capital of the United Kingdom?",
                    "What is the capital of the United States?",
                    "What is the capital of Canada?",
                    "What is the capital of Mexico?",
                    "What is the capital of Brazil?"
                };

            List<string> answers = new List<string>
                {
                    "paris",
                    "berlin",
                    "rome",
                    "madrid",
                    "lisbon",
                    "london",
                    "washington",
                    "ottawa",
                    "mexico city",
                    "brasilia"
                };

            try
            {
                for (int i = 0; i < questions.Count; i++)
                {
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    Console.Clear();

                    Console.WriteLine(questions[i]);
                    string answer = await ReadLineWithTimeout(TimeSpan.FromSeconds(10));
                    if (answer == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Time's up!");
                        Console.ResetColor();
                    }
                    if (!IsValidInput(answer))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input!");
                        Console.ResetColor();
                        continue;
                    }
                    answer = answer.ToLower();
                    if (answer == answers[i])
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Correct!");
                        score++;
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Incorrect!");
                        Console.ResetColor();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                Console.WriteLine($"You scored {score} out of {questions.Count}");
            }
        }
        static async Task<string> ReadLineWithTimeout(TimeSpan timeout)
        {
            using (CancellationTokenSource cts = new CancellationTokenSource(timeout))
            {
                Task<string> readLineTask = Task.Run(() => Console.ReadLine(), cts.Token);
                Task delayTask = Task.Delay(timeout, cts.Token);

                Task completedTask = await Task.WhenAny(readLineTask, delayTask);

                if (completedTask == readLineTask)
                {
                    cts.Cancel();
                    return await readLineTask;
                }
                else
                {
                    return null;
                }
            }
        }
        static bool IsValidInput(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            foreach (char c in input)
            {
                if (!char.IsLetter(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
