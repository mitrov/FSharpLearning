open System.IO

let getFiles path =
    try
        Directory.GetFiles path |> Seq.ofArray
    with _ -> Seq.empty

let getDirectories path =
    try
        Directory.GetDirectories path |> Seq.ofArray
    with _ -> Seq.empty
        
let rec allFilesUnder basePath  =
    seq {
        yield! getFiles basePath

        for subdir in getDirectories basePath do
        yield! allFilesUnder subdir
    }

let allFilesUnder' basePath  =
    let rec accAllFilesUnder basePath acc = 
        getFiles basePath
        for subdir in getDirectories basePath do
            accAllFilesUnder (subdir) (Seq.append )
    accAllFilesUnder basePath Seq.singleton

[<EntryPoint>]
let main argv = 
    allFilesUnder' @"C:\FsEye"
    |> Seq.iter(printfn "%s")
    0 // return an integer exit code
