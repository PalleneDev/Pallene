using Avalonia;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;

namespace Pallene.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string Greeting { get; } = "Welcome to Avalonia!";

        public string Version { get; } = $"v{Assembly.GetExecutingAssembly().GetName().Version?.ToString()}" ?? "Unknown Version";
    }
}
