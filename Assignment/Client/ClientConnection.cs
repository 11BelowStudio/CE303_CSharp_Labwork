using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Sockets;
using Shared;
using System.Text.Json;

namespace Client
{
    class ClientConnection : IDisposable
    {

        private readonly StreamReader reader;
        private readonly StreamWriter writer;

        private readonly JsonSerializerOptions jsonOptions;

        //makes this ClientConnection, connected to the specified hostname and the specified port.
        internal ClientConnection(string hostname, int port)
        {
            TcpClient theClient = new TcpClient(hostname, port);

            NetworkStream stream = theClient.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);

            jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                PropertyNameCaseInsensitive = true,
            };
        }

        //sends connect request + recieves response
        internal ConnectResponse Connect()
        {
            writer.WriteLine(JsonSerializer.Serialize<Request>(new Request(Request.CONNECT_REQUEST), jsonOptions));
            writer.Flush();
            ConnectResponse r = JsonSerializer.Deserialize<ConnectResponse>(reader.ReadLine(), jsonOptions);
            return r;
        }

        //sends info request + recieves response
        internal GenericResponse GetInfo()
        {
            writer.WriteLine(JsonSerializer.Serialize<Request>(new Request(Request.INFO_REQUEST), jsonOptions));
            writer.Flush();
            return JsonSerializer.Deserialize<GenericResponse>(reader.ReadLine(), jsonOptions);
        }

        //sends trade request + recieves response
        internal GiveStockResponse GiveStock(string recipient)
        {
            writer.WriteLine(JsonSerializer.Serialize<Request>(new Request(Request.TRADE_REQUEST, recipient), jsonOptions));
            writer.Flush();
            return JsonSerializer.Deserialize<GiveStockResponse>(reader.ReadLine(), jsonOptions);
        }

        //sends disconnect request + recieves response
        internal GenericResponse Disconnect()
        {
            writer.WriteLine(JsonSerializer.Serialize<Request>(new Request(Request.DISCONNECT_REQUEST), jsonOptions));
            writer.Flush();
            return JsonSerializer.Deserialize<GenericResponse>(reader.ReadLine(), jsonOptions);
        }

        //disposes of the streams
        public void Dispose()
        {
            Disconnect();
            reader.Close();
            writer.Close();
        }
    }
}
