# cynet-assignment

To complete the [Assignment]( https://onedrive.live.com/view.aspx?cid=3028dbdcc7b0dadf&page=view&resid=3028DBDCC7B0DADF!489616&parId=3028DBDCC7B0DADF!489615&authkey=!AO3hW2VXQm8NB5k&app=Word)
I have had to research on next topics

## Tcp listeners

[StackOverflow discussion](https://stackoverflow.com/questions/47525939/multi-threaded-tcp-listner-server-in-c-sharp) pointed me to several candidates

 * [DotNetty used on Azure IoT services](https://github.com/Azure/DotNetty/tree/dev/examples/WebSockets.Server)
 * [System.IO.Pipelines](https://devblogs.microsoft.com/dotnet/system-io-pipelines-high-performance-io-in-net/).
 * [Akka.Streams TCP stream](https://getakka.net/articles/streams/workingwithstreamingio.html).
 * [TcpListener](https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener?view=net-5.0)
 
 
 I have picked the simplest sync implementation using TcpListener due to requirement to store traffic into single file.

## File output

 * [FileStream](https://docs.microsoft.com/ru-ru/dotnet/api/system.io.filestream?view=net-5.0)
 * [StreamWriter](https://docs.microsoft.com/ru-ru/dotnet/api/system.io.streamwriter?view=net-5.0)

I have picked `FileStream` to use file locking `FileShare.None` to block write to this file from other applications, besides `StreamWriter` is actually based on FileStream

Sync implementation usually is faster as we need to lock/unlock the output file in async implementation and File.IO
operations are very expensive and it should be cheaper to open exclusively `traffic.txt` and output traffic using sync Write() method.
 
From my experience to satisfy

`Make Sure that we can be persistent.`
`Create a smart processing operation that will lower the chances of having bottlenecks.`

I would ask to change requirement to use a file to keep the traffic information
The most bullet proof implementation would be [scylla database](https://github.com/datastax/csharp-driver/), 
that has replication mechanism and load balancing.

Another solution would be use of [zeromq](https://github.com/zeromq/netmq)

Here is very good article on how to implement reliable client/server solution for simple [Request-Reply](https://zguide.zeromq.org/docs/chapter4/) patterns with code examples.
 
 
 

