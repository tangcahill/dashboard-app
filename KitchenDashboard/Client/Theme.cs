// Shared/Theme.cs
using MudBlazor;

namespace KitchenDashboard.Shared
{
    // Client/Shared/Theme.cs
    using MudBlazor;

    public static class Theme
    {
        public static MudTheme Current { get; } = new MudTheme()
        {
            // 1. Use PaletteLight for light‐mode colors
            PaletteLight = new PaletteLight
            {
                Primary = "#1976d2",
                Secondary = "#ff5722",
                Background = "#f0f2f5",
                Surface = "#ffffff",
                AppbarBackground = "#1976d2",
                DrawerBackground = "#ffffff",
                TextPrimary = "#212121",
                TextSecondary = "#424242"
            },

            // 2. Typography must use string values for FontWeight & LineHeight
            Typography = new Typography()
            {
                Default = new DefaultTypography()
                {
                    FontFamily = new[] { "Ubuntu", "Montserrat", "Roboto", "sans-serif" },
                    FontSize = "1rem",
                    LineHeight = "1.6"
                },
                H4 = new H4Typography() { FontSize = "2rem", FontWeight = "600" },
                H5 = new H5Typography() { FontSize = "1.75rem", FontWeight = "500" },
                H6 = new H6Typography() { FontSize = "1.5rem", FontWeight = "500" },
                Body1 = new Body1Typography() { FontSize = "1.125rem" },
                Button = new ButtonTypography() { FontSize = "1rem", TextTransform = "none" }
            }
        };
    }

}
