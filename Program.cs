using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;
using Rug.Osc;

namespace LightsControl;

public class Program
{
    static OscReceiver Receiver;
    static Thread Thread;

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddMudServices();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        Function.Init();
        Osc.Init("127.0.0.1", 7700, 9000);
        Osc.SendDmx(Enumerable.Range(1, 512).ToList(), 0);


        app.Run();
    }
}