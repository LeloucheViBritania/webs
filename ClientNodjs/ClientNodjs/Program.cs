using System;
using System.Net.Sockets;
using System.Text;

namespace clientn
{
    class Program
    {

        static string HOST = "192.168.70.51";
        static int PORT = 12450;

        static TcpClient client;


        static void OpenConnection()
        {
            if (client != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("---Connection is already open---");
            }
            else
            {
                try
                {
                    client = new TcpClient();
                    client.Connect(HOST, PORT);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Connected... ");

                }
                catch (Exception ex)
                {
                    client = null;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error : " + ex.Message);
                }
            }
        }


        static void CloseConnection()
        {
            if (client == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Connection is not open or already closed");
                return;
            }

            try
            {
                client.Close();

            }
            catch (Exception)
            {


            }
            finally
            {
                client = null;
            }


            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Connection closed succesful");

        }


        static void SendData(string data)
        {
            if (client == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Connection is already closed");
                return;
            }

            //send
            NetworkStream stream = client.GetStream();
            byte[] byteToSend = ASCIIEncoding.ASCII.GetBytes(data);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Sending : " + data);
            Console.WriteLine();
            stream.Write(byteToSend, 0, byteToSend.Length);

            //receive
            byte[] byteToRead = new byte[client.ReceiveBufferSize];
            int byteRead = stream.Read(byteToRead, 0, client.ReceiveBufferSize);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Server Response : " + Encoding.ASCII.GetString(byteToRead, 0, byteRead));

        }

        static void Main(string[] args)
        {
            string lineRead;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Etablissement de la liaison, veuillez patientez...");
            OpenConnection();
            do
            {


                Console.Write("Enter data to send : >>");
                string data = Console.ReadLine();
                SendData(data);
                Console.WriteLine();

                Console.Write("Voulez vous repondre (Oui ou Non): ");
                lineRead = Console.ReadLine();

                if (lineRead.ToLower() == "non")
                {
                    CloseConnection();
                    break;
                }

                /*switch (lineRead)
                {
                    case "1":
                        OpenConnection();
                        break;
                    case "2":
                        Console.Write("Enter data to send : >>");
                        string data = Console.ReadLine();
                        SendData(data);
                        break;
                    case "3":
                        CloseConnection();
                        break;
                }*/

            } while (lineRead.ToLower() != "non");
        }
    }
}