open Beach
open Score

[<EntryPoint>]
let main argv =
    let n = Name.T "Rusalka"
    match Name.validate n with
    | Ok _ -> printfn "ok"
    | Error e -> printfn "%s" e
    let p =
        { Continent = "Europe"
          WaterBody = "Black Sea"
          Country = Some "Bulgaria"
          L1 = Some "Dobrich"
          L2 = Some "Shabla"
          L3 = None
          L4 = None }

    let c = Coordinates "123,456"
    let s = Score.create [ SandQuality 8. ]
    let a = AddedBy(System.Guid.NewGuid())

    match s with
    | Ok sc ->
        let b = Beach.create n p c sc a

        printfn "%O" b
    | Error e -> printfn "%s" e

    0
