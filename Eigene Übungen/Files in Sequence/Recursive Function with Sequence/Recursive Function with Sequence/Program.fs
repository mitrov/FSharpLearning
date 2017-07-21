open System.IO

let enumerateFiles path =
    try
        Directory.EnumerateFiles path
    with _ -> Seq.empty

let enumerateDirectories path =
    try
        Directory.EnumerateDirectories path
    with _ -> Seq.empty

let rec allFiles paths =
    if Seq.isEmpty paths then Seq.empty else
        seq { yield! paths |> Seq.collect enumerateFiles
              yield! paths |> Seq.collect enumerateDirectories |> allFiles }

[<EntryPoint>]
let main argv = 
    ["c:\\FsEye"]|> allFiles |> Seq.length
    0 // return an integer exit code
