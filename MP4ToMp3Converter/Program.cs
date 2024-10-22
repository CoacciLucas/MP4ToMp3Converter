using NAudio.Lame;
using NAudio.Wave;

namespace MP4ToMp3Converter;

class Program
{
    [STAThread] // Necessário para usar OpenFileDialog e SaveFileDialog
    static void Main(string[] args)
    {
        var inputFilePaths = OpenFiles();  // Seleciona múltiplos arquivos de entrada MP4
        if (inputFilePaths == null || inputFilePaths.Length == 0)
        {
            Console.WriteLine("Nenhum arquivo foi selecionado.");
            return;
        }

        foreach (var inputFilePath in inputFilePaths)
        {
            var outputFilePath = Path.ChangeExtension(inputFilePath, ".mp3"); // Troca a extensão para .mp3
            ConvertMp4ToMp3(inputFilePath, outputFilePath);
        }
    }

    private static string[]? OpenFiles()
    {
        using var openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Video Files (*.mp4)|*.mp4";
        openFileDialog.Title = "Selecione arquivos MP4";
        openFileDialog.Multiselect = true; // Permite seleção de múltiplos arquivos

        return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileNames : null;
    }

    static void ConvertMp4ToMp3(string inputFile, string outputFile)
    {
        try
        {
            using (var reader = new MediaFoundationReader(inputFile))
            {
                using (var writer = new LameMP3FileWriter(outputFile, reader.WaveFormat, LAMEPreset.VBR_90))
                {
                    reader.CopyTo(writer);
                }
            }

            Console.WriteLine($"Conversão concluída: {outputFile}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao converter {inputFile}: {ex.Message}");
        }
    }
}