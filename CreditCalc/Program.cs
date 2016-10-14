using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace CreditCalc
{
    class Program
    {

        static void Start(string today, string FSdocPath)
        {
            Console.Clear();
            Console.WriteLine("Escolha uma operação");
            Console.WriteLine("1 - Adicionar");
            Console.WriteLine("2 - Remover (NOT WORKING)");
            Console.WriteLine("3 - Consultar");
            Console.Write(">");
            int startresp;
            string nulltest = "undefined";
            try
            {
                startresp = Convert.ToInt32(Console.ReadLine());

            }
            catch (Exception e)
            {
                Console.WriteLine("Invalido! Tenha certeza de digitar corretamente");
                Console.WriteLine("Enter para continuar");
                Console.ReadKey();
                Start(today, FSdocPath);
                return;
            }
            bool interruptor = true;

            while (interruptor)
            {
                switch (startresp)
                {
                    case 1:
                        Adicionar(today, FSdocPath);
                        interruptor = false;
                        break;
                    case 3:
                        Consultar(today, FSdocPath);
                        interruptor = false;
                        break;
                    default:
                        Console.WriteLine("Sua resposta não é valida, digite uma das opções acima:");
                        Console.Write(">");
                        nulltest = Console.ReadLine();
                        if (String.IsNullOrEmpty(nulltest))
                        {
                            startresp = 50;
                        }
                        else
                        {
                            startresp = Convert.ToInt32(nulltest);
                        }

                        break;
                }
            }
        }

        static void Adicionar(string today, string FSdocPath)
        {
            Console.Clear();
            Console.WriteLine("Escolha a data:");
            Console.WriteLine("1 - Hoje");
            Console.WriteLine("2 - Outro");
            Console.Write(">");
            int respdata;
            string data = "undefined";
            try
            {
                respdata = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalido! Tenha certeza de digitar corretamente");
                Console.WriteLine("Enter para continuar");
                Console.ReadKey();
                Adicionar(today, FSdocPath);
                return;
            }

            string nulltest = "undefined";
            bool interruptor = true;
            while (interruptor)
            {
                switch (respdata)
                {
                    case 1:
                        data = today;
                        interruptor = false;
                        break;
                    case 2:
                        Console.Clear();
                        Console.Write("Digite o dia, mês e ano(dd/MM/yyyy): ");
                        data = Console.ReadLine();
                        if (String.IsNullOrEmpty(data))
                            data = DateTime.Now.ToString("dd/MM/yyyy");
                        interruptor = false;
                        break;
                    default:
                        Console.WriteLine("Sua resposta não é valida, digite uma das opções acima:");
                        Console.Write(">");
                        nulltest = Console.ReadLine();
                        if (String.IsNullOrEmpty(nulltest))
                        {
                            respdata = 50;
                        }
                        else
                        {
                            respdata = Convert.ToInt32(nulltest);
                        }
                        break;
                }
            }

            Console.Clear();
            Console.WriteLine("Insira uma descrição");
            Console.Write(">");
            string descricao = Console.ReadLine();
            if (String.IsNullOrEmpty(descricao))
                descricao = "---";

            Console.Clear();
            Console.WriteLine("Digite o valor:");
            Console.Write(">");
            string valor = Console.ReadLine();
            valor = valor.Replace(',', '.');
            if (String.IsNullOrEmpty(valor))
                valor = "0";

            Dictionary<string, string> dicio = new Dictionary<string, string>();
            dicio.Add(data, valor);

            foreach (var entry in dicio)
            {
                string entrykey = entry.Key + "," + descricao + "," + entry.Value;
                File.AppendAllText(FSdocPath, entrykey + Environment.NewLine);
            }
            Console.WriteLine("Adicionado!");
            Console.WriteLine("Realizar outra operação? (S/N)");
            string respagain = Console.ReadLine().ToUpper();
            if (respagain == "S")
            {
                Adicionar(today, FSdocPath);
            } else
            {
                Start(today, FSdocPath);
            }

        }

        /*   static void Remover()
           {
               var arraymeu = File.ReadAllLines(FSdocPath);
               for (var i = 0; i < arraymeu.Length; i += 2)
               {
                   dicio.Add(arraymeu[i + 1], arraymeu[i]);
               }

           } */

        static void Consultar(string today, string FSdocPath)
        {
            Console.Clear();
            int count = 0;
            var textLines = File.ReadAllLines(FSdocPath);
            string[] dataArray = new string[textLines.Length];
            foreach (var line in textLines)
            {
                for (int i = 0; i < dataArray.Length; i++)
                {
                        dataArray = line.Split(',');

                        Console.Write(dataArray[i]);

                        Console.Write("      ");
                        count++;
                    if ((count % 3) == 0) {
                        Console.WriteLine();
                    }
                    }

            }
            Console.WriteLine("Enter: Voltar");
            Console.ReadKey();
            Start(today, FSdocPath);
        }


        static void Main(string[] args)
        {
            string today = DateTime.Now.ToString("dd/MM/yyyy");
            string month = DateTime.Now.Month.ToString();
            Console.WriteLine(today);
            string FSdocPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"CreditCalc/CreditCalc.fsdoc");
            FileStream FSdoc;
            if (File.Exists(FSdocPath))
            {
                FSdoc = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"CreditCalc/CreditCalc.fsdoc"), FileMode.Open, FileAccess.ReadWrite);
                Console.WriteLine("'CreditCalc.fsdoc' Encontrado");
            }
            else
            {
                Console.WriteLine("Arquivo 'CreditCalc.fsdoc' não foi encontrado");
                Console.WriteLine("Criando um novo arquivo 'CreditCalc.fsdoc'");
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CreditCalc"));
                using (File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"CreditCalc/CreditCalc.fsdoc"))) { }
                Console.WriteLine("...");
                FSdoc = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"CreditCalc/CreditCalc.fsdoc"), FileMode.Open, FileAccess.ReadWrite);
                Console.WriteLine("Feito!");
            }
            FSdoc.Close();
            Console.WriteLine("Enter para continuar...");
            Console.ReadKey();
            Console.Clear();

            Start(today, FSdocPath);


            }
        }
    }
