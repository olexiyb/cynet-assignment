using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace cynet_assignment
{
    /**
     * 
     * Next examples from documentation were used to develop the code
     * https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener?view=net-5.0
     * https://docs.microsoft.com/ru-ru/dotnet/api/system.io.filestream.lock?view=net-5.0
     */
    class CynetListener
    {
        static void Main()
        {
            TcpListener server = null;
            try
            {
                Int32 port = 7777;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);
                String tmpDir = Path.GetTempPath();
                if (!Directory.Exists(tmpDir))
                {
                    Directory.CreateDirectory(tmpDir);
                }
                Environment.CurrentDirectory = (tmpDir);
                String trafficFile = "traffic.txt";
                Console.WriteLine("Listen on http://{0}:{1} traffic output={2}/{3}", localAddr, port, tmpDir, trafficFile);

                // Start listening for client requests.
                server.Start();

                // We open or create a file stream for traffic data with exclusive rights
                using (FileStream fileStream = new FileStream(
                    trafficFile, FileMode.OpenOrCreate,
                    FileAccess.Write, FileShare.None))
                {
                    fileStream.Seek(0, SeekOrigin.End);

                    ProcessTraffic(server, fileStream);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        private static void ProcessTraffic(TcpListener server, FileStream fileStream)
        {
            // Buffer for reading data, we use the same as
            // https://referencesource.microsoft.com/#mscorlib/system/io/filestream.cs,396
            Byte[] bytes = new Byte[4096];
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            // Enter the listening loop.
            while (true)
            {
                // Perform a blocking call to accept requests.
                // You could also use server.AcceptSocket() here.
                TcpClient client = server.AcceptTcpClient();
                NetworkStream stream = client.GetStream();

                int i;
                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    fileStream.Write(bytes, 0, i);
                }

                fileStream.Write(encoding.GetBytes(Environment.NewLine));
                fileStream.Flush();

                // Shutdown and end connection
                client.Close();
            }
        }
    }
}