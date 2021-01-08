# Kommunikation

Die Kommunikation zwischen Weboberfläche, dem Controller und den Nodes wird fortfolgend beschrieben.
![](res/ControlflowOverview.svg)

## Gebiete
Die Kommunikation wird in 4 Teilkommunikation aufgeteilt:
- Controlflow Sychronisation
- Media Sychronisation
- Edit Sychronisation
- Timer Sychronisation


## Controlflow
Der Controlflow kümmert sich um das Verbinden der Nodes, die Zustandskontrolle der Nodes und das Liveaktivieren der einzelnen Layers.

So kann eine Node 3 Zustände Annehmen:
- Disconnected
- Edit
- Live

![](res/ControlflowClassdiagram.svg)
Der `ControlfowService` ist die zentrale Klasse die die Kommunikation steuert.
So filter der Service die passenden Informationen fü die Nodes, das jede Node nur die eigenen Nodeinformationn kennt. 

Dabei kennt der `ControlfowService` weder den Hub noch den GrpcServer.
Der Hub reportet über Methoden dem `ControlfowService` die Befehle, der `ControlfowService` gibt diese Informationen dann über einen asychronen Stream weiter, indem der Grpc Server die `IAsyncEnumerable` Streams über Methoden bei bedarf anfordert.

``` CSharp
public interface IControlflowService {
    Task ReportSomeDataAsync(SomeData payload); //Vielleicht aus synchron
    IAsyncEnumerable<SomeData> GetSomeDataStreamAsync(string nodename); //Alernative eine Predicte<SomeArgs> als Filter Function,
}
```

## Media
//Todo

## Edit
//Todo

## Timer
[Siehe Renderer](renderer.md)