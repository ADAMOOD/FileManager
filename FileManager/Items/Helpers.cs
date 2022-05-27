using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager.Items
{
    internal class Helpers
    {
		public static void ClearCurrentConsoleArea(int top)
        {
            Console.SetCursorPosition(0, top);
            for (int i = 0; i < 50; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, 0);
            Console.SetCursorPosition(0, top);
        }
	}
}
