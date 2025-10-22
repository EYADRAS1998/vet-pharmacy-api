using MudBlazor;

namespace BlazorApp.Themes
{
    public class CustomTheme : MudTheme
    {
        public CustomTheme()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = "#1976d2", // Blue
                Secondary = "#9c27b0", // Purple
                Background = "#f5f5f5", // Light grey
                AppbarBackground = "#1976d2",
                AppbarText = "#ffffff",
                DrawerBackground = "#ffffff",
                DrawerText = "#000000",
                Surface = "#ffffff",
                TextPrimary = "#000000",
                TextSecondary = "#555555"
            };

            PaletteDark = new PaletteDark()
            {
                Primary = "#90caf9",
                Secondary = "#ce93d8",
                Background = "#121212",
                Surface = "#1e1e1e",
                AppbarBackground = "#333333",
                DrawerBackground = "#1e1e1e",
                TextPrimary = "#ffffff",
                TextSecondary = "#cccccc"
            };

        }
    }
}
