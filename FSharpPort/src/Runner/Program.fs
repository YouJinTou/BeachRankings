open BR.Core.Beach

[<EntryPoint>]
let main argv =
    let n = "Rusalka"

    let p =
        { Continent = "Europe"
          WaterBody = "Black Sea"
          Country = Some "Bulgaria"
          L1 = Some "Dobrich"
          L2 = Some "Shabla"
          L3 = None
          L4 = None }

    let c = Coordinates "123,456"
    let s = createScore [ SandQuality 8. ]

    match s with
    | Ok sc ->
        let b =
            createBeach n p c sc (System.Guid.NewGuid())

        printfn "%O" b
    | Error e -> printfn "%s" e

    0
