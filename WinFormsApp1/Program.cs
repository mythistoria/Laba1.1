namespace WinFormsApp1
{
    internal static class Program
    {
        /// <summary>
        /// Точка входа WinForms-приложения: настройка и запуск главной формы.
        /// </summary>
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}