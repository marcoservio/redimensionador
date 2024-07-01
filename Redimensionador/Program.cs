using Redimensionador;

using System.Drawing;

Console.WriteLine("Redimencionador\n");

Console.Write("Digite a altura para redimencionar a imagem: ");
_ = int.TryParse(Console.ReadLine(), out int novaAltura);

if(novaAltura == 0)
    novaAltura = 200;

var thread = new Thread(Redimencionar);
thread.Start();

Console.Read();

void Redimencionar()
{
    CriarDiretorios();

    FileStream fileStream;
    FileInfo fileInfo;

    while (true)
    {
        //var novaAltura = 200;
        var arquivosEntrada = Directory.EnumerateFiles(nameof(Diretorios.Arquivos_Entrada));

        foreach (var arquivo in arquivosEntrada)
        {
            fileStream = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            fileInfo = new FileInfo(arquivo);

            var caminho = Path.Combine(Environment.CurrentDirectory,
                nameof(Diretorios.Arquivos_Redimencionados), $"{DateTime.Now.Millisecond}_{fileInfo.Name}");

            Redimensionador(Image.FromStream(fileStream), novaAltura, caminho);

            fileStream.Close();

            var caminhoFinalizado = Path.Combine(Environment.CurrentDirectory, nameof(Diretorios.Arquivos_Finalizados),
                    fileInfo.Name);

            fileInfo.MoveTo(caminhoFinalizado);
        }

        Thread.Sleep(TimeSpan.FromSeconds(5));
    }
}

static void CriarDiretorios()
{
    if (!Directory.Exists(nameof(Diretorios.Arquivos_Entrada)))
        Directory.CreateDirectory(nameof(Diretorios.Arquivos_Entrada));

    if (!Directory.Exists(nameof(Diretorios.Arquivos_Redimencionados)))
        Directory.CreateDirectory(nameof(Diretorios.Arquivos_Redimencionados));

    if (!Directory.Exists(nameof(Diretorios.Arquivos_Finalizados)))
        Directory.CreateDirectory(nameof(Diretorios.Arquivos_Finalizados));
}

static void Redimensionador(Image imagem, int altura, string caminho)
{
    double ratio = (double)altura / imagem.Height;
    int novaLargura = (int)(imagem.Width * ratio);
    int novaAltura = (int)(imagem.Height * ratio);

    var novaImagem = new Bitmap(novaLargura, novaAltura);

    using var g = Graphics.FromImage(novaImagem);

    g.DrawImage(imagem, 0, 0, novaLargura, novaAltura);

    novaImagem.Save(caminho);
    imagem.Dispose();
}