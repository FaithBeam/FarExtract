using System;
using System.IO;
using System.Reflection;
using Sims.Far;

namespace FarExtract
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length != 1 || Path.GetExtension(args[0]) != ".far")
                {
                    DoExit();
                }

                var outDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException(),
                    Path.GetFileNameWithoutExtension(args[0]) ?? throw new InvalidOperationException());
                if (!Directory.Exists(outDir))
                {
                    Directory.CreateDirectory(outDir);
                }

                var far = new Far(args[0]);
                foreach (var me in far.Manifest.ManifestEntries)
                {
                    Console.WriteLine(Path.Combine(Path.GetFileNameWithoutExtension(args[0]), me.Filename));
                    far.Extract(me, outputDirectory: outDir);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.Error.WriteLine("Press Enter to exit");
                Console.ReadLine();
                throw;
            }
            finally
            {
                Console.WriteLine();
                Console.WriteLine("Finished. Press Enter to exit.");
                Console.ReadLine();
            }
        }

        private static void DoExit()
        {
            Console.Error.WriteLine("Please provide only the path to a .far file.");
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}