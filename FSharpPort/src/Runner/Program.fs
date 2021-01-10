open BR.Core.Beach

[<EntryPoint>]
let main argv =
    let n = Name "Rusalka"

    let p =
        { Continent = "Europe"
          WaterBody = "Black Sea"
          Country = Some "Bulgaria"
          L1 = Some "Dobrich"
          L2 = Some "Shabla"
          L3 = None
          L4 = None }

    let c = Coordinates "123,456"
    let s = Score.create [ Score.SandQuality 8. ]
    let a = AddedBy(System.Guid.NewGuid())

    match s with
    | Ok sc ->
        let b = create n p c sc a

        printfn "%O" b
    | Error e -> printfn "%s" e

    0
