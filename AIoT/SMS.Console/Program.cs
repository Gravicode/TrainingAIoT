// See https://aka.ms/new-console-template for more information
using SMS.Console.Helper;
using Spectre.Console;

AzureDevice device=null;
AnsiConsole.MarkupLine("[green]{0}[/]", Markup.Escape("Selamat datang di aplikasi [SMS] IoT"));
// Synchronous
AnsiConsole.Status()
    .Start("Mulai proses...", ctx =>
    {
        // Simulate some work
        AnsiConsole.MarkupLine("Mulai koneksi ke Azure IoT...");
        Thread.Sleep(100);
        device = new AzureDevice("HostName=BmcIoTHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=1hyvQvLk7mWsQmBJ/MuTxWqiwr1nqtNnBs6gNqspta4=", "myFirstDevice");
        // Update the status and spinner
        ctx.Status("Konesi berhasil");
        ctx.Spinner(Spinner.Known.Star);
        ctx.SpinnerStyle(Style.Parse("green"));
        Thread.Sleep(2000);
       
    });

while (true && device!=null)
{
    var message = AnsiConsole.Ask<string>("Ketik [green]pesan[/] yang mau dikirim! [yellow]('q' untuk keluar)[/]");
    if (message == "q") break;
    var hasil = await device.SendMessageToDevice(message);
    if (hasil)
    {
        AnsiConsole.MarkupLine($"pesan [green]berhasil[/] terkirim {Emoji.Known.ThumbsUp}");
    }
    else
    {
        AnsiConsole.MarkupLine($"pesan [red]gagal[/] terkirim {Emoji.Known.Alien}");
    }
    var rule = new Rule("[red]PESAN BARU[/]");
    AnsiConsole.Write(rule);
}

AnsiConsole.MarkupLine("Terima kasih, sampai jumpa. :person_running:");
System.Console.ReadLine();
