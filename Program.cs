using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SynchronousSocketListener
{

    // Incoming data from the client.  
    public static string data = null;

    public static void StartListening()
    {
        // Data buffer for incoming data.  
        byte[] bytes = new Byte[1024];
        byte[] msg = null;
        // Establish the local endpoint for the socket.  
        // Dns.GetHostName returns the name of the   
        // host running the application.  
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        
        //ipHostInfo.AddressList[1] = new IPAddress(Encoding.ASCII.GetBytes("178.251.45.244"));
        IPAddress ipAddress = ipHostInfo.AddressList[1];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 80); //11000 server ip

        //IPAddress y = ipHostInfo.AddressList[1];
        //Console.Write("Server IP: " + y);

        // Create a TCP/IP socket.  
        Socket listener = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to the local endpoint and   
        // listen for incoming connections.  
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);

            // Start listening for connections.  
            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
                // Program is suspended while waiting for an incoming connection.  
                Socket handler = listener.Accept();
                data = null;

                // An incoming connection needs to be processed.  
                while (true)
                {
                    int bytesRec = handler.Receive(bytes);
                    data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    Console.WriteLine("Client message received : {0}", data);
                    msg = Encoding.ASCII.GetBytes("Message successfully received by server.");


                    //Replying back to client
                    //handler.Send(msg); //Error occurs

                    //int bytesRec1 = handler.Receive(bytes);
                    //string response = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    //response = "Packet successfully received by the server.";
                    //byte[] resp = Encoding.ASCII.GetBytes(response);
                    //handler.Send(resp);
                }

                // Show the data on the console.  

                // Echo the data back to the client.  
                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nPress ENTER to continue...");
        Console.Read();

    }

    public static int Main(String[] args)
    {
        StartListening();
        
        return 0;
    }
}
