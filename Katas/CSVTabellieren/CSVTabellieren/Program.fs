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

        let rightPaddedContent = 
            lines
            |> Seq.map (Seq.zip columnsWidth)
            |> Seq.map (Seq.map (fun x -> 
                                     let totalWidth,text = x 
                                     text.PadRight(totalWidth) + "|") )
        let header = 
            Seq.take 1 rightPaddedContent
            |> Seq.concat |> String.concat ""

        let separator = 
            columnsWidth 
            |> Array.map (fun x -> sprintf "%s+" (String.replicate x "-"))
            |> String.concat "" 

        let lines =
            Seq.skip 1 rightPaddedContent
            |> Seq.map (fun x -> Seq.append x [Environment.NewLine])
            |> Seq.concat |> String.concat ""
        
        String.concat Environment.NewLine [header
                                           separator
                                           lines]
                      
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