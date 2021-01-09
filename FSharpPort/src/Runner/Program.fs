open BR.Core.Beach

[<EntryPoint>]
let main argv =
    let n = Name "Rusalka"

    let p =
        { Continent = "Europe"
          WaterBody = "Black Sea" }

    let c = Coordinates "123,456"
    let s = createScore [ SandQuality 10.1 ]

    match s with
    | Ok sc ->
        let b = create n p c sc (System.Guid.NewGuid())
        printfn "%O" b
    | Error e -> printfn "%s" e

    0
