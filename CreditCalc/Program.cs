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
            Console.WriteLine("2 - Remover");
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
                    case 2:
                        Remover(today, FSdocPath);
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
                        Console.WriteLine("Digite o dia, mês e ano(dd/MM/yyyy)");
                        Console.Write(">");
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
            Console.WriteLine("Data: {0}", data);
            Console.WriteLine("Insira uma descrição");
            Console.Write(">");
            string descricao = Console.ReadLine();
            if (String.IsNullOrEmpty(descricao))
                descricao = "---";

            Console.Clear();
            Console.WriteLine("Data:              {0}", data);
            Console.WriteLine("Descrição:         {0}", descricao);
            Console.Write    ("Digite o valor:    R$ ");
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
            Console.WriteLine();
            Console.WriteLine("Adicionado!");
            Console.WriteLine();
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

        static void Remover(string today, string FSdocPath)
        {
            Console.Clear();
            var textLines = File.ReadAllLines(FSdocPath);
            List<string[]> dataList = new List<string[]>();
            int count = 0;
            string[] dataArray = new string[textLines.Length];
            foreach (var line in textLines)
            {

                dataArray[count] = line;
                count++;

            }
            count = 0;
            foreach(var line in dataArray)
            {
                count++;
                Console.WriteLine("ID {0} - {1}", count, dataArray[count-1]);
            }
            Console.WriteLine();
            Console.WriteLine("Digite o ID que deseja remover (Deixar em branco para voltar)");
            Console.Write(">");
            bool interruptor = true;
            string respbruto = Console.ReadLine();
            int removerresp = 0;
            while (interruptor) {
                if (String.IsNullOrEmpty(respbruto))
                    Start(today, FSdocPath);
                removerresp = Convert.ToInt32(respbruto);

                if (removerresp > count)
                {
                    Console.WriteLine("Sua resposta não é valida, digite um dos ID's acima (Deixar em branco para voltar)");
                    Console.Write(">");
                    respbruto = Console.ReadLine();
                }else
                {
                    interruptor = false;
                }
            }
            string[] dataArrayOrg = new string[dataArray.Length - 1];
            dataArray[removerresp - 1] = null;
            count = 0;
            for (int i = 0; i < dataArray.Length; i++)
            {
                if(dataArray[i] != null)
                {
                    if (i == count)
                    {
                        string dataArraystring = dataArray[i];
                        dataArrayOrg[i] = dataArraystring;
                        count++;
                    } else
                    {
                        string dataArraystring = dataArray[i];
                        dataArrayOrg[i - 1] = dataArraystring;
                        count++;
                    }
                    }
                    
            }
            File.WriteAllLines(FSdocPath, dataArrayOrg);
                Remover(today, FSdocPath);
        }

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
            Console.WriteLine();
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
                Console.WriteLine("CreditCalc.fsdoc Encontrado");
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
