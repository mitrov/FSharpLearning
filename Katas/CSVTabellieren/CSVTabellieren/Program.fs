// http://ccd-school.de/coding-dojo/function-katas/csv-tabellieren/

open System
open System.IO

module CSVParser =

    let internal splitLine(line : string) =
        line.Split ','

    let parse (input : string) =
        input.Split ([|'\n'|], StringSplitOptions.None)
        |> Seq.map splitLine

    let getColumnsWidth lines =
        lines
        |> Seq.map (Array.map String.length)
        |> Seq.reduce (Array.map2 max)

module CSVFormater =

    let tabulate (lines : seq<array<string>>) (columnsWidth : array<int>) =

        let padRightTable =
            let padRightLine line =                          
                let padRight totalWidth (text: string) = 
                    text.PadRight totalWidth + "|"
                Seq.map2 padRight columnsWidth line                
            Seq.map padRightLine lines

        let createLine =
            String.concat ""

        let tableHeader = 
            Seq.head padRightTable

        let tableBody =
            Seq.tail padRightTable          

        let separator =             
            columnsWidth 
            |> Seq.map (fun width -> sprintf "%s+" (String.replicate width "-"))
                    
        seq {
            yield tableHeader
            yield separator
            yield! tableBody
        }
        |> Seq.map createLine
        |> String.concat Environment.NewLine
                      
[<EntryPoint>]
let main argv = 
    let inputString = File.ReadAllText("..\..\Fielding.csv")
    let csv = CSVParser.parse(inputString)
    let columnsWidth = CSVParser.getColumnsWidth csv 
    let output = CSVFormater.tabulate csv columnsWidth
    File.WriteAllText("..\..\Fielding-new.csv",output)
    Console.Write(output);
    Console.ReadKey() |> ignore
    0