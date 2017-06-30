//___________________________________________________________________
// http://ccd-school.de/coding-dojo/function-katas/csv-tabellieren/
// __________________________________________________________________

open System

let inputString = "Name;Strasse;Ort;Alter\r\nPeter Pan;Am Hang 5;12345 Einsam;42\r\nMaria Schmitz;Kölner Straße 45;50123 Köln;43\r\nPaul Meier;Münchener Weg 1;87654 München;65"

let spalten (zeile : string) =
    zeile.Split ';'

let zeilen =
    inputString.Split ([|Environment.NewLine|], StringSplitOptions.None)
    |> Seq.map spalten

let spaltenBreiten = 
    zeilen 
    |> Seq.map (Seq.map String.length) 
    |> Seq.reduce (Seq.map2 max)

let padRightZeilen=
    zeilen
    |> Seq.map (Seq.zip spaltenBreiten)
    |> Seq.map (Seq.map (fun x -> 
                             let totalWidth,text = x 
                             text.PadRight(totalWidth)))

let printZeile (zeile : seq<string>) =
    zeile
    |> Seq.iter (fun x -> printf "%s|" x)

let printUeberschrift =
    Seq.take 1 padRightZeilen 
    |> Seq.iter (printZeile)
    printfn ""
    spaltenBreiten
    |> Seq.iter (fun x -> printf "%s+" (String.replicate x "-"))
    printfn ""

let printRest =
    Seq.skip 1 padRightZeilen
    |> Seq.iter (fun x -> printZeile x
                          printfn "")
[<EntryPoint>]
let main argv = 
    printUeberschrift
    printRest
    Console.ReadKey() |> ignore
    0 // return an integer exit code